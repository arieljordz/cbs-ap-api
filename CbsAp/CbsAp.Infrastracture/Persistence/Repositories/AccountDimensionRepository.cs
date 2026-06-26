using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.AccountDimension;
using CbsAp.Application.DTOs.PermissionManagement.OperationDTO;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Infrastracture.Contexts;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class AccountDimensionRepository : IAccountDimensionRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public AccountDimensionRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<PaginatedList<AccountDimensionSearchDTO>> GetSearchAccountDimensionAsync(
            long? roleId,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token)
        {
            // Get all Entity IDs assigned to the role
            var assignedEntityIds = await _dbcontext.RoleEntities
                .AsNoTracking()
                .Where(x => x.RoleID == roleId.Value)
                .Select(x => x.EntityProfileID)
                .ToListAsync(token);

            // Accounts
            var accountQuery =
                from account in _dbcontext.Accounts.AsNoTracking()
                where account.IsActive
                      && account.EntityProfileID.HasValue
                join entity in _dbcontext.EntityProfiles
                    on account.EntityProfileID equals entity.EntityProfileID
                select new AccountDimensionSearchDTO
                {
                    RoleId = roleId.Value,
                    EntityProfileId = account.EntityProfileID,
                    Entity = entity.EntityName,
                    Category = "Account",
                    Assigned = account.AccountID + " - " + account.AccountName,
                    IsAssigned = assignedEntityIds.Contains(account.EntityProfileID.Value)
                };

            // Dimensions
            var dimensionQuery =
                from dimension in _dbcontext.Dimensions.AsNoTracking()
                where dimension.IsActive
                join entity in _dbcontext.EntityProfiles
                    on dimension.EntityProfileID equals entity.EntityProfileID
                select new AccountDimensionSearchDTO
                {
                    RoleId = roleId.Value,
                    EntityProfileId = dimension.EntityProfileID,
                    Entity = entity.EntityName,
                    Category = "Dimension",
                    Assigned = dimension.DimensionCode + " - " + dimension.Name,
                    IsAssigned = assignedEntityIds.Contains(dimension.EntityProfileID)
                };

            var query = accountQuery.Union(dimensionQuery);

            if (string.IsNullOrWhiteSpace(sortField))
            {
                query = query
                    .OrderBy(x => x.Entity)
                    .ThenBy(x => x.Category)
                    .ThenBy(x => x.Assigned);
            }

            return await query
                .OrderByDynamic(sortField, sortOrder)
                .ToPaginatedListAsync(pageNumber, pageSize, token);
        }
    }
}