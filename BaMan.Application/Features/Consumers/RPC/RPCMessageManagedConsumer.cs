using System.Threading.Channels;
using BaMan.Shared.ManagedChannels;
using BaMan.Shared.ManagedRedis;
using BaMan.Shared.Types.Messages;

namespace BaMan.Application.Features.Consumers.RPC
{
    public class RPCMessageManagedConsumer : IManagedConsumer<RPCMessage>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ManagedChannel<RPCResult> _managedChannel;

        public RPCMessageManagedConsumer(IServiceProvider serviceProvider, ManagedChannel<RPCResult> managedChannel)
        {
            _serviceProvider = serviceProvider;
            _managedChannel = managedChannel;
        }

        public async Task Consume(RPCMessage message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Redis Consumer will now try and invoke the method");
            Console.ResetColor();


            var resultMessage = new RPCResult()
            {
                CorrelationId = message.Id,
                RanToCompletion = false,
                Result = null,
                Exception = null
            };


            try
            {
                // 1. Get the IRemoteService type
                Type type = typeof(IRemoteService);

                // 2. Resolve the service instance
                var service = _serviceProvider.GetService(type);
                if (service == null)
                {
                    resultMessage.Exception = new Exception($"Service {type.Name} not registered !");
                    await _managedChannel.Writer.WriteAsync(resultMessage);
                    return;
                }

                // 3. Get the method info
                var method = type.GetMethod(message.MethodName);
                if (method == null)
                {
                    resultMessage.Exception = new Exception($"Method '{message.MethodName}' not found in type '{type.Name}");
                    await _managedChannel.Writer.WriteAsync(resultMessage);
                    return;
                }

                // 4. Get method parameters
                var parameters = method.GetParameters();
                var arguments = new object[parameters.Length];

                // 5. Match parameters from dictionary to method parameters
                for (int i = 0; i < parameters.Length; i++)
                {
                    var paramName = parameters[i].Name;
                    if (message.Parameters.TryGetValue(paramName, out var paramValue))
                    {
                        try
                        {
                            // Convert string parameter to the expected type
                            var paramType = parameters[i].ParameterType;
                            arguments[i] = Convert.ChangeType(paramValue, paramType);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error converting parameter '{paramName}' from string to {parameters[i].ParameterType.Name}: {ex.Message}");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Parameter '{paramName}' not found in message parameters.");
                        return;
                    }
                }

                // 6. Invoke the method
                var result = method.Invoke(service, arguments);

                // 7. If the method is async, await the result
                if (result is Task taskResult)
                {
                    await taskResult;
                }

                // 8. No exception thrown by the actual method =>
                resultMessage.RanToCompletion = true;

                // 9. Add the result if the method returns a value
                if (method.ReturnType != typeof(void) && method.ReturnType != typeof(Task))
                {
                    var actualResult = result is Task task ? await GetTaskResult(task) : result;
                    resultMessage.Result = actualResult;
                }

                // 9. Publish the result
                await _managedChannel.Writer.WriteAsync(resultMessage);
            }
            catch (Exception ex)
            {
                // Publish error result
                resultMessage.Exception = ex;
                await _managedChannel.Writer.WriteAsync(resultMessage);
                return;
            }
        }

        private async Task<object> GetTaskResult(Task task)
        {
            await task;
            var resultProperty = task.GetType().GetProperty("Result");
            return resultProperty?.GetValue(task);
        }
    }
}