namespace CbsAp.Application.FakeStoreData.FakeDTO
{
    public class FakeSearchUser
    {
        public long UserAccountID { get; set; }
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<FakeUserRole> UserRoles { get; set; }
    }

    public class FakeUserRole
    {
        public long RoleID { get; set; }
        public string RoleName { get; set; }
    }
}