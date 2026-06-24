using Azure.Core;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.GoodReceipts;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Contexts;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using LinqKit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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
            string? GoodReceipt,
            CancellationToken token)
        {
            ExpressionStarter<PurchaseOrder> predicate = PredicateBuilder.New<PurchaseOrder>(true);

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(EntityName), po => po.EntityProfile!.EntityName.Contains(EntityName!))

                .AndIf(!string.IsNullOrEmpty(PONo), po =>
                po.PoNo!.Contains(PONo!))

                .AndIf(!string.IsNullOrEmpty(Supplier), po =>
                po.SupplierInfo!.SupplierName!.Contains(Supplier!))

                .AndIf(IsActive.HasValue, po => po.IsActive == IsActive)

                           .AndIf(!string.IsNullOrEmpty(GoodReceipt),
                po => po.GoodsReceiptLines!
                         .Any(grl =>
                             grl.GoodsReceipt != null &&
                             grl.GoodsReceipt.GoodsReceiptNumber.Contains(GoodReceipt!)
                 ));

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
                Active = p.IsActive!.Value ? "Yes" : "No",
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
            string? GoodReceipt,
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

                .AndIf(IsActive.HasValue, po => po.IsActive == IsActive)

                .AndIf(!string.IsNullOrEmpty(GoodReceipt),
                po => po.GoodsReceiptLines!
                         .Any(grl =>
                             grl.GoodsReceipt != null &&
                             grl.GoodsReceipt.GoodsReceiptNumber.Contains(GoodReceipt!)
                 ));

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
                PurchaseOrderID = p.PurchaseOrderID,
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
                 .Include(x=>x.GoodsReceiptLines)
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
                       .SelectMany(po => po.PurchaseOrderLines!.Select(line =>                        
                            new
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
                            }
                          ))
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

        public async Task<PurchaseOrderHeaderDto> GetPurchaseOrderByID(long purchaseOrderId)
        {
            // CTE equivalent
            var tempGoodsReceiptLine =
                from grl in _dbcontext.GoodsReceiptLines
                select new
                {
                    grl.PurchaseOrderNo,
                    grl.GoodsReceiptID
                };

            var result = new PurchaseOrderHeaderDto();

            result = await
                (from po in _dbcontext.PurchaseOrders
                 where po.PurchaseOrderID == purchaseOrderId

                 // LEFT JOIN TempGoodsReceiptLine
                 join grl in tempGoodsReceiptLine
                     on po.PoNo equals grl.PurchaseOrderNo
                     into grlJoin
                 from grl in grlJoin.DefaultIfEmpty()

                     // LEFT JOIN GoodReceipts
                 join gr in _dbcontext.GoodsReceipts
                     on grl.GoodsReceiptID equals gr.GoodsReceiptID
                     into grJoin
                 from gr in grJoin.DefaultIfEmpty()

                     // LEFT JOIN EntityProfile
                 join ep in _dbcontext.EntityProfiles
                     on po.EntityProfileID equals ep.EntityProfileID
                     into epJoin
                 from ep in epJoin.DefaultIfEmpty()

                     // LEFT JOIN SupplierInfo
                 join si in _dbcontext.SupplierInfos
                     on po.SupplierInfoID equals si.SupplierInfoID
                     into siJoin
                 from si in siJoin.DefaultIfEmpty()

                 select new PurchaseOrderHeaderDto
                 {
                     Entity = ep.EntityName,
                     SupplierName = si.SupplierName,
                     SupplierNo = si.SupplierID,
                     PurchaseOrderNo = po.PoNo,
                     PurchaseDate = po.PurchaseDate.ToString("dd/MM/yyyy"),
                     GoodsReceiptNo = gr.GoodsReceiptNumber,
                     GoodsReceiptDate = gr.DeliveryDate!.Value.ToString("dd/MM/yyyy"),
                     Currency = po.Currency,
                     Keyword1 = po.MatchReference1,
                     Keyword2 = po.MatchReference2,
                     FreeField1 = po.FreeField1,
                     FreeField2 = po.FreeField2,
                     FreeField3 = po.FreeField3,
                     Status = (po.IsActive == true) ? "Active" : "Inactive",
                     OrderNotes = po.Note,
                     PurchaseOrderAmount = po.NetAmount.ToString(),
                     MatchStatus = (po.MatchStatus == MatchingStatus.FullyMatched) ? "Full Matched" : (po.MatchStatus == MatchingStatus.PartiallyMatched) ? "Partially Matched" : "Unmatched"
                 }).FirstOrDefaultAsync();

            /*
               UnMatched = 0,
        FullyMatched = 1,
        PartiallyMatched = 2
             */
            //Get the sum

            var sumItems = await this.GetSumOfPurchaseOrderLineList(purchaseOrderId);

            result.SumInvoiceAmount = sumItems.Item1.ToString();
            result.SumGoodReceivedAmount = sumItems.Item2.ToString();
            result.OutstandingAmount = sumItems.Item3.ToString();

            return result ?? new PurchaseOrderHeaderDto();

        }

        private async Task<(decimal, decimal, decimal)> GetSumOfPurchaseOrderLineList(long purchaseOrderId)
        {
            var getPO = _dbcontext.PurchaseOrders.FirstOrDefault(x => x.PurchaseOrderID == purchaseOrderId)?.PoNo;

            var grResult = _dbcontext.GoodsReceiptLines.Where(x => x.PurchaseOrderNo == getPO).Select(x => new grlineDTO
            {
                GoodsReceiptLineID = x.GoodsReceiptLineID,
                GoodsReceipt = x.GoodsReceipt,
                PurchaseOrder = x.PurchaseOrder,
                GoodsReceiptID = x.GoodsReceiptID,
                LineNo = x.LineNo,
                Qty = x.Qty,
                Amount = x.Amount,
                SupplierNo = x.SupplierNo,
                PurchaseOrderNo = x.PurchaseOrderNo,
                ReceiptNo = x.ReceiptNo,
                FreeField1 = x.FreeField1,
                FreeField2 = x.FreeField2,
                FreeField3 = x.FreeField3,
                InvoiceStatus = x.InvoiceStatus,
            });

            var result =
        from pol in _dbcontext.PurchaseOrderLines
        join po in _dbcontext.PurchaseOrders
            on pol.PurchaseOrderID equals po.PurchaseOrderID
        // LEFT JOIN GoodsReceiptLine ON PoNo + LineNo
        join grl in grResult
            on new { PoNo = po.PoNo, LineNo = pol.LineNo }
            equals new { PoNo = grl.PurchaseOrderNo, LineNo = grl.LineNo } into grlGroup
        from grl in grlGroup.DefaultIfEmpty()

        join polMatching in _dbcontext.PurchaseOrderMatchTrackings
            on pol.PurchaseOrderLineID equals polMatching.PurchaseOrderLineID into polMarchingGroup
        from polMatching in polMarchingGroup.DefaultIfEmpty()



            // LEFT JOIN GoodsReceipts
        join gr in _dbcontext.GoodsReceipts
            on grl.GoodsReceiptID equals gr.GoodsReceiptID into grGroup
        from gr in grGroup.DefaultIfEmpty()

        where po.PurchaseOrderID == purchaseOrderId

        select new PurchaseHeaderLineDetailsDto
        {
            InvoiceAmount = polMatching.Invoice.NetAmount == null ? 0 : (pol.NetAmount ?? 0 - polMatching.Invoice.NetAmount),
            GoodReceiveAmount = pol.Price ?? 0 * (grl.Qty == null ? 0 : grl.Qty),
            OutstandingAmount = (pol.NetAmount - (polMatching.Invoice.NetAmount == null ? 0 : (pol.NetAmount ?? 0 - polMatching.Invoice.NetAmount))) ?? 0
        };

            var invoiceAmountSum = result.Sum(x => x.InvoiceAmount);
            var goodReceiveAmountSum = result.Sum(x => x.GoodReceiveAmount);
            var outstandingAmountSum = result.Sum(x => x.OutstandingAmount);


            return (invoiceAmountSum, goodReceiveAmountSum, outstandingAmountSum);
        }

 


        public async Task<PaginatedList<PurchaseHeaderLineDetailsDto>> GetPurchaseOrderListByID(long purchaseOrderId, int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,string? searchLine, CancellationToken token)
        {
            var getPO = _dbcontext.PurchaseOrders.FirstOrDefault(x => x.PurchaseOrderID == purchaseOrderId)?.PoNo;

            var grResult = _dbcontext.GoodsReceiptLines.Where(x => x.PurchaseOrderNo == getPO).Select(x => new grlineDTO
            {
               GoodsReceiptLineID = x.GoodsReceiptLineID,
                GoodsReceipt = x.GoodsReceipt,
                PurchaseOrder = x.PurchaseOrder,
                GoodsReceiptID = x.GoodsReceiptID,
                LineNo = x.LineNo,
                Qty = x.Qty,
                Amount = x.Amount,
                SupplierNo = x.SupplierNo,
                PurchaseOrderNo = x.PurchaseOrderNo,
                ReceiptNo = x.ReceiptNo,
                FreeField1 = x.FreeField1,
                FreeField2 = x.FreeField2,
                FreeField3 = x.FreeField3,
                InvoiceStatus = x.InvoiceStatus,
            });

            var result =
        from pol in _dbcontext.PurchaseOrderLines
        join po in _dbcontext.PurchaseOrders
            on pol.PurchaseOrderID equals po.PurchaseOrderID
        // LEFT JOIN GoodsReceiptLine ON PoNo + LineNo
        join grl in grResult
            on new { PoNo = po.PoNo, LineNo = pol.LineNo }
            equals new { PoNo = grl.PurchaseOrderNo, LineNo = grl.LineNo } into grlGroup
        from grl in grlGroup.DefaultIfEmpty()

        join polMatching in _dbcontext.PurchaseOrderMatchTrackings
            on pol.PurchaseOrderLineID equals polMatching.PurchaseOrderLineID into polMarchingGroup from polMatching in polMarchingGroup.DefaultIfEmpty()


                   
            // LEFT JOIN GoodsReceipts
        join gr in _dbcontext.GoodsReceipts
            on grl.GoodsReceiptID equals gr.GoodsReceiptID into grGroup
        from gr in grGroup.DefaultIfEmpty()

        where po.PurchaseOrderID == purchaseOrderId

        select new PurchaseHeaderLineDetailsDto
        {
            PurchaseOrderLineID = pol.PurchaseOrderLineID,
            PurchaseNumber = po.PoNo,
            LineNumber = pol.LineNo,
            Item = pol.Item,
            Description = pol.Description,
            POOrderQuantity = pol.Qty,

            GoodsReceiptNo = gr.GoodsReceiptNumber,
            GRReceiptedQuantity = grl.Qty,

            GRReceiptDateDisplayString = gr.DeliveryDate != null
                ? gr.DeliveryDate.Value.UtcDateTime.ToString("yyyy-MM-dd")
                : null,

            GRReceiptDate = gr.DeliveryDate,

            VarianceQuantity = pol.Qty - ((grl.Qty == null ? 0 : grl.Qty)),

            UnitType = pol.Unit,
            UnitPrice = pol.Price ?? 0,
            POOrderAmount = pol.NetAmount ?? 0,

            POReceiptedAmount = grl.Amount,
            VarianceAmount = (pol.NetAmount) - ((grl.Amount == null ? 0 : grl.Amount)),

            LineCurrency = po.Currency,

            GoodReceiptedStatus =
                pol.DeliveryStatus == (int)POLineDeliveryStatus.PartiallyDelivered
                    ? "Partially Delivered"
                    : pol.DeliveryStatus == (int)POLineDeliveryStatus.FullyDelivered
                        ? "Fully Delivered"
                        : "Not Delivered",

            InvoiceMatchStatus =
                pol.InvoiceStatus == (int)InvoicePOMatchingStatus.PartiallyMatched
                    ? "Partially Matched"
                    : pol.InvoiceStatus == (int)InvoicePOMatchingStatus.FullyMatched
                        ? "Fully Matched"
                        : "Unmatched",
            InvoiceNo = polMatching.Invoice.InvoiceNo,
            InvoiceId = polMatching.Invoice.InvoiceID,
            InvoiceAmount = polMatching.Invoice.NetAmount == null ? 0 : (pol.NetAmount ?? 0 - polMatching.Invoice.NetAmount),
            GoodReceiveAmount = pol.Price ?? 0 * (grl.Qty == null ? 0 : grl.Qty),
            OutstandingAmount = (pol.NetAmount - (polMatching.Invoice.NetAmount == null ? 0 : (pol.NetAmount ?? 0 - polMatching.Invoice.NetAmount))) ?? 0
        };

            if (sortField == "grReceiptDateDisplayString")
            {
                sortField = "GRReceiptDate";
            }

            ExpressionStarter<PurchaseHeaderLineDetailsDto> predicate
          = PredicateBuilder.New<PurchaseHeaderLineDetailsDto>(true);

            if (!string.IsNullOrEmpty(searchLine))
            {

                predicate.And(x => (x.LineNumber.ToString()
                + x.Item
                + x.Description
                + x.POOrderQuantity.ToString()
                + x.UnitType
                + x.UnitPrice.ToString()
                + x.GoodsReceiptNo
                + x.POOrderAmount.ToString()
                + x.LineCurrency
                + x.GRReceiptedQuantity.ToString()
                + x.POReceiptedAmount.ToString()
                + x.InvoiceAmount.ToString()
                + x.VarianceAmount.ToString()
                + x.InvoiceMatchStatus
                + x.InvoiceNo
                + x.GoodReceiveAmount.ToString()
                + x.OutstandingAmount.ToString()
                ).Contains(searchLine));

                result = result.Where(predicate);
            }
         

            var finalResult = await
                result.OrderByDynamic(sortField, sortOrder)
                .ToPaginatedListAsync(pageNumber, pageSize, token);

            return finalResult;
        }



        public async Task<PaginatedList<BatchListPurchaseOrderDto>> BatchListPurchaseOrder(
        string? EntityName,
        string? PONo,
        string? Supplier,
        bool? IsActive,
        string? GoodReceipt,
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

                .AndIf(IsActive.HasValue, po => po.IsActive == IsActive)

                .AndIf(!string.IsNullOrEmpty(GoodReceipt),
                po => po.GoodsReceiptLines!
                         .Any(grl =>
                             grl.GoodsReceipt != null &&
                             grl.GoodsReceipt.GoodsReceiptNumber.Contains(GoodReceipt!)
                 ));

            var query = _dbcontext.PurchaseOrders
                .Include(p => p.SupplierInfo)
                .Include(p => p.EntityProfile)
                .AsNoTracking()
                .Where(predicate);

            if (string.IsNullOrEmpty(sortField))
            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoPOSearch = query.Select(p => new BatchListPurchaseOrderDto
            {
                EntityName = p.EntityProfile!.EntityName!,
                PurchaseOrderID = p.PurchaseOrderID,
                PoNo = p.PoNo,
                SupplierName = p.SupplierInfo!.SupplierName!,
                SupplierTaxID = p.SupplierTaxID,
                Currency = p.Currency,
                NetAmount = p.NetAmount,
                IsActive = p.IsActive
            });

            var poSearchPagination = await
                dtoPOSearch.OrderByDynamic(sortField, sortOrder)
                .ToPaginatedListAsync(pageNumber, pageSize, token);

            int index = 0;

            foreach (var item in poSearchPagination.Data)
            {
                index++;

                item.IndexId = index;
            }

            return poSearchPagination;
        }

        public Task<List<ExportPoDetailSearchDto>> ExportPoDetailSearch(long? PurchaseOrderId, string? searchLine, CancellationToken token)
        {
            var getPO = _dbcontext.PurchaseOrders.FirstOrDefault(x => x.PurchaseOrderID == PurchaseOrderId)?.PoNo;

            var grResult = _dbcontext.GoodsReceiptLines.Where(x => x.PurchaseOrderNo == getPO).Select(x => new grlineDTO
            {
                GoodsReceiptLineID = x.GoodsReceiptLineID,
                GoodsReceipt = x.GoodsReceipt,
                PurchaseOrder = x.PurchaseOrder,
                GoodsReceiptID = x.GoodsReceiptID,
                LineNo = x.LineNo,
                Qty = x.Qty,
                Amount = x.Amount,
                SupplierNo = x.SupplierNo,
                PurchaseOrderNo = x.PurchaseOrderNo,
                ReceiptNo = x.ReceiptNo,
                FreeField1 = x.FreeField1,
                FreeField2 = x.FreeField2,
                FreeField3 = x.FreeField3,
                InvoiceStatus = x.InvoiceStatus,
            });

            var result =
        from pol in _dbcontext.PurchaseOrderLines
        join po in _dbcontext.PurchaseOrders
            on pol.PurchaseOrderID equals po.PurchaseOrderID
        // LEFT JOIN GoodsReceiptLine ON PoNo + LineNo
        join grl in grResult
            on new { PoNo = po.PoNo, LineNo = pol.LineNo }
            equals new { PoNo = grl.PurchaseOrderNo, LineNo = grl.LineNo } into grlGroup
        from grl in grlGroup.DefaultIfEmpty()

        join polMatching in _dbcontext.PurchaseOrderMatchTrackings
            on pol.PurchaseOrderLineID equals polMatching.PurchaseOrderLineID into polMarchingGroup
        from polMatching in polMarchingGroup.DefaultIfEmpty()



            // LEFT JOIN GoodsReceipts
        join gr in _dbcontext.GoodsReceipts
            on grl.GoodsReceiptID equals gr.GoodsReceiptID into grGroup
        from gr in grGroup.DefaultIfEmpty()

        where po.PurchaseOrderID == PurchaseOrderId

        select new ExportPoDetailSearchDto
        {
      
            PurchaseNumber = po.PoNo,
            LineNumber = pol.LineNo,
            Item = pol.Item,
            Description = pol.Description,
            POOrderQuantity = pol.Qty,

            GoodsReceiptNo = gr.GoodsReceiptNumber,
            GRReceiptedQuantity = grl.Qty,


            GRReceiptDateDisplayString = gr.DeliveryDate != null
                ? gr.DeliveryDate.Value.UtcDateTime.ToString("yyyy-MM-dd")
                : null,

            VarianceQuantity = pol.Qty - ((grl.Qty == null ? 0 : grl.Qty)),

            UnitType = pol.Unit,
            UnitPrice = pol.Price ?? 0,
            POOrderAmount = pol.NetAmount ?? 0,

            POReceiptedAmount = grl.Amount,
            VarianceAmount = (pol.NetAmount) - ((grl.Amount == null ? 0 : grl.Amount)),

            LineCurrency = po.Currency,

            GoodReceiptedStatus =
                pol.DeliveryStatus == (int)POLineDeliveryStatus.PartiallyDelivered
                    ? "Partially Delivered"
                    : pol.DeliveryStatus == (int)POLineDeliveryStatus.FullyDelivered
                        ? "Fully Delivered"
                        : "Not Delivered",

            InvoiceMatchStatus =
                pol.InvoiceStatus == (int)InvoicePOMatchingStatus.PartiallyMatched
                    ? "Partially Matched"
                    : pol.InvoiceStatus == (int)InvoicePOMatchingStatus.FullyMatched
                        ? "Fully Matched"
                        : "Unmatched",
            InvoiceNo = polMatching.Invoice.InvoiceNo,
            InvoiceAmount = polMatching.Invoice.NetAmount == null ? 0 : (pol.NetAmount ?? 0 - polMatching.Invoice.NetAmount),
            GoodReceiveAmount = pol.Price ?? 0 * (grl.Qty == null ? 0 : grl.Qty),
            OutstandingAmount = (pol.NetAmount - (polMatching.Invoice.NetAmount == null ? 0 : (pol.NetAmount ?? 0 - polMatching.Invoice.NetAmount))) ?? 0

        };

            ExpressionStarter<ExportPoDetailSearchDto> predicate
          = PredicateBuilder.New<ExportPoDetailSearchDto>(true);

            if (!string.IsNullOrEmpty(searchLine))
            {

                predicate.And(x => (x.LineNumber.ToString()
                    + x.Item
                    + x.Description
                    + x.POOrderQuantity.ToString()
                    + x.UnitType
                    + x.UnitPrice.ToString()
                    + x.GoodsReceiptNo
                    + x.POOrderAmount.ToString()
                    + x.LineCurrency
                    + x.GRReceiptedQuantity.ToString()
                    + x.POReceiptedAmount.ToString()
                    + x.InvoiceAmount.ToString()
                    + x.VarianceAmount.ToString()
                    + x.InvoiceMatchStatus
                    + x.InvoiceNo
                    + x.GoodReceiveAmount.ToString()
                    + x.OutstandingAmount.ToString()
                    ).Contains(searchLine));

                result = result.Where(predicate);
            }
            return result.ToListAsync(token);
        }
    }
}