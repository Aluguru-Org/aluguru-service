using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mubbi.Marketplace.Catalog.ViewModels
{
    public class UpdateCategoryViewModel : IDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Code { get; set; }
        public Guid? MainCategoryId { get; set; }
    }
}
