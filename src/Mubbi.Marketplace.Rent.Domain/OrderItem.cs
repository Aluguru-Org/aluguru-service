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

            ValidateCreation();
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
            EntityConcerns.SmallerOrEqualThan(0, amount, "The amount cannot be smaller or equal than 0");

            Amount += amount;
        }

        internal void UpdateAmount(int amount)
        {
            EntityConcerns.SmallerThan(0, amount, "The amount cannot be smaller than 0");

            Amount = amount;
        }

        public override void ValidateCreation()
        {
            EntityConcerns.IsEqual(Guid.Empty, ProductId, "The field ProductId be empty");
            EntityConcerns.IsEmpty(ProductName, "The field ProductName cannot be empty");
            EntityConcerns.SmallerOrEqualThan(0, Amount, "The field Amount cannot be smaller or qual than 0");
            EntityConcerns.SmallerOrEqualThan(0, ProductPrice, "The field ProductPrice cannot be smaller or qual than 0");
        }
    }
}
