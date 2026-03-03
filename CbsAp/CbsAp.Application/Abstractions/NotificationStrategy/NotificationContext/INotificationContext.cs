using CbsAp.Domain.Enums;

namespace CbsAp.Application.Abstractions.NotificationStrategy.NotificationContext
{
    public interface INotificationContext
    {
        INotificationStrategy GetNotificationTypeStrategy(NotificationType notificationType);
    }
}