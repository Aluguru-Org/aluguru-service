using Aluguru.Marketplace.Domain;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Catalog.ViewModels
{
    public class CreateCategoryViewModel : IDto
    {        
        public Guid? MainCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [SwaggerSchema("Formatted category name. Examples: 'cellphone', 'trip-equipament', 'gamer-pc'")]
        public string Uri { get; set; }
    }
}
