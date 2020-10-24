using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Register.Domain.Jwt;
using Aluguru.Marketplace.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace Aluguru.Marketplace.Register.Services
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
                Audience = _settings.Audience,
                Subject = tokenBuilder.IdentityClaims,
                Expires = DateTime.UtcNow.AddHours(_settings.Expiration),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
