using System.Threading.Channels;

namespace Mint.Core.Transport
{
    public interface IMessageTransporter
    {
        Task Send<TMessage>(TMessage message, string destination, CancellationToken token= default);
        Task Publish<TMessage>(TMessage message, CancellationToken token = default);
    }
}
