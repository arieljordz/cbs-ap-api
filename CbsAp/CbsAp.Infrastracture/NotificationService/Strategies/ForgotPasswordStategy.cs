using CbsAp.Application.Abstractions.NotificationStrategy;
using CbsAp.Application.Abstractions.Services.Notification;
using CbsAp.Infrastracture.NotificationService.Constants;

namespace CbsAp.Infrastracture.NotificationService.Strategies
{
    public class ForgotPasswordStategy : INotificationStrategy
    {
        private readonly IEmailService _emailService;

        public NotificationType NotificationType => NotificationType.ForgotPasswordNotification;

        public ForgotPasswordStategy(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<bool> SendNotificationAsync(string receipient, Dictionary<string, string> bindData)
        {
            return await _emailService.SendEmailAsync(receipient, "CBS AP - Forgot Password",
                NoticationConfiguration.ForgotPasswordNotifTemplate, bindData);
        }
    }
}