namespace CbsAp.Application.DTOs.SystemSettings
{
    public class ArchiveInvSettingDto
    {
        public long SystemVariableID { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Value { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
