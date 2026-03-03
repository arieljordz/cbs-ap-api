using CbsAp.Application.Configurations;
using CbsAp.Application.Configurations.constants;

namespace CbsAp.API.Middlewares.Compliance.Policies
{
    public static class CorsServiceCollection
    {
        public static IServiceCollection AddCorsWithConfiguration(this IServiceCollection services, Action<CORSOptions> configure)
        {
            var corsOptions = new CORSOptions();
            configure(corsOptions);

            services.AddCors(options =>
            {
                options.AddPolicy(PolicyConstants.CorsPolicyName, builder =>
                {
                    builder.WithOrigins(corsOptions.AllowedOrigins!)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                });
                options.AddPolicy(PolicyConstants.CorsAttachmentPolicy, builder =>
                {
                    builder.WithOrigins(corsOptions.AllowedOrigins!)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithExposedHeaders("Content-Disposition");
                });
                options.AddPolicy(PolicyConstants.CorsImageRangePolicy, builder =>
                {
                    builder.WithOrigins(corsOptions.AllowedOrigins!)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithExposedHeaders("Content-Range", "Accept-Ranges", "Content-Length");
                });

                
            });

            return services;
        }
    }
}