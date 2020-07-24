using Mubbi.Marketplace.Domain;
using System.ComponentModel.DataAnnotations;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class LoginUserViewModel : IDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
