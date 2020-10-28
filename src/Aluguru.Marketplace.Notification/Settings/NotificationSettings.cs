using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Notification.Settings
{
    public class NotificationSettings
    {
        public string Sender { get; set; }
        public string SenderEmail { get; set; }
        public string ClientDomain { get; set; }
        public string ClientBackofficeDomain { get; set; }
    }
}
