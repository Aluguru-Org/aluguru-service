using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Register.ViewModels
{
    public class UpdateUserPasswordViewModel : IDto
    {
        public string Password { get; set; }
    }
}
