using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.UserManagement;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IUserManagementRepository
    {
        Task<IEnumerable<UserAccount>> GetUserAccountByUserAccountIDAsync(long userAccountID);

        Task<IEnumerable<UserAccount>> GetActiveUserAccounts();

        Task<PaginatedList<UserSearchPaginationDTO>> GetUserSearchWithRolesAsync(
            string fullName,
            string userID,
            bool? isActive,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token);

        Task<IQueryable<ActiveUsersDTO>> GetActiveUserByRoleAsync(long roleID);

        Task<IQueryable<ActiveUsersDTO>> GetActiveUserNotInRoleAsync(long roleID);

        IQueryable<UserAccount> GetUserAccountAsQueryable();
    }
}