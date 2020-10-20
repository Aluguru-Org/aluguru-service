using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;

namespace Aluguru.Marketplace.Register.ViewModels
{
    public class UpdateUserRoleViewModel : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
