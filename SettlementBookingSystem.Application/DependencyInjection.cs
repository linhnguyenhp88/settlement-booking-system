using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SettlementBookingSystem.Application.Behaviours;
using SettlementBookingSystem.Domain.Interfaces;
using SettlementBookingSystem.Infrastructure.EntityFrameworkCore;
using SettlementBookingSystem.Infrastructure.Repositories;
using System.Linq;
using System.Reflection;

namespace SettlementBookingSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddAutoMapper(assembly);

            services.AddDbContexts();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddLogging();
            services.AddFluentValidation(assembly);
            services.AddMediatR(cfg => cfg.AsScoped(), assembly);
    
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));

            return services;
        }

        private static void AddDbContexts(this IServiceCollection services)
        {
            services.AddDbContext<SettlementBookingSystemContext>(options =>
            {
                options.UseInMemoryDatabase("SettlementBookingSystemDb");
            });     
        }

        private static void AddFluentValidation(this IServiceCollection services, Assembly assembly)
        {
            var validatorType = typeof(IValidator<>);

            var validatorTypes = assembly
                .GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == validatorType))
                .ToList();

            foreach (var validator in validatorTypes)
            {
                var requestType = validator.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IValidator<>))
                    .Select(i => i.GetGenericArguments()[0])
                    .First();

                var validatorInterface = validatorType
                    .MakeGenericType(requestType);

                services.AddTransient(validatorInterface, validator);
            }
        }
    }
}
