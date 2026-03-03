namespace CbsAp.Application.DTOs.Entity
{
    public class EntityRoleDto
    {
      
        public long RoleID { get; set; }    
        public List<GetAllEntityDto> EntityProfiles { get; set; }
      
    }
}