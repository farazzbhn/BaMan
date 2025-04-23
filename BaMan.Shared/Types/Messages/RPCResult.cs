using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaMan.Shared.Types.Messages
{
    public class RPCResult
    {
        public Guid CorrelationId { get; set; }
        public bool RanToCompletion { get; set; }
        public object? Result { get; set; }
        public Exception? Exception { get; set; }
    }
}
