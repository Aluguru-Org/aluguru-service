using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Data;
using System;
using System.Threading.Tasks;
using PampaDevs.Utils;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Mubbi.Marketplace.Register.Domain.Repositories
{
    public static class UserQueryRepositoryExtensions
    {
        public static async Task<User> GetUserAsync(this IQueryRepository<User> repository, Guid userId, bool disableTracking = true)
        {
            var user = await repository.GetByIdAsync(userId);
            return user;
        }

        public static async Task<User> GetUserByEmailAsync(this IQueryRepository<User> repository, string email, bool disableTracking = true)
        {
            var user = await repository.Queryable()
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();

            return user;
        }

        public static async Task<List<User>> GetUsersByRoleAsync(this IQueryRepository<User> repository, string role, bool disableTracking = true)
        {
            var users = await repository.Queryable()
                .Where(x => x.UserRole.Name == role)
                .ToListAsync();

            return users;
        }
    }
}
