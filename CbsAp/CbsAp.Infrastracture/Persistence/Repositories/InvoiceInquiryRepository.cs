using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.InvoiceInquiry;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.Helpers;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Contexts;
using LinqKit;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

            var excludedQueues = new[]
            {
              InvoiceQueueType.ExceptionQueue,
              InvoiceQueueType.ArchiveQueue,
            };


            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(u => u.QueueType.HasValue && ! excludedQueues.Contains(u.QueueType.Value));



            predicate = predicate
            .AndIf(dto.SupplierInfoID.HasValue,
            u => u.SupplierInfo != null && u.SupplierInfo.SupplierInfoID == dto.SupplierInfoID.Value)



            .AndIf(!string.IsNullOrEmpty(dto.InvoiceNumber),
            u => u.InvoiceNo.Contains(dto.InvoiceNumber))



            .AndIf(!string.IsNullOrEmpty(dto.PONumber),
            u => u.PoNo.Contains(dto.PONumber))



            .AndIf(dto.RoleID.HasValue,
            u => u.ApproverRole == dto.RoleID.Value)



            .AndIf(dto.Status != null && dto.Status.Any(),
            u => u.StatusType.HasValue && dto.Status.Contains(u.StatusType.Value));



            DateTimeOffset? invoiceFrom = dto.InvoiceDateFrom?.Date;
            DateTimeOffset? invoiceTo = dto.InvoiceDateTo?.Date.AddDays(1).AddTicks(-1);



            DateTimeOffset? dueFrom = dto.InvoiceDueDateFrom?.Date;
            DateTimeOffset? dueTo = dto.InvoiceDueDateTo?.Date.AddDays(1).AddTicks(-1);



            DateTimeOffset? paymentFrom = dto.PaymentDateFrom?.Date;
            DateTimeOffset? paymentTo = dto.PaymentDateTo?.Date.AddDays(1).AddTicks(-1);



            DateTimeOffset? scanFrom = dto.ScanDateFrom?.Date;
            DateTimeOffset? scanTo = dto.ScanDateTo?.Date.AddDays(1).AddTicks(-1);



            predicate = predicate
            .AndIf(invoiceFrom.HasValue, u => u.InvoiceDate >= invoiceFrom.Value)
            .AndIf(invoiceTo.HasValue, u => u.InvoiceDate <= invoiceTo.Value)
            .AndIf(dueFrom.HasValue, u => u.DueDate >= dueFrom.Value)
            .AndIf(dueTo.HasValue, u => u.DueDate <= dueTo.Value)
           //.AndIf(paymentFrom.HasValue, u => u.PaymentDate >= paymentFrom.Value)
           //.AndIf(paymentTo.HasValue, u => u.PaymentDate <= paymentTo.Value)
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
                //PaymentDate = e.PaymentDate,
                ScanDate = e.ScanDate,
                Status = e.StatusType != null ? e.StatusType.ToString() : null,
                Role = e.ApproverInvoices != null ? e.ApproverInvoices.RoleName : string.Empty,
                ApprovedBy = e.ApprovedUserInvoices != null ? $"{e.ApprovedUserInvoices.FirstName} {e.ApprovedUserInvoices.LastName}" : string.Empty
               
            }).ToListAsync(token);



            var result = await dtoList
            .OrderByDynamic(sortField, sortOrder)
            .ToPaginatedListAsync(pageNumber, pageSize, token);



            return result;
        }



        public Task<List<ExportInvoiceInquiryDto>> ExportInvoiceInquiryToExcel(
        int? SupplierInfoID,
        string? InvoiceNumber,
        string? PONumber,
        int? RoleID,
        List<InvoiceStatusType>? Status,
        DateTimeOffset? InvoiceDateFrom,
        DateTimeOffset? InvoiceDateTo,
        DateTimeOffset? InvoiceDueDateFrom,
        DateTimeOffset? InvoiceDueDateTo,
        DateTimeOffset? PaymentDateFrom,
        DateTimeOffset? PaymentDateTo,
        DateTimeOffset? ScanDateFrom,
        DateTimeOffset? ScanDateTo,
        CancellationToken token)
        {

            var excludedQueues = new[]
            {
              InvoiceQueueType.ExceptionQueue,
              InvoiceQueueType.ArchiveQueue,
            };



            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(u => u.QueueType.HasValue && !excludedQueues.Contains(u.QueueType.Value));



            predicate = predicate
            .AndIf(SupplierInfoID.HasValue,
            u => u.SupplierInfo != null &&
            u.SupplierInfo.SupplierInfoID == SupplierInfoID.Value)



            .AndIf(!string.IsNullOrEmpty(InvoiceNumber),
            u => u.InvoiceNo.Contains(InvoiceNumber))



            .AndIf(!string.IsNullOrEmpty(PONumber),
            u => u.PoNo.Contains(PONumber))



            .AndIf(RoleID.HasValue,
            u => u.ApproverRole == RoleID.Value)



            .AndIf(Status != null && Status.Any(),
            u => u.StatusType.HasValue && Status.Contains(u.StatusType.Value));



            DateTimeOffset? invoiceFrom = InvoiceDateFrom?.Date;
            DateTimeOffset? invoiceTo = InvoiceDateTo?.Date.AddDays(1).AddTicks(-1);
            DateTimeOffset? dueFrom = InvoiceDueDateFrom?.Date;
            DateTimeOffset? dueTo = InvoiceDueDateTo?.Date.AddDays(1).AddTicks(-1);
            DateTimeOffset? paymentFrom = PaymentDateFrom?.Date;
            DateTimeOffset? paymentTo = PaymentDateTo?.Date.AddDays(1).AddTicks(-1);
            DateTimeOffset? scanFrom = ScanDateFrom?.Date;
            DateTimeOffset? scanTo = ScanDateTo?.Date.AddDays(1).AddTicks(-1);



            predicate = predicate
            .AndIf(invoiceFrom.HasValue, u => u.InvoiceDate >= invoiceFrom.Value)
            .AndIf(invoiceTo.HasValue, u => u.InvoiceDate <= invoiceTo.Value)
            .AndIf(dueFrom.HasValue, u => u.DueDate >= dueFrom.Value)
            .AndIf(dueTo.HasValue, u => u.DueDate <= dueTo.Value)
           //.AndIf(paymentFrom.HasValue, u => u.PaymentDate >= paymentFrom.Value)
           //.AndIf(paymentTo.HasValue, u => u.PaymentDate <= paymentTo.Value)
                           .AndIf(scanFrom.HasValue, u => u.ScanDate >= scanFrom.Value)
            .AndIf(scanTo.HasValue, u => u.ScanDate <= scanTo.Value);



            var query = _dbcontext.Invoices
            .AsNoTracking()
            .Include(x => x.SupplierInfo)
            .Include(x => x.ApproverInvoices)
            .AsExpandable()
            .Where(predicate);



            var dtoSearchInvoiceInquiry = query.Select(e => new ExportInvoiceInquiryDto
            {
                InvoiceID = e.InvoiceID,
                SupplierName = e.SupplierInfo != null ? e.SupplierInfo.SupplierName : null,
                InvoiceDate = e.InvoiceDate!.Value.LocalDateTime.ToString("yyyy-MM-dd"),//ToPhilippineTime()!.Value.ToString("yyyy-MM-dd"),
                InvoiceNumber = e.InvoiceNo,
                PONumber = e.PoNo,
                DueDate = e.DueDate!.Value.LocalDateTime.ToString("yyyy-MM-dd"),
                GrossAmount = e.TotalAmount.ToString("F2"),
                //PaymentDate = e.PaymentDate,
                ScanDate = e.ScanDate!.Value.LocalDateTime.ToString("yyyy-MM-dd"),
                Status = e.StatusType != null ? e.StatusType.ToString() : null,
                Role = e.ApproverInvoices != null ? e.ApproverInvoices.RoleName : string.Empty,
                ApprovedBy = e.ApprovedUserInvoices != null ? $"{e.ApprovedUserInvoices.FirstName} {e.ApprovedUserInvoices.LastName}" : string.Empty
            });



            return dtoSearchInvoiceInquiry.ToListAsync(token);
        }



    }
}