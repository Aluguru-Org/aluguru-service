using Microsoft.EntityFrameworkCore;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using Aluguru.Marketplace.Rent.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.Rent.Data.Repositories
{
    public static class VoucherRepositoryExtensions
    {
        public static async Task<Voucher> GetVoucherAsync(this IQueryRepository<Voucher> repository, string code, bool disableTracking = true)
        {
            var voucher = await repository.FindOneAsync(
                voucher => voucher.Code == code,
                voucher => voucher.Include(x => x.Orders),
                disableTracking);

            return voucher;
        }

        public static async Task<List<Voucher>> GetVouchersAsync(this IQueryRepository<Voucher> repository, bool disableTracking = true)
        {
            var queryable = repository.Queryable();

            if (disableTracking) queryable = queryable.AsNoTracking();

            var categories = await queryable.ToListAsync();

            return categories;
        }
    }
}
