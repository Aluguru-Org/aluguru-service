using System;
namespace Mubbi.Marketplace.Rent.ViewModels
{
    public class CreateOrderItemViewModel
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public decimal ProductPrice { get; set; }
    }
}