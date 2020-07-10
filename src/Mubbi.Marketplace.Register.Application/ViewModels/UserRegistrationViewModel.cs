using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class UserRegistrationViewModel
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public AddressViewModel Address { get; set; }
        public DocumentViewModel Document { get; set; }
    }
}
