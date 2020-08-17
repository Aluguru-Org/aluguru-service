using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Catalog.Domain;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Catalog.Data.Repositories
{
    public static class RentPeriodRepositoryExtensions
    {
        public static async Task<RentPeriod> GetRentPeriodAsync(this IQueryRepository<RentPeriod> repository, Guid rentPeriodId, bool disableTracking = true)
        {
            var rentPeriod = await repository.GetByIdAsync(
                rentPeriodId,
                null,
                disableTracking);

            return rentPeriod;
        }

        public static async Task<RentPeriod> GetRentPeriodByNameAsync(this IQueryRepository<RentPeriod> repository, string name, bool disableTracking = true)
        {
            var rentPeriod = await repository.FindOneAsync(
                x => x.Name == name,
                null,
                disableTracking);

            return rentPeriod;
        }

        public static async Task<List<RentPeriod>> GetRentPeriodsAsync(this IQueryRepository<RentPeriod> repository, bool disableTracking = true)
        {
            var rentPeriods = await repository.ListAsync(null, null, disableTracking);

            return rentPeriods.ToList();
        }
    }
}
