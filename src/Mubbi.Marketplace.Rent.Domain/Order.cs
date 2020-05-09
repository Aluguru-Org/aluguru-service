using Mubbi.Marketplace.Shared.DomainObjects;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Mubbi.Marketplace.Rent.Domain
{
    public class Order : Entity, IAggregateRoot
    {
        private readonly List<OrderItem> _orders;

        public Order(Guid clientId, decimal discount, decimal totalPrice)
        {
            ClientId = clientId;
            Discount = discount;
            TotalPrice = totalPrice;
        }

        public int Code { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool VoucherUsed { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalPrice { get; private set; }
        public DateTime CreationDate { get; private set; }
        public EOrderStatus OrderStatus { get; private set; }
        public IReadOnlyCollection<OrderItem> Orders => _orders;

        public Voucher Voucher { get; private set; }
        public override void ValidateCreation()
        {
            throw new NotImplementedException();
        }
    }
}
