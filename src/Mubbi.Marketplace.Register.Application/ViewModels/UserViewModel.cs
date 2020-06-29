using System;

namespace Mubbi.Marketplace.Register.Application.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public AddressViewModel AddressViewModel { get; set; }
        public DocumentViewModel DocumentViewModel { get; set; }
    }
}
