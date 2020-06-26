using System.ComponentModel.DataAnnotations;

namespace Mubbi.Marketplace.Register.Application.ViewModels
{
    public class UserLoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public int Password { get; set; }
    }
}
