using MediatR;
using System;

namespace Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications
{
    public class DomainNotification : Message, INotification
    {
        public DomainNotification(string key, string value)
        {
            Timestamp = DateTime.UtcNow;
            DomainNotificationId = Guid.NewGuid();
            Version = 1;
            Key = key;
            Value = value;
        }

        public DateTime Timestamp { get; private set; }
        public Guid DomainNotificationId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public int Version { get; private set; }
    }
}
