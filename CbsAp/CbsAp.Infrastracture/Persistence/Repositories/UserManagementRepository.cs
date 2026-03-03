using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.UserManagement;
using CbsAp.Infrastracture.Contexts;
using LinqKit;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public UserManagementRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<UserAccount>> GetActiveUserAccounts()
        {
            var userAccounts = await _dbcontext.UserAccounts
                .AsNoTracking()
                .Where(u => u.IsActive).ToListAsync();
            return userAccounts;
        }

        public async Task<IQueryable<ActiveUsersDTO>> GetActiveUserByRoleAsync(long roleID)
        {
            var results = await _dbcontext.UserAccounts
                .AsNoTracking()
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .Where(u => u.IsActive && _dbcontext.UserRoles
                .Any(ur => ur.RoleID == roleID && ur.UserAccountID == u.UserAccountID))
                .Select(dto => new ActiveUsersDTO
                {
                    UserAccountID = dto.UserAccountID,
                    UserID = dto.UserID,
                    FullName = $"{dto.FirstName} {dto.LastName}"
                }).ToListAsync();

            return results.AsQueryable();
        }

        public async Task<IQueryable<ActiveUsersDTO>> GetActiveUserNotInRoleAsync(long roleID)
        {
            var results = await _dbcontext.UserAccounts
                .AsNoTracking()
                .Include(r => r.UserRoles)
                //  .ThenInclude(r => r.Role)
                .Where(u => u.IsActive && !_dbcontext.UserRoles
                .Any(ur => ur.RoleID == roleID && ur.UserAccountID == u.UserAccountID))
                .Select(dto => new ActiveUsersDTO
                {
                    UserAccountID = dto.UserAccountID,
                    UserID = dto.UserID,
                    FullName = $"{dto.FirstName} {dto.LastName}"
                }).ToListAsync();
            return results.AsQueryable();
        }

        public async Task<IEnumerable<UserAccount>> GetUserAccountByUserAccountIDAsync(long userAccountID)
        {
            var userAccountDetail = await _dbcontext.UserAccounts
                .Where(u => u.UserAccountID == userAccountID)

                 .Include(ur => ur.UserRoles)
                 .ThenInclude(r => r.Role).ToListAsync();

            return userAccountDetail ?? Enumerable.Empty<UserAccount>();
        }

        public async Task<PaginatedList<UserSearchPaginationDTO>> GetUserSearchWithRolesAsync(
            string fullName,
            string userID,
            bool? isActive,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token
            )
        {
            ExpressionStarter<UserAccount> predicate
             = PredicateBuilder.New<UserAccount>(u => !u.IsUserPartialDeleted);

            if (!string.IsNullOrEmpty(fullName))
            {
                predicate = predicate.And(
                    SearchCustomPredicates.SearchFullName<UserAccount>(fullName,
                    u => u.FirstName, x => x.LastName));
            }
            if (!string.IsNullOrEmpty(userID))
            {
                predicate = predicate.And(u => u.UserID.Contains(userID));
            }

            if (isActive.HasValue)
            {
                predicate = predicate.And(u => u.IsActive == isActive);
            }

            var query = _dbcontext.UserAccounts
            .Include(ur => ur.UserRoles)
             .ThenInclude(r => r.Role)
             .AsNoTracking()
            .AsExpandable()
            .Where(predicate);

            if (string.IsNullOrEmpty(sortField))

            {
                // Apply default sorting if no specific sortField is provided
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var projectedQuery = query.ProjectToType<UserSearchPaginationDTO>();

            var userAccountlist = await projectedQuery
              .OrderByDynamic(sortField, sortOrder)
              .ToPaginatedListAsync(pageNumber, pageSize, token);

            return userAccountlist;
        }

        public IQueryable<UserAccount> GetUserAccountAsQueryable()
        {
            return _dbcontext.UserAccounts.AsQueryable();
        }
    }
}
