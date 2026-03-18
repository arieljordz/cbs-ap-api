using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.Entity;

namespace CbsAp.Application.Abstractions.Services.Entity
{
    public interface IEntityService
    {
        Task<bool> IsEntityExist(string entityName, string entityCode, long? entityProfileID = null);

        Task<bool> CreateEntity(EntityProfile entity, CancellationToken cancellationToken);

        Task<bool> UpdateEntity(EntityProfile entity, List<EntityMatchingConfigDto>? incomingConfigs, CancellationToken cancellationToken);

        Task<EntityDto?> GetEntityByIdAsync(long entityProfileID);

        Task<PaginatedList<EntitySearchDto>> SearchEntityPagination(
            string? EntityName,
            string? EntityCode,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token);

        Task<List<ExportEntityDto>> ExportEntityToExcel(string? EntityName, string? EntityCode, CancellationToken token);
    }
}