using Microsoft.Extensions.DependencyInjection;

namespace BaMan.Shared.ManagedChannels
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a C# channel with a single reader ( and multiple writers ) to the system. <br />
        /// <b>TMessageProcessor</b> which is the Background processor service for the specified <b>TMessage</b> type must implement <see cref="ManagedChannelProcessorBackgroundService{TMessage}"/>. <br />
        /// Inject <see cref="ManagedChannel{T}"/> and publish messages using <see cref="ManagedChannel{T}.Writer"/> within the application layer <br />
        /// <b>TMessageProcessor</b> : <see cref="ManagedChannelProcessorBackgroundService{TMessage}"/> already has access to the reader as in <see cref="ManagedChannelProcessorBackgroundService{TMessage}.Reader"/>
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <typeparam name="TMessageProcessor"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddManagedChannel<TMessage, TMessageProcessor>(this IServiceCollection services) where TMessageProcessor : ManagedChannelProcessorBackgroundService<TMessage>
        {
            var channel = new ManagedChannel<TMessage>();
            services.AddSingleton(channel);
            services.AddHostedService<TMessageProcessor>();
            return services;
        }
    }


}
