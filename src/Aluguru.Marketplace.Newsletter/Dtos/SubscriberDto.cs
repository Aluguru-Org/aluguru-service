using Aluguru.Marketplace.Domain;
using System.ComponentModel.DataAnnotations;

namespace Aluguru.Marketplace.Newsletter.Dtos
{
    public class SubscriberDto : IDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._]+@[a-zA-Z0-9]+\.[a-z]+(?:\.[a-zA-Z]+)?", ErrorMessage = "Invalid E-Mail")]
        public string Email { get; set; }
    }
}
