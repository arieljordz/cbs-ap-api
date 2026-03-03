namespace CbsAp.Domain.Entities.UserManagement
{
    public sealed class EmailValidationStatus
    {
        public int EmailValidationStatusId { get; set; }
        public string? ReasonCode { get; set; }
        public string? Description { get; set; }
        // public ICollection<UserLogInfo>? UserLogInfos { get; set; }
    }
}