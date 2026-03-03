namespace CbsAp.Application.DTOs.UserManagement
{
    public class UpdateUserDTO
    {
        public long UserAccountID { get; set; }

        public string UserID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string? Password { get; set; }

        public bool IsActive { get; set; }

        public bool IsLockedOut { get; set; }

        public List<long> userRoles { get; set; }

        public bool PasswordMandatory { get; set; } = false;
    }
}