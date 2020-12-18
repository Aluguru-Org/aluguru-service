using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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
using Aluguru.Marketplace.Register.Usecases.LogInBackofficeClient;
using Aluguru.Marketplace.Register.Services;
using Aluguru.Marketplace.Register.Usecases.GetUsersByRole;
using Aluguru.Marketplace.Register.Usecases.UpadeUserName;
using Aluguru.Marketplace.Register.Usecases.UpdateUserRole;
using Aluguru.Marketplace.Register.Usecases.DeleteUserRole;
using Aluguru.Marketplace.Catalog.Usecases.DeleteProduct;
using Aluguru.Marketplace.Register.Usecases.GetUserById;
using Aluguru.Marketplace.Rent.Usecases.GetOrders;
using Aluguru.Marketplace.Rent.Usecases.GetOrder;
using Aluguru.Marketplace.Catalog.Usecases.CreateRentPeriod;
using Aluguru.Marketplace.Catalog.Usecases.GetRentPeriods;
using Aluguru.Marketplace.Catalog.Usecases.DeleteRentPeriod;
using Aluguru.Marketplace.Rent.Usecases.CreateOrder;
using Aluguru.Marketplace.Rent.Usecases.DeleteOrder;
using Aluguru.Marketplace.Rent.Usecases.RemoveVoucher;
using Aluguru.Marketplace.Rent.Usecases.AddOrderItem;
using Aluguru.Marketplace.Rent.Usecases.ApplyVoucher;
using Aluguru.Marketplace.Rent.Usecases.CreateVoucher;
using Aluguru.Marketplace.Rent.Usecases.DeleteVoucher;
using Aluguru.Marketplace.Rent.Usecases.GetVouchers;
using Aluguru.Marketplace.Rent.AutoMapper;
using Aluguru.Marketplace.Crosscutting.AzureStorage;
using Aluguru.Marketplace.Catalog.Usecases.AddProductImage;
using Aluguru.Marketplace.Crosscutting.Mailing;
using Aluguru.Marketplace.Register.Usecases.UpdateUserPassword;
using Aluguru.Marketplace.Communication.IntegrationEvents;
using Aluguru.Marketplace.Notification.Handlers;
using Aluguru.Marketplace.Register.Usecases.ActivateUser;
using Aluguru.Marketplace.Notification.Settings;
using Aluguru.Marketplace.Crosscutting.Iugu;
using Aluguru.Marketplace.Catalog.Usecases.DeleteProductImage;
using Aluguru.Marketplace.Catalog.Usecases.AddCategoryImage;
using Aluguru.Marketplace.Catalog.Usecases.DeleteCategoryImage;
using Aluguru.Marketplace.Notification.Usecases.SendAccountActivationEmail;
using Aluguru.Marketplace.Newsletter.AutoMapper;
using Aluguru.Marketplace.Newsletter.Services;
using Aluguru.Marketplace.Payment.Usecases.PayOrder;
using Aluguru.Marketplace.Payment.AutoMapper;
using Aluguru.Marketplace.Payment.Usecases.GetPayment;
using Aluguru.Marketplace.Rent.Usecases.StartOrder;
using Aluguru.Marketplace.Catalog.Handlers;
using Aluguru.Marketplace.Catalog.Usecases.DebitProductStock;
using Aluguru.Marketplace.Rent.Handler;
using Aluguru.Marketplace.Rent.Usecases.CancelOrderProcessing;
using Aluguru.Marketplace.Payment.Usecases.UpdateInvoiceStatus;
using Aluguru.Marketplace.Rent.Usecases.ConfirmOrderPayment;
using Aluguru.Marketplace.Rent.Usecases.RemoveOrderItem;
using Aluguru.Marketplace.Rent.Usecases.UpdateOrderItemAmount;
using Aluguru.Marketplace.Register.Usecases.UpadeUserAddress;
using Aluguru.Marketplace.Register.Usecases.UpadeUserDocument;
using Aluguru.Marketplace.Register.Usecases.UpadeUserContact;
using Aluguru.Marketplace.Crosscutting.Google;
using Aluguru.Marketplace.Rent.Usecases.CalculateOrderFreigth;
using Aluguru.Marketplace.Crosscutting.Viacep;
using Aluguru.Marketplace.Rent.Usecases.OrderPreview;
using Aluguru.Marketplace.Register.Usecases.LogInClient;
using Aluguru.Marketplace.Notification.Usecases.SendOrderPaymentConfirmedEmail;
using Aluguru.Marketplace.Notification.Usecases.SendOrderStartedEmail;

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
            services.Configure<NotificationSettings>(configuration.GetSection("NotificationSettings"));
            services.Configure<IuguSettings>(configuration.GetSection("IuguSettings"));
            services.Configure<GoogleSettings>(configuration.GetSection("GoogleSettings"));
            services.Configure<ViacepSettings>(configuration.GetSection("ViacepSettings"));

            return services;
        }

        public static IServiceCollection AddServiceComponents(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddHttpClient();
            services.AddMediatR(assemblies);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandValidationHandler<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            services.AddAutoMapper(
                typeof(RegisterContextMappingConfiguration), 
                typeof(CatalogContextMappingConfiguration),
                typeof(RentContextMappingConfiguration),
                typeof(NewsletterContextMappingConfiguration),
                typeof(PaymentContextMappingConfiguration)
            );

            //
            // ===================== Newsletter Context =====================
            //
            services.AddScoped<INewsletterService, NewsletterService>();

            //
            // ===================== Register Context =====================
            //

            // AuthUser
            services.AddScoped<IRequestHandler<LogInUserBackofficeCommand, LogInUserBackofficeCommandResponse>, LogInUserBackofficeHandler>();
            services.AddScoped<IRequestHandler<LogInUserClientCommand, LogInUserClientCommandResponse>, LogInUserClientHandler>();
            services.AddScoped<ITokenBuilderService, TokenBuilderService>();

            // User Role
            services.AddScoped<IRequestHandler<CreateUserRoleCommand, CreateUserRoleCommandResponse>, CreateUserRoleHandler>();
            services.AddScoped<IRequestHandler<GetUserRolesCommand, GetUserRolesCommandResponse>, GetUserRolesHandler>();
            services.AddScoped<IRequestHandler<GetUsersByRoleCommand, GetUsersByRoleCommandResponse>, GetUsersByRoleHandler>();
            services.AddScoped<IRequestHandler<UpdateUserRoleCommand, UpdateUserRoleCommandResponse>, UpdateUserRoleHandler>();
            services.AddScoped<IRequestHandler<DeleteUserRoleCommand, bool>, DeleteUserRoleHandler>();

            // User
            services.AddScoped<IRequestHandler<GetUserByIdCommand, GetUserByIdCommandResponse>, GetUserByIdHandler>();
            services.AddScoped<IRequestHandler<CreateUserCommand, CreateUserCommandResponse>, CreateUserHandler>();
            services.AddScoped<IRequestHandler<UpdateUserNameCommand, bool>, UpdateUserNameHandler>();
            services.AddScoped<IRequestHandler<UpdateUserAddressCommand, bool>, UpdateUserAddressHandler>();
            services.AddScoped<IRequestHandler<UpdateUserDocumentCommand, bool>, UpdateUserDocumentHandler>();
            services.AddScoped<IRequestHandler<UpdateUserContactCommand, bool>, UpdateUserContactHandler>();
            services.AddScoped<IRequestHandler<UpdateUserPasswordCommand, bool>, UpdateUserPasswordHandler>();
            services.AddScoped<IRequestHandler<ActivateUserCommand, bool>, ActivateUserHandler>();
            services.AddScoped<IRequestHandler<DeleteUserCommand, bool>, DeleteUserHandler>();

            //
            // ===================== Catalog Context =====================
            //

            // Product
            services.AddScoped<INotificationHandler<OrderStartedEvent>, OrderStartedHandler>();
            services.AddScoped<IRequestHandler<CreateProductCommand, CreateProductCommandResponse>, CreateProductHandler>();
            services.AddScoped<IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>, UpdateProductHandler>();
            services.AddScoped<IRequestHandler<GetProductCommand, GetProductCommandResponse>, GetProductHandler>();
            services.AddScoped<IRequestHandler<GetProductsCommand, GetProductsCommandResponse>, GetProductsHandler>();
            services.AddScoped<IRequestHandler<DeleteProductCommand, bool>, DeleteProductHandler>();
            services.AddScoped<IRequestHandler<DebitProductStockCommand, bool>, DebitProductStockHandler>();
            services.AddScoped<IRequestHandler<AddProductImageCommand, AddProductImageCommandResponse>, AddProductImageHandler>();
            services.AddScoped<IRequestHandler<DeleteProductImageCommand, DeleteProductImageCommandResponse>, DeleteProductImageHandler>();

            // Category
            services.AddScoped<IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponse>, CreateCategoryHandler>();
            services.AddScoped<IRequestHandler<UpdateCategoryCommand, UpdateCategoryCommandResponse>, UpdateCategoryHandler>();
            services.AddScoped<IRequestHandler<GetCategoriesCommand, GetCategoriesCommandResponse>, GetCategoriesCommandHandler>();
            services.AddScoped<IRequestHandler<GetProductsByCategoryCommand, GetProductsByCategoryCommandResponse>, GetProductsByCategoryHandler>();
            services.AddScoped<IRequestHandler<DeleteCategoryCommand, bool>, DeleteCategoryHandler>();
            services.AddScoped<IRequestHandler<UpdateCategoryImageCommand, UpdateCategoryImageCommandResponse>, UpdateCategoryImageHandler>();
            services.AddScoped<IRequestHandler<DeleteCategoryImageCommand, DeleteCategoryImageCommandResponse>, DeleteCategoryImageHandler>();

            // Rent Period
            services.AddScoped<IRequestHandler<CreateRentPeriodCommand, CreateRentPeriodCommandResponse>, CreateRentPeriodHandler>();
            services.AddScoped<IRequestHandler<GetRentPeriodsCommand, GetRentPeriodsCommandResponse>, GetRentPeriodsCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteRentPeriodCommand, bool>, DeleteRentPeriodHandler>();

            //
            // ===================== Rent Context =====================
            //

            // Order 
            services.AddScoped<INotificationHandler<OrderStockRejectedEvent>, OrderEventHandler>();
            services.AddScoped<INotificationHandler<OrderPaidEvent>, OrderEventHandler>();
            services.AddScoped<IRequestHandler<GetOrdersCommand, GetOrdersCommandResponse>, GetOrdersHandler>();
            services.AddScoped<IRequestHandler<GetOrderCommand, GetOrderCommandResponse>, GetOrderHandler>();
            services.AddScoped<IRequestHandler<CreateOrderCommand, CreateOrderCommandResponse>, CreateOrderHandler>();
            services.AddScoped<IRequestHandler<StartOrderCommand, StartOrderCommandResponse>, StartOrderHandler>();
            services.AddScoped<IRequestHandler<CancelOrderProcessingCommand, bool>, CancelOrderProcessingHandler>();
            services.AddScoped<IRequestHandler<AddOrderItemCommand, AddOrderItemCommandResponse>, AddOrderItemHandler>();
            services.AddScoped<IRequestHandler<CalculateOrderFreigthCommand, CalculateOrderFreigthCommandResponse>, CalculateOrderFreigthHandler>();
            services.AddScoped<IRequestHandler<UpdateOrderItemAmountCommand, UpdateOrderItemAmountCommandResponse>, UpdateOrderItemAmountHandler>();
            services.AddScoped<IRequestHandler<RemoveOrderItemCommand, RemoveOrderItemCommandResponse>, RemoveOrderItemHandler>();
            services.AddScoped<IRequestHandler<ApplyVoucherCommand, ApplyVoucherCommandResponse>, ApplyVoucherHandler>();
            services.AddScoped<IRequestHandler<RemoveVoucherCommand, DeleteVoucherCommandResponse>, RemoveVoucherHandler>();
            services.AddScoped<IRequestHandler<DeleteOrderCommand, bool>, DeleteOrderHandler>();
            services.AddScoped<IRequestHandler<ConfirmOrderPaymentCommand, bool>, ConfirmOrderPaymentHandler>();
            services.AddScoped<IRequestHandler<OrderPreviewCommand, OrderPreviewCommandResponse>, OrderPreviewHandler>();

            // Voucher 
            services.AddScoped<IRequestHandler<GetVouchersCommand, GetVouchersCommandResponse>, GetVouchersHandler>();
            services.AddScoped<IRequestHandler<CreateVoucherCommand, CreateVoucherCommandResponse>, CreateVoucherHandler>();
            services.AddScoped<IRequestHandler<DeleteVoucherCommand, bool>, DeleteVoucherHandler>();

            //
            // ===================== Payment Context =====================
            //
            services.AddScoped<IRequestHandler<GetPaymentCommand, GetPaymentCommandResponse>, GetPaymentHandler>();
            services.AddScoped<IRequestHandler<PayOrderCommand, PayOrderCommandResponse>, PayOrderHandler>();
            services.AddScoped<IRequestHandler<UpdateInvoiceStatusCommand, bool>, UpdateInvoiceStatusHandler>();

            //
            // ===================== Notification Context =====================
            //
            services.AddScoped<INotificationHandler<UserRegisteredEvent>, NotificationHandler>();
            services.AddScoped<INotificationHandler<OrderStockAcceptedEvent>, NotificationHandler>();
            services.AddScoped<INotificationHandler<OrderPaidEvent>, NotificationHandler>();
            services.AddScoped<IRequestHandler<SendAccountActivationEmailCommand, bool>, SendAccountActivationEmailHandler>();
            services.AddScoped<IRequestHandler<SendPaymentConfirmedEmailCommand, bool>, SendPaymentConfirmedEmailHandler>();
            services.AddScoped<IRequestHandler<SendOrderStartedEmailCommand, bool>, SendOrderStartedEmailHandler>();

            //
            // ===================== Crosscutting =====================
            //
            services.AddScoped<IAzureStorageGateway, AzureStorageGateway>();
            services.AddScoped<IMailingService, MailingService>();
            services.AddScoped<IIuguService, IuguService>();
            services.AddScoped<IDistanceMatrixService, DistanceMatrixService>();
            services.AddScoped<IGeocodeService, GeocodeService>();
            services.AddScoped<ICepService, CepService>();

            return services;
        }
    }
}
