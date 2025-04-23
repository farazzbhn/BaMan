using BaMan.Shared.Types.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaMan.Application.Features.Services.RPC
{
    public interface IRPCService
    {
        public void AddResult(RPCResult result);
        public Task<RPCResult> WaitForResultAsync(Guid id, TimeSpan timeout);

    }
}
