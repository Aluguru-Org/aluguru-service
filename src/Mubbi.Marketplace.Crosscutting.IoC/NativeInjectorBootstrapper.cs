using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mubbi.Marketplace.Data;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.Handlers;
using Mubbi.Marketplace.Infrastructure.Data;
using Mubbi.Marketplace.Infrastructure.UnitOfWork;
using Mubbi.Marketplace.Register.Application.Usecases.LogInUser;
using System;
using System.Reflection;

namespace Mubbi.Marketplace.Crosscutting.IoC
{
    public static class NativeInjectorBootstrapper
    {
        public static IServiceCollection AddServiceComponents(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
        {
            services.AddDbContext<MubbiContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, EfUnitOfWork<MubbiContext>>();
            services.AddScoped<IEfUnitOfWork<MubbiContext>, EfUnitOfWork<MubbiContext>>();

            services.AddMediatR(assemblies);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandValidationHandler<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            services.AddScoped<IRequestHandler<LogInUserCommand, LogInUserCommandResponse>, LogInUserHandler>();

            return services;
        }
    }
}
