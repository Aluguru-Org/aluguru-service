using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.Application.ViewModels
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public AddressViewModel AddressViewModel { get; set; }
        public DocumentViewModel DocumentViewModel { get; set; }
    }
}
