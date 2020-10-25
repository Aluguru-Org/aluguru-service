using Aluguru.Marketplace.Communication.IntegrationEvents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Catalog.Handlers
{
    public class OrderStartedHandler : INotificationHandler<OrderStartedEvent>
    {
        private readonly IMediatorHandler _mediatorHandler;
        public OrderStartedHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(OrderStartedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
