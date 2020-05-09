using Mubbi.Marketplace.Shared.DomainObjects;
using System;

namespace Mubbi.Marketplace.Rent.Domain
{
    public class OrderItem : Entity
    {
        public OrderItem(Guid productId, string productName, int amount, decimal productPrice)
        {
            ProductId = productId;
            ProductName = productName;
            Amount = amount;
            ProductPrice = productPrice;
        }

        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Amount { get; private set; }
        public decimal ProductPrice { get; private set; }

        public virtual Order Order { get; set; }

        internal void AssociateOrder(Guid orderId)
        {
            OrderId = orderId;
        }

        public decimal CalculatePrice()
        {
            return Amount * ProductPrice;
        }

        internal void AddAmount(int amount)
        {
            Amount += amount;
        }

        internal void UpdateAmount(int amount)
        {
            Amount = amount;
        }

        public override void ValidateCreation()
        {
            throw new NotImplementedException();
        }
    }
}
