using Mubbi.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.ViewModels
{
    public class UpdateUserPasswordViewModel : IDto
    {
        public string Password { get; set; }
    }
}
