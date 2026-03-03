using CbsAp.API.Middlewares.Common;
using CbsAp.API.Middlewares.Compliance.Policies;
using CbsAp.API.Middlewares.StartUp;
using CbsAp.API.Middlewares.SwaggerConfigureOptions;
using CbsAp.API.SignalRHub;
using CbsAp.Application;
using CbsAp.Application.Configurations.constants;
using CbsAp.Infrastracture;
using CbsAp.Infrastracture.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppSettings();

//Registering Logging
builder.Host.SerilogRequestLogging(builder.Configuration);
//builder.Host.con

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCorsWithConfiguration(options =>
{    
    options.AllowedOrigins = builder.Configuration.GetSection("CORSOptions:AllowedOrigins")
    .Get<string[]>()!;
});

//Swagger UI custom configurations
builder.Services.AddApplicationVersioning();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

//Registering Application

builder.Services
      .AddAppSettings(builder.Configuration)
     .AddApplication()
     .AddInfrastructure(builder.Configuration);

builder.Services.AddSignalR();

var app = builder.Build();
app.MapHub<LockHub>("/api/lockhub").RequireAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseConfigureSwaggerUI();
}

app.UseCors(PolicyConstants.CorsPolicyName);
app.UseHttpsRedirection();

app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();

//Compliance Requirements
app.UseApplicationCompliance();

//httpContext
app.UseMiddleware<UserContextMiddleware>();

app.MapControllers();

app.Run();