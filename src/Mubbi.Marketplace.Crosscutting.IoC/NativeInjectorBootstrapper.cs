using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mubbi.Marketplace.Catalog.AutoMapper;
using Mubbi.Marketplace.Catalog.Usecases.CreateCategory;
using Mubbi.Marketplace.Catalog.Usecases.CreateProduct;
using Mubbi.Marketplace.Catalog.Usecases.DeleteCategory;
using Mubbi.Marketplace.Catalog.Usecases.GetCategories;
using Mubbi.Marketplace.Catalog.Usecases.GetProduct;
using Mubbi.Marketplace.Catalog.Usecases.GetProducts;
using Mubbi.Marketplace.Catalog.Usecases.GetProductsByCategory;
using Mubbi.Marketplace.Catalog.Usecases.UpdateCategory;
using Mubbi.Marketplace.Catalog.Usecases.UpdateProduct;
using Mubbi.Marketplace.Data;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.Handlers;
using Mubbi.Marketplace.Infrastructure.Data;
using Mubbi.Marketplace.Infrastructure.UnitOfWork;
using Mubbi.Marketplace.Register.AutoMapper;
using Mubbi.Marketplace.Register.Usecases.GetUserRoles;
using Mubbi.Marketplace.Register.Usecases.CreateUserRole;
using Mubbi.Marketplace.Register.Usecases.CreateUser;
using Mubbi.Marketplace.Register.Usecases.DeleteUser;
using Mubbi.Marketplace.Register.Usecases.LogInUser;
using System.Reflection;
using Mubbi.Marketplace.Register.Services;
using Mubbi.Marketplace.Register.Usecases.GetUsersByRole;
using Mubbi.Marketplace.Register.Usecases.UpadeUser;
using Mubbi.Marketplace.Register.Usecases.UpdateUserRole;
using Mubbi.Marketplace.Register.Usecases.DeleteUserRole;
using Mubbi.Marketplace.Catalog.Usecases.DeleteProduct;
using Mubbi.Marketplace.Register.Usecases.GetUserById;

namespace Mubbi.Marketplace.Crosscutting.IoC
{
    public static class NativeInjectorBootstrapper
    {
        public static IServiceCollection AddDataComponents(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceConnection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MubbiContext>(options => options.UseSqlServer(serviceConnection));

            services.AddScoped<IUnitOfWork, EfUnitOfWork<MubbiContext>>();

            return services;
        }

        public static IServiceCollection AddInMemoryDataComponents(this IServiceCollection services)
        {
            services.AddDbContext<MubbiContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"));

            services.AddScoped<IUnitOfWork, EfUnitOfWork<MubbiContext>>();

            return services;
        }

        public static IServiceCollection AddServiceComponents(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddMediatR(assemblies);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandValidationHandler<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            services.AddAutoMapper(typeof(RegisterContextMappingConfiguration), typeof(CatalogContextMappingConfiguration));

            // AuthUser
            services.AddScoped<IRequestHandler<LogInUserCommand, LogInUserCommandResponse>, LogInUserHandler>();
            services.AddScoped<ITokenBuilderService, TokenBuilderService>();

            // User Role Command Handlers
            services.AddScoped<IRequestHandler<CreateUserRoleCommand, CreateUserRoleCommandResponse>, CreateUserRoleHandler>();
            services.AddScoped<IRequestHandler<GetUserRolesCommand, GetUserRolesCommandResponse>, GetUserRolesHandler>();
            services.AddScoped<IRequestHandler<GetUsersByRoleCommand, GetUsersByRoleCommandResponse>, GetUsersByRoleHandler>();
            services.AddScoped<IRequestHandler<UpdateUserRoleCommand, UpdateUserRoleCommandResponse>, UpdateUserRoleHandler>();
            services.AddScoped<IRequestHandler<DeleteUserRoleCommand, bool>, DeleteUserRoleHandler>();

            // User Command Handlers
            services.AddScoped<IRequestHandler<GetUserByIdCommand, GetUserByIdCommandResponse>, GetUserByIdHandler>();
            services.AddScoped<IRequestHandler<CreateUserCommand, CreateUserCommandResponse>, CreateUserHandler>();
            services.AddScoped<IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse>, UpdateUserHandler>();
            services.AddScoped<IRequestHandler<DeleteUserCommand, bool>, DeleteUserHandler>();

            // Product Command Handlers
            services.AddScoped<IRequestHandler<CreateProductCommand, CreateProductCommandResponse>, CreateProductHandler>();
            services.AddScoped<IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>, UpdateProductHandler>();
            services.AddScoped<IRequestHandler<GetProductCommand, GetProductCommandResponse>, GetProductHandler>();
            services.AddScoped<IRequestHandler<GetProductsCommand, GetProductsCommandResponse>, GetProductsHandler>();
            services.AddScoped<IRequestHandler<DeleteProductCommand, bool>, DeleteProductHandler>();

            // Category Command Handlers
            services.AddScoped<IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponse>, CreateCategoryHandler>();
            services.AddScoped<IRequestHandler<UpdateCategoryCommand, UpdateCategoryCommandResponse>, UpdateCategoryHandler>();
            services.AddScoped<IRequestHandler<GetCategoriesCommand, GetCategoriesCommandResponse>, GetCategoriesCommandHandler>();
            services.AddScoped<IRequestHandler<GetProductsByCategoryCommand, GetProductsByCategoryCommandResponse>, GetProductsByCategoryHandler>();
            services.AddScoped<IRequestHandler<DeleteCategoryCommand, bool>, DeleteCategoryHandler>();

            return services;
        }
    }
}
