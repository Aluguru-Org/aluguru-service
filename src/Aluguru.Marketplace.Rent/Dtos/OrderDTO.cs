using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Rent.Dtos
{
    public class OrderDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? VoucherId { get; set; }
        public bool VoucherUsed { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public VoucherDTO Voucher { get; set; }
    }
}
