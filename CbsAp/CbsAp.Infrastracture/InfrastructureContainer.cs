using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Abstractions.Services.Shared;
using CbsAp.Infrastracture.Extensions;
using CbsAp.Infrastracture.Persistence.ContextDependencyChecker;
using CbsAp.Infrastracture.Utility.ExcelGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CbsAp.Infrastracture
{
    public static class InfrastructureContainer
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDatabaseContext();
            services.AddAuthentication(configuration);
            services.AddRepositories();

            services.AddNotifications(configuration);

            services.AddSingleton<IExcelService, ExcelService>();
            services.AddScoped<IDbSetDependencyChecker, DbSetDependencyChecker>();

            return services;
        }
    }
}