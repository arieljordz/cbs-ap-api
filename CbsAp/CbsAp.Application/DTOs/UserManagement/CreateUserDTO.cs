namespace CbsAp.Application.DTOs.UserManagement
{
    public class CreateUserDTO
    {
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
        public List<int> UserRoles { get; set; }
    }
}