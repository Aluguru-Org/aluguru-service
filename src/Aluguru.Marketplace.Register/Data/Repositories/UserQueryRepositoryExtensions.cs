using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Aluguru.Marketplace.Register.Domain.Repositories
{
    public static class UserQueryRepositoryExtensions
    {
        public static async Task<User> GetUserAsync(this IQueryRepository<User> repository, Guid userId, bool disableTracking = true)
        {
            var user = await repository.GetByIdAsync(
                userId,
                x => x.Include(x => x.Contact).Include(x => x.Document).Include(x => x.UserRole).Include(x => x.Address),
                disableTracking);

            return user;
        }

        public static async Task<User> GetUserByEmailAsync(this IQueryRepository<User> repository, string email, bool disableTracking = true)
        {
            var user = await repository.FindOneAsync(
                x => x.Email == email,
                x => x.Include(x => x.UserRole).ThenInclude(x => x.UserClaims).Include(x => x.Address),
                disableTracking);

            return user;
        }
    }
}
