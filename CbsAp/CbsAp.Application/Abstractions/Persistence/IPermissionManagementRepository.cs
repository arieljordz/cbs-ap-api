using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.DTOs.PermissionManagement.OperationDTO;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.PermissionManagement;
using System.Linq;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IPermissionManagementRepository
    {
        Task<PaginatedList<PermissionSearchDTO>> GetSearchPermissionAsync(
            long? permissionID,
            string? permissionName,
            bool? isActive,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token
            );

        Task<IQueryable<PermissionSearchByIdDTO>> GetSearchPermissionIDAsync(
          long permissionID
          );

        Task<IQueryable<PermissionDetailDTO>> GetPermissionByRoleAsync(long roleID);

        Task<IQueryable<PermissionDetailDTO>> GetPermissionNotInRoleAsync(long roleID);

        Task<List<PermissionReportDTO>> ExportExcelPermissionAsync(
         long? permissionID,
         string? permissionName,
         bool? isActive,
         CancellationToken token
         );

        IEnumerable<ControlElementDTO> GetPermissionOperationsByRole(long roleId);
        IEnumerable<OperationsDTO> GetOperationsByRole(long roleId);

        IQueryable<Permission> GetPermissionsAsQueryable();
    }
}