using Microsoft.EntityFrameworkCore;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Data;
using Mubbi.Marketplace.Register.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Register.Data.Repositories
{
    public static class UserRoleQueryRepositoryExtensions
    {
        public static async Task<UserRole> GetUserRoleAsync(this IQueryRepository<UserRole> repository, Guid userRoleId, bool disableTracking = true)
        {
            var userRole = await repository.GetByIdAsync(
                userRoleId,
                x => x.Include(x => x.UserClaims),
                !disableTracking);

            return userRole;
        }
        public static async Task<List<User>> GetUsersByRoleAsync(this IQueryRepository<UserRole> repository, string roleName, bool disableTracking = true)
        {
            var userRole = await repository.FindOneAsync(
                x => x.Name == roleName,
                x => x.Include(x => x.Users),
                !disableTracking);

            return userRole.Users.ToList();
        }
    }
}
