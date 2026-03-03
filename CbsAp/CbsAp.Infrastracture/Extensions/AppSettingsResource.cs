using CbsAp.Application.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Infrastracture.Extensions
{
    public static class AppSettingsResource
    {
       public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<AppSettings>>().Value);

            services.Configure<AuthConfig>(configuration.GetSection("AuthConfig"));

            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<AuthConfig>>().Value);


            return services;
        }
    }
}
