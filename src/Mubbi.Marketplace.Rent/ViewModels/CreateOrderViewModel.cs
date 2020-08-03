using System.Collections.Generic;
using System;
namespace Mubbi.Marketplace.Rent.ViewModels
{
    public class CreateOrderViewModel
    {
        public Guid UserId { get ;set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public List<CreateOrderItemViewModel> OrderItems { get; set; }
    }
}