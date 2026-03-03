using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.DTOs.PermissionManagement.OperationDTO;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Infrastracture.Contexts;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class PermissionManagementRepository : IPermissionManagementRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public PermissionManagementRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public Task<List<PermissionReportDTO>> ExportExcelPermissionAsync(
            long? permissionID,
            string? permissionName,
            bool? isActive,
            CancellationToken token)
        {
            ExpressionStarter<Permission> predicate = PredicateBuilder
            .New<Permission>(p => p.IsActive || !p.IsActive);

            if (!string.IsNullOrEmpty(permissionName))
            {
                predicate = predicate.And(u => u.PermissionName.Contains(permissionName));
            }

            if (permissionID.HasValue)
            {
                predicate = predicate.And(u => u.PermissionID == permissionID);
            }


            var query = _dbcontext.Permissions
                // .AsNoTracking()
                .Include(p => p.PermissionGroups)
                .ThenInclude(pg => pg.RolePermissions)
                .ThenInclude(rpg => rpg.Role)
                .ThenInclude(r => r.UserRoles)
                .AsQueryable()
                .AsExpandable()

                .Where(predicate);

            var dtoQuery =
                  query
                    .Select(p => new PermissionReportDTO
                    {
                        PermissionGroupID = p.PermissionID,
                        PermissionGroupName = p.PermissionName,
                        ActiveStatus = p.IsActive ? "Yes" : "No",

                        CountofAssignedRoles = _dbcontext.RolePermissionGroups
                                .AsNoTracking()
                                .Where(r => r.PermissionID == p.PermissionID)
                                .Select(r => r.RoleID)
                                .Distinct()
                                .Count(),

                        CountofUsers = _dbcontext.RolePermissionGroups
                                .AsNoTracking()
                                .Where(r => r.PermissionID == p.PermissionID)
                                .Join(_dbcontext.UserRoles,
                                   pr => pr.Role.RoleID,
                                   r => r.RoleID,
                                   (pr, r) => new { pr, r })
                                 .Join(_dbcontext.UserAccounts,
                                   ua => ua.r.UserAccountID,
                                   a => a.UserAccountID,

                                   (pr, r) => new { pr, r })
                                 .Where(r => !r.r.IsUserPartialDeleted)
                                .Select(pu => pu.r.UserAccountID)
                                .Count(),
                    });

            return dtoQuery.ToListAsync();


        }

        public async Task<IQueryable<PermissionDetailDTO>> GetPermissionByRoleAsync(long roleID)
        {


            var results = await _dbcontext.Permissions
              .AsNoTracking()
              .Include(r => r.PermissionGroups)
              .Where(permission => permission.IsActive && _dbcontext.RolePermissionGroups
               .Any(rpg => rpg.PermissionID == permission.PermissionID
                            && rpg.RoleID == roleID))
              .Select(dto => new PermissionDetailDTO
              {
                  PermissionID = dto.PermissionID,
                  PermissionName = dto.PermissionName

              })
              .ToListAsync();
            return results.AsQueryable();
        }

        public async Task<IQueryable<PermissionDetailDTO>> GetPermissionNotInRoleAsync(long roleID)
        {
            var results = await _dbcontext.Permissions
               .AsNoTracking()
               .Include(r => r.PermissionGroups)
               .Where(permission => permission.IsActive && !_dbcontext.RolePermissionGroups
                .Any(rpg => rpg.PermissionID == permission.PermissionID
                             && rpg.RoleID == roleID))
               .Select(dto => new PermissionDetailDTO
               {
                   PermissionID = dto.PermissionID,
                   PermissionName = dto.PermissionName

               })
               .ToListAsync();
            return results.AsQueryable();
        }

        public async Task<PaginatedList<PermissionSearchDTO>> GetSearchPermissionAsync(
            long? permissionID,
            string? permissionName,
            bool? isActive,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token)
        {
            ExpressionStarter<Permission> predicate = PredicateBuilder
                .New<Permission>(p => p.IsActive || !p.IsActive);

            if (!string.IsNullOrEmpty(permissionName))
            {
                predicate = predicate.And(u => u.PermissionName.Contains(permissionName));
            }

            if (permissionID.HasValue)
            {
                predicate = predicate.And(u => u.PermissionID == permissionID);
            }

            if (isActive.HasValue)
            {
                predicate = predicate.And(u => u.IsActive == isActive);
            }

            var query = _dbcontext.Permissions
                // .AsNoTracking()
                .Include(p => p.PermissionGroups)
                .ThenInclude(pg => pg.RolePermissions)
                .ThenInclude(rpg => rpg.Role)
                .ThenInclude(r => r.UserRoles)
                .AsQueryable()
                .AsExpandable()

                .Where(predicate);

            if (string.IsNullOrEmpty(sortField))

            {
                // Apply default sorting if no specific sortField is provided
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery =
              query
                .Select(p => new PermissionSearchDTO
                {
                    PermissionID = p.PermissionID,
                    PermissionName = p.PermissionName,
                    PermissionGroupName = p.PermissionName,
                    IsActive = p.IsActive,

                    CountofRoles = _dbcontext.RolePermissionGroups
                            .AsNoTracking()
                            .Where(r => r.PermissionID == p.PermissionID)
                            .Select(r => r.RoleID)
                            .Distinct()
                            .Count(),

                    CountofUsers = _dbcontext.RolePermissionGroups
                            .AsNoTracking()
                            .Where(r => r.PermissionID == p.PermissionID)
                            .Join(_dbcontext.UserRoles,
                               pr => pr.Role.RoleID,
                               r => r.RoleID,
                               (pr, r) => new { pr, r })
                             .Join(_dbcontext.UserAccounts,
                               ua => ua.r.UserAccountID,
                               a => a.UserAccountID,

                               (pr, r) => new { pr, r })
                             .Where(r => !r.r.IsUserPartialDeleted)
                            .Select(pu => pu.r.UserAccountID)
                            .Count(),
                });

            var permissionSearchList = await dtoQuery
                .OrderByDynamic(sortField, sortOrder)
                .ToPaginatedListAsync(pageNumber, pageSize, token);

            return permissionSearchList;
        }

        public async Task<IQueryable<PermissionSearchByIdDTO>> GetSearchPermissionIDAsync(long permissionID)
        {
            ExpressionStarter<Permission> predicate = PredicateBuilder
                        .New<Permission>(p => p.PermissionID == permissionID);

            var permissionSearchList = await _dbcontext.Permissions
                .AsNoTracking()
                .Include(p => p.PermissionGroups)
                .ThenInclude(pg => pg.RolePermissions)
                .ThenInclude(rpg => rpg.Role)
                .ThenInclude(r => r.UserRoles)
                .AsExpandable()

                .Where(predicate)
                .Select(p => new PermissionSearchByIdDTO
                {
                    PermissionID = p.PermissionID,

                    PermissionGroupName = p.PermissionName,
                    GroupPanel = _dbcontext.PermissionGroups
                            //.AsNoTracking()
                            .Where(r => r.PermissionID == p.PermissionID)
                            .Join(_dbcontext.Operations,
                                pg => pg.OperationID,
                                 op => op.OperationID,
                                 (pg, op) => new { pg, op })
                            .GroupBy(grp => grp.op.Panel)
                            .Select(g => new GroupPanelDTO
                            {

                                Panel = g.Key,
                                Operations =
                                g.Select(o => new OperationsDTO
                                {
                                    OperationID = o.pg.OperationID,
                                    OperationName = o.pg.Operations.OperationName,
                                    Panel = o.pg.Operations.Panel,
                                    ApplyOperationIn = o.pg.Operations.ApplyOperationIn,
                                    Access = o.pg.Access,
                                    PermissionGroupID = o.pg.PermissionGroupID,
                                })
                            }).ToList(),

                    Roles = _dbcontext.RolePermissionGroups
                            .AsNoTracking()
                            .Where(r => r.PermissionID == p.PermissionID)
                            .Join(_dbcontext.Roles,
                                 rpg => rpg.RoleID,
                                 r => r.RoleID,
                                 (rpg, r) => new { rpg, r })
                             .GroupBy(rpg => rpg.r.RoleID)
                             .Select(rmg =>
                               new RoleDTO
                               {
                                   RoleID = rmg.Key,
                                   RoleName = rmg.First().r.RoleName
                               })
                             .ToList()
                }).ToListAsync();

            return permissionSearchList.AsQueryable();
        }

        public IQueryable<Permission> GetPermissionsAsQueryable()
        {
            return _dbcontext.Permissions
                .AsNoTracking().AsQueryable();
        }

        public IEnumerable<OperationsDTO> GetOperationsByRole(long roleId)
        {
            var operationsList = _dbcontext.Roles
                    .Where(r => r.RoleID == roleId)
                    .Join(_dbcontext.RolePermissionGroups,
                        r => r.RoleID,
                        rpg => rpg.RoleID,
                        (r, rpg) => new { r, rpg })
                    .Join(_dbcontext.Permissions,
                        rrpg => rrpg.rpg.PermissionID,
                        p => p.PermissionID,
                        (rrpg, p) => new { rrpg.r, rrpg.rpg, p })
                    .Join(_dbcontext.PermissionGroups,
                        rrp => rrp.p.PermissionID,
                        pg => pg.PermissionID,
                        (rrp, pg) => new { rrp.r, rrp.rpg, rrp.p, pg })
                    .Join(_dbcontext.Operations,
                        rrp_pg => rrp_pg.pg.OperationID,
                        o => o.OperationID,
                        (rrp_pg, o) => new { rrp_pg.r, rrp_pg.rpg, rrp_pg.p, rrp_pg.pg, o })
                    
                    .Where(x => x.pg.Access == true && x.p.IsActive == true)
                    .Select(x => new OperationsDTO()
                    {
                       OperationID = x.o.OperationID,
                       OperationName = x.o.OperationName,
                       ApplyOperationIn = x.o.ApplyOperationIn,
                    })
                    .ToList();


            return operationsList;

        }

        public  IEnumerable<ControlElementDTO> GetPermissionOperationsByRole(long roleId)
        { 
            var operationsList = _dbcontext.Roles
                    .Where(r => r.RoleID == roleId)
                    .Join(_dbcontext.RolePermissionGroups,
                        r => r.RoleID,
                        rpg => rpg.RoleID,
                        (r, rpg) => new { r, rpg })
                    .Join(_dbcontext.Permissions,
                        rrpg => rrpg.rpg.PermissionID,
                        p => p.PermissionID,
                        (rrpg, p) => new { rrpg.r, rrpg.rpg, p })
                    .Join(_dbcontext.PermissionGroups,
                        rrp => rrp.p.PermissionID,
                        pg => pg.PermissionID,
                        (rrp, pg) => new { rrp.r, rrp.rpg, rrp.p, pg })
                    .Join(_dbcontext.Operations,
                        rrp_pg => rrp_pg.pg.OperationID,
                        o => o.OperationID,
                        (rrp_pg, o) => new { rrp_pg.r, rrp_pg.rpg, rrp_pg.p, rrp_pg.pg, o })
                    .Join(_dbcontext.ControlElements,
                        ro => ro.o.OperationID,
                        ce => ce.OperationID,
                        (ro, ce) => new { ro.r, ro.rpg, ro.p, ro.pg, ro.o, ce })
                    .Where(x => x.pg.Access == true && x.p.IsActive == true)
                    .Select(x => new ControlElementDTO()
                    {
                       OperationID =  x.ce.OperationID,
                       ActionName = x.ce.ActionName,
                       ActionType= x.ce.ActionType
                    })
                    .ToList();


            return operationsList;

        }
    }
}