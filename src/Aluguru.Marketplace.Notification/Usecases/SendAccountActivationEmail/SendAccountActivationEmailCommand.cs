using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;

namespace Aluguru.Marketplace.Notification.Usecases.SendAccountActivationEmail
{
    public class SendAccountActivationEmailCommand : Command<bool>
    {
        public SendAccountActivationEmailCommand(Guid userId, string userName, string email, string activationHash)
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            ActivationHash = activationHash;
        }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ActivationHash { get; set; }
    }    
}
