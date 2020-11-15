using Aluguru.Marketplace.Domain;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Register.Dtos
{
    public class CreateUserRoleDTO : IDto
    {
        public string Name { get; set; }
        public List<CreateUserClaimDTO> UserClaims { get; set; }
    }
}