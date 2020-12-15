namespace Aluguru.Marketplace.Security
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public int Expiration { get; set; }
        public string Issuer { get; set; }
        public string ClientAudience { get; set; }
        public string BackofficeAudience { get; set; }
        public string[] Audiences { get; set; }
    }
}
