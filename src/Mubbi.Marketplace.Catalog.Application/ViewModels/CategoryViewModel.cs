using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Swagger;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mubbi.Marketplace.Catalog.ViewModels
{
    public class CategoryViewModel : IDto
    {
        public Guid Id { get; set; }
        public Guid? MainCategoryId { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public int Code { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        [SwaggerExclude]
        public CategoryViewModel MainCategory { get; set; }
        public List<CategoryViewModel> SubCategories { get; set; }
        [SwaggerExclude]
        public List<ProductViewModel> Products { get; set; }
    }
}
