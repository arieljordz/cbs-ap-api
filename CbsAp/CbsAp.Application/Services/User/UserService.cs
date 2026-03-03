using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Authentication;
using CbsAp.Domain.Entities.UserManagement;

namespace CbsAp.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUnitofWork _unitofWork;

        public UserService(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<bool> userExist(string userName)
        {
            return await _unitofWork.GetRepository<UserAccount>()
               .AnyAsync(u => u.UserID == userName && u.IsActive && !u.IsUserPartialDeleted);
        }
    }
}