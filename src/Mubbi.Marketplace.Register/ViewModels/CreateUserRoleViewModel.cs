using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class CreateUserRoleViewModel : IDto
    {
        public string Name { get; set; }
        public List<CreateUserClaimViewModel> UserClaims { get; set; }
    }
}