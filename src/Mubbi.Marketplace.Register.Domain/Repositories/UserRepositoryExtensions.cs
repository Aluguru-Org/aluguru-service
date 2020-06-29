using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Register.Domain.Models;
using Mubbi.Marketplace.Infrastructure.Data;
using System;
using System.Threading.Tasks;
using PampaDevs.Utils;

namespace Mubbi.Marketplace.Register.Domain.Repositories
{
    public static class UserRepositoryExtensions
    {
        public static async Task<User> GetUserAsync(this IQueryRepository<User> repository, Guid userId, bool disableTracking = true)
        {
            var user = await repository.GetByIdAsync(userId);

            return user;
        }
    }
}
