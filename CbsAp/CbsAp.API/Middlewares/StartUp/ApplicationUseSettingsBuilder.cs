using System.Diagnostics.CodeAnalysis;

namespace CbsAp.API.Middlewares.StartUp
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationUseSettingsBuilder
    {
        public static IHostBuilder ConfigureAppSettings(this IHostBuilder host)
        {
            var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            host.ConfigureAppConfiguration((builderContext, configBuilder) =>
             {
                 configBuilder.AddJsonFile("appsettings.json", false, true);
                 configBuilder.AddJsonFile($"appsettings.{enviroment}.json", true, true);
                 configBuilder.AddEnvironmentVariables();
             }

            );

            return host;
        }
    }
}