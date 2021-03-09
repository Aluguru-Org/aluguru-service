using Aluguru.Marketplace.Domain;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aluguru.Marketplace.Catalog.Dtos
{
    public class UpdateCategoryDTO : IDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [SwaggerSchema("Formatted category name. Examples: 'cellphone', 'trip-equipament', 'gamer-pc'")]
        public string Uri { get; set; }
        public bool Highlights { get; set; }
        public Guid? MainCategoryId { get; set; }
    }
}
