using CbsAp.Application.Abstractions.NotificationStrategy;
using CbsAp.Application.Abstractions.NotificationStrategy.NotificationContext;
using CbsAp.Domain.Enums;

namespace CbsAp.Infrastracture.Contexts.Notification
{
    public class NotificationContext : INotificationContext
    {
        private readonly IEnumerable<INotificationStrategy> _notificationStrategy;

        public NotificationContext(IEnumerable<INotificationStrategy> notificationStrategy)
        {
            _notificationStrategy = notificationStrategy;
        }
        public INotificationStrategy GetNotificationTypeStrategy(NotificationType notificationType)
        {
            var notificationTypeStrategy =
                _notificationStrategy
                .FirstOrDefault(ns => ns.NotificationType == notificationType);
            return notificationTypeStrategy!;
        }
    }
}