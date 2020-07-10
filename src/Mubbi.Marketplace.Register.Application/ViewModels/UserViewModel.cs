using System;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public AddressViewModel Address { get; set; }
        public DocumentViewModel Document { get; set; }
    }
}
