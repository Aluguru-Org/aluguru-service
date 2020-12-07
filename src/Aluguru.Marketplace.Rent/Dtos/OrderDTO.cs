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
        public bool FreigthCalculated { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalFreigthPrice { get; set; }
        public string DeliveryAddress { get; private set; }
        public string OrderStatus { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public VoucherDTO Voucher { get; set; }
    }
}
