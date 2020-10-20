using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Infrastructure.Bus.Messages
{
    public abstract class Event : Message, INotification
    {
        public Guid AggregateId { get; protected set; }
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
