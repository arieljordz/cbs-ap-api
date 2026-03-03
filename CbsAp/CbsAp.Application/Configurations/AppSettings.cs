namespace CbsAp.Application.Configurations
{
    public class AppSettings
    {
        public string? ConnectionString { get; set; }
        public string? logFilePathLocation { get; set; }
        public string? WebUrl { get; set; }
        public int RessetPasswordLimitPerDay { get; set; } = 2;
        public int RessetPasswordLimitWithoutLogin { get; set; } = 3;

        public string? InvAttachmentStoragePath { get; set; }

        public string? ValidationRuleFilePath { get; set; }
    }
}