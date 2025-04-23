using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace BaMan.Shared.ManagedRedis
{
    public class RedisConnectionMan
    {
        private readonly RedisOptions _options;
        private readonly Lazy<ConnectionMultiplexer> LazyConnection;

        public RedisConnectionMan(IOptions<RedisOptions> options)
        {
            _options = options.Value;
            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_options.ConnectionString));
        }

        public ConnectionMultiplexer Connection => LazyConnection.Value;
    }
}
