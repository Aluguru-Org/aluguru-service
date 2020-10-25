using Aluguru.Marketplace.Domain;
using System;

namespace Aluguru.Marketplace.Communication.Dtos
{
    public class OrderItemDTO : IDto
    {
        public OrderItemDTO(Guid productId, string productName, int amount, decimal productPrice)
        {
            ProductId = productId;
            ProductName = productName;
            Amount = amount;
            ProductPrice = productPrice;
        }

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal CalculatePrice()
        {
            return Amount * ProductPrice;
        }
    }
}
