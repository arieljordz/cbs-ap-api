using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Json;

namespace Cbsap.Agent
{
    public static class LoggingExtension
    {
        public static void AddLogging(this IServiceCollection services, IConfiguration configuration)
        {
            var serilogLogger = new LoggerConfiguration()
            .WriteTo.Console()
            //.WriteTo.File("log.txt")
            //.ReadFrom.Configuration(configuration)
            .WriteTo.Map("DateTime", DateTime.UtcNow.ToString("yyyyMMddHH")
                    , (dateTime, wt) =>
                    wt.File(new JsonFormatter(), $"{AppDomain.CurrentDomain.BaseDirectory}/logs/log-{dateTime}.json"))
            .CreateLogger();

            services.AddLogging(builder =>
            {
                //builder.SetMinimumLevel(logLevel);
                builder.AddSerilog(logger: serilogLogger, dispose: true);
            });
        }
    }
}
