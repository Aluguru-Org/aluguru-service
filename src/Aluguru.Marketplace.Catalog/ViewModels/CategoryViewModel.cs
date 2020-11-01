using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Catalog.ViewModels
{
    public class CategoryViewModel : IDto
    {
        public Guid Id { get; set; }
        public Guid? MainCategoryId { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        public string Name { get; set; }
        [SwaggerSchema("Formatted category name. Examples: 'cellphone', 'trip-equipament', 'gamer-pc'")]
        public string Uri { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        [SwaggerExclude]
        public CategoryViewModel MainCategory { get; set; }
        public List<CategoryViewModel> SubCategories { get; set; }
        [SwaggerExclude]
        public List<ProductViewModel> Products { get; set; }
    }
}
