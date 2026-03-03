namespace CbsAp.Infrastracture.NotificationService
{
    public class EmailConfiguration
    {
        public string? From { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool DefaultCredential { get; set; }
    }
}