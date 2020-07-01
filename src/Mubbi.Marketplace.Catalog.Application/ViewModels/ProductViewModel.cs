using Mubbi.Marketplace.Catalog.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mubbi.Marketplace.Catalog.Application.ViewModels
{
    public class ProductViewModel 
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public string Descrpition { get; set; }
        
        [Required(ErrorMessage = "The field {0} is required")]
        public string Image { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public ERentType RentType { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public TimeSpan MinRentTime { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public TimeSpan MaxRentTime { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public DateTime CreationDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}")]
        [Required(ErrorMessage = "The field {0} is required")]
        public int StockQuantity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}")]
        [Required(ErrorMessage = "The field {0} is required")]
        public decimal Height { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}")]
        [Required(ErrorMessage = "The field {0} is required")]
        public int Width { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}")]
        [Required(ErrorMessage = "The field {0} is required")]
        public int Depth { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
