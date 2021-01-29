using Aluguru.Marketplace.Domain;
using System;

namespace Aluguru.Marketplace.Payment.Dtos
{
    public class PayOrderDTO : IDto
    {
        public Guid OrderId { get; set; }
        public string Token { get; set; }
        public int? Installments { get; set; }
    }
}
