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
        public OrderStartedHandler()
        {
                            
        }

        public Task Handle(OrderStartedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
