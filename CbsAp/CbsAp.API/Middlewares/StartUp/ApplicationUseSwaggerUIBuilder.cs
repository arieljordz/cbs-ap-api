using Asp.Versioning.ApiExplorer;
using System.Diagnostics.CodeAnalysis;

namespace CbsAp.API.Middlewares.StartUp
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationUseSwaggerUIBuilder
    {
        public static IApplicationBuilder UseConfigureSwaggerUI(this IApplicationBuilder app)
        {
            var apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseSwaggerUI(options =>
            {
            
               
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                   
                    options.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json",
                   // options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                       $"CBS-AP API {description.GroupName.ToUpperInvariant()}");
                }
                options.DefaultModelsExpandDepth(-1);

               // options.RoutePrefix = String.Empty;
            });
            return app;
        }
    }
}