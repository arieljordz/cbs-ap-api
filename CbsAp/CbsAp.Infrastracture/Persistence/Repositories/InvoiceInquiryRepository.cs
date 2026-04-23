using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.InvoiceInquiry;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Contexts;
using LinqKit;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class InvoiceInquiryRepository : IInvoiceInquiryRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public InvoiceInquiryRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<PaginatedList<InvoiceInquiryDto>> SearchInvoiceInquiryWithPagination(
        InvoiceInquirySearchDto dto,
        int pageNumber,
        int pageSize,
        string? sortField,
        int? sortOrder,
        CancellationToken token)
        {
            var allowedQueues = new[]
            {
                InvoiceQueueType.ExceptionQueue,
                InvoiceQueueType.RejectionQueue,
                InvoiceQueueType.MyInvoices
            };

            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(u => u.QueueType.HasValue && allowedQueues.Contains(u.QueueType.Value));

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(dto.SupplierName),
                    u => u.SupplierInfo != null && u.SupplierInfo.SupplierName.Contains(dto.SupplierName))

                .AndIf(!string.IsNullOrEmpty(dto.InvoiceNumber),
                    u => u.InvoiceNo.Contains(dto.InvoiceNumber))

                .AndIf(!string.IsNullOrEmpty(dto.PONumber),
                    u => u.PoNo.Contains(dto.PONumber))

                .AndIf(dto.Status.HasValue,
                    u => (int)u.StatusType == dto.Status.Value);

            DateTimeOffset? invoiceFrom = dto.InvoiceDateFrom?.Date;
            DateTimeOffset? invoiceTo = dto.InvoiceDateTo?.Date.AddDays(1).AddTicks(-1);

            DateTimeOffset? scanFrom = dto.ScanDateFrom?.Date;
            DateTimeOffset? scanTo = dto.ScanDateTo?.Date.AddDays(1).AddTicks(-1);

            predicate = predicate
                .AndIf(invoiceFrom.HasValue, u => u.InvoiceDate >= invoiceFrom.Value)
                .AndIf(invoiceTo.HasValue, u => u.InvoiceDate <= invoiceTo.Value)
                .AndIf(scanFrom.HasValue, u => u.ScanDate >= scanFrom.Value)
                .AndIf(scanTo.HasValue, u => u.ScanDate <= scanTo.Value);

            var query = _dbcontext.Invoices
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

            if (string.IsNullOrEmpty(sortField))
            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoList = await query.Select(e => new InvoiceInquiryDto
            {
                InvoiceID = e.InvoiceID,
                SupplierName = e.SupplierInfo != null ? e.SupplierInfo.SupplierName : null,
                InvoiceDate = e.InvoiceDate,
                InvoiceNumber = e.InvoiceNo,
                PONumber = e.PoNo,
                DueDate = e.DueDate,
                GrossAmount = e.TotalAmount.ToString("F2"),

                NextRole = e.QueueType == InvoiceQueueType.ExceptionQueue
                            ? string.Empty
                            : (e.InvInfoRoutingLevels != null
                                ? e.InvInfoRoutingLevels
                                    .Where(r => r.InvFlowStatus == (int)InvFlowStatus.Pending)
                                    .OrderBy(r => r.Level)
                                    .Select(r => r.Role != null ? r.Role.RoleName : null)
                                    .FirstOrDefault()
                                : null) ?? string.Empty,

                ExceptionReason = string.Join("; ", e.InvoiceActivityLog!
                    .Where(a => a.InvoiceID == e.InvoiceID &&
                                a.IsCurrentValidationContext == true &&
                                (a.Action == InvoiceActionType.Validate || a.Action == InvoiceActionType.Import) &&
                                !string.IsNullOrEmpty(a.Reason))
                    .Select(a => a.Reason) ?? Enumerable.Empty<string>()),

                Status = e.StatusType != null ? e.StatusType.ToString() : null
            }).ToListAsync(token);

            var result = await dtoList
                .OrderByDynamic(sortField, sortOrder)
                .ToPaginatedListAsync(pageNumber, pageSize, token);

            return result;
        }

        public Task<List<ExportInvoiceInquiryDto>> ExportInvoiceInquiryToExcel(
        string? SupplierName,
        string? InvoiceNumber,
        string? PONumber,
        int? Status,
        DateTimeOffset? ScanDateFrom,
        DateTimeOffset? ScanDateTo,
        DateTimeOffset? InvoiceDateFrom,
        DateTimeOffset? InvoiceDateTo,
        CancellationToken token)
        {
            var allowedQueues = new[]
            {
                InvoiceQueueType.ExceptionQueue,
                InvoiceQueueType.RejectionQueue,
                InvoiceQueueType.MyInvoices
            };

            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(
                u => u.QueueType.HasValue && allowedQueues.Contains(u.QueueType.Value)
            );

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(SupplierName),
                    u => u.SupplierInfo != null &&
                         u.SupplierInfo.SupplierName.Contains(SupplierName))

                .AndIf(!string.IsNullOrEmpty(InvoiceNumber),
                    u => u.InvoiceNo.Contains(InvoiceNumber))

                .AndIf(!string.IsNullOrEmpty(PONumber),
                    u => u.PoNo.Contains(PONumber))

                .AndIf(Status.HasValue,
                    u => (int)u.StatusType == Status.Value);

            DateTimeOffset? invoiceFrom = InvoiceDateFrom?.Date;
            DateTimeOffset? invoiceTo = InvoiceDateTo?.Date.AddDays(1).AddTicks(-1);
            DateTimeOffset? scanFrom = ScanDateFrom?.Date;
            DateTimeOffset? scanTo = ScanDateTo?.Date.AddDays(1).AddTicks(-1);

            predicate = predicate
                .AndIf(invoiceFrom.HasValue, u => u.InvoiceDate >= invoiceFrom.Value)
                .AndIf(invoiceTo.HasValue, u => u.InvoiceDate <= invoiceTo.Value)
                .AndIf(scanFrom.HasValue, u => u.ScanDate >= scanFrom.Value)
                .AndIf(scanTo.HasValue, u => u.ScanDate <= scanTo.Value);

            var query = _dbcontext.Invoices
                .AsNoTracking()
                .Include(x => x.SupplierInfo)
                .AsExpandable()
                .Where(predicate);

            var dtoSearchInvoiceInquiry = query.Select(e => new ExportInvoiceInquiryDto
            {
                InvoiceID = e.InvoiceID,
                SupplierName = e.SupplierInfo != null ? e.SupplierInfo.SupplierName : null,
                InvoiceDate = e.InvoiceDate,
                InvoiceNumber = e.InvoiceNo,
                PONumber = e.PoNo,
                DueDate = e.DueDate,
                GrossAmount = e.TotalAmount.ToString("F2"),

                NextRole = e.InvInfoRoutingLevels != null && e.InvInfoRoutingLevels.Any()
                    ? e.InvInfoRoutingLevels
                        .OrderByDescending(r => r.Level)
                        .Select(r => r.Role != null ? r.Role.RoleName : null)
                        .FirstOrDefault()
                    : "N/A",

                ExceptionReason = string.Join("; ", e.InvoiceActivityLog!
                    .Where(a => a.InvoiceID == e.InvoiceID &&
                                a.IsCurrentValidationContext == true &&
                                (a.Action == InvoiceActionType.Validate ||
                                 a.Action == InvoiceActionType.Import) &&
                                !string.IsNullOrEmpty(a.Reason))
                    .Select(a => a.Reason) ?? Enumerable.Empty<string>()),

                Status = e.StatusType != null ? e.StatusType.ToString() : null,
                ScanDate = e.ScanDate
            });

            return dtoSearchInvoiceInquiry.ToListAsync(token);
        }

    }
}