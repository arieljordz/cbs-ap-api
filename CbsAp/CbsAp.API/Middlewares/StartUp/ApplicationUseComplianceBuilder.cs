using CbsAp.API.Middlewares.Compliance;

namespace CbsAp.API.Middlewares.StartUp
{
    public static class ApplicationUseComplianceBuilder
    {
        public static IApplicationBuilder UseApplicationCompliance(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ApplicationAntiXssMiddleware>();
            builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            return builder;
        }
    }
}