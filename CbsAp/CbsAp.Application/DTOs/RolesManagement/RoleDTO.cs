namespace CbsAp.Application.DTOs.RolesManagement
{
    public class RoleDTO
    {
        public long RoleID { get; set; }

        public string? RoleName { get; set; }
        public decimal? AuthorisationLimit { get; set; }
    }
}