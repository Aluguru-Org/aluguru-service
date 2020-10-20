using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Security
{
    public static class Claims
    {
        public const string Category = nameof(Category);
        public const string Product = nameof(Product);
        public const string RentPeriod = nameof(RentPeriod);

        public const string User = nameof(User);
        public const string UserRole = nameof(UserRole);

        public const string Order = nameof(Order);
        public const string Voucher = nameof(Voucher);
    }
}
