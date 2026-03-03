using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Enums;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Roles.Queries.SearchActions
{
    public class SearchRoleQueryHandler : IQueryHandler<SearchRolePaginationParamQuery,
        ResponseResult<PaginatedList<RoleSearchDTO>>>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;

        public SearchRoleQueryHandler(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        public async Task<ResponseResult<PaginatedList<RoleSearchDTO>>> Handle(
            SearchRolePaginationParamQuery request,
            CancellationToken cancellationToken)
        {
            ExpressionStarter<Role> predicate = PredicateBuilder.New<Role>(p => p.IsActive || !p.IsActive);
            
            if (!string.IsNullOrEmpty(request.Entity))
            {
                predicate = predicate.And(u => u.RoleEntities
                .Any(e => e.EntityProfile.EntityName.Contains(request.Entity)));
            }
            
            if (!string.IsNullOrEmpty(request.RoleName))
            {
                predicate = predicate.And(u => u.RoleName!.Contains(request.RoleName));
            }

            if (request.IsActive.HasValue)
            {
                predicate = predicate.And(u => u.IsActive == request.IsActive);
            }

            var roleSearchQuery = _roleManagementRepository.GetRoleAsQueryable()
                .AsNoTracking()
                .Include(r => r.RolePermissionGroups)
                .Include(r => r.RoleEntities)
                    .ThenInclude(re => re.EntityProfile)
                .OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate)
                .AsExpandable()
                .Where(predicate);

            if (string.IsNullOrEmpty(request.SortField))
            {
                roleSearchQuery = roleSearchQuery.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var paginatedRoleSearchQuery = roleSearchQuery.Select(r => new RoleSearchDTO
            {
                RoleID = r.RoleID,
                RoleName = r.RoleName,
                Entity = string.Join(",",
                    r.RoleEntities
                        .Select(re => re.EntityProfile.EntityName)
                        .Distinct()
                ),
                RoleManager1 = r.RelatedRoleManager1 != null ? r.RelatedRoleManager1.RoleName : string.Empty,
                RoleManager2 = r.RelatedRoleManager2 != null ? r.RelatedRoleManager2.RoleName : string.Empty, 
                AuthorisationLimit = r.AuthorisationLimit,
                IsActive = r.IsActive
            });

            var taxCodePagination = await paginatedRoleSearchQuery.OrderByDynamic(request.SortField, request.SortOrder)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

            return taxCodePagination == null ?
                 ResponseResult<PaginatedList<RoleSearchDTO>>
                .NotFound(MessageConstants.Message("Roles", MessageOperationType.NotFound))
              : ResponseResult<PaginatedList<RoleSearchDTO>>.SuccessRetrieveRecords(taxCodePagination, "Roles");
        }
    }
}