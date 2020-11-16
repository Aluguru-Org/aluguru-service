using Aluguru.Marketplace.Domain;
using System;

namespace Aluguru.Marketplace.Payment.Dtos
{
    public class PaymentDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string InvoiceId { get; set; }
        public string Url { get; set; }
        public string Pdf { get; set; }
        public string Identification { get; set; }
        public bool Paid { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
