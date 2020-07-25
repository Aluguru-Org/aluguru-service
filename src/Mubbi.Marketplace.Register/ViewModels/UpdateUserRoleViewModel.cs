using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class UpdateUserRoleViewModel : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
