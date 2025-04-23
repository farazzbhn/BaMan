using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaMan.Shared.ManagedRedis
{
    public interface IManagedConsumer<TMessage>
    {
        public abstract Task Consume(TMessage message);
    }
}
