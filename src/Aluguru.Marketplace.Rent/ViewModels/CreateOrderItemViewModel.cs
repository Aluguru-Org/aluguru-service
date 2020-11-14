using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Rent.ViewModels
{
    public class CreateOrderItemViewModel
    {
        [Required]
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        [Required]
        public DateTime RentStartDate { get; set; }
        public int RentDays { get; set; }
        public Guid? SelectedRentPeriod { get; set; }
        public int? Amount { get; set; }
        public List<CustomFieldResponseViewModel> Responses { get; set; }
    }
}