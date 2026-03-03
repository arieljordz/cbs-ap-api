namespace CbsAp.Application.DTOs.UserManagement
{
    public class UserRolesDTO
    {
        public long RoleID { get; set; }
        public List<ActiveUsersDTO> ActiveUsers { get; set; }
    }
}