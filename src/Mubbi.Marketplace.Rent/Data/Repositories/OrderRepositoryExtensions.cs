using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Data;
using Mubbi.Marketplace.Rent.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Rent.Data.Repositories
{
    public static class OrderRepositoryExtensions
    {
        public static async Task<Order> GetOrderAsync(this IQueryRepository<Order> repository, Guid orderId, bool disableTracking = true)
        {
            var order = await repository.GetByIdAsync(
                orderId,
                order => order.Include(x => x.OrderItems).Include(x => x.Voucher),
                disableTracking);

            return order;
        }
    }
}
