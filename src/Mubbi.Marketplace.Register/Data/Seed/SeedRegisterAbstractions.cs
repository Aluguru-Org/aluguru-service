using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Data;
using Mubbi.Marketplace.Register.Domain;
using Mubbi.Marketplace.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Register.Data.Seed
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

                var adminRole = userRoleQueryRepository.FindOneAsync(x => x.Name == "Admin").Result;

                if (adminRole == null)
                {
                    var userRoleRepository = unitOfWork.Repository<UserRole>();

                    adminRole = new UserRole("Admin");

                    userRoleRepository.Add(adminRole);
                }

                var companyRole = userRoleQueryRepository.FindOneAsync(x => x.Name == "Company").Result;

                if (companyRole == null)
                {
                    var userRoleRepository = unitOfWork.Repository<UserRole>();

                    companyRole = new UserRole("Company");

                    userRoleRepository.Add(companyRole);
                }

                var userRole = userRoleQueryRepository.FindOneAsync(x => x.Name == "User").Result;

                if (userRole == null)
                {
                    var userRoleRepository = unitOfWork.Repository<UserRole>();

                    userRole = new UserRole("User");

                    userRoleRepository.Add(userRole);
                }

                var admin = userQueryRepository.FindOneAsync(x => x.Email == "contato@mubbi.com.br").Result;

                if (admin == null)
                {
                    var userRepository = unitOfWork.Repository<User>();

                    admin = new User(Guid.Parse("96d1fb97-47e9-4ad5-b07e-448f88defd9c"), "contato@mubbi.com.br", "24/74c0ZcIuP++wd2rtW88mWx3EOc1JFX66v634WEQE=", "Mubbi Admin", adminRole.Id);

                    userRepository.Add(admin);
                }

                unitOfWork.SaveChanges();
            }            

            return app;
        }


    }
}
