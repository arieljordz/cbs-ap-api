using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Entity;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Services.Entity
{
    // REFACTOR : this service should be extracted on specified entity cqrs handler.
    public class EntityService : IEntityService
    {
        private readonly IUnitofWork _unitofWork;

        private readonly IEntityRepository _entityRepository;

        public EntityService(IUnitofWork unitofWork, IEntityRepository entityRepository)
        {
            _unitofWork = unitofWork;
            _entityRepository = entityRepository;
        }

        public async Task<bool> CreateEntity(EntityProfile entity, CancellationToken cancellationToken)
        {
            await _unitofWork.GetRepository<EntityProfile>()
                 .AddAsync(entity);
            return await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
        }

        public async Task<bool> IsEntityExist(string entityName, string entityCode, long? entityProfileID = null)
        {
            // update checking
            if (entityProfileID != null)
            {
                string normalizedEntityName = entityName.ToLower().Trim();
                string normalizedEntityCode = entityCode.ToLower().Trim();

                var supplierInfo = await _unitofWork.GetRepository<EntityProfile>()
                    .ApplyPredicateAsync(e => e.EntityProfileID != entityProfileID);

                var duplicates =
                  await supplierInfo.Select(e => new { e.EntityName, e.EntityCode })
                    .ToListAsync();

                bool idExists = duplicates.Any(e => e.EntityCode!.ToLower().Trim() == normalizedEntityCode);
                bool nameExists = duplicates.Any(e => e.EntityName!.ToLower().Trim() == normalizedEntityName);

                return idExists || nameExists;
            }
            // new record checking
            return await _unitofWork.GetRepository<EntityProfile>()
                 .AnyAsync(e => e.EntityName == entityName || e.EntityCode == entityCode);
        }

        public async Task<bool> UpdateEntity(EntityProfile entity, CancellationToken cancellationToken)
        {
            var matchConfigType = new[] { MatchingConfigType.PO, MatchingConfigType.GR };

            if (entity.MatchingConfigs != null)
            {
                foreach (MatchingConfigType configType in matchConfigType)
                {
                    // MatchingConfigType.GR is not yet in the recoord
                    var incomingConfig = entity.MatchingConfigs!
                        .FirstOrDefault(x => x.ConfigType == configType);
                    var existingConfig = await _unitofWork.GetRepository<EntityMatchingConfig>()
                        .ApplyPredicateAsync(x => x.EntityProfileID == entity.EntityProfileID && x.ConfigType == configType);

                    var exist = existingConfig.FirstOrDefault();

                    if (incomingConfig != null) // skip for incoming new record
                    {
                        if (exist != null)
                        {
                            incomingConfig.Adapt(exist);
                            await _unitofWork.GetRepository<EntityMatchingConfig>().UpdateAsync(exist.MatchingConfigID, exist);
                        }
                        else // skip for incoming new record
                        {
                            incomingConfig.EntityProfileID = entity.EntityProfileID;
                            await _unitofWork.GetRepository<EntityMatchingConfig>().AddAsync(incomingConfig);
                        }
                    }
                    else if (existingConfig.Any())
                    {
                        await _unitofWork.GetRepository<EntityMatchingConfig>().DeleteAsync(exist);
                    }
                }
            }
            await _unitofWork.GetRepository<EntityProfile>().UpdateAsync(entity.EntityProfileID, entity);
            return await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
        }

        public async Task<EntityDto?> GetEntityByIdAsync(long entityProfileID)
        {
            var entity = await _entityRepository.GetEntityByID(entityProfileID)!;

            return entity;
        }

        public async Task<PaginatedList<EntitySearchDto>> SearchEntityPagination(string? EntityName, string? EntityCode, int pageNumber, int pageSize, string? sortField, int? sortOrder, CancellationToken token)
        {
            var entityPagination = await _entityRepository.SearchEntityWithPagination(
                EntityName,
                EntityCode,
                pageNumber,
                pageSize,
                sortField,
                sortOrder,
                token);
            return entityPagination!;
        }

        public async Task<List<ExportEntityDto>> ExportEntityToExcel(string? EntityName, string? EntityCode, CancellationToken token)
        {
            var result = await _entityRepository.ExportEntityToExcel(EntityName, EntityCode, token);

            return result;
        }
    }
}