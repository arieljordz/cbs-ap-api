namespace CbsAp.Application.DTOs
{
    public class UpdatePasswordDTO
    {
        public long UserLogInfoID { get; set; }
        public long UserAccountID { get; set; }
        public string UserID { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string? LastUpdatedBy { get; set; }
    }
}