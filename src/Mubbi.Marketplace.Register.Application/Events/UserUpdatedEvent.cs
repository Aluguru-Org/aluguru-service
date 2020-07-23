using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Register.Domain;
using System;

namespace Mubbi.Marketplace.Register.Events
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
