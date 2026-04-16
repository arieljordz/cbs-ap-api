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

            var predicate = PredicateBuilder.New<Invoice>(true);

            predicate = predicate.And(u => u.QueueType.HasValue && allowedQueues.Contains(u.QueueType.Value));

            if (!string.IsNullOrEmpty(dto.SupplierName))
            {
                predicate = predicate.And(u => u.SupplierInfo != null && u.SupplierInfo.SupplierName.Contains(dto.SupplierName));
            }

            if (!string.IsNullOrEmpty(dto.InvoiceNumber))
            {
                predicate = predicate.And(u => u.InvoiceNo.Contains(dto.InvoiceNumber));
            }

            if (!string.IsNullOrEmpty(dto.PONumber))
            {
                predicate = predicate.And(u => u.PoNo.Contains(dto.PONumber));
            }

            if (dto.Status.HasValue)
            {
                predicate = predicate.And(u => (int)u.StatusType == dto.Status.Value);
            }

            DateTimeOffset? invoiceFrom = dto.InvoiceDateFrom?.Date;
            DateTimeOffset? invoiceTo = dto.InvoiceDateTo?.Date.AddDays(1).AddTicks(-1);

            DateTimeOffset? scanFrom = dto.ScanDateFrom?.Date;
            DateTimeOffset? scanTo = dto.ScanDateTo?.Date.AddDays(1).AddTicks(-1);

            if (invoiceFrom.HasValue)
            {
                predicate = predicate.And(u => u.InvoiceDate >= invoiceFrom.Value);
            }

            if (invoiceTo.HasValue)
            {
                predicate = predicate.And(u => u.InvoiceDate <= invoiceTo.Value);
            }

            if (scanFrom.HasValue)
            {
                predicate = predicate.And(u => u.ScanDate >= scanFrom.Value);
            }

            if (scanTo.HasValue)
            {
                predicate = predicate.And(u => u.ScanDate <= scanTo.Value);
            }

            var query = _dbcontext.Invoices
                .AsNoTracking()
                .Include(x => x.SupplierInfo)
                .Where(predicate);

            if (string.IsNullOrEmpty(sortField))
            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery = query.Select(e => new InvoiceInquiryDto
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
                                                    (a.Action == InvoiceActionType.Validate || a.Action == InvoiceActionType.Import) &&
                                                    !string.IsNullOrEmpty(a.Reason))
                                        .Select(a => a.Reason) ?? Enumerable.Empty<string>()),
                Status = e.StatusType != null ? e.StatusType.ToString() : null
            });

            var result = await dtoQuery
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

            var predicate = PredicateBuilder.New<Invoice>(true);

            predicate = predicate.And(u =>
                u.QueueType.HasValue && allowedQueues.Contains(u.QueueType.Value));

            if (!string.IsNullOrEmpty(SupplierName))
                predicate = predicate.And(u => u.SupplierInfo != null &&
                    u.SupplierInfo.SupplierName.Contains(SupplierName));

            if (!string.IsNullOrEmpty(InvoiceNumber))
                predicate = predicate.And(u => u.InvoiceNo.Contains(InvoiceNumber));

            if (!string.IsNullOrEmpty(PONumber))
                predicate = predicate.And(u => u.PoNo.Contains(PONumber));

            if (Status.HasValue)
                predicate = predicate.And(u => (int)u.StatusType == Status.Value);

            DateTimeOffset? invoiceFrom = InvoiceDateFrom?.Date;
            DateTimeOffset? invoiceTo = InvoiceDateTo?.Date.AddDays(1).AddTicks(-1);
            DateTimeOffset? scanFrom = ScanDateFrom?.Date;
            DateTimeOffset? scanTo = ScanDateTo?.Date.AddDays(1).AddTicks(-1);

            if (invoiceFrom.HasValue)
                predicate = predicate.And(u => u.InvoiceDate >= invoiceFrom.Value);

            if (invoiceTo.HasValue)
                predicate = predicate.And(u => u.InvoiceDate <= invoiceTo.Value);

            if (scanFrom.HasValue)
                predicate = predicate.And(u => u.ScanDate >= scanFrom.Value);

            if (scanTo.HasValue)
                predicate = predicate.And(u => u.ScanDate <= scanTo.Value);

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
                                                    (a.Action == InvoiceActionType.Validate || a.Action == InvoiceActionType.Import) &&
                                                    !string.IsNullOrEmpty(a.Reason))
                                        .Select(a => a.Reason) ?? Enumerable.Empty<string>()),
                Status = e.StatusType != null ? e.StatusType.ToString() : null,
                ScanDate = e.ScanDate
            });

            return dtoSearchInvoiceInquiry.ToListAsync(token);
        }
    }
}