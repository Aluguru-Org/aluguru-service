using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using static PampaDevs.Utils.Helpers.DateTimeHelper;

namespace Aluguru.Marketplace.Register.Domain.Jwt
{
    public class JwtTokenBuilder
    {
        private List<Claim> _claims;
        private readonly User _user;

        public ClaimsIdentity IdentityClaims => new ClaimsIdentity(_claims);

        public JwtTokenBuilder(User user)
        {
            _claims = new List<Claim>();
            _user = user;
        }

        public JwtTokenBuilder WithUserClaims()
        {
            _claims.Add(new Claim(ClaimTypes.Role, _user.UserRole.Name));
            foreach (var userClaim in _user.UserRole.UserClaims)
            {
                _claims.Add(new Claim(userClaim.Type, userClaim.Value));
            }

            return this;
        }

        public JwtTokenBuilder WithJwtClaims()
        {
            _claims.Add(new Claim(JwtRegisteredClaimNames.Sub, _user.Id.ToString()));
            _claims.Add(new Claim("name", _user.FullName));
            _claims.Add(new Claim(JwtRegisteredClaimNames.Email, _user.Email));
            _claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            _claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(NewDateTime()).ToString()));
            _claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(NewDateTime()).ToString(), ClaimValueTypes.Integer64));

            return this;
        }

        public JwtTokenBuilder AddClaims(ICollection<Claim> claims)
        {
            _claims.AddRange(claims);

            return this;
        }

        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
