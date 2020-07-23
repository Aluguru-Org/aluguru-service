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

namespace Mubbi.Marketplace.Crosscutting.IoC
{
    public static class NativeInjectorBootstrapper
    {
        public static IServiceCollection AddServiceComponents(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
        {
            services.AddDbContext<MubbiContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, EfUnitOfWork<MubbiContext>>();

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
            services.AddScoped<IRequestHandler<CreateUserRoleCommand, CreateUserRoleCommandResponse>, CreateRoleHandler>();
            services.AddScoped<IRequestHandler<GetUserRolesCommand, GetUserRolesCommandResponse>, GetUserRolesHandler>();
            services.AddScoped<IRequestHandler<GetUsersByRoleCommand, GetUsersByRoleCommandResponse>, GetUsersByRoleHandler>();

            // User Command Handlers
            services.AddScoped<IRequestHandler<LogInUserCommand, LogInUserCommandResponse>, LogInUserHandler>();
            services.AddScoped<IRequestHandler<CreateUserCommand, CreateUserCommandResponse>, CreateUserHandler>();
            services.AddScoped<IRequestHandler<DeleteUserCommand, bool>, DeleteUserHandler>();

            // Product Command Handlers
            services.AddScoped<IRequestHandler<CreateProductCommand, CreateProductCommandResponse>, CreateProductHandler>();
            services.AddScoped<IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>, UpdateProductHandler>();
            services.AddScoped<IRequestHandler<GetProductCommand, GetProductCommandResponse>, GetProductHandler>();
            services.AddScoped<IRequestHandler<GetProductsCommand, GetProductsCommandResponse>, GetProductsHandler>();

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
