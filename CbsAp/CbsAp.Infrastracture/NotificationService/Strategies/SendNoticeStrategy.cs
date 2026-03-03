using CbsAp.Application.Abstractions.NotificationStrategy;
using CbsAp.Application.Abstractions.Services.Notification;
using System.Net.Mail;

namespace CbsAp.Infrastracture.NotificationService.Strategies
{
    public class SendNoticeStrategy : INotificationStrategy
    {
        public NotificationType NotificationType => NotificationType.NoticeNotification;
        private readonly ISendNoticeService _sendNoticeService;
        public SendNoticeStrategy(ISendNoticeService sendNoticeService)
        {
            _sendNoticeService = sendNoticeService;
        }
        public async Task<bool> SendNotificationAsync(string recipient, Dictionary<string, string> bindData)
        {
           return await _sendNoticeService.SendNoticeEmailAsync (recipient, bindData);
        }
    }
}