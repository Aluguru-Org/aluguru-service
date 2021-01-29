namespace Aluguru.Marketplace.Register.Domain
{
    public static class AluguruRoles
    {
        public const string User = nameof(User);
        public const string Company = nameof(Company);
        public const string Admin = nameof(Admin);

        public static bool Contains(string role)
        {
            return role == User
                || role == Company
                || role == Admin;
        }
    }
}
