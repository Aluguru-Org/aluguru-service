using Aluguru.Marketplace.Domain;
using System;
using static PampaDevs.Utils.Helpers.IdHelper;
using PampaDevs.Utils;

namespace Aluguru.Marketplace.Payment.Domain
{
    public class Payment : AggregateRoot
    {
        public Payment(Guid userId, Guid orderId, EPaymentMethod paymentMethod, string invoiceId, string url, string pdf, string identification)
            : base(NewId())
        {
            UserId = userId;
            OrderId = orderId;
            PaymentMethod = paymentMethod;
            InvoiceId = invoiceId;
            Url = url;
            Pdf = pdf;
            Identification = identification;

            ValidateEntity();
        }

        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public EPaymentMethod PaymentMethod { get; set; }
        public string InvoiceId { get; set; }
        public string Url { get; set; }
        public string Pdf { get; set; }
        public string Identification { get; set; }
        public bool Paid { get; set; }

        public void MarkAsPaid()
        {
            Paid = true;
        }

        protected override void ValidateEntity()
        {
            Ensure.NotEqual(UserId, Guid.Empty, "The field UserId from Payment cannot be empty");
            Ensure.NotEqual(OrderId, Guid.Empty, "The field OrderId from Payment cannot be empty");
            Ensure.NotNullOrEmpty(InvoiceId, "The field InvoiceId from Payment cannot be empty");
            Ensure.NotNullOrEmpty(Url, "The field Url from Payment cannot be empty");
            Ensure.NotNullOrEmpty(Pdf, "The field Pdf from Payment cannot be empty");
        }
    }
}
