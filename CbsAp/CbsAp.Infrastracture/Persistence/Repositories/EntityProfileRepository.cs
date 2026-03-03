using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Infrastracture.Contexts;
using LinqKit;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class EntityProfileRepository : IEntityRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public EntityProfileRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public Task<List<ExportEntityDto>> ExportEntityToExcel(string? EntityName, string? EntityCode, CancellationToken token)
        {
            ExpressionStarter<EntityProfile> predicate = PredicateBuilder.New<EntityProfile>(true);

            if (!string.IsNullOrEmpty(EntityName))
            {
                predicate = predicate.And(u => u.EntityName.Contains(EntityName));
            }
            if (!string.IsNullOrEmpty(EntityCode))
            {
                predicate = predicate.And(u => u.EntityCode.Contains(EntityCode));
            }
            var query = _dbcontext.EntityProfiles
              .AsNoTracking()
              .AsExpandable()
              .Where(predicate);

            var dtoSearchEntity =
                query.Select(e => new ExportEntityDto
                {
                    EntityName = e.EntityName,
                    EntityCode = e.EntityCode,
                    TaxID = e.TaxID,
                    EmailAddress = e.EmailAddress,
                });

            return dtoSearchEntity.ToListAsync(token);
        }

        public async Task<IQueryable<EntityRoleDto>> GetAvailableEntityByRoleAsync(long roleID)
        {
            var results =
               await _dbcontext.Roles
               .AsNoTracking()
               .Include(e => e.RoleEntities)
               .ThenInclude(x => x.EntityProfile)

               .Where(r => r.RoleID == roleID)
               .Select(dto => new EntityRoleDto
               {
                   RoleID = dto.RoleID,
                   EntityProfiles = dto.RoleEntities
                        .Select
                        (x => new GetAllEntityDto
                        {
                            EntityProfileID = x.EntityProfileID,
                            EntityCode = x.EntityProfile.EntityCode,
                            EntityName = x.EntityProfile.EntityName
                        }).ToList()
               }
               )
               .ToListAsync();

            return results.AsQueryable();
        }

        public async Task<EntityDto>? GetEntityByID(long entityProfileID)
        {
            var entity = await _dbcontext.EntityProfiles
                .Include(c => c.MatchingConfigs)
                .SingleOrDefaultAsync(e => e.EntityProfileID == entityProfileID);

            var dto = entity.Adapt<EntityDto>();

            return dto!;
        }

        public async Task<IQueryable<GetAllEntityDto>> GetEntityProfileNotInRoleAsync(long roleID)
        {
            var result = await _dbcontext.EntityProfiles
                .AsNoTracking()
                .Where(entity => !_dbcontext.RoleEntities
                 .Any(re => re.EntityProfileID == entity.EntityProfileID
                               && re.RoleID == roleID))
               .Select(profile => new GetAllEntityDto
               {
                   EntityProfileID = profile.EntityProfileID,
                   EntityCode = profile.EntityCode,
                   EntityName = profile.EntityName
               }
               ).ToListAsync();

            return result.AsQueryable();
        }

        public async Task<PaginatedList<EntitySearchDto>> SearchEntityWithPagination(
            string? EntityName,
            string? EntityCode,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token)
        {
            ExpressionStarter<EntityProfile> predicate = PredicateBuilder.New<EntityProfile>(true);

            if (!string.IsNullOrEmpty(EntityName))
            {
                predicate = predicate.And(u => u.EntityName.Contains(EntityName));
            }
            if (!string.IsNullOrEmpty(EntityCode))
            {
                predicate = predicate.And(u => u.EntityCode.Contains(EntityCode));
            }

            var query = _dbcontext.EntityProfiles
                .AsNoTracking()
                .Where(predicate);

            if (string.IsNullOrEmpty(sortField))

            {
                // Apply default sorting if no specific sortField is provided
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoSearchEntity = query.Select(e => new EntitySearchDto
            {
                EntityProfileID = e.EntityProfileID,
                EntityName = e.EntityName,
                EntityCode = e.EntityCode,
                TaxID = e.TaxID,
                EmailAddress = e.EmailAddress,
            });

            var entityPagination = await dtoSearchEntity.OrderByDynamic(sortField, sortOrder)
                .ToPaginatedListAsync(pageNumber, pageSize, token);
            return entityPagination;
        }
    }
}