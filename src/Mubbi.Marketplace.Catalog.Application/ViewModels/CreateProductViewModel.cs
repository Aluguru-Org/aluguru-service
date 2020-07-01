using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mubbi.Marketplace.Catalog.Application.ViewModels
{
    public class CreateProductViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        public Guid? ChildrenCategoryId { get; set; }
    }
}
