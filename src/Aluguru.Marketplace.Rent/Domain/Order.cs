using FluentValidation.Results;
using Aluguru.Marketplace.Domain;
using PampaDevs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static PampaDevs.Utils.Helpers.DateTimeHelper;
using static PampaDevs.Utils.Helpers.IdHelper;

namespace Aluguru.Marketplace.Rent.Domain
{
    public class Order : AggregateRoot
    {
        private readonly List<OrderItem> _orderItems;

        public Order(Guid clientId)
            : base(NewId())
        {
            UserId = clientId;
            OrderStatus = EOrderStatus.Draft;

            _orderItems = new List<OrderItem>();

            ValidateEntity();
        }

        protected Order()
            : base(NewId())
        {
            _orderItems = new List<OrderItem>();
        }

        public Guid UserId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool VoucherUsed { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalPrice { get; private set; }
        public EOrderStatus OrderStatus { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Voucher Voucher { get; private set; }

        public void Initiate()
        {
            Ensure.That(OrderStatus < EOrderStatus.Initiated, "The order is already initiated");
            OrderStatus = EOrderStatus.Initiated;
            DateUpdated = NewDateTime();
        }

        public void MarkAsPaid()
        {
            Ensure.That(OrderStatus == EOrderStatus.Initiated, "The order status cannot be mark as paid unless it has been initiated");
            OrderStatus = EOrderStatus.PaymentConfirmed;
            DateUpdated = NewDateTime();
        }

        public void CancelInitiation()
        {
            Ensure.That(OrderStatus != EOrderStatus.Draft, "The order is already draft");
            OrderStatus = EOrderStatus.Draft;
            DateUpdated = NewDateTime();
        }

        public void AddItem(OrderItem orderItem)
        {
            orderItem.AssociateOrder(Id);

            if (ItemExists(orderItem))
            {
                var existingItem = _orderItems.FirstOrDefault(x => x.ProductId == orderItem.ProductId);
                existingItem.AddAmount(orderItem.Amount);
                orderItem = existingItem;
            }
            else
            {
                _orderItems.Add(orderItem);
            }

            CalculateOrderPrice();
        }

        public void RemoveItem(Guid itemId)
        {
            var existingItem = _orderItems.FirstOrDefault(x => x.Id == itemId);

            Ensure.NotNull(existingItem, "The item does not belong to the order");

            _orderItems.Remove(existingItem);

            CalculateOrderPrice();
        }

        public ValidationResult ApplyVoucher(Voucher voucher)
        {
            var validationResult = voucher.IsValid();
            if (!validationResult.IsValid) return validationResult;
            
            VoucherId = voucher.Id;
            Voucher = voucher;
            VoucherUsed = true;

            CalculateOrderPrice();

            return validationResult;
        }

        public void RemoveVoucher()
        {
            if (Voucher == null) return;

            Voucher = null;
            VoucherUsed = false;

            CalculateOrderPrice();
        }

        public bool ItemExists(OrderItem orderItem)
        {
            return _orderItems.Any(x => x.Id == orderItem.Id);
        }

        private void CalculateOrderPrice()
        {
            TotalPrice = _orderItems.Sum(x => x.CalculatePrice());
            CalculateOrderDiscount();
        }

        private void CalculateOrderDiscount()
        {
            if (!VoucherUsed) return;

            decimal discount = 0;

            var price = TotalPrice;

            if (Voucher.VoucherType == EVoucherType.Percent)
            {
                if (Voucher.PercentualDiscount.HasValue)
                {
                    discount = (price * Voucher.PercentualDiscount.Value) / 100;
                    price -= discount;
                }
            }
            else
            {
                if (Voucher.ValueDiscount.HasValue)
                {
                    discount = Voucher.ValueDiscount.Value;
                    price -= discount;

                }
            }

            TotalPrice = price < 0 ? 0 : price;
            Discount = discount;
        }

        protected override void ValidateEntity()
        {
            Ensure.NotEqual(UserId, Guid.Empty, "The field ClientId from Order cannot be empty");
        }
    }
}
