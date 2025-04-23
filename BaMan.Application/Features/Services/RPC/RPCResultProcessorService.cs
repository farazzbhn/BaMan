using BaMan.Shared.ManagedChannels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaMan.Shared.Types.Messages;

namespace BaMan.Application.Features.Services.RPC
{
    public class RPCResultProcessorService : ManagedChannelProcessorBackgroundService<RPCResult>
    {
        private readonly IRPCService _rpcService;


        public RPCResultProcessorService(ManagedChannel<RPCResult> channel, IRPCService rpcService) : base(channel)
        {
            _rpcService = rpcService;
        }

        /// <summary>
        /// Continuously reads messages from the managed channel and enqueues them for processing.
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                try
                {
                    await foreach (var msg in Reader.ReadAllAsync(stoppingToken))
                    {
                        _rpcService.AddResult(msg);
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Processing canceled.");
                }
            }
        }

    }
}
