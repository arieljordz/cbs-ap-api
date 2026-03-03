using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Features.Roles.Queries.Common;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Enums;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Roles.Queries
{
    public class ExportRolesQueryHandler : IQueryHandler<ExportRolesQuery, ResponseResult<byte[]>>
    {
        private readonly IRoleManagementRepository _roleManagementRepository;
        private readonly IExcelService _excelService;

        public ExportRolesQueryHandler(IRoleManagementRepository roleManagementRepository, IExcelService excelService)
        {
            _roleManagementRepository = roleManagementRepository;
            _excelService = excelService;
        }

        public async Task<ResponseResult<byte[]>> Handle(ExportRolesQuery request, CancellationToken cancellationToken)
        {
            ExpressionStarter<Role> predicate = PredicateBuilder.New<Role>(p => p.IsActive || !p.IsActive);

            if (!string.IsNullOrEmpty(request.EntityName))
            {
                predicate = predicate.And(u => u.RoleEntities
                .Any(e => e.EntityProfile.EntityName.Contains(request.EntityName)));
            }

            if (!string.IsNullOrEmpty(request.RoleName))
            {
                predicate = predicate.And(u => u.RoleName!.Contains(request.RoleName));
            }

            if (request.IsActive.HasValue)
            {
                predicate = predicate.And(u => u.IsActive == request.IsActive);
            }

            var roleSearchQuery = await _roleManagementRepository.GetRoleAsQueryable()
                .AsNoTracking()
                .Include(r => r.RolePermissionGroups)
                    .ThenInclude(rpg => rpg.Permission)
                .Include(r => r.RoleEntities)
                    .ThenInclude(re => re.EntityProfile)
                .OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate)
                .AsExpandable()
                .Where(predicate).Select(r => new RolesExportDto
                {
                    RoleName = r.RoleName,
                    EntityName = string.Join(",",
                        r.RoleEntities
                            .Select(re => re.EntityProfile.EntityName)
                            .Distinct()
                    ),
                    RoleManagerOne = r.RelatedRoleManager1 != null ? r.RelatedRoleManager1.RoleName : string.Empty,
                    RoleManagerTwo = r.RelatedRoleManager2 != null ? r.RelatedRoleManager2.RoleName : string.Empty, 
                    AuthorisationLimit = r.AuthorisationLimit.ToString() ?? string.Empty,
                    PermissionGroups = string.Join(",",
                        r.RolePermissionGroups
                            .Select(rpg => rpg.Permission.PermissionName)
                            .Distinct()
                    ),
                    IsActive = r.IsActive
                }).ToListAsync(cancellationToken: cancellationToken); ;

            if (roleSearchQuery.Count == 0 || roleSearchQuery == null)
            {
                return ResponseResult<byte[]>.NotFound(MessageConstants.Message("Tax Code", MessageOperationType.NotFound));
            }

            var excelBytes = await Task.Run(() => _excelService.GenerateExcel(roleSearchQuery, "Roles"));

            return ResponseResult<byte[]>.Success(excelBytes, "Export excel data");
        }
    }
}
