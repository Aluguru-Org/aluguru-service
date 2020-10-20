using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aluguru.Marketplace.Catalog.ViewModels
{
    public class UpdateCategoryViewModel : IDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid? MainCategoryId { get; set; }
    }
}
