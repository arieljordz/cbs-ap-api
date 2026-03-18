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
            return await _unitofWork.SaveChanges(cancellationToken);
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

        public async Task<bool> UpdateEntity(EntityProfile entity, List<EntityMatchingConfigDto>? incomingConfigs, CancellationToken cancellationToken)
        {
            var matchConfigType = new[] { MatchingConfigType.POMT, MatchingConfigType.PO, MatchingConfigType.GR };

            if (incomingConfigs != null)
            {
                entity.MatchingConfigs ??= new List<EntityMatchingConfig>();

                foreach (MatchingConfigType configType in matchConfigType)
                {
                    // Find incoming config for this type
                    var incomingConfigDto = incomingConfigs
                        .FirstOrDefault(x => Enum.TryParse<MatchingConfigType>(x.ConfigType, ignoreCase: true, out var parsed) && parsed == configType);

                    // Get existing config from DB
                    var existingConfig = await _unitofWork.GetRepository<EntityMatchingConfig>()
                        .ApplyPredicateAsync(x => x.EntityProfileID == entity.EntityProfileID && x.ConfigType == configType);

                    var exist = existingConfig.FirstOrDefault();

                    if (incomingConfigDto != null) // incoming config exists
                    {
                        if (exist != null) // update existing record
                        {
                            incomingConfigDto.Adapt(exist);
                            exist.EntityProfileID = entity.EntityProfileID;
                            await _unitofWork.GetRepository<EntityMatchingConfig>().UpdateAsync(exist.MatchingConfigID, exist);
                        }
                        else // add new record
                        {
                            var newConfig = new EntityMatchingConfig
                            {
                                EntityProfileID = entity.EntityProfileID,
                                ConfigType = Enum.Parse<MatchingConfigType>(incomingConfigDto.ConfigType),
                                MatchingLevel = incomingConfigDto.MatchingLevel,
                                InvoiceMatchBasis = incomingConfigDto.InvoiceMatchBasis,
                                DollarAmt = incomingConfigDto.DollarAmt,
                                PercentageAmt = incomingConfigDto.PercentageAmt
                            };

                            await _unitofWork.GetRepository<EntityMatchingConfig>().AddAsync(newConfig);
                        }
                    }
                    else if (exist != null) // delete missing config
                    {
                        await _unitofWork.GetRepository<EntityMatchingConfig>().DeleteAsync(exist);
                    }
                }
            }
            await _unitofWork.GetRepository<EntityProfile>().UpdateAsync(entity.EntityProfileID, entity);
            return await _unitofWork.SaveChanges(cancellationToken);
        }

        public async Task<EntityDto?> GetEntityByIdAsync(long entityProfileID)
        {
            var entity = await _entityRepository.GetEntityByID(entityProfileID)!;

            if (entity == null)
                return null;

            var dto = entity.Adapt<EntityDto>();

            return dto;
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