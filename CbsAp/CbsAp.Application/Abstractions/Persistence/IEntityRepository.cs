using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IEntityRepository
    {
        /// <summary>
        ///  Assigned Role for Entity
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        Task<IQueryable<EntityRoleDto>> GetAvailableEntityByRoleAsync(long roleID);

        /// <summary>
        /// Get Available Entity
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        Task<IQueryable<GetAllEntityDto>> GetEntityProfileNotInRoleAsync(long roleID);

        Task<EntityDto?> GetEntityByID(long entityProfileID);

        Task<PaginatedList<EntitySearchDto>> SearchEntityWithPagination(
            string? EntityName,
            string? EntityCode,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token);

        Task<List<ExportEntityDto>> ExportEntityToExcel(
         string? EntityName,
         string? EntityCode,
         CancellationToken token
     );
    }
}