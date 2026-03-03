using Asp.Versioning;
using System.Diagnostics.CodeAnalysis;

namespace CbsAp.API.Middlewares.SwaggerConfigureOptions
{
    [ExcludeFromCodeCoverage]
    public static class ApiVersioning
    {
        public static IServiceCollection AddApplicationVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(
                options =>
                {
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader()
                        );
                }
                ).AddApiExplorer(

                   apiExplorer =>
                   {
                       apiExplorer.GroupNameFormat = "'v'VVV";
                       apiExplorer.SubstituteApiVersionInUrl = true;
                   }
                );

            services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

            return services;
        }
    }
}