using CbsAp.Application.Abstractions.Shared;
using CbsAp.Application.Behaviors;
using CbsAp.Application.Configurations;
using CbsAp.Application.Services;
using CbsAp.Application.Shared.Encryption;
using CbsAp.Application.Shared.Generator;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime;


namespace CbsAp.Application
{
    public static class ApplicationContainer
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddValidatorsFromAssembly(assembly);
            services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddHttpContextAccessor();

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
           

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            services.AddScoped<IHasher, CredentialHasher>();
            services.AddScoped<IPasswordGenerator, PasswordGenerator>();


            services.AddServiceContainer();
           

            return services;
        }
    }
}