using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Data;
using Aluguru.Marketplace.Register.Domain;
using Aluguru.Marketplace.Security;
using System;
using System.Collections.Generic;
using System.Text;
using Aluguru.Marketplace.Register.Data.Repositories;

namespace Aluguru.Marketplace.Register.Data.Seed
{
    public static class SeedRegisterAbstractions
    {
        public static IApplicationBuilder SeedRegisterContext(this IApplicationBuilder app)
        {
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var cryptography = serviceScope.ServiceProvider.GetRequiredService<ICryptography>();
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
                    new UserClaim(Claims.Voucher, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.Payment, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Payment, ClaimValues.Write.ToString())
                };
                UserRole adminRole = CreateRole("Admin", adminClaims, unitOfWork, userRoleQueryRepository);

                unitOfWork.SaveChanges();

                List<UserClaim> companyClaims = new List<UserClaim>()
                {
                    new UserClaim(Claims.Product, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Product, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.Category, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.RentPeriod, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.User, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.User, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.Order, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Voucher, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Payment, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Payment, ClaimValues.Write.ToString())
                };
                UserRole companyRole = CreateRole("Company", companyClaims, unitOfWork, userRoleQueryRepository);

                unitOfWork.SaveChanges();

                List<UserClaim> userClaims = new List<UserClaim>()
                {
                    new UserClaim(Claims.Product, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Category, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.RentPeriod, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.User, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.User, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.Order, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Order, ClaimValues.Write.ToString()),
                    new UserClaim(Claims.Voucher, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Payment, ClaimValues.Read.ToString()),
                    new UserClaim(Claims.Payment, ClaimValues.Write.ToString())
                };
                UserRole userRole = CreateRole("User", userClaims, unitOfWork, userRoleQueryRepository);
                
                unitOfWork.SaveChanges();

                var admin = userQueryRepository.FindOneAsync(x => x.Email == "admin@aluguru.com.br").Result;

                if (admin == null)
                {
                    var userRepository = unitOfWork.Repository<User>();
                    
                    var activationHash = cryptography.CreateRandomHash();
                    admin = new User(Guid.Parse("96d1fb97-47e9-4ad5-b07e-448f88defd9c"), "admin@aluguru.com.br", cryptography.Encrypt("really"), "Aluguru Admin", adminRole.Id, activationHash);

                    admin.Activate(activationHash);

                    admin.UpdateDocument(new Document("11111111111111", EDocumentType.CNPJ));
                    admin.UpdateContact(new Contact("Felipe", "51983468863", "admin@aluguru.com.br"));
                    admin.UpdateAddress(new Address("General Lima e Silva", "480", "Centro Histórico", "Porto Alegre", "RS", "Brasil", "some-zipcode", "ap 02"));

                    userRepository.Add(admin);
                }

                unitOfWork.SaveChanges();
            }

            return app;
        }

        private static UserRole CreateRole(string roleName, List<UserClaim> userClaims, IUnitOfWork unitOfWork, IQueryRepository<UserRole> userRoleQueryRepository)
        {
            var role = userRoleQueryRepository.GetUserRoleAsync(roleName, false).Result;
            var userRoleRepository = unitOfWork.Repository<UserRole>();

            if (role == null)
            {
                role = new UserRole(roleName);

                role.AddClaims(userClaims);

                userRoleRepository.Add(role);
            }
            else
            {
                role.AddClaims(userClaims);

                userRoleRepository.Update(role);
            }

            return role;
        }
    }
}
