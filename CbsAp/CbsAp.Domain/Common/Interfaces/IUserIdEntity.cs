namespace CbsAp.Domain.Common.Interfaces
{
    public interface IUserIdEntity
    {
        // User Id required to be an Email Address
        public string UserID { get; set; }
    }
}