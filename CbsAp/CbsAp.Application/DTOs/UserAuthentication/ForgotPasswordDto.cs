namespace CbsAp.Application.DTOs.UserAuthentication
{
    public class ForgotPasswordDto
    {
        public string? PasswordrecoveryToken { get; set; }
        public bool? IsPasswordRecoveryTokenUsed { get; set; } = false;
        public DateTimeOffset? RecoveryTokenTime { get; set; }
    }
}