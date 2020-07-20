using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mubbi.Marketplace.Register.Domain;
using Mubbi.Marketplace.Register.Domain.Jwt;
using Mubbi.Marketplace.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace Mubbi.Marketplace.Register.Services
{
    public interface ITokenBuilderService
    {
        string BuildToken(User user, Action<JwtTokenBuilder> options);
    }

    public class TokenBuilderService : ITokenBuilderService
    {
        private readonly JwtSettings _settings;

        public TokenBuilderService(IOptions<JwtSettings> options)
        {
            _settings = options?.Value;
        }

        public string BuildToken(User user, Action<JwtTokenBuilder> options)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.SecretKey));

            var tokenBuilder = new JwtTokenBuilder(user);

            options?.Invoke(tokenBuilder);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _settings.Issuer,
                Audience = _settings.Audiences.Aggregate((i, j) => $"{i}{(string.IsNullOrEmpty(j) ? "" : $",{j}")}"),
                Subject = tokenBuilder.IdentityClaims,
                Expires = DateTime.UtcNow.AddHours(_settings.Expiration),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
