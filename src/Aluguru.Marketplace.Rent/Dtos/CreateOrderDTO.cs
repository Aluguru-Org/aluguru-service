using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Rent.Dtos
{
    public class CreateOrderDTO
    {
        [Required]
        public Guid UserId { get ;set; }
        [Required]
        public List<AddOrderItemDTO> OrderItems { get; set; }
    }
}