using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Register.ViewModels
{
    public class UserRoleViewModel : IDto
    {
        public string Name { get; set; }
        public List<UserClaimViewModel> UserClaims { get; set; }
    }
}
