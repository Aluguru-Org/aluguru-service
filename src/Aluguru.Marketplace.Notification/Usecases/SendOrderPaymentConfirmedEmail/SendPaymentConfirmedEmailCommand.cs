using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Notification.Usecases.SendOrderPaymentConfirmedEmail
{
    public class SendPaymentConfirmedEmailCommand : Command<bool>
    {
        public SendPaymentConfirmedEmailCommand(Guid orderId, string userName, string userEmail)
        {
            OrderId = orderId;
            UserName = userName;
            UserEmail = userEmail;
        }

        public Guid OrderId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}
