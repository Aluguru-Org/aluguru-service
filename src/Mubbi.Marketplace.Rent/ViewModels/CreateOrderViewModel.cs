using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mubbi.Marketplace.Rent.ViewModels
{
    public class CreateOrderViewModel
    {
        [Required]
        public Guid UserId { get ;set; }
        [Required]
        public List<CreateOrderItemViewModel> OrderItems { get; set; }
    }
}