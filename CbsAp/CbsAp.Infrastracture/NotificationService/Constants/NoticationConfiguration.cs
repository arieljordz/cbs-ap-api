namespace CbsAp.Infrastracture.NotificationService.Constants
{
    public static class NoticationConfiguration
    {
        public const string EmailNotificationPath = "EmailNotificationTemplates";
        // assets

        public const string HeaderLogo = $"{EmailNotificationPath}/assets/cbsap-grey.png";
        public const string FooterLogo = $"{EmailNotificationPath}/assets/poweredby-cbsanz.png";

        public const string NoticeNotificationTemplate = $"{EmailNotificationPath}/NoticeNotification.html";

        public const string ForgotPasswordNotifTemplate = $"{EmailNotificationPath}/ForgotPasswordNotif";

        public const string NewUserNotificationTemplate = $"{EmailNotificationPath}/NewUserNotif";
    }
}