using CbsAp.Application.Configurations;
using CbsAp.Infrastracture.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CbsAp.Infrastracture.Extensions
{
    public static class DbContextResource
    {
        public static void AddDatabaseContext(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var appSettings = serviceProvider.GetRequiredService<AppSettings>();
                options.UseLazyLoadingProxies().UseSqlServer(appSettings.ConnectionString,
               builder =>
                   builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                   .EnableSensitiveDataLogging();
            }

            );
        }
    }
}