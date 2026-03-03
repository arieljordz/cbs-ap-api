using System.Net.Mail;

namespace CbsAp.Application.Abstractions.NotificationStrategy
{
    public interface INotificationStrategy
    {
        Task<bool> SendNotificationAsync(string receipient , Dictionary<string, string> bindData);
        NotificationType NotificationType { get; }
    }
}