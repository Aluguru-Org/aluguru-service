using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class UserClaimViewModel : IDto
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
