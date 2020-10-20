using System.Collections.Generic;

namespace Aluguru.Marketplace.Security
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public int Expiration { get; set; }
        public string Issuer { get; set; }
        public string[] Issuers { get; set; }
        public string Audience { get; set; }
        public string[] Audiences { get; set; }
    }
}
