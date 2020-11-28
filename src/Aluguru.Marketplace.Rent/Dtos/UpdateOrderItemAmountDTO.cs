using Aluguru.Marketplace.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Rent.Dtos
{
    public class UpdateOrderItemAmountDTO : IDto
    {
        [Required]
        public Guid OrderItemId { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
