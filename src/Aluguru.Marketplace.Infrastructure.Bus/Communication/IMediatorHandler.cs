using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Infrastructure.Bus.Communication
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T @event) where T : Event;
        Task<TResponse> SendCommand<T, TResponse>(T command) where T : Command<TResponse>;
        Task PublishNotification<T>(T notification) where T : DomainNotification;
    }
}
