using CbsAp.Application.Abstractions.NotificationStrategy;
using CbsAp.Application.Abstractions.NotificationStrategy.NotificationContext;
using CbsAp.Application.Abstractions.Services.Notification;
using CbsAp.Infrastracture.Contexts.Notification;
using CbsAp.Infrastracture.NotificationService;
using CbsAp.Infrastracture.NotificationService.Strategies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CbsAp.Infrastracture.Extensions
{
    public static class NotificationResource
    {
        public static void AddNotifications(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfigurations = new EmailConfiguration();
            configuration.Bind(nameof(EmailConfiguration),emailConfigurations);
            services.AddSingleton(Options.Create(emailConfigurations));
            services.AddScoped<INotificationContext, NotificationContext>();
            services.AddScoped<INotificationStrategy, SendNewUserStrategy>();
            services.AddScoped<INotificationStrategy, SendNoticeStrategy>();

         
            services.AddScoped<ISendNewUserService, SendNewUserService>();
            services.AddScoped<ISendNoticeService, SendNoticeService>();

            services.AddScoped<INotificationStrategy, ForgotPasswordStategy>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();

            services.AddScoped<IEmailService, EmailService>();


        }
    }
}