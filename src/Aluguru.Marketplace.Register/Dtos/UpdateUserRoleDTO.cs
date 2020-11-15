using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Register.Dtos
{
    public class UpdateUserRoleDTO : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
