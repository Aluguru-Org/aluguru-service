using Mubbi.Marketplace.Domain;
using System.ComponentModel.DataAnnotations;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class UserLoginViewModel : IDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
