using System.Threading.Channels;
using Microsoft.Extensions.Hosting;

namespace BaMan.Shared.ManagedChannels;
public abstract class ManagedChannelProcessorBackgroundService<TMessage> : BackgroundService
{
    protected readonly ChannelReader<TMessage> Reader;

    public ManagedChannelProcessorBackgroundService
    (
        ManagedChannel<TMessage> channel
    )
    {
        Reader = channel.Reader;
    }

    protected abstract override Task ExecuteAsync(CancellationToken stoppingToken);
}