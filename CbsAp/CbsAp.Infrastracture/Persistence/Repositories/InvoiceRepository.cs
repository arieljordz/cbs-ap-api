using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.InvInfoRoutingLevel;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.InvoicingArchive;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Contexts;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection.Metadata;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public InvoiceRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public Task<List<ExportArchiveInvoiceDto>> ExportArchiveInvoice(
            string? SupplierName,
            string? InvoiceNo,
            string? PONo,
            CancellationToken token)
        {
            ExpressionStarter<InvoiceArchive> predicate = PredicateBuilder.New<InvoiceArchive>(true);

            predicate = predicate
             .AndIf(!string.IsNullOrEmpty(SupplierName), s => s.SupplierInfo != null && s.SupplierInfo.SupplierName!.Contains(SupplierName!))
            .AndIf(!string.IsNullOrEmpty(InvoiceNo), s => s.InvoiceNo!.Contains(InvoiceNo!))
            .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!));

            var query = _dbcontext.InvoiceArchives
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

            var dtoQuery = query.Select(i => new ExportArchiveInvoiceDto
            {
                Entity = i.EntityProfile != null ? i.EntityProfile.EntityName : string.Empty,
                SuppName = i.SupplierInfo != null ? i.SupplierInfo.SupplierName : string.Empty,
                InvoiceNo = i.InvoiceNo,
                PoNo = i.PoNo,
                InvoiceDate = i.InvoiceDate.HasValue
                ? i.InvoiceDate.Value.ToString("yyyy-MM-dd")
                : null,
                DueDate = i.DueDate.HasValue
                ? i.DueDate.Value.ToString("yyyy-MM-dd")
                : null,
                GrossAmount = i.TotalAmount,
                ExceptionReason = null,
            });

            return dtoQuery.ToListAsync(token);
        }

        public Task<List<ExportExceptionInvoiceDto>> ExportExceptionInvoice(
            string? SupplierName,
            string? InvoiceNo,
            string? PONo,
            CancellationToken token)
        {
            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(i => i.StatusType == InvoiceStatusType.Exception
          || i.QueueType == InvoiceQueueType.ExceptionQueue);

            predicate = predicate
            .AndIf(!string.IsNullOrEmpty(SupplierName), s => s.SupplierInfo!.SupplierName!.Contains(SupplierName!))
            .AndIf(!string.IsNullOrEmpty(InvoiceNo), s => s.InvoiceNo!.Contains(InvoiceNo!))
            .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!))
            .And(s => s.QueueType == InvoiceQueueType.ExceptionQueue);

            var query = _dbcontext.Invoices
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

            var dtoQuery = query.Select(i => new ExportExceptionInvoiceDto
            {
                Entity = i.EntityProfile!.EntityName,
                SuppName = i.SupplierInfo!.SupplierName,
                InvoiceNo = i.InvoiceNo,
                PoNo = i.PoNo,
                InvoiceDate = i.InvoiceDate.HasValue
                ? i.InvoiceDate.Value.ToString("yyyy-MM-dd")
                : null,
                DueDate = i.DueDate.HasValue
                ? i.DueDate.Value.ToString("yyyy-MM-dd")
                : null,
                GrossAmount = i.TotalAmount,

                ExceptionReason = null
            });

            return dtoQuery.ToListAsync(token);
        }

        public Task<List<ExportMyInvoiceDto>> ExportMyInvoiceToExcel(
            string? SupplierName,
            string? InvoiceNo,
            string? PONo,
            CancellationToken token)
        {
            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(true);

            predicate = predicate
           .AndIf(!string.IsNullOrEmpty(SupplierName), s => s.SupplierInfo!.SupplierName!.Contains(SupplierName!))
           .AndIf(!string.IsNullOrEmpty(InvoiceNo), s => s.InvoiceNo!.Contains(InvoiceNo!))
           .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!))
           .And(s => s.QueueType == InvoiceQueueType.MyInvoices);

            var query = _dbcontext.Invoices
                .AsNoTracking()
                .AsExpandable()
                .AsQueryable()
                .Where(predicate);

            var dtoQuery = query.Select(i => new ExportMyInvoiceDto
            {
                Entity = i.EntityProfile!.EntityName,
                SuppName = i.SupplierInfo!.SupplierName,
                InvoiceDate = i.InvoiceDate.HasValue
                ? i.InvoiceDate.Value.ToString("yyyy-MM-dd")
                : null,
                InvoiceNo = i.InvoiceNo,
                PoNo = i.PoNo,
                DueDate = i.DueDate.HasValue
                ? i.DueDate.Value.ToString("yyyy-MM-dd")
                : null,
                GrossAmount = i.TotalAmount,
                NextRole = null,
            });
            return dtoQuery.ToListAsync(token);
        }

        public Task<List<ExportRejectedInvoiceDto>> ExportRejectedInvoice(
            string? SupplierName,
            string? InvoiceNo,
            string? PONo,
            CancellationToken token)
        {
            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(i => i.StatusType == InvoiceStatusType.Rejected
           || i.QueueType == InvoiceQueueType.RejectionQueue);

            predicate = predicate
             .AndIf(!string.IsNullOrEmpty(SupplierName), s => s.SupplierInfo!.SupplierName!.Contains(SupplierName!))
            .AndIf(!string.IsNullOrEmpty(InvoiceNo), s => s.InvoiceNo!.Contains(InvoiceNo!))
            .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!))
            .And(s => s.QueueType == InvoiceQueueType.RejectionQueue);

            var query = _dbcontext.Invoices
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

            var dtoQuery = query.Select(i => new ExportRejectedInvoiceDto
            {
                Entity = i.EntityProfile!.EntityName,
                SuppName = i.SupplierInfo!.SupplierName,
                InvoiceNo = i.InvoiceNo,
                PoNo = i.PoNo,
                InvoiceDate = i.InvoiceDate.HasValue
                ? i.InvoiceDate.Value.ToString("yyyy-MM-dd")
                : null,
                DueDate = i.DueDate.HasValue
                ? i.DueDate.Value.ToString("yyyy-MM-dd")
                : null,
                GrossAmount = i.TotalAmount,

                ArchiveDate = null,
                InvoiceApprover = null
            });

            return dtoQuery.ToListAsync(token);
        }

        public async Task<PaginatedList<ArchiveInvoiceSearchDto>> GetArchiveInvoiceSearch(
            string? SupplierName,
            string? InvoiceNo,
            string? PONo,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token)
        {
            ExpressionStarter<InvoiceArchive> predicate = PredicateBuilder.New<InvoiceArchive>(true);

            predicate = predicate
             .AndIf(!string.IsNullOrEmpty(SupplierName), s => s.SupplierInfo != null && s.SupplierInfo.SupplierName!.Contains(SupplierName!))
            .AndIf(!string.IsNullOrEmpty(InvoiceNo), s => s.InvoiceNo!.Contains(InvoiceNo!))
            .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!));

            var query = _dbcontext.InvoiceArchives
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

            if (string.IsNullOrEmpty(sortField))

            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery = await query.Select(i => new ArchiveInvoiceSearchDto
            {
                InvoiceID = i.InvoiceID,
                Entity = i.EntityProfile != null ? i.EntityProfile.EntityName : string.Empty,
                SuppName = i.SupplierInfo != null ? i.SupplierInfo.SupplierName : string.Empty,
                InvoiceNo = i.InvoiceNo,
                PoNo = i.PoNo,
                InvoiceDate = i.InvoiceDate.HasValue
                ? i.InvoiceDate.Value.ToString("dd/MM/yyyy")
                : null,
                DueDate = i.DueDate.HasValue
                ? i.DueDate.Value.ToString("dd/MM/yyyy")
                : null,
                GrossAmount = i.TotalAmount.ToString("F2"),
                ExceptionReason = null,
                IsSelected = false
            }).ToListAsync();

            var pagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                 .ToPaginatedListAsync(pageNumber, pageSize, token);
            return pagination;
        }

        public async Task<PaginatedList<ExceptionInvoiceSearchDto>> GetExceptionInvoiceSearch(
            string? SupplierName,
            string? InvoiceNo,
            string? PONo,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token)
        {
            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(i => i.StatusType == InvoiceStatusType.Exception
           || i.QueueType == InvoiceQueueType.ExceptionQueue
           || (i.InvRoutingFlowID != null && !i.InvInfoRoutingLevels.Any()));

            predicate = predicate
             .AndIf(!string.IsNullOrEmpty(SupplierName), s => s.SupplierInfo!.SupplierName!.Contains(SupplierName!))
            .AndIf(!string.IsNullOrEmpty(InvoiceNo), s => s.InvoiceNo!.Contains(InvoiceNo!))
            .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!));

            var query = _dbcontext.Invoices
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

            if (string.IsNullOrEmpty(sortField))

            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery = await query.Select(i => new ExceptionInvoiceSearchDto
            {
                InvoiceID = i.InvoiceID,
                Entity = i.EntityProfile!.EntityName,
                SuppName = i.SupplierInfo!.SupplierName,
                InvoiceNo = i.InvoiceNo,
                PoNo = i.PoNo,
                InvoiceDate = i.InvoiceDate.HasValue
                ? i.InvoiceDate.Value.ToString("dd/MM/yyyy")
                : null,
                DueDate = i.DueDate.HasValue
                ? i.DueDate.Value.ToString("dd/MM/yyyy")
                : null,
                GrossAmount = i.TotalAmount.ToString("F2"),
                ExceptionReason = string.Join("; ", i.InvoiceActivityLog!
                                        .Where(a => a.InvoiceID == i.InvoiceID && 
                                                    a.IsCurrentValidationContext == true && 
                                                    (a.Action == InvoiceActionType.Validate || a.Action ==  InvoiceActionType.Import)  && 
                                                    !string.IsNullOrEmpty(a.Reason))
                                        .Select(a => a.Reason) ?? Enumerable.Empty<string>()),
                IsSelected = false
            }).ToListAsync();
        
            var pagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                 .ToPaginatedListAsync(pageNumber, pageSize, token);
            return pagination;
        }

        public async Task<PaginatedList<InvAllocLineDto>> GetInvAllocLinePerInvoice(
            long? invoiceID,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token)
        {
            ExpressionStarter<InvAllocLine> predicate =
               PredicateBuilder.New<InvAllocLine>();

            predicate = predicate
                .AndIf(invoiceID.HasValue, i => i.InvoiceID == invoiceID);

            var query = _dbcontext.InvoicesAllocLines
                .AsNoTracking()
                .AsQueryable()
                .AsExpandable()
                .Where(predicate);

            if (string.IsNullOrEmpty(sortField))
            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery = query.Select(ia => new InvAllocLineDto
            {
                InvAllocLineID = ia.InvAllocLineID,
                InvoiceID = ia.InvoiceID,
                LineNo = ia.LineNo,
                PoNo = ia.PoNo,
                PoLineNo = ia.PoLineNo,
                Account = ia.AccountID,
                LineDescription = ia.LineDescription,
                Qty = ia.Qty,
                LineNetAmount = ia.LineNetAmount,
                LineTaxAmount = ia.LineTaxAmount,
                LineAmount = ia.LineAmount,
                Currency = ia.Currency,
                TaxCodeID = ia.TaxCodeID,
                LineApproved = ia.LineApproved,
                Note = ia.Note,
                FreeFields = ia.FreeFields != null
                                ? ia.FreeFields.Select(f => new InvAllocLineFreeFieldDto
                                {
                                    FieldKey = f.FieldKey,
                                    FieldValue = f.FieldValue
                                }).ToList()
                                : new List<InvAllocLineFreeFieldDto>(),

                Dimensions = ia.Dimensions != null
                                ? ia.Dimensions.Select(f => new InvAllocLineDimensionDto
                                {
                                    DimensionKey = f.DimensionKey,
                                    DimensionValue = f.DimensionValue
                                }).ToList()
                                : new List<InvAllocLineDimensionDto>(),
            });

            var invoiceAllocationPagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                 .ToPaginatedListAsync(pageNumber, pageSize, token);
            return invoiceAllocationPagination;
        }

        public async Task<List<InvAllocEntryDto>> GetInvoiceAllocationInfo(long? invoiceID, CancellationToken token)
        {
            ExpressionStarter<InvAllocLine> predicate =
               PredicateBuilder.New<InvAllocLine>(alloc => alloc.InvoiceID == invoiceID!);

            predicate = predicate
                .AndIf(invoiceID.HasValue, i => i.InvoiceID == invoiceID);

            var query = _dbcontext.InvoicesAllocLines
                .Include(m => m.PurchaseOrderMatchTrackings)
                .AsNoTracking()
                .AsQueryable()
                .AsExpandable()
                .Where(predicate);

            return await query.Select(dto => new InvAllocEntryDto
            {
                InvAllocLineID = dto.InvAllocLineID,
                InvoiceID = dto.InvoiceID,
                LineNo = dto.LineNo,
                PoLineNo = dto.PoLineNo,
                PoNo = dto.PoNo,
                Qty = dto.Qty,
                LineDescription = dto.LineDescription,
                Note = dto.Note,
                LineNetAmount = dto.LineNetAmount,
                LineTaxAmount = dto.LineTaxAmount,
                LineAmount = dto.LineAmount,
                TaxCodeID = dto.TaxCodeID,
                Account = dto.AccountID,
                IsFromPOMatching = dto.PurchaseOrderMatchTrackings.Any(a => a.InvAllocLineID == dto.InvAllocLineID),
            }).ToListAsync(token);
        }

        public async Task<InvoiceDto> GetInvoiceInfo(long invoiceID, CancellationToken token)
        {
            var invoice = await _dbcontext.Invoices
                .AsNoTracking()                
                .Where(x => x.InvoiceID == invoiceID)
                .Select(x => new InvoiceDto
                {
                    InvoiceID = x.InvoiceID,
                    InvoiceNo = x.InvoiceNo,
                    InvoiceDate = x.InvoiceDate.HasValue ? x.InvoiceDate!.Value.ToAudDateTreatAsLocal() : null,
                    MapID = x.MapID,
                    ScanDate = x.ScanDate,
                    CreatedDate = x.CreatedDate,
                    EntityProfileID = x.EntityProfile!.EntityProfileID,
                    SupplierInfoID = x.SupplierInfoID,
                    KeywordID = x.KeywordID,
                    Keyword = x.Keyword != null ? x.Keyword.KeywordName : string.Empty,
                    SuppABN = x.SupplierInfo!.SupplierTaxID,
                    SuppName = x.SupplierInfo!.SupplierName,
                    SupplierNo = x.SupplierInfo!.SupplierID,
                    SuppBankAccount = x.SuppBankAccount,
                    DueDate = x.DueDate,
                    PoNo = x.PoNo,
                    GrNo = x.GrNo,
                    Currency = x.Currency,
                    NetAmount = x.NetAmount,
                    TaxAmount = x.TaxAmount,
                    TotalAmount = x.TotalAmount,
                    TaxCodeID = x.TaxCodeID,
                    PaymentTerm = x.PaymentTerm,
                    Note = x.Note,
                    ApproverRole = x.ApproverRole,
                    ApprovedUser = x.ApprovedUser,
                    QueueType = x.QueueType,
                    StatusType = x.StatusType,
                    RoutingFlowName = x.InvRoutingFlow != null ? x.InvRoutingFlow.InvRoutingFlowName : null,


                    InvoiceAllocationLines = x.InvoiceAllocationLines!.Select(dto => new InvAllocLineDto
                    {
                        InvAllocLineID = dto.InvAllocLineID,
                        InvoiceID = dto.InvoiceID,
                        LineNo = dto.LineNo,
                        LineDescription = dto.LineDescription,
                        PoLineNo = dto.PoLineNo,
                        PoNo = dto.PoNo,
                        Qty = dto.Qty,
                        Note = dto.Note,
                        LineNetAmount = dto.LineNetAmount,
                        LineTaxAmount = dto.LineTaxAmount,
                        LineAmount = dto.LineAmount,
                        TaxCodeID = dto.TaxCodeID,
                    }).ToList(),
                    InvInfoRoutingLevels = x.InvInfoRoutingLevels!.Select(dto => new InvInfoRoutingLevelDto
                    {
                        InvInfoRoutingLevelID = dto.InvInfoRoutingLevelID,
                        InvoiceID = dto.InvoiceID,
                        InvRoutingFlowID = dto.InvRoutingFlowID,
                        RoleID = dto.RoleID,
                        Level = dto.Level,

                    }).ToList()


                }).FirstOrDefaultAsync();

            return invoice!;
        }

        public async Task<PaginatedList<InvMyInvoiceSearchDto>> GetMyInvoiceSearch(
            string? SupplierName,
            string? InvoiceNo,
            string? PONo,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token)
        {
            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(
                i => i.StatusType == InvoiceStatusType.ForApproval
                || i.StatusType == InvoiceStatusType.ApprovalOnHold);

            predicate = predicate
             .AndIf(!string.IsNullOrEmpty(SupplierName), s => s.SupplierInfo!.SupplierName!.Contains(SupplierName!))
            .AndIf(!string.IsNullOrEmpty(InvoiceNo), s => s.InvoiceNo!.Contains(InvoiceNo!))
            .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!));

            var query = _dbcontext.Invoices
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

            if (string.IsNullOrEmpty(sortField))

            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery = await query.Select(i => new InvMyInvoiceSearchDto
            {
                InvoiceID = i.InvoiceID,
                Entity = i.EntityProfile!.EntityName,
                SuppName = i.SupplierInfo!.SupplierName,
                InvoiceNo = i.InvoiceNo,
                PoNo = i.PoNo,
                InvoiceDate = i.InvoiceDate.HasValue
                ? i.InvoiceDate.Value.ToString("dd/MM/yyyy")
                : null,
                DueDate = i.DueDate.HasValue
                ? i.DueDate.Value.ToString("dd/MM/yyyy")
                : null,
                GrossAmount = i.TotalAmount.ToString("F2"),
                NextRole = null,
                ExceptionReason = string.Join("; ", i.InvoiceActivityLog!
                                        .Where(a => a.InvoiceID == i.InvoiceID &&
                                                    a.IsCurrentValidationContext == true &&
                                                    (a.Action == InvoiceActionType.Validate ||  a.Action == InvoiceActionType.Import)    &&
                                                    !string.IsNullOrEmpty(a.Reason))
                                        .Select(a => a.Reason) ?? Enumerable.Empty<string>()),
                IsSelected = false
            }).ToListAsync();

            var myInvoiceSearchPagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                 .ToPaginatedListAsync(pageNumber, pageSize, token);
            return myInvoiceSearchPagination;
        }

        public async Task<PaginatedList<RejectedInvoiceSearchDto>> GetRejectedInvoiceSearch(string? SupplierName, string? InvoiceNo, string? PONo, int pageNumber, int pageSize, string? sortField, int? sortOrder, CancellationToken token)
        {
            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(i => i.StatusType == InvoiceStatusType.Rejected
            || i.QueueType == InvoiceQueueType.RejectionQueue);

            predicate = predicate
             .AndIf(!string.IsNullOrEmpty(SupplierName), s => s.SupplierInfo!.SupplierName!.Contains(SupplierName!))
            .AndIf(!string.IsNullOrEmpty(InvoiceNo), s => s.InvoiceNo!.Contains(InvoiceNo!))
            .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!));

            var query = _dbcontext.Invoices
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

            if (string.IsNullOrEmpty(sortField))

            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery = await query.Select(i => new RejectedInvoiceSearchDto
            {
                InvoiceID = i.InvoiceID,
                Entity = i.EntityProfile!.EntityName,
                SuppName = i.SupplierInfo!.SupplierName,
                InvoiceNo = i.InvoiceNo,
                PoNo = i.PoNo,
                InvoiceDate = i.InvoiceDate.HasValue
                ? i.InvoiceDate.Value.ToString("dd/MM/yyyy")
                : null,
                DueDate = i.DueDate.HasValue
                ? i.DueDate.Value.ToString("dd/MM/yyyy")
                : null,
                GrossAmount = i.TotalAmount.ToString("F2"),
                ArchiveDate = null,
                InvoiceApprover = null
            }).ToListAsync();

            var pagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                 .ToPaginatedListAsync(pageNumber, pageSize, token);
            return pagination;
        }

        public async Task<PaginatedList<LoadInvoiceCommentsDto>> LoadInvoiceComments(long? InvoiceID, int pageNumber, int pageSize, string? sortField, int? sortOrder, CancellationToken token)
        {
            ExpressionStarter<InvoiceComment> predicate = PredicateBuilder.New<InvoiceComment>(true);

            predicate = predicate
             .AndIf(InvoiceID.HasValue, s => s.InvoiceID == InvoiceID);

            var query = _dbcontext.InvoiceComments
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

            if (string.IsNullOrEmpty(sortField))

            {
                query = query.OrderByDescending(p => p.CreatedDate);
            }

            var dtoQuery = query.AsEnumerable().Select(i => new LoadInvoiceCommentsDto
            {
                InvoiceCommentID = i.InvoiceCommentID,
                Comment = i.Comment,
                CreatedBy = i.CreatedBy!,
                CreatedDate = i.CreatedDate?.ToLocalTime()
                .ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture)
            }).ToList();

            var pagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                 .ToPaginatedListAsync(pageNumber, pageSize, token);
            return pagination;
        }

        public async Task<InvoiceNavigationResultDto> GetAdjacentInvoiceId(
            long invoiceID,
            bool isNext,
            InvoiceStatusType? statusType,
            InvoiceQueueType? queueType,
            CancellationToken token)
        {
            var currentInvoiceState = await _dbcontext.Invoices
                .AsNoTracking()
                .Where(i => i.InvoiceID == invoiceID)
                .Select(i => new
                {
                    i.InvoiceID,
                    i.StatusType,
                    i.QueueType
                })
                .FirstOrDefaultAsync(token);

            if (currentInvoiceState == null)
            {
                return new InvoiceNavigationResultDto(false, null);
            }

            var statusFilter = statusType ?? currentInvoiceState.StatusType;
            var queueFilter = queueType ?? currentInvoiceState.QueueType;

            var query = _dbcontext.Invoices
                .AsNoTracking()
                .Where(i => i.InvoiceID != invoiceID);

            if (statusFilter.HasValue)
            {
                var status = statusFilter.Value;
                query = query.Where(i => i.StatusType == status);
            }
            else
            {
                query = query.Where(i => i.StatusType == null);
            }

            if (queueFilter.HasValue)
            {
                var queue = queueFilter.Value;
                query = query.Where(i => i.QueueType == queue);
            }
            else
            {
                query = query.Where(i => i.QueueType == null);
            }

            query = isNext
                ? query.Where(i => i.InvoiceID > invoiceID)
                       .OrderBy(i => i.InvoiceID)
                : query.Where(i => i.InvoiceID < invoiceID)
                       .OrderByDescending(i => i.InvoiceID);

            var adjacentInvoiceId = await query
                .Select(i => (long?)i.InvoiceID)
                .FirstOrDefaultAsync(token);

            return new InvoiceNavigationResultDto(true, adjacentInvoiceId);
        }

        public async Task<PaginatedList<InvSearchSupplierDto>> SearchSupplierWithPagination(
            string? SupplierID,
            string? SupplierName,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token)
        {
            ExpressionStarter<SupplierInfo> predicate =
               PredicateBuilder.New<SupplierInfo>(s => s.IsActive || !s.IsActive);

            predicate = predicate
                .AndIf(!string.IsNullOrEmpty(SupplierID), s => s.SupplierID!.Contains(SupplierID!))
                .AndIf(!string.IsNullOrEmpty(SupplierName), s => s.SupplierName!.Contains(SupplierName!));

            var query = _dbcontext.SupplierInfos
                .Include(s => s.Account)
                .Include(s => s.EntityProfile)
                .AsNoTracking()
                .AsQueryable()
                .AsExpandable()
                .Where(predicate);

            if (string.IsNullOrEmpty(sortField))

            {
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery = query.Select(s => new InvSearchSupplierDto
            {
                SupplierInfoID = s.SupplierInfoID,
                SupplierID = s.SupplierID,
                SupplierName = s.SupplierName,
                SupplierTaxID = s.SupplierTaxID,
                Entity = s.EntityProfile!.EntityName,
                IsActive = s.IsActive
            });

            var supplierPagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                 .ToPaginatedListAsync(pageNumber, pageSize, token);
            return supplierPagination;
        }
    }
}