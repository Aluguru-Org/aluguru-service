using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Aluguru.Marketplace.Catalog.AutoMapper;
using Aluguru.Marketplace.Catalog.Usecases.CreateCategory;
using Aluguru.Marketplace.Catalog.Usecases.CreateProduct;
using Aluguru.Marketplace.Catalog.Usecases.DeleteCategory;
using Aluguru.Marketplace.Catalog.Usecases.GetCategories;
using Aluguru.Marketplace.Catalog.Usecases.GetProduct;
using Aluguru.Marketplace.Catalog.Usecases.GetProducts;
using Aluguru.Marketplace.Catalog.Usecases.GetProductsByCategory;
using Aluguru.Marketplace.Catalog.Usecases.UpdateCategory;
using Aluguru.Marketplace.Catalog.Usecases.UpdateProduct;
using Aluguru.Marketplace.Data;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.Handlers;
using Aluguru.Marketplace.Infrastructure.Data;
using Aluguru.Marketplace.Infrastructure.UnitOfWork;
using Aluguru.Marketplace.Register.AutoMapper;
using Aluguru.Marketplace.Register.Usecases.GetUserRoles;
using Aluguru.Marketplace.Register.Usecases.CreateUserRole;
using Aluguru.Marketplace.Register.Usecases.CreateUser;
using Aluguru.Marketplace.Register.Usecases.DeleteUser;
using Aluguru.Marketplace.Register.Usecases.LogInUser;
using System.Reflection;
using Aluguru.Marketplace.Register.Services;
using Aluguru.Marketplace.Register.Usecases.GetUsersByRole;
using Aluguru.Marketplace.Register.Usecases.UpadeUser;
using Aluguru.Marketplace.Register.Usecases.UpdateUserRole;
using Aluguru.Marketplace.Register.Usecases.DeleteUserRole;
using Aluguru.Marketplace.Catalog.Usecases.DeleteProduct;
using Aluguru.Marketplace.Register.Usecases.GetUserById;
using Aluguru.Marketplace.Rent.Usecases.GetOrders;
using Aluguru.Marketplace.Rent.Usecases.GetOrder;
using Stripe;
using Aluguru.Marketplace.Catalog.Usecases.CreateRentPeriod;
using Aluguru.Marketplace.Catalog.Usecases.GetRentPeriods;
using Aluguru.Marketplace.Catalog.Usecases.DeleteRentPeriod;
using Aluguru.Marketplace.Rent.Usecases.CreateOrder;
using Aluguru.Marketplace.Rent.Usecases.DeleteOrder;
using Aluguru.Marketplace.Rent.Usecases.RemoveVoucher;
using Aluguru.Marketplace.Rent.Usecases.UpdateOrder;
using Aluguru.Marketplace.Rent.Usecases.ApplyVoucher;
using Aluguru.Marketplace.Rent.Usecases.CreateVoucher;
using Aluguru.Marketplace.Rent.Usecases.DeleteVoucher;
using Aluguru.Marketplace.Rent.Usecases.GetVouchers;
using Aluguru.Marketplace.Rent.AutoMapper;
using Aluguru.Marketplace.Crosscutting.AzureStorage;
using Aluguru.Marketplace.Catalog.Usecases.AddProductImage;
using Aluguru.Marketplace.Crosscutting.Mailing;
using Aluguru.Marketplace.Register.Usecases.UpdateUserPassword;

namespace Aluguru.Marketplace.Crosscutting.IoC
{
    public static class NativeInjectorBootstrapper
    {
        public static IServiceCollection AddDataComponents(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceConnection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AluguruContext>(options => options.UseSqlServer(serviceConnection));

            services.AddScoped<IUnitOfWork, EfUnitOfWork<AluguruContext>>();

            return services;
        }

        public static IServiceCollection AddInMemoryDataComponents(this IServiceCollection services)
        {
            services.AddDbContext<AluguruContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"));

            services.AddScoped<IUnitOfWork, EfUnitOfWork<AluguruContext>>();

            return services;
        }

        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureStorageSettings>(configuration.GetSection("AzureStorageSettings"));
            services.Configure<MailingSettings>(configuration.GetSection("MailingSettings"));

            return services;
        }

        public static IServiceCollection AddServiceComponents(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddMediatR(assemblies);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandValidationHandler<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            services.AddAutoMapper(
                typeof(RegisterContextMappingConfiguration), 
                typeof(CatalogContextMappingConfiguration),
                typeof(RentContextMappingConfiguration));

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
            services.AddScoped<IRequestHandler<UpdateUserPasswordCommand, bool>, UpdateUserPasswordHandler>();
            services.AddScoped<IRequestHandler<DeleteUserCommand, bool>, DeleteUserHandler>();

            // Product Command Handlers
            services.AddScoped<IRequestHandler<CreateProductCommand, CreateProductCommandResponse>, CreateProductHandler>();
            services.AddScoped<IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>, UpdateProductHandler>();
            services.AddScoped<IRequestHandler<GetProductCommand, GetProductCommandResponse>, GetProductHandler>();
            services.AddScoped<IRequestHandler<GetProductsCommand, GetProductsCommandResponse>, GetProductsHandler>();
            services.AddScoped<IRequestHandler<DeleteProductCommand, bool>, DeleteProductHandler>();
            services.AddScoped<IRequestHandler<AddProductImageCommand, AddProductImageCommandResponse>, AddProductImageHandler>();

            // Category Command Handlers
            services.AddScoped<IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponse>, CreateCategoryHandler>();
            services.AddScoped<IRequestHandler<UpdateCategoryCommand, UpdateCategoryCommandResponse>, UpdateCategoryHandler>();
            services.AddScoped<IRequestHandler<GetCategoriesCommand, GetCategoriesCommandResponse>, GetCategoriesCommandHandler>();
            services.AddScoped<IRequestHandler<GetProductsByCategoryCommand, GetProductsByCategoryCommandResponse>, GetProductsByCategoryHandler>();
            services.AddScoped<IRequestHandler<DeleteCategoryCommand, bool>, DeleteCategoryHandler>();

            // Rent Period Command Handlers
            services.AddScoped<IRequestHandler<CreateRentPeriodCommand, CreateRentPeriodCommandResponse>, CreateRentPeriodHandler>();
            services.AddScoped<IRequestHandler<GetRentPeriodsCommand, GetRentPeriodsCommandResponse>, GetRentPeriodsCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteRentPeriodCommand, bool>, DeleteRentPeriodHandler>();

            // Order Command Handlers
            services.AddScoped<IRequestHandler<GetOrdersCommand, GetOrdersCommandResponse>, GetOrdersHandler>();
            services.AddScoped<IRequestHandler<GetOrderCommand, GetOrderCommandResponse>, GetOrderHandler>();
            services.AddScoped<IRequestHandler<CreateOrderCommand, CreateOrderCommandResponse>, CreateOrderHandler>();
            services.AddScoped<IRequestHandler<UpdateOrderCommand, UpdateOrderCommandResponse>, UpdateOrderHandler>();
            services.AddScoped<IRequestHandler<ApplyVoucherCommand, ApplyVoucherCommandResponse>, ApplyVoucherHandler>();
            services.AddScoped<IRequestHandler<RemoveVoucherCommand, DeleteVoucherCommandResponse>, RemoveVoucherHandler>();
            services.AddScoped<IRequestHandler<DeleteOrderCommand, bool>, DeleteOrderHandler>();

            // Voucher Command Handlers
            services.AddScoped<IRequestHandler<GetVouchersCommand, GetVouchersCommandResponse>, GetVouchersHandler>();
            services.AddScoped<IRequestHandler<CreateVoucherCommand, CreateVoucherCommandResponse>, CreateVoucherHandler>();
            services.AddScoped<IRequestHandler<DeleteVoucherCommand, bool>, DeleteVoucherHandler>();

            // Payment Services
            services.AddScoped<PaymentIntentService>();

            // CrossCutting
            services.AddScoped<IAzureStorageGateway, AzureStorageGateway>();
            services.AddScoped<IMailingService, MailingService>();

            return services;
        }
    }
}
