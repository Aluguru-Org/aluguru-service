using Aluguru.Marketplace.Domain;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Register.Dtos
{
    public class UserRegistrationDTO : IDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
        public AddressDTO Address { get; set; }
        public ContactDTO Contact { get; set; }
        public DocumentDTO Document { get; set; }
    }
}