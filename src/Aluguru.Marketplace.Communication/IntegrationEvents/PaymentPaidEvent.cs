﻿using Aluguru.Marketplace.Infrastructure.Bus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Communication.IntegrationEvents
{
    public class PaymentPaidEvent : Event
    {
        public PaymentPaidEvent(string userName, string email, string orderUrl, string paymentUrl, string paymentPdf)
        {
            UserName = userName;
            Email = email;
            OrderUrl = orderUrl;
            PaymentUrl = paymentUrl;
            PaymentPdf = paymentPdf;
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string OrderUrl { get; set; }
        public string PaymentUrl { get; set; }
        public string PaymentPdf { get; set; }
    }
}
