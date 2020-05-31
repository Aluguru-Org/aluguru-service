using Mubbi.Marketplace.Register.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.Domain.Services
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
