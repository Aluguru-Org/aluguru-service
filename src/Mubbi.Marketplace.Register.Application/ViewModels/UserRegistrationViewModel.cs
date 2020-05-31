using Mubbi.Marketplace.Register.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.Application.DataTransferObjects
{
    public class UserRegistrationViewModel
    {
        public string UserName { get; set; }
        public int Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public AddressViewModel AddressViewModel { get; set; }
        public DocumentViewModel DocumentViewModel { get; set; }
    }
}
