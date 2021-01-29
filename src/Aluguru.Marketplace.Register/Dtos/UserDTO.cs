using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Register.Dtos
{
    public class UserDTO : IDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public ContactDTO Contact { get; set; }
        public UserRoleDTO UserRole { get; set; }
        public DocumentDTO Document { get; set; }
        public AddressDTO Address { get; set; }
    }
}