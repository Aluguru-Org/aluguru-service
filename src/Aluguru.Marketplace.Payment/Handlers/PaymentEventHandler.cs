using Aluguru.Marketplace.Communication.IntegrationEvents;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Payment.Handlers
{
    public class PaymentEventHandler : INotificationHandler<OrderStockConfirmedEvent>
    {
        public Task Handle(OrderStockConfirmedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
