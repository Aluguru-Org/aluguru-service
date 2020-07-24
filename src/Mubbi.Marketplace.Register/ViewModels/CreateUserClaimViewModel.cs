using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class CreateUserClaimViewModel : IDto
    {
        public Guid Id { get; set; }
        public Guid UserRoleId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
