using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aluguru.Marketplace.Register.Data.Seed
{
    public static class SeedRegisterAbstractions
    {
        public static IApplicationBuilder SeedRegisterContext(this IApplicationBuilder app)
        {
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var unitOfWork = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var userRoleQueryRepository = unitOfWork.QueryRepository<UserRole>();                

                var userQueryRepository = unitOfWork.QueryRepository<User>();

                List<UserClaim> adminClaims = new List<UserClaim>()
                {
                    new UserClaim(Claims.Product, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Product, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.Category, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Category, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.RentPeriod, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.RentPeriod, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.User, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.User, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.UserRole, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.UserRole, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.Order, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Order, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.Voucher, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Voucher, ClaimValues.Write.ToString())
                };
                UserRole adminRole = CreateRole("Admin", adminClaims, unitOfWork, userRoleQueryRepository);

                List<UserClaim> companyClaims = new List<UserClaim>()
                {
                    new UserClaim(Claims.Product, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Product, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.Category, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.RentPeriod, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.User, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.User, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.Order, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Voucher, ClaimValues.Read.ToString())
                };
                UserRole companyRole = CreateRole("Company", companyClaims, unitOfWork, userRoleQueryRepository);

                List<UserClaim> userClaims = new List<UserClaim>()
                {
                    new UserClaim(Claims.Product, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Category, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.RentPeriod, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.User, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.User, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.Order, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Order, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.Voucher, ClaimValues.Read.ToString())
                };
                UserRole userRole = CreateRole("User", userClaims, unitOfWork, userRoleQueryRepository);
                
                var admin = userQueryRepository.FindOneAsync(x => x.Email == "admin@aluguru.com.br").Result;

                if (admin == null)
                {
                    var userRepository = unitOfWork.Repository<User>();

                    admin = new User(Guid.Parse("96d1fb97-47e9-4ad5-b07e-448f88defd9c"), "admin@aluguru.com.br", "24/74c0ZcIuP++wd2rtW88mWx3EOc1JFX66v634WEQE=", "Aluguru Admin", adminRole.Id);

                    admin.Activate();

                    admin.Document = new Document("11111111111111", EDocumentType.CNPJ);
                    admin.Address = new Address(Guid.Parse("96d1fb97-47e9-4ad5-b07e-448f88defd9c"), "some-street", "some-number", "some-neighborhood", "some-city", "some-state", "some-country", "some-zipcode");

                    userRepository.Add(admin);
                }

                unitOfWork.SaveChanges();
            }

            return app;
        }

        private static UserRole CreateRole(string roleName, List<UserClaim> userClaims, IUnitOfWork unitOfWork, IQueryRepository<UserRole> userRoleQueryRepository)
        {
            var role = userRoleQueryRepository.FindOneAsync(x => x.Name == roleName).Result;

            if (role == null)
            {
                var userRoleRepository = unitOfWork.Repository<UserRole>();

                role = new UserRole(roleName);

                role.AddClaims(userClaims);

                userRoleRepository.Add(role);
            }

            return role;
        }
    }
}
