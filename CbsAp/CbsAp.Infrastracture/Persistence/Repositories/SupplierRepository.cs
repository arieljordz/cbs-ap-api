using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Infrastracture.Contexts;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public SupplierRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public Task<List<ExportSupplierDto>> ExportSupplierToExcel(string? EntityName, string? SupplierID, string? SupplierName, bool? IsActive, CancellationToken token)
        {
            ExpressionStarter<SupplierInfo> predicate =
               PredicateBuilder.New<SupplierInfo>(s => s.IsActive || !s.IsActive);

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(EntityName), s => s.EntityProfile!.EntityName.Contains(EntityName!))
                .AndIf(!string.IsNullOrEmpty(SupplierID), s => s.SupplierID!.Contains(SupplierID!))
                .AndIf(!string.IsNullOrEmpty(SupplierName), s => s.SupplierName!.Contains(SupplierName!))
                .AndIf(IsActive.HasValue, s => s.IsActive == IsActive);

            var query = _dbcontext.SupplierInfos
                .Include(s => s.Account)
                .Include(s => s.EntityProfile)
                .Include(s => s.InvRoutingFlow)
                .AsNoTracking()
                .AsQueryable()
                .AsExpandable()
                .Where(predicate);

            var dtoSupplierExport = query.Select(s => new ExportSupplierDto
            {
                Entity = s.EntityProfile!.EntityName,
                SupplierID = s.SupplierID,
                SupplierName = s.SupplierName,
                SupplierTaxID = s.SupplierTaxID,
                BankAccount = s.Account!.AccountName,
                PaymentTerms = s.PaymentTerms,
                InvoiceRoutingFlow = s.InvRoutingFlow!.InvRoutingFlowName,
                ActiveStatus = s.IsActive ? "Yes" : "No"
            });

            return dtoSupplierExport.ToListAsync(token);
        }

        public async Task<PaginatedList<SupplierSearchDto>> SearchSupplierWithPagination(
            string? EntityName,
            string? SupplierID,
            string? SupplierName,
            bool? IsActive,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token)
        {
            ExpressionStarter<SupplierInfo> predicate =
                PredicateBuilder.New<SupplierInfo>(s => s.IsActive || !s.IsActive);

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(EntityName), s => s.EntityProfile!.EntityName.Contains(EntityName!))
                .AndIf(!string.IsNullOrEmpty(SupplierID), s => s.SupplierID!.Contains(SupplierID!))
                .AndIf(!string.IsNullOrEmpty(SupplierName), s => s.SupplierName!.Contains(SupplierName!))
                .AndIf(IsActive.HasValue, s => s.IsActive == IsActive);

            var query = _dbcontext.SupplierInfos
                .Include(s => s.Account)
                .Include(s => s.EntityProfile)
                .Include(s => s.InvRoutingFlow)
                .AsNoTracking()
                .AsQueryable()
                .AsExpandable()
                .Where(predicate);

            if (string.IsNullOrEmpty(sortField))

            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery = query.Select(s => new SupplierSearchDto
            {
                Entity = s.EntityProfile!.EntityName,
                SupplierInfoID = s.SupplierInfoID,
                SupplierID = s.SupplierID,
                SupplierName = s.SupplierName,
                SupplierTaxID = s.SupplierTaxID,
                BankAccount = s.Account!.AccountName,
                PaymentTerms = s.PaymentTerms,
                InvRoutingFlowName = s.InvRoutingFlow!.InvRoutingFlowName,
                IsActive = s.IsActive,
            });

            var supplierPagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                 .ToPaginatedListAsync(pageNumber, pageSize, token);
            return supplierPagination;
        }
    }
}