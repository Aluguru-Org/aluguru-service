using Aluguru.Marketplace.Domain;
using System;

namespace Aluguru.Marketplace.Communication.Dtos
{
    public class OrderItemDTO : IDto
    {
        public OrderItemDTO(Guid productId, string productName, int amount, decimal productPrice, DateTime? rentStartDate)
        {
            ProductId = productId;
            ProductName = productName;
            Amount = amount;
            ProductPrice = productPrice;
            RentStartDate = rentStartDate;
        }

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public decimal ProductPrice { get; set; }
        public DateTime? RentStartDate { get; set; }
        public decimal CalculatePrice()
        {
            return Amount * ProductPrice;
        }
    }
}
