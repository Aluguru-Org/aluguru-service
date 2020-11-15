using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Register.Dtos
{
    public class UserClaimDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UserRoleId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
