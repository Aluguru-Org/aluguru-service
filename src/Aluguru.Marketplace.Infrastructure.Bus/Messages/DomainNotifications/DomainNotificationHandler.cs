using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        private List<DomainNotification> _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification>();

        }

        public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
        {
            _notifications.Add(notification);
            return Task.CompletedTask;
        }

        public virtual List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public virtual Dictionary<string, string[]> GetNotificationErrors()
        {
            var keys = _notifications.Select(x => x.Key).Distinct();
            var errors = new Dictionary<string, string[]>();
            foreach(var key in keys)
            {
                errors[key] = _notifications.Where(x => x.Key.Equals(key, System.StringComparison.Ordinal)).Select(x => x.Value).ToArray();
            }
            return errors;
        }

        public virtual bool HasNotifications()
        {
            return _notifications.Any();
        }
    }
}
