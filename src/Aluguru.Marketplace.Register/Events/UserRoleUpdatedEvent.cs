using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using Aluguru.Marketplace.Register.Domain;
using System;

namespace Aluguru.Marketplace.Register.Events
{
    class UserRoleUpdatedEvent : Event
    {
        public UserRoleUpdatedEvent(Guid userId, UserRole userRole)
        {
            UserId = userId;
            User = userRole;
        }

        public Guid UserId { get; }
        public UserRole User { get; }
    }
}
