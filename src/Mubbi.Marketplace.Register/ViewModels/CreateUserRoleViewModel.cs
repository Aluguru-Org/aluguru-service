﻿using Mubbi.Marketplace.Domain;
using System.Collections.Generic;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class CreateUserRoleViewModel : IDto
    {
        public string Name { get; set; }
        public List<CreateUserClaimViewModel> UserClaims { get; set; }
    }
}