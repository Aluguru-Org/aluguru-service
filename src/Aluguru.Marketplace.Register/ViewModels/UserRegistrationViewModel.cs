using Aluguru.Marketplace.Domain;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Register.ViewModels
{
    public class UserRegistrationViewModel : IDto
    {
        public string FullName { get; set; }
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$&*=-])(?=.*[0-9])(?=.*[a-z]).{8,32}$")]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
