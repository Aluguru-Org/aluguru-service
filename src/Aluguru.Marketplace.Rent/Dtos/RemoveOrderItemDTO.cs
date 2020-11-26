using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Rent.Dtos
{
    public class RemoveOrderItemDTO : IDto
    {
        [Required]
        public Guid ProductId { get; set; }
    }
}