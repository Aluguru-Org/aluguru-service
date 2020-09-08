using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Security
{
    public static class Policies
    {
        public const string AllowSpecificOrigins = nameof(AllowSpecificOrigins);
        public const string NotAnonymous = nameof(NotAnonymous);

        #region Catalog Context
        public const string CategoryReader = nameof(CategoryReader);
        public const string CategoryWriter = nameof(CategoryWriter);

        public const string ProductReader = nameof(ProductReader);
        public const string ProductWriter = nameof(ProductWriter);

        public const string RentPeriodReader = nameof(RentPeriodReader);
        public const string RentPeriodWriter = nameof(RentPeriodWriter);
        #endregion

        #region Register Context
        public const string UserReader = nameof(UserReader);
        public const string UserWriter = nameof(UserWriter);

        public const string UserRoleReader = nameof(UserRoleReader);
        public const string UserRoleWriter = nameof(UserRoleWriter);
        #endregion

        #region Order Context
        public const string OrderReader = nameof(OrderReader);
        public const string OrderWriter = nameof(OrderWriter);

        public const string VoucherReader = nameof(VoucherReader);
        public const string VoucherWriter = nameof(VoucherWriter);
        #endregion
    }
}
