using CbsAp.Application.Abstractions.NotificationStrategy;
using CbsAp.Application.Abstractions.Services.Notification;
using CbsAp.Infrastracture.NotificationService.Constants;

namespace CbsAp.Infrastracture.NotificationService.Strategies
{
    public class SendNewUserStrategy : INotificationStrategy
    {
        public NotificationType NotificationType => NotificationType.NewUserNotification;
        private readonly IEmailService _emailService;

        public SendNewUserStrategy(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<bool> SendNotificationAsync(string receipient, Dictionary<string, string> bindData)
        {
            return await _emailService.SendEmailAsync(receipient, "CBS AP - New User Notfication",
              NoticationConfiguration.NewUserNotificationTemplate, bindData);
        }
    }
}