using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaMan.Shared.ManagedRedis
{
    public static class ServiceCollectionExtensions
    {
        public static ManagedRedisConfigurer AddManagedRedis(this IServiceCollection services, IConfiguration configuration)
        {
            // Register RedisOptions and RedisMan
            services.Configure<RedisOptions>(configuration.GetSection(nameof(RedisOptions)));


            services.AddSingleton<RedisConnectionMan>();

            var redisMan = services.BuildServiceProvider().GetService<RedisConnectionMan>();
            return new ManagedRedisConfigurer(redisMan, services);
        }
    }
}
