using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mubbi.Marketplace.Register.Domain.Models;
using Mubbi.Marketplace.Register.Domain.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mubbi.Marketplace.Register.Domain.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserContextSettigns _userContextSettigns;
        public TokenService(IOptions<UserContextSettigns> options)
        {
            _userContextSettigns = options?.Value;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = GenerateSecurityTokenDescriptor(user);
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }        

        private SecurityTokenDescriptor GenerateSecurityTokenDescriptor(User user)
        {
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.Email, user.Role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(GetUserSecret()), SecurityAlgorithms.HmacSha256Signature)
            };
        }

        private byte[] GetUserSecret()
        {
            return Encoding.ASCII.GetBytes(_userContextSettigns.Secret);
        }
    }
}
