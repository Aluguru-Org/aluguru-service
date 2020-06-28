using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Infrastructure.Bus.Messages
{
    public abstract class Event : Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
