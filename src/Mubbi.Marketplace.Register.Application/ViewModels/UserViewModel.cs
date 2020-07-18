using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class UserViewModel : IDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public UserRoleViewModel UserRole { get; set; }
        public DocumentViewModel Document { get; set; }
        public List<AddressViewModel> Addresses { get; set; }
    }
}