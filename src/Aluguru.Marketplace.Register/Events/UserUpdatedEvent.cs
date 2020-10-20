using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Register.Domain;
using System;

namespace Aluguru.Marketplace.Register.Events
{
    public class UserUpdatedEvent : Event
    {
        public UserUpdatedEvent(Guid userId, User user)
        {
            UserId = userId;
            User = user;
        }

        public Guid UserId { get; }
        public User User { get; }
    }
}
