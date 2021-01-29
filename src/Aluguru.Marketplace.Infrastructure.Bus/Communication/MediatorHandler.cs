using MediatR;
using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Infrastructure.Bus.Communication
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishEvent<T>(T @event) where T : Event
        {
            await _mediator.Publish(@event);
        }

        public async Task PublishNotification<T>(T notification) where T : DomainNotification
        {
            await _mediator.Publish(notification);
        }

        public async Task<TResponse> SendCommand<T, TResponse>(T command) where T : Command<TResponse>
        {
            return await _mediator.Send(command);
        }
    }
}
