using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BaMan.Shared.ManagedRedis
{
    public class ManagedRedisConfigurer
    {
        private readonly RedisConnectionMan _redisConnectionMan;
        private readonly IServiceCollection _services;

        public ManagedRedisConfigurer(RedisConnectionMan redisConnectionMan, IServiceCollection services)
        {
            _redisConnectionMan = redisConnectionMan;
            _services = services;
        }

        /// <summary>
        /// Register a publisher for the specified message type. <br />
        /// Messages published using the managed publisher are published to redis with <c>typeof(TMessage).FullName</c> as the "topic"
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        public ManagedRedisConfigurer AddManagedPublisher<TMessage>()
        {
            _services.AddSingleton<ManagedPublisher<TMessage>>();
            return this;
        }

        /// <summary>
        /// Subscribes to redis channel using the <c>typeof(TMessage).FullName</c> for the "topic" 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <typeparam name="TMessageConsumer"></typeparam>
        /// <returns></returns>
        public ManagedRedisConfigurer RegisterManagedConsumer<TMessage, TMessageConsumer>() where TMessageConsumer : class, IManagedConsumer<TMessage>
        {
            // Register the consumer
            _services.AddScoped<IManagedConsumer<TMessage>, TMessageConsumer>();

            string channel = typeof(TMessage).FullName;

            // Subscribe to the Redis channel and invoke Consume when a message is received
            _redisConnectionMan.Connection.GetSubscriber().Subscribe(channel, async (channel, message) =>
            {
                // Resolve the consumer instance from the service provider
                var serviceProvider = _services.BuildServiceProvider();
                var consumer = serviceProvider.GetRequiredService<IManagedConsumer<TMessage>>();

                // Deserialize the message if necessary and call Consume
                var typedMessage = JsonConvert.DeserializeObject<TMessage>(message);
                await consumer.Consume(typedMessage);
            });

            return this;
        }
    }

}