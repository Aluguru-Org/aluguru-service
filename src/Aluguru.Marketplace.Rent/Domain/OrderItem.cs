using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Aluguru.Marketplace.Rent.Domain
{
    public class OrderItem : Entity
    {
        public OrderItem(Guid productId, string productName, int amount, decimal productPrice)
            : base(NewId())
        {
            ProductId = productId;
            ProductName = productName;
            Amount = amount;
            ProductPrice = productPrice;

            ValidateEntity();
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
            Ensure.That(amount > 0, "The amount cannot be smaller or equal than 0");

            Amount += amount;
        }

        internal void UpdateAmount(int amount)
        {
            Ensure.That(amount > 0, "The amount cannot be smaller than 0");

            Amount = amount;
        }

        protected override void ValidateEntity()
        {
            Ensure.NotEqual(Guid.Empty, ProductId, "The field ProductId be empty");
            Ensure.NotNullOrEmpty(ProductName, "The field ProductName cannot be empty");
            Ensure.That(Amount > 0, "The field Amount cannot be smaller or qual than 0");
            Ensure.That(ProductPrice > 0, "The field ProductPrice cannot be smaller or qual than 0");
        }
    }
}
