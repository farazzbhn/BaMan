using System.Threading.Channels;

namespace BaMan.Shared.ManagedChannels
{
    public class ManagedChannel<T>
    {
        private readonly Channel<T> _channel;

        public ManagedChannel()
        {
            _channel = Channel.CreateUnbounded<T>(new UnboundedChannelOptions
            {
                SingleReader = true, // Set to true if there's only one consumer
                SingleWriter = false  // Set to true if there's only one producer
            });
        }

        public ChannelReader<T> Reader => _channel.Reader;
        public ChannelWriter<T> Writer => _channel.Writer;
    }

}
