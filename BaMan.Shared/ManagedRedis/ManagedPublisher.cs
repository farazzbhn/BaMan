using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BaMan.Shared.ManagedRedis
{
    public sealed class ManagedPublisher<TMessage>
    {
        private readonly RedisConnectionMan _redisConnectionMan;
        private readonly string _channel;

        public ManagedPublisher(RedisConnectionMan redisConnectionMan)
        {
            _redisConnectionMan = redisConnectionMan;
            _channel = typeof(TMessage).FullName;
        }

        public async Task<long> PublishAsync(TMessage message, CommandFlags flags = CommandFlags.FireAndForget)
        {
            return await _redisConnectionMan.Connection.GetSubscriber().PublishAsync(_channel,
                JsonConvert.SerializeObject(message), CommandFlags.FireAndForget);
        }
    }
}
