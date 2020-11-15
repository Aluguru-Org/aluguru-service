using Aluguru.Marketplace.Domain;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Register.Dtos
{
    public class LoginUserDTO : IDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
