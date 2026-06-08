using Azure.Core;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Contexts;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Vml;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public PurchaseOrderRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public Task<List<ExportPoSearchDto>> ExportPoSearch(
            string? EntityName, 
            string? PONo, string? 
            Supplier,
            bool? IsActive,
            CancellationToken token)
        {
            ExpressionStarter<PurchaseOrder> predicate = PredicateBuilder.New<PurchaseOrder>(true);

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(EntityName), po => po.EntityProfile!.EntityName.Contains(EntityName!))

                .AndIf(!string.IsNullOrEmpty(PONo), po =>
                po.PoNo!.Contains(PONo!))

                .AndIf(!string.IsNullOrEmpty(Supplier), po =>
                po.SupplierInfo!.SupplierName!.Contains(Supplier!))

                .AndIf(IsActive.HasValue, po => po.IsActive == IsActive);

            var query = _dbcontext.PurchaseOrders
                .Include(p => p.SupplierInfo)
                .Include(p => p.EntityProfile)
                .AsNoTracking()
                .Where(predicate);
            var dtoPOSearch = query.Select(p => new ExportPoSearchDto
            {
                EntityName = p.EntityProfile!.EntityName,
                PoNo = p.PoNo,
                SupplierName = p.SupplierInfo!.SupplierName,
                SupplierTaxID = p.SupplierTaxID,
                Currency = p.Currency,
                NetAmount = p.NetAmount,
                Active = p.IsActive!.Value  ? "Yes" : "No",
            });

            return dtoPOSearch.ToListAsync(token);
        }

        public async Task<List<SearchPoLinesDto>> GetPOMatchingByInvID(string PONo, long InvoiceID, CancellationToken cancellationToken)
        {
            ExpressionStarter<PurchaseOrderMatchTracking> predicate
                = PredicateBuilder.New<PurchaseOrderMatchTracking>(true);

            predicate = predicate.And(pm => pm.InvoiceID == InvoiceID);

            var query = _dbcontext.PurchaseOrderMatchTrackings
                .Include(p => p.PurchaseOrder)
                .Include(p => p.PurchaseOrderLine)
                .AsNoTracking()
                .AsExpandable()
                .AsQueryable()
                .Where(predicate);

            var totalMatchedQty = await _dbcontext.PurchaseOrderMatchTrackings
                .Where(x => x.PurchaseOrder.PoNo == PONo)
                .SumAsync(s => (decimal?)s.Qty, cancellationToken) ?? 0m;
            var dtoQuery = await query.Select(
            lines => new SearchPoLinesDto
            {
                PoLines = new List<PoLinesDto> { new PoLinesDto {
                   PurchaseOrderMatchTrackingID = lines.PurchaseOrderMatchTrackingID,
                    PurchaseOrderLineID = lines.PurchaseOrderLineID,
                    PurchaseOrderID = lines.PurchaseOrderID,
                    InvoiceID  = lines.InvoiceID,
                    InvAllocLineID = lines.InvAllocLineID,
                    PoNo = lines.PurchaseOrder!.PoNo,
                    SupplierNo = lines.PurchaseOrder!.SupplierNo,
                    LineNo  =lines.PurchaseOrderLine!.LineNo,
                    Description = lines.PurchaseOrderLine.Description,
                    AccountID = lines.PurchaseOrderLine.AccountID,
                    AccountName = lines.PurchaseOrderLine.Account!.AccountName,
                    OriginalQty = lines.PurchaseOrderLine.Qty,
                    Qty = lines.Qty,
                    BasedQty = lines.Qty,
                    Price = lines.PurchaseOrderLine.Price,
                    Amount = lines.Qty * lines.PurchaseOrderLine.Price,
                    NetAmount = lines.NetAmount,
                    TaxAmount =  lines.TaxAmount,
                    Status = lines.MatchingStatus,
                    BaseRemainingQty = lines.PurchaseOrderLine.Qty -  _dbcontext.PurchaseOrderMatchTrackings
                            .Where(x => x.PurchaseOrderLineID == lines.PurchaseOrderLineID )
                            .Sum(s => s.Qty),

                    RemainingQty  = lines.PurchaseOrderLine.Qty -  _dbcontext.PurchaseOrderMatchTrackings
                            .Where(x => x.PurchaseOrderLineID == lines.PurchaseOrderLineID )
                            .Sum(s => s.Qty),
                    IsAvailableOrder = true,
                    isForEditPOMatching = true,
                    TotalMatchedQty = lines.Qty,
                 } }
            }
             ).ToListAsync(cancellationToken);

            return dtoQuery.Count > 0 ? dtoQuery : null;
        }

        public async Task<PaginatedList<POSearchDto>> PoSearch(
            string? EntityName,
            string? PONo,
            string? Supplier,
            bool? IsActive,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token)
        {
            ExpressionStarter<PurchaseOrder> predicate = PredicateBuilder.New<PurchaseOrder>(true);

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(EntityName), po => po.EntityProfile!.EntityName.Contains(EntityName!))

                .AndIf(!string.IsNullOrEmpty(PONo), po =>
                po.PoNo!.Contains(PONo!))

                .AndIf(!string.IsNullOrEmpty(Supplier), po =>
                po.SupplierInfo!.SupplierName!.Contains(Supplier!))

                .AndIf(IsActive.HasValue, po => po.IsActive == IsActive);

            var query = _dbcontext.PurchaseOrders
                .Include(p => p.SupplierInfo)
                .Include(p => p.EntityProfile)
                .AsNoTracking()
                .Where(predicate);

            if (string.IsNullOrEmpty(sortField))
            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoPOSearch = query.Select(p => new POSearchDto
            {
                EntityName = p.EntityProfile!.EntityName,
                PoNo = p.PoNo,
                SupplierName = p.SupplierInfo!.SupplierName,
                SupplierTaxID = p.SupplierTaxID,
                Currency = p.Currency,
                NetAmount = p.NetAmount,
                IsActive = p.IsActive!
            });

            var poSearchPagination = await
                dtoPOSearch.OrderByDynamic(sortField, sortOrder)
                .ToPaginatedListAsync(pageNumber, pageSize, token);

            return poSearchPagination;
        }

        public async Task<List<SearchPoLinesDto>> SearchPoLines(
            string? SupplierName,
            string? SupplierTaxID,
            string? PONo,
            DateTime? PODateFrom,
            DateTime? PODateTo,
            string? DeliveryNo,
            string? supplierNo,
            List<long>? ExcludesMatchPOLineIds,
            bool IsAvailableOrder,
            CancellationToken token)
        {
            ExpressionStarter<PurchaseOrder> predicate
                 = PredicateBuilder.New<PurchaseOrder>(true);

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(SupplierName), p => p.SupplierInfo!.SupplierName! == SupplierName)

            .AndIf(!string.IsNullOrEmpty(SupplierTaxID), p => p.SupplierInfo!.SupplierTaxID! == SupplierTaxID!)

            .AndIf(!string.IsNullOrEmpty(PONo), p => p.PoNo! == PONo)

             .AndIf(PODateFrom.HasValue, p => p.PurchaseDate >= PODateFrom!.Value)
             .AndIf(PODateTo.HasValue, p => p.PurchaseDate <= PODateTo!.Value)
             .AndIf(!string.IsNullOrEmpty(supplierNo), p => p.SupplierNo!.Contains(supplierNo!));

            // .AndIf(!string.IsNullOrEmpty(DeliveryNo), p => p.PoNo!.Contains(DeliveryNo!));
            //.AndIf(!string.IsNullOrEmpty(Keyword), p => p.PoNo!.Contains(Keyword!));

            var query =
                 _dbcontext.PurchaseOrders
                 .Include(s => s.PurchaseOrderLines)
                 .Include(m => m.PurchaseOrderMatchTrackings)
                 .Include(s => s.SupplierInfo)
                 .AsNoTracking()
                 .AsQueryable()
                 .AsExpandable()
                 .Where(predicate)
                 .Where(p => p.PurchaseOrderLines!.Any(a => a.IsActive == true))
                ;

            var results = new List<SearchPoLinesDto>();

            if (IsAvailableOrder)
            {
                var tempResults = await query
                       .SelectMany(po => po.PurchaseOrderLines!.Select(line => new
                       {
                           PurchaseOrderLine = line,
                           AccountID = line.AccountID,
                           AccountName = line.Account!.AccountName,
                           PONo = po.PoNo,
                           SupplierNo = po.SupplierNo,
                           PurchaseOrderID = po.PurchaseOrderID,
                           OriginalQty = line.Qty,
                           TotalMatchedQty = po.PurchaseOrderMatchTrackings!
                                     .Where(m => m.PurchaseOrderLineID == line.PurchaseOrderLineID)
                                     .Sum(m => (decimal?)m.Qty) ?? 0m
                       }))
                       .Select(r => new SearchPoLinesDto
                       {
                           PoLines = new List<PoLinesDto>
                           {
                               new PoLinesDto {
                                PurchaseOrderLineID = r.PurchaseOrderLine.PurchaseOrderLineID,
                                PoNo = r.PONo,
                                SupplierNo = r.SupplierNo,
                                AccountID = r.AccountID,
                                AccountName = r.AccountName,
                                TotalMatchedQty = r.TotalMatchedQty,
                                BaseRemainingQty = r.OriginalQty - r.TotalMatchedQty,
                                RemainingQty = r.OriginalQty - r.TotalMatchedQty,
                                OriginalQty = r.OriginalQty,
                                LineNo = r.PurchaseOrderLine.LineNo,
                                PurchaseOrderID =  r.PurchaseOrderID,
                                Description =  r.PurchaseOrderLine.Description,
                                Price =  r.PurchaseOrderLine.Price,
                                Amount  =  (r.OriginalQty - r.TotalMatchedQty) * r.PurchaseOrderLine.Price,
                                BasedQty=0,
                                Qty = r.OriginalQty - r.TotalMatchedQty,
                               }
                           }
                       })
                       .ToListAsync(token);

                results = tempResults
                   .Where(y => y.PoLines!.Any(x => x.BaseRemainingQty > 0))
                   .OrderBy(y => y.PoLines!.First().LineNo)
                   .ToList();
            }
            else
            {
                var tempResults = await query
                       .SelectMany(po => po.PurchaseOrderLines!.Select(line => new
                       {
                           PurchaseOrderLine = line,
                           AccountID = line.AccountID,
                           AccountName = line.Account!.AccountName,
                           PONo = po.PoNo,
                           SupplierNo = po.SupplierNo,
                           PurchaseOrderID = po.PurchaseOrderID,
                           OriginalQty = line.Qty,
                           TotalMatchedQty = po.PurchaseOrderMatchTrackings!
                                     .Where(m => m.PurchaseOrderLineID == line.PurchaseOrderLineID)
                                     .Sum(m => (decimal?)m.Qty) ?? 0m,
                           IsFullyMatched = po.PurchaseOrderMatchTrackings!
                                     .Any(m => m.PurchaseOrderLineID == line.PurchaseOrderLineID
                                     && m.MatchingStatus == POMatchingStatus.FullyMatched)
                       }))
                       .Select(r => new SearchPoLinesDto
                       {
                           PoLines = new List<PoLinesDto>
                           {
                               new PoLinesDto {
                                PurchaseOrderLineID = r.PurchaseOrderLine.PurchaseOrderLineID,
                                PoNo = r.PONo,
                                SupplierNo = r.SupplierNo,
                                AccountID = r.AccountID,
                                AccountName = r.AccountName,
                                TotalMatchedQty = r.TotalMatchedQty,
                                BaseRemainingQty = r.OriginalQty - r.TotalMatchedQty,
                                RemainingQty = r.OriginalQty - r.TotalMatchedQty,

                                OriginalQty = r.OriginalQty,
                                LineNo = r.PurchaseOrderLine.LineNo,
                                PurchaseOrderID =  r.PurchaseOrderID,
                                Description =  r.PurchaseOrderLine.Description,
                                Price =   r.PurchaseOrderLine.Price,
                                Amount = (r.OriginalQty - r.TotalMatchedQty) * r.PurchaseOrderLine.Price,
                                BasedQty=0,
                                Qty = r.OriginalQty - r.TotalMatchedQty,
                               }
                           }
                       })
                       .ToListAsync(token);

                results = tempResults
                   .OrderBy(y => y.PoLines!.First().LineNo)
                   .ToList();
            }

            return results;
        }
    }
}