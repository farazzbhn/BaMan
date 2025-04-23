using BaMan.Shared.Types.Messages;
using System;
using System.Collections.Concurrent;
using System.Threading;

using System.Threading.Tasks;

namespace BaMan.Application.Features.Services.RPC;

public class RPCService : IRPCService
{
    private static ConcurrentDictionary<Guid, RPCResult> s_results = new();
    private static ConcurrentDictionary<Guid, SemaphoreSlim> s_waitHandles = new();

    public void AddResult(RPCResult result)
    {
        // Only proceed if someone is actually waiting for this result
        if (s_waitHandles.TryRemove(result.CorrelationId, out var semaphore))
        {
            s_results.TryAdd(result.CorrelationId, result);
            // Notify the waiter
            semaphore.Release();
        }
        // no one interested in the result ? why bother ?
    }

    public async Task<RPCResult> WaitForResultAsync(Guid id, TimeSpan timeout)
    {
        // Check if the result already exists 
        if (s_results.TryGetValue(id, out var existingResult))
        {
            return existingResult;
        }

        var semaphore = new SemaphoreSlim(0, 1);
        s_waitHandles.TryAdd(id, semaphore);

        try
        {
            var acquired = await semaphore.WaitAsync(timeout);

            if (!acquired)
            {
                s_waitHandles.TryRemove(id, out _);

                return new RPCResult()
                {
                    CorrelationId = id,
                    RanToCompletion = false,
                    Result = null,
                    Exception = new TimeoutException(),
                };
            }

            s_results.TryGetValue(id, out var result);
            return result!;
        }
        finally
        {
            semaphore.Dispose();
        }
    }
}
