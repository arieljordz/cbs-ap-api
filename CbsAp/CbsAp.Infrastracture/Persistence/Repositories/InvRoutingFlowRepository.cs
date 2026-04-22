using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.InvInfoRoutingLevel;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Contexts;
using DocumentFormat.OpenXml.Office2010.Excel;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class InvRoutingFlowRepository : IInvRoutingFlowRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public InvRoutingFlowRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public Task<List<ExportInvRoutingFlowDto>> ExportInvRoutingFlowToExcel(
            string? EntityName,
            string? InvRoutingFlowName,
            string? LinkSupplier,
            string? Roles,
            string? MatchReference,
            CancellationToken cancellationToken)
        {
            ExpressionStarter<InvRoutingFlow> predicate =
              PredicateBuilder.New<InvRoutingFlow>(i => i.IsActive || !i.IsActive);

            predicate = predicate
                 .AndIf(!string.IsNullOrEmpty(EntityName), s => s.EntityProfile!.EntityName.Contains(EntityName!))
                 .AndIf(!string.IsNullOrEmpty(InvRoutingFlowName), s => s.InvRoutingFlowName!.Contains(InvRoutingFlowName!))
                 .AndIf(!string.IsNullOrEmpty(LinkSupplier), s => s.SupplierInfo!.SupplierName!.Contains(LinkSupplier!))
                 .AndIf(!string.IsNullOrEmpty(Roles), s => s.Levels!.Any(s => s.Role!.RoleName!.Contains(Roles)))
                 .AndIf(!string.IsNullOrEmpty(MatchReference), s => s.MatchReference!.Contains(MatchReference!));

            var query = _dbcontext.InvRoutingFlows!
                .Include(r => r.Levels!)
                 .ThenInclude(s => s.Role)
                 .ThenInclude(r => r!.UserRoles)
                .AsNoTracking()
                .AsQueryable()
                .AsExpandable()
                .Where(predicate);

            var dtoInvRoutingFlowExport = query.Select(i => new ExportInvRoutingFlowDto
            {
                Entity = i.EntityProfile!.EntityName,
                InvoiceRoutingFlowName = i.InvRoutingFlowName,
                SuppliersLinked = i.SupplierInfo!.SupplierName,
                Roles = string.Join(",", i.Levels!.Select(r => r.Role!.RoleName)),
                Users = string.Join(",", i.Levels!
                        .SelectMany(r => r.Role!.UserRoles)
                        .Select(ur => ur.UserAccount.UserID)),
                MatchReference = i.MatchReference
            });

            return dtoInvRoutingFlowExport.ToListAsync(cancellationToken);
        }

        public async Task<List<InvInfoRoutingLevelDto>> GetInvInfoRoutingFlow(
            long invoiceID, 
            long? keywordID, 
            long? supplierInfoID,
            CancellationToken cancellationToken)
        {
            ExpressionStarter<InvInfoRoutingLevel> predicate =
              PredicateBuilder.New<InvInfoRoutingLevel>();

            predicate =
                predicate.And(x => x.InvoiceID == invoiceID);

            //remove this checking on keyword and supplier
            //if (keywordID is not null  && keywordID > 0)
            //    predicate = predicate.And(x => x.KeywordID == keywordID);

            //else if (supplierInfoID is not null && supplierInfoID > 0)
            //    predicate = predicate.And(x => x.SupplierInfoID == supplierInfoID);

            var query = _dbcontext.InvInfoRoutingsLevels
                .AsNoTracking()
                .AsQueryable()
                .AsExpandable()
                .Where(predicate);

            var dto = query.Select(dto => new InvInfoRoutingLevelDto
            {
                InvoiceID = dto.InvoiceID,
                InvInfoRoutingLevelID = dto.InvInfoRoutingLevelID,
                InvRoutingFlowID = dto.InvRoutingFlowID,
                Level = dto.Level,
                RoleID = dto.RoleID,
                KeywordID = dto.KeywordID,
                SupplierInfoID = dto.SupplierInfoID,
                FlowStatus = dto.InvFlowStatus.HasValue ? (InvFlowStatus)dto.InvFlowStatus.Value : InvFlowStatus.Pending
            });

            var test = dto.ToList();

            return  await dto.ToListAsync(cancellationToken);
        }

        public async Task<InvRoutingFlow?> GetInvRoutingFlowByIdAsync(long invRoutingFlowID, CancellationToken cancellationToken)
        {
            var invRoutingFlow = await _dbcontext.InvRoutingFlows
                .Include(l => l.Levels)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.InvRoutingFlowID == invRoutingFlowID, cancellationToken);

            return invRoutingFlow ?? null;
        }

        public async Task<PaginatedList<SearchInvRoutingFlowDto>> InvRoutingFlowSearchWithPagination(
            string? EntityName,
            string? InvRoutingFlowName,
            string? LinkSupplier,
            string? Roles,
            string? MatchReference,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken cancellationToken)
        {
            ExpressionStarter<InvRoutingFlow> predicate =
              PredicateBuilder.New<InvRoutingFlow>(i => i.IsActive || !i.IsActive);

            predicate = predicate
                 .AndIf(!string.IsNullOrEmpty(EntityName), s => s.EntityProfile!.EntityName.Contains(EntityName!))
                 .AndIf(!string.IsNullOrEmpty(InvRoutingFlowName), s => s.InvRoutingFlowName!.Contains(InvRoutingFlowName!))
                 .AndIf(!string.IsNullOrEmpty(LinkSupplier), s => s.SupplierInfo!.SupplierName!.Contains(LinkSupplier!))
                 .AndIf(!string.IsNullOrEmpty(Roles), s => s.Levels!.Any(s => s.Role!.RoleName!.Contains(Roles)))
                 .AndIf(!string.IsNullOrEmpty(MatchReference), s => s.MatchReference!.Contains(MatchReference!));

            var query = _dbcontext.InvRoutingFlows!
                .Include(r => r.Levels!)
                 .ThenInclude(s => s.Role)
                 .ThenInclude(r => r.UserRoles)
                .AsNoTracking()
                .AsQueryable()
                .AsExpandable()
                .Where(predicate);
            if (string.IsNullOrEmpty(sortField))
            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery = query.Select(i => new SearchInvRoutingFlowDto
            {
                InvRoutingFlowID = i.InvRoutingFlowID,
                Entity = i.EntityProfile!.EntityName,
                InvoiceRoutingFlowName = i.InvRoutingFlowName,
                SuppliersLinked = i.SupplierInfo!.SupplierName,
                MatchReference = i.MatchReference
            });

            var invRoutingPagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                             .ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
            return invRoutingPagination;
        }

        public async Task<PaginatedList<SearchInvRoutingFlowRolesDto>> InvRoutingFlowSearchWithRolesPagination(
            long? InvRoutingFlowID,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken cancellationToken)
        {
            ExpressionStarter<InvRoutingFlow> predicate =
               PredicateBuilder.New<InvRoutingFlow>(i => i.IsActive || !i.IsActive);

            predicate = predicate
                 .AndIf(InvRoutingFlowID.HasValue, s => s.InvRoutingFlowID == InvRoutingFlowID);

            var query = _dbcontext.InvRoutingFlows!
                .Include(r => r.Levels!)
                 .ThenInclude(s => s.Role)
                .AsNoTracking()
                .AsQueryable()
                .AsExpandable()
                .Where(predicate);
            if (string.IsNullOrEmpty(sortField))
            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery = query
                .SelectMany(i => i.Levels!)
                .Where(l => l.Role != null && l.Role.RoleName != null)
                .Select(r => new SearchInvRoutingFlowRolesDto
                {
                    RoleName = r.Role!.RoleName!
                });

            var invRoutingRolesPagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                             .ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
            return invRoutingRolesPagination;
        }

        public async Task<PaginatedList<SearchInvRoutingFlowUserDto>> InvRoutingFlowSearchWithUsersPagination(long? InvRoutingFlowID, int pageNumber, int pageSize, string? sortField, int? sortOrder, CancellationToken cancellationToken)
        {
            ExpressionStarter<InvRoutingFlow> predicate =
              PredicateBuilder.New<InvRoutingFlow>(i => i.IsActive || !i.IsActive);

            predicate = predicate
                 .AndIf(InvRoutingFlowID.HasValue, s => s.InvRoutingFlowID == InvRoutingFlowID);

            var query = _dbcontext.InvRoutingFlows!
                .Include(r => r.Levels!)
                 .ThenInclude(s => s.Role)
                 .ThenInclude(u => u.UserRoles)
                .AsNoTracking()
                .AsQueryable()
                .AsExpandable()
                .Where(predicate);
            if (string.IsNullOrEmpty(sortField))
            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery = query
                    .SelectMany(i => i.Levels!
                        .Where(r => r.Role != null && r.Role.UserRoles != null)
                        .SelectMany(r => r.Role!.UserRoles!)
                        .Where(ur => ur.UserAccount != null)
                        .Select(ur => ur.UserAccount!.UserID)
                    )
                    .Where(userId => !string.IsNullOrEmpty(userId))
                    .Distinct()
                    .Select(userId => new SearchInvRoutingFlowUserDto
                    {
                        UserID = userId!
                    });

            var invRoutingRolesPagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                             .ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
            return invRoutingRolesPagination;
        }

        public async Task<bool> IsInvRoutingFlowExist(
            long? supplierInfoID,
            string invRoutingFlowName,
            long? invRoutingFlowID = null)
        {
            if (!supplierInfoID.HasValue)
            {
                return await _dbcontext.InvRoutingFlows.AnyAsync(r =>
                  r.InvRoutingFlowName!.ToLower().Trim() == invRoutingFlowName.ToLower().Trim() &&
                  (!invRoutingFlowID.HasValue || r.InvRoutingFlowID != invRoutingFlowID)
                );
            }

            return await _dbcontext.InvRoutingFlows.AnyAsync(r =>
             (!invRoutingFlowID.HasValue || r.InvRoutingFlowID != invRoutingFlowID) &&
                (r.InvRoutingFlowName!.ToLower().Trim() == invRoutingFlowName.ToLower().Trim() ||
                    r.SupplierInfoID == supplierInfoID
                )
              );
        }
    }
}