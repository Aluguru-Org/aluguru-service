using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Register.Dtos
{
    public class UserRoleDTO : IDto
    {
        public string Name { get; set; }
        public List<UserClaimDTO> UserClaims { get; set; }
    }
}
