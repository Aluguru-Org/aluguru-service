using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Register.Dtos
{
    public class UpdateUserDTO : IDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public DocumentDTO Document { get; set; }
        public ContactDTO Contact { get; set; }
        public AddressDTO Address { get; set; }
    }
}
