using Mubbi.Marketplace.Domain;
using System.ComponentModel.DataAnnotations;

namespace Mubbi.Marketplace.Catalog.Application.ViewModels
{
    public class CreateCategoryViewModel : IDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Code { get; set; }
    }
}
