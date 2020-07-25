using Mubbi.Marketplace.Infrastructure.Bus.Messages;
using Mubbi.Marketplace.Register.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.Events
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
