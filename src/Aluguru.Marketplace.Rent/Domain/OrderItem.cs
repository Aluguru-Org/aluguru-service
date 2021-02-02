using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using static PampaDevs.Utils.Helpers.DateTimeHelper;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Aluguru.Marketplace.Rent.Domain
{
    public class OrderItem : Entity
    {
        public OrderItem(Guid companyId, Guid productId, string productUri, string productName, string productImageUrl, DateTime rentStartDate, int rentDays, int amount, decimal productPrice)
            : base(NewId())
        {
            CompanyId = companyId;
            OrderItemStatus = EOrderItemStatus.Initiated;
            ProductId = productId;  
            ProductUri = productUri;
            ProductImageUrl = productImageUrl;
            ProductName = productName;
            RentStartDate = rentStartDate;
            RentDays = rentDays;
            Amount = amount;
            ProductPrice = productPrice;

            ValidateEntity();
        }
        public Guid CompanyId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductUri { get; private set; }
        public string ProductImageUrl { get; private set; }
        public EOrderItemStatus OrderItemStatus { get; set; }
        public DateTime RentStartDate { get; set; }
        public int RentDays { get; set; }
        public string ProductName { get; private set; }
        public int Amount { get; private set; }
        public decimal ProductPrice { get; private set; }
        public decimal FreigthPrice { get; private set; }

        public virtual Order Order { get; set; }

        internal void AssociateOrder(Guid orderId)
        {
            OrderId = orderId;
        }

        public decimal CalculatePrice()
        {
            return Amount * (ProductPrice + FreigthPrice);
        }

        public void MarkAsDelivered()
        {
            Ensure.That(OrderItemStatus == EOrderItemStatus.Initiated, $"The order item {Id} is already canceled, returned or delivered");

            OrderItemStatus = EOrderItemStatus.Delivered;
            DateUpdated = NewDateTime();
        }

        public void MarkAsReturned()
        {
            OrderItemStatus = EOrderItemStatus.Returned;
            DateUpdated = NewDateTime();
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

        internal void UpdateFreigthPrice(decimal freigthPrice)
        {
            Ensure.That(freigthPrice >= 0, "The FreigthPrice cannot be smaller than 0");
            FreigthPrice = freigthPrice;
        }

        protected override void ValidateEntity()
        {
            Ensure.NotEqual(Guid.Empty, ProductId, "The field ProductId cannot be empty");
            Ensure.NotEqual(Guid.Empty, CompanyId, "The field CompanyId cannot be empty");
            Ensure.NotNullOrEmpty(ProductName, "The field ProductName cannot be empty");
            Ensure.NotNullOrEmpty(ProductUri, "The field Product uri cannot be empty");
            Ensure.That(RentDays > 0, "The field RentDays cannot be smaller or qual than 0");
            Ensure.That(Amount > 0, "The field Amount cannot be smaller or qual than 0");
            Ensure.That(ProductPrice > 0, "The field ProductPrice cannot be smaller or qual than 0");
            Ensure.That(FreigthPrice >= 0, "The field FreightPrice cannot be smaller than 0");
        }
    }
}
