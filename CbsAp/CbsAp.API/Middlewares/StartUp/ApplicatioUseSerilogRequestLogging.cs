using CbsAp.Application.Configurations;
using Serilog;
using Serilog.Formatting.Json;

namespace CbsAp.API.Middlewares.StartUp
{
    public static class ApplicatioUseSerilogRequestLogging
    {
        public static IHostBuilder SerilogRequestLogging(this IHostBuilder host, IConfiguration configuration)
        {
            var appSettings = configuration.GetRequiredSection("AppSettings").Get<AppSettings>();

            host.UseSerilog((hostingContext, loggerConfiguration) =>

                 loggerConfiguration
                 .ReadFrom.Configuration(hostingContext.Configuration)
                 .WriteTo.Map("UtcDateTime", DateTime.UtcNow.ToString("yyyyMMddHH")
                    , (UtcDateTime, wt) =>
                    wt.File(new JsonFormatter(),
                    $"{appSettings?.logFilePathLocation}/{DateTime.Now.ToString("yyyyMMdd")}/log-{UtcDateTime}.json"))

                  .Filter.ByExcluding(c =>
                  {
                      if (!c.Properties.ContainsKey("Path"))
                          return false;
                      return c.Properties["Path"].ToString().StartsWith("/swagger");
                  })
                  .Filter.ByIncludingOnly(logEvent => logEvent.Level == Serilog.Events.LogEventLevel.Error
                  )

            );

            return host;
        }
    }
}