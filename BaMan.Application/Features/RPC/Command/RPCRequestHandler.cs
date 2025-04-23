using BaMan.Application.Features.Services.RPC;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BaMan.Shared.Types.Messages;
using BaMan.Shared.ManagedRedis;
using Newtonsoft.Json.Linq;

namespace BaMan.Application.Features.RPC.Command
{
    public class RPCRequest : IRequest<object>
    {
        [ModelBinder(Name = "name")]
        public string Name { get; set; }


        [ModelBinder(Name = "params")]
        public Dictionary<string, string> Params { get; set; }

    }

    public class RPCRequestHandler : IRequestHandler<RPCRequest, object>
    {
        private readonly ManagedPublisher<RPCMessage> _redisManagedPublisher;
        private readonly IRPCService _rpcService;


        public RPCRequestHandler(
            ManagedPublisher<RPCMessage> redisManagedPublisher, 
            IRPCService rpcService
        )
        {
            _redisManagedPublisher = redisManagedPublisher; 
            _rpcService = rpcService;
        }


        public async Task<object> Handle(RPCRequest request, CancellationToken cancellationToken)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Publishing RPCMessage \t {request.Name} ");
            Console.ResetColor();

            Guid id = Guid.NewGuid();

            var rpc = new RPCMessage
            {
                Id = id,
                MethodName = request.Name,
                Parameters = request.Params
            };

            await _redisManagedPublisher.PublishAsync(rpc);

            var result = await _rpcService.WaitForResultAsync(id, TimeSpan.FromSeconds(5));

            if (result.RanToCompletion)
            {
                if (result.Result is not null)
                    return result.Result;

                return string.Empty;
            }
            else
            {
                throw result.Exception!;
            }

        }
    }
}
