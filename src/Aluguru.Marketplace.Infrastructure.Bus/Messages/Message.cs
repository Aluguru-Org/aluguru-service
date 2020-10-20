using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Infrastructure.Bus.Messages
{
    public abstract class Message
    {
        public string MessageType { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
