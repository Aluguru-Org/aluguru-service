using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Notification.Usecases.SendAccountActivationEmail
{
    public class SendAccountActivationEmailCommand : Command<bool>
    {
        public SendAccountActivationEmailCommand(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; set; }
    }    
}
