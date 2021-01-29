using Aluguru.Marketplace.Domain;
using System;

namespace Aluguru.Marketplace.Register.Dtos
{
    public class UpdateUserContactDTO : IDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}