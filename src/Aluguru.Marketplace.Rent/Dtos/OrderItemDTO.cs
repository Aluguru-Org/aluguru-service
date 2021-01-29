using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Rent.Dtos
{
    public class OrderItemDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; private set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime RentStartDate { get; set; }
        public int RentDays { get; set; }
        public string ProductName { get; set; }
        public string ProductUri { get; private set; }  
        public string ProductImageUrl { get; private set; }
        public int Amount { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal FreigthPrice { get; private set; }
    }
}
