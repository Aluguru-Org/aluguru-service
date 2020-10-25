using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Communication.Dtos
{
    public class OrderDTO : IDto
    {
        public OrderDTO(Guid id, Guid userId, decimal totalPrice, List<OrderItemDTO> orderItems)
        {
            Id = id;
            UserId = userId;
            TotalPrice = totalPrice;
            OrderItems = orderItems;
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }
}
