using Aluguru.Marketplace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Register.Dtos
{
    public class UpdateUserNameDTO : IDto
    {
        public string FullName { get; set; }
    }
}
