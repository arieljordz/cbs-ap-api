using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing;
using CbsAp.Application.DTOs.Invoicing.InvInfoRoutingLevel;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Features.Shared;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.ActivityLog;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.InvoicingArchive;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Enums;
using CbsAp.Infrastracture.Contexts;
using CBSAP.ValidationEngine;
using CBSAP.ValidationEngine.Core;
using DocumentFormat.OpenXml.Wordprocessing;
using LinqKit;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq.Expressions;
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
            string? SupplierName, string? InvoiceNo, string? PONo, long roleId,
            string? paymentTerm, string? supplierNo, string? suppABN,
            string? suppBankAccount, int? entityProfileID, string? grNo,
            DateTime? startInvoiceDate, DateTime? endInvoiceDate,
            DateTime? startDueDate, DateTime? endDueDate, int? daystillDue,
            decimal? netAmount, int? taxCodeID, decimal? taxAmount,
            string? currency, decimal? totalAmount, string? invRoutingFlowName,
            string? nextRole, string? keyword, string? mapID,
            DateTime? startScanDate, DateTime? endScanDate, string? invoiceID,
            CancellationToken token)
        {
            ExpressionStarter<InvoiceArchive> predicate =
                PredicateBuilder.New<InvoiceArchive>(true);

            var roleEntityIds = _dbcontext.RoleEntities
                .Where(r => r.RoleID == roleId)
                .Select(r => r.EntityProfileID)
                .ToList();
             
            if (roleEntityIds.Any())

            {
                predicate = predicate.And(i => i.EntityProfileID.HasValue && roleEntityIds.Contains(i.EntityProfileID.Value));
            }

            predicate =
                predicate
                    .AndIf(!string.IsNullOrEmpty(SupplierName),
                           s => s.SupplierInfo != null &&
                                s.SupplierInfo.SupplierName!.Contains(SupplierName!))
                    .AndIf(!string.IsNullOrEmpty(InvoiceNo),
                           s => s.InvoiceNo!.Contains(InvoiceNo!))
                    .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!))
                    .AndIf(!string.IsNullOrWhiteSpace(supplierNo),
                           s => s.SupplierInfo!.SupplierID!.Contains(supplierNo!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppABN),
                           s => s.SupplierInfo!.SupplierTaxID!.Contains(suppABN!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppBankAccount),
                           s => s.SuppBankAccount!.Contains(suppBankAccount!))

                    // Payment Term (string filter, not numeric)
                    .AndIf(!string.IsNullOrWhiteSpace(paymentTerm),
                           s => s.PaymentTerm != null && s.PaymentTerm == paymentTerm)

                    // Entity Profile
                    .AndIf(entityProfileID.HasValue && entityProfileID.Value != 0,
                           s => s.EntityProfileID == entityProfileID)

                    // GR No
                    .AndIf(!string.IsNullOrWhiteSpace(grNo),
                           s => s.GrNo!.Contains(grNo!))

                    // Invoice Date Range
                    .AndIf(startInvoiceDate.HasValue,
                           s => s.InvoiceDate >= startInvoiceDate)

                    .AndIf(endInvoiceDate.HasValue,
                           s => s.InvoiceDate <= endInvoiceDate)

                    // Due Date Range
                    .AndIf(startDueDate.HasValue, s => s.DueDate >= startDueDate)

                    .AndIf(endDueDate.HasValue, s => s.DueDate <= endDueDate)

                    // Scan Date Range
                    .AndIf(startScanDate.HasValue, s => s.ScanDate >= startScanDate)

                    .AndIf(endScanDate.HasValue, s => s.ScanDate <= endScanDate)

                    // Amount filters
                    .AndIf(netAmount.HasValue && netAmount.Value != 0,
                           s => s.NetAmount == netAmount)

                    .AndIf(taxCodeID.HasValue && taxCodeID.Value != 0,
                           s => s.TaxCodeID == taxCodeID)

                    .AndIf(taxAmount.HasValue && taxAmount.Value != 0,
                           s => s.TaxAmount == taxAmount)

                    .AndIf(!string.IsNullOrWhiteSpace(currency),
                           s => s.Currency == currency)

                    .AndIf(totalAmount.HasValue && totalAmount.Value != 0,
                           s => s.TotalAmount == totalAmount)

                    // Keyword
                    .AndIf(!string.IsNullOrWhiteSpace(keyword),
                           s => s.Keyword != null &&
                                s.Keyword.KeywordName.Contains(keyword!))

                    // Map ID
                    .AndIf(!string.IsNullOrWhiteSpace(mapID), s => s.MapID == mapID)

                    // Invoice ID
                    .AndIf(!string.IsNullOrWhiteSpace(invoiceID),
                           s => s.InvoiceID.ToString() == invoiceID)
                    /*
                       Daystill due
                       the value of invDueDateCalculation is base on web/assets/json
                     */
                    .AndIf(daystillDue.HasValue && daystillDue.Value != 0,
                           s => s.EntityProfile != null &&
                                (
                                    // Based on ScanDate
                                    (s.EntityProfile.InvDueDateCalculation == 1 &&
                                     s.ScanDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.ScanDate.Value,
                                         s.ScanDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on CreatedDate
                                    (s.EntityProfile.InvDueDateCalculation == 2 &&
                                     s.CreatedDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.CreatedDate.Value,
                                         s.CreatedDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on InvoiceDate
                                    (s.EntityProfile.InvDueDateCalculation == 3 &&
                                     s.InvoiceDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.InvoiceDate.Value,
                                         s.InvoiceDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)));

            var query =
                _dbcontext.InvoiceArchives.AsNoTracking().AsExpandable().Where(
                    predicate);

            var dtoQuery = query.Select(i => new ExportArchiveInvoiceDto
            {
                SupplierName = i.SupplierInfo != null ? i.SupplierInfo.SupplierName : string.Empty,
                InvoiceDate = i.InvoiceDate!.Value.LocalDateTime.ToString("yyyy-MM-dd"),
                InvoiceNumber = i.InvoiceNo,
                PoNumber = i.PoNo,
                DueDate = i.DueDate!.Value.LocalDateTime.ToString("yyyy-MM-dd"),
                GrossAmount = i.TotalAmount,
              //  ExceptionReason = null,
            });

            return dtoQuery.ToListAsync(token);
        }

        public Task<List<ExportExceptionInvoiceDto>> ExportExceptionInvoice(
            string? SupplierName, string? InvoiceNo, string? PONo, long roleId,
            string? paymentTerm, string? supplierNo, string? suppABN,
            string? suppBankAccount, int? entityProfileID, string? grNo,
            DateTime? startInvoiceDate, DateTime? endInvoiceDate,
            DateTime? startDueDate, DateTime? endDueDate, int? daystillDue,
            decimal? netAmount, int? taxCodeID, decimal? taxAmount,
            string? currency, decimal? totalAmount, string? invRoutingFlowName,
            string? nextRole, string? keyword, string? mapID,
            DateTime? startScanDate, DateTime? endScanDate, string? invoiceID,
            CancellationToken token)
        {
            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(
                i => i.StatusType == InvoiceStatusType.Exception ||
                     i.QueueType == InvoiceQueueType.ExceptionQueue);

            var roleEntityIds = _dbcontext.RoleEntities
            .Where(r => r.RoleID == roleId)
            .Select(r => r.EntityProfileID)
            .ToList();

            if (roleEntityIds.Any())
            {
                predicate = predicate.And(i => i.EntityProfileID.HasValue && roleEntityIds.Contains(i.EntityProfileID.Value));
            }


            predicate =
                predicate
                    .AndIf(!string.IsNullOrEmpty(SupplierName),
                           s => s.SupplierInfo!.SupplierName!.Contains(SupplierName!))
                    .AndIf(!string.IsNullOrEmpty(InvoiceNo),
                           s => s.InvoiceNo!.Contains(InvoiceNo!))
                    .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!))
                    .AndIf(!string.IsNullOrWhiteSpace(supplierNo),
                           s => s.SupplierInfo!.SupplierID!.Contains(supplierNo!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppABN),
                           s => s.SupplierInfo!.SupplierTaxID!.Contains(suppABN!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppBankAccount),
                           s => s.SuppBankAccount!.Contains(suppBankAccount!))

                    // Payment Term (string filter, not numeric)
                    .AndIf(!string.IsNullOrWhiteSpace(paymentTerm),
                           s => s.PaymentTerm != null && s.PaymentTerm == paymentTerm)

                    // Entity Profile
                    .AndIf(entityProfileID.HasValue && entityProfileID.Value != 0,
                           s => s.EntityProfileID == entityProfileID)

                    // GR No
                    .AndIf(!string.IsNullOrWhiteSpace(grNo),
                           s => s.GrNo!.Contains(grNo!))

                    // Invoice Date Range
                    .AndIf(startInvoiceDate.HasValue,
                           s => s.InvoiceDate >= startInvoiceDate)

                    .AndIf(endInvoiceDate.HasValue,
                           s => s.InvoiceDate <= endInvoiceDate)

                    // Due Date Range
                    .AndIf(startDueDate.HasValue, s => s.DueDate >= startDueDate)

                    .AndIf(endDueDate.HasValue, s => s.DueDate <= endDueDate)

                    // Scan Date Range
                    .AndIf(startScanDate.HasValue, s => s.ScanDate >= startScanDate)

                    .AndIf(endScanDate.HasValue, s => s.ScanDate <= endScanDate)

                    // Amount filters
                    .AndIf(netAmount.HasValue && netAmount.Value != 0,
                           s => s.NetAmount == netAmount)

                    .AndIf(taxCodeID.HasValue && taxCodeID.Value != 0,
                           s => s.TaxCodeID == taxCodeID)

                    .AndIf(taxAmount.HasValue && taxAmount.Value != 0,
                           s => s.TaxAmount == taxAmount)

                    .AndIf(!string.IsNullOrWhiteSpace(currency),
                           s => s.Currency == currency)

                    .AndIf(totalAmount.HasValue && totalAmount.Value != 0,
                           s => s.TotalAmount == totalAmount)

                    // Routing Flow
                    .AndIf(!string.IsNullOrWhiteSpace(invRoutingFlowName),
                           s => s.InvRoutingFlow != null &&
                                s.InvRoutingFlow!.InvRoutingFlowName!.Contains(
                                    invRoutingFlowName!))

                    // Next Role
                    .AndIf(
                        !string.IsNullOrWhiteSpace(nextRole),
                        s => s.InvInfoRoutingLevels!.Where(x => x.InvFlowStatus == 0)
                                 .Select(x => x.Role!.RoleName)
                                 .FirstOrDefault()!.Contains(nextRole!))

                    // Keyword
                    .AndIf(!string.IsNullOrWhiteSpace(keyword),
                           s => s.Keyword != null &&
                                s.Keyword.KeywordName.Contains(keyword!))

                    // Map ID
                    .AndIf(!string.IsNullOrWhiteSpace(mapID), s => s.MapID == mapID)

                    // Invoice ID
                    .AndIf(!string.IsNullOrWhiteSpace(invoiceID),
                           s => s.InvoiceID.ToString() == invoiceID)
                    /*
                       Daystill due
                       the value of invDueDateCalculation is base on web/assets/json
                     */
                    .AndIf(daystillDue.HasValue && daystillDue.Value != 0,
                           s => s.EntityProfile != null &&
                                (
                                    // Based on ScanDate
                                    (s.EntityProfile.InvDueDateCalculation == 1 &&
                                     s.ScanDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.ScanDate.Value,
                                         s.ScanDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on CreatedDate
                                    (s.EntityProfile.InvDueDateCalculation == 2 &&
                                     s.CreatedDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.CreatedDate.Value,
                                         s.CreatedDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on InvoiceDate
                                    (s.EntityProfile.InvDueDateCalculation == 3 &&
                                     s.InvoiceDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.InvoiceDate.Value,
                                         s.InvoiceDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)))
                    .And(s => s.QueueType == InvoiceQueueType.ExceptionQueue);

            var query =
                _dbcontext.Invoices.AsNoTracking().AsExpandable().Where(predicate);

            var dtoQuery = query.Select(i => new ExportExceptionInvoiceDto
            {
                
                SupplierName = i.SupplierInfo!.SupplierName,
                InvoiceDate = i.InvoiceDate!.Value.LocalDateTime.ToString("yyyy-MM-dd"),
                InvoiceNumber = i.InvoiceNo,
                PoNumber = i.PoNo,
                DueDate = i.DueDate!.Value.LocalDateTime.ToString("yyyy-MM-dd"),
                GrossAmount = i.TotalAmount,

                ExceptionReason = string.Join("; ", i.InvoiceActivityLog!
                .Where(
                 
                    a => a.InvoiceID == i.InvoiceID &&
                    a.IsCurrentValidationContext == true &&
                    (a.Action == InvoiceActionType.Validate ||
                     a.Action == InvoiceActionType.Import) &&
                     !string.IsNullOrEmpty(a.Reason))
                .Select(a => a.Reason) ??
                Enumerable.Empty<string>()),
                    
                    
            });

            return dtoQuery.ToListAsync(token);
        }

        public Task<List<ExportMyInvoiceDto>> ExportMyInvoiceToExcel(
            string? SupplierName, string? InvoiceNo, string? PONo, long roleId,
            string? paymentTerm, string? supplierNo, string? suppABN,
            string? suppBankAccount, int? entityProfileID, string? grNo,
            DateTime? startInvoiceDate, DateTime? endInvoiceDate,
            DateTime? startDueDate, DateTime? endDueDate, int? daystillDue,
            decimal? netAmount, int? taxCodeID, decimal? taxAmount,
            string? currency, decimal? totalAmount, string? invRoutingFlowName,
            string? nextRole, string? keyword, string? mapID,
            DateTime? startScanDate, DateTime? endScanDate, string? invoiceID,
            CancellationToken token)
        {
            ExpressionStarter<Invoice> predicate =
                PredicateBuilder.New<Invoice>(true);

            var roleEntityIds = _dbcontext.RoleEntities
                .Where(r => r.RoleID == roleId)
                .Select(r => r.EntityProfileID)
                .ToList();

                 if (roleEntityIds.Any())
            {
                predicate = predicate.And(i => i.EntityProfileID.HasValue && roleEntityIds.Contains(i.EntityProfileID.Value));
            }


            predicate = 
                predicate
                    .AndIf(!string.IsNullOrEmpty(SupplierName),
                           s => s.SupplierInfo!.SupplierName!.Contains(SupplierName!))
                    .AndIf(!string.IsNullOrEmpty(InvoiceNo),
                           s => s.InvoiceNo!.Contains(InvoiceNo!))
                    .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!))
                    .AndIf(!string.IsNullOrWhiteSpace(supplierNo),
                           s => s.SupplierInfo!.SupplierID!.Contains(supplierNo!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppABN),
                           s => s.SupplierInfo!.SupplierTaxID!.Contains(suppABN!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppBankAccount),
                           s => s.SuppBankAccount!.Contains(suppBankAccount!))

                    // Payment Term (string filter, not numeric)
                    .AndIf(!string.IsNullOrWhiteSpace(paymentTerm),
                           s => s.PaymentTerm != null && s.PaymentTerm == paymentTerm)

                    // Entity Profile
                    .AndIf(entityProfileID.HasValue && entityProfileID.Value != 0,
                           s => s.EntityProfileID == entityProfileID)

                    // GR No
                    .AndIf(!string.IsNullOrWhiteSpace(grNo),
                           s => s.GrNo!.Contains(grNo!))

                    // Invoice Date Range
                    .AndIf(startInvoiceDate.HasValue,
                           s => s.InvoiceDate >= startInvoiceDate)

                    .AndIf(endInvoiceDate.HasValue,
                           s => s.InvoiceDate <= endInvoiceDate)

                    // Due Date Range
                    .AndIf(startDueDate.HasValue, s => s.DueDate >= startDueDate)

                    .AndIf(endDueDate.HasValue, s => s.DueDate <= endDueDate)

                    // Scan Date Range
                    .AndIf(startScanDate.HasValue, s => s.ScanDate >= startScanDate)

                    .AndIf(endScanDate.HasValue, s => s.ScanDate <= endScanDate)

                    // Amount filters
                    .AndIf(netAmount.HasValue && netAmount.Value != 0,
                           s => s.NetAmount == netAmount)

                    .AndIf(taxCodeID.HasValue && taxCodeID.Value != 0,
                           s => s.TaxCodeID == taxCodeID)

                    .AndIf(taxAmount.HasValue && taxAmount.Value != 0,
                           s => s.TaxAmount == taxAmount)

                    .AndIf(!string.IsNullOrWhiteSpace(currency),
                           s => s.Currency == currency)

                    .AndIf(totalAmount.HasValue && totalAmount.Value != 0,
                           s => s.TotalAmount == totalAmount)

                    // Routing Flow
                    .AndIf(!string.IsNullOrWhiteSpace(invRoutingFlowName),
                           s => s.InvRoutingFlow != null &&
                                s.InvRoutingFlow!.InvRoutingFlowName!.Contains(
                                    invRoutingFlowName!))

                    // Next Role
                    .AndIf(
                        !string.IsNullOrWhiteSpace(nextRole),
                        s => s.InvInfoRoutingLevels!.Where(x => x.InvFlowStatus == 0)
                                 .Select(x => x.Role!.RoleName)
                                 .FirstOrDefault()!.Contains(nextRole!))

                    // Keyword
                    .AndIf(!string.IsNullOrWhiteSpace(keyword),
                           s => s.Keyword != null &&
                                s.Keyword.KeywordName.Contains(keyword!))

                    // Map ID
                    .AndIf(!string.IsNullOrWhiteSpace(mapID), s => s.MapID == mapID)

                    // Invoice ID
                    .AndIf(!string.IsNullOrWhiteSpace(invoiceID),
                           s => s.InvoiceID.ToString() == invoiceID)
                    /*
                       Daystill due
                       the value of invDueDateCalculation is base on web/assets/json
                     */
                    .AndIf(daystillDue.HasValue && daystillDue.Value != 0,
                           s => s.EntityProfile != null &&
                                (
                                    // Based on ScanDate
                                    (s.EntityProfile.InvDueDateCalculation == 1 &&
                                     s.ScanDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.ScanDate.Value,
                                         s.ScanDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on CreatedDate
                                    (s.EntityProfile.InvDueDateCalculation == 2 &&
                                     s.CreatedDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.CreatedDate.Value,
                                         s.CreatedDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on InvoiceDate
                                    (s.EntityProfile.InvDueDateCalculation == 3 &&
                                     s.InvoiceDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.InvoiceDate.Value,
                                         s.InvoiceDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)))
                    .And(s => s.QueueType == InvoiceQueueType.MyInvoices &&
                              s.ApproverRole == roleId);

            var query =
                _dbcontext.Invoices.AsNoTracking().AsExpandable().AsQueryable().Where(
                    predicate);

            var dtoQuery = query.Select(i => new ExportMyInvoiceDto
            {
                SupplierName = i.SupplierInfo!.SupplierName,
              InvoiceDate = i.InvoiceDate!.Value.LocalDateTime.ToString("yyyy-MM-dd"),
                InvoiceNumber = i.InvoiceNo,
                PoNumber = i.PoNo,
                DueDate = i.DueDate!.Value.LocalDateTime.ToString("yyyy-MM-dd"),
                GrossAmount = i.TotalAmount,
                NextRole = i.InvInfoRoutingLevels != null ? i.StatusType == InvoiceStatusType.ReadyForExport ? string.Empty : i.InvInfoRoutingLevels!
                                  .Where(i => i.InvFlowStatus == 0)
                                  .OrderBy(o => o.Level)
                                  .Select(s => s.Role.RoleName)
                                  .FirstOrDefault() : "N/A",
                ExceptionReason = string.Join(";", i.InvoiceActivityLog!
                       .Where(
                         a => a.InvoiceID == i.InvoiceID &&
                         a.IsCurrentValidationContext == true &&
                         (a.Action == InvoiceActionType.Validate ||
                         a.Action == InvoiceActionType.Import) &&
                         !string.IsNullOrEmpty(a.Reason))
                       .Select(a => a.Reason) ?? Enumerable.Empty<string>()),
                    
                    
            });
            return dtoQuery.ToListAsync(token);
        }

        public Task<List<ExportRejectedInvoiceDto>> ExportRejectedInvoice(
            string? SupplierName, string? InvoiceNo, string? PONo,long roleId,
            string? paymentTerm, string? supplierNo, string? suppABN,
            string? suppBankAccount, int? entityProfileID, string? grNo,
            DateTime? startInvoiceDate, DateTime? endInvoiceDate,
            DateTime? startDueDate, DateTime? endDueDate, int? daystillDue,
            decimal? netAmount, int? taxCodeID, decimal? taxAmount,
            string? currency, decimal? totalAmount, string? invRoutingFlowName,
            string? nextRole, string? keyword, string? mapID,
            DateTime? startScanDate, DateTime? endScanDate, string? invoiceID,
            CancellationToken token)
        {
            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(
                i => i.StatusType == InvoiceStatusType.Rejected ||
                     i.QueueType == InvoiceQueueType.RejectionQueue);

            var roleEntityIds = _dbcontext.RoleEntities
                .Where(r => r.RoleID == roleId)
                .Select(r => r.EntityProfileID)
                .ToList();

                 if (roleEntityIds.Any())
            {
                predicate = predicate.And(i => i.EntityProfileID.HasValue && roleEntityIds.Contains(i.EntityProfileID.Value));
            }

            predicate =
                predicate
                    .AndIf(!string.IsNullOrEmpty(SupplierName),
                           s => s.SupplierInfo!.SupplierName!.Contains(SupplierName!))
                    .AndIf(!string.IsNullOrEmpty(InvoiceNo),
                           s => s.InvoiceNo!.Contains(InvoiceNo!))
                    .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!))
                    .AndIf(!string.IsNullOrWhiteSpace(supplierNo),
                           s => s.SupplierInfo!.SupplierID!.Contains(supplierNo!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppABN),
                           s => s.SupplierInfo!.SupplierTaxID!.Contains(suppABN!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppBankAccount),
                           s => s.SuppBankAccount!.Contains(suppBankAccount!))

                    // Payment Term (string filter, not numeric)
                    .AndIf(!string.IsNullOrWhiteSpace(paymentTerm),
                           s => s.PaymentTerm != null && s.PaymentTerm == paymentTerm)

                    // Entity Profile
                    .AndIf(entityProfileID.HasValue && entityProfileID.Value != 0,
                           s => s.EntityProfileID == entityProfileID)

                    // GR No
                    .AndIf(!string.IsNullOrWhiteSpace(grNo),
                           s => s.GrNo!.Contains(grNo!))

                    // Invoice Date Range
                    .AndIf(startInvoiceDate.HasValue,
                           s => s.InvoiceDate >= startInvoiceDate)

                    .AndIf(endInvoiceDate.HasValue,
                           s => s.InvoiceDate <= endInvoiceDate)

                    // Due Date Range
                    .AndIf(startDueDate.HasValue, s => s.DueDate >= startDueDate)

                    .AndIf(endDueDate.HasValue, s => s.DueDate <= endDueDate)

                    // Scan Date Range
                    .AndIf(startScanDate.HasValue, s => s.ScanDate >= startScanDate)

                    .AndIf(endScanDate.HasValue, s => s.ScanDate <= endScanDate)

                    // Amount filters
                    .AndIf(netAmount.HasValue && netAmount.Value != 0,
                           s => s.NetAmount == netAmount)

                    .AndIf(taxCodeID.HasValue && taxCodeID.Value != 0,
                           s => s.TaxCodeID == taxCodeID)

                    .AndIf(taxAmount.HasValue && taxAmount.Value != 0,
                           s => s.TaxAmount == taxAmount)

                    .AndIf(!string.IsNullOrWhiteSpace(currency),
                           s => s.Currency == currency)

                    .AndIf(totalAmount.HasValue && totalAmount.Value != 0,
                           s => s.TotalAmount == totalAmount)

                    // Routing Flow
                    .AndIf(!string.IsNullOrWhiteSpace(invRoutingFlowName),
                           s => s.InvRoutingFlow != null &&
                                s.InvRoutingFlow!.InvRoutingFlowName!.Contains(
                                    invRoutingFlowName!))

                    // Next Role
                    .AndIf(
                        !string.IsNullOrWhiteSpace(nextRole),
                        s => s.InvInfoRoutingLevels!.Where(x => x.InvFlowStatus == 0)
                                 .Select(x => x.Role!.RoleName)
                                 .FirstOrDefault()!.Contains(nextRole!))

                    // Keyword
                    .AndIf(!string.IsNullOrWhiteSpace(keyword),
                           s => s.Keyword != null &&
                                s.Keyword.KeywordName.Contains(keyword!))

                    // Map ID
                    .AndIf(!string.IsNullOrWhiteSpace(mapID), s => s.MapID == mapID)

                    // Invoice ID
                    .AndIf(!string.IsNullOrWhiteSpace(invoiceID),
                           s => s.InvoiceID.ToString() == invoiceID)
                    /*
                       Daystill due
                       the value of invDueDateCalculation is base on web/assets/json
                     */
                    .AndIf(daystillDue.HasValue && daystillDue.Value != 0,
                           s => s.EntityProfile != null &&
                                (
                                    // Based on ScanDate
                                    (s.EntityProfile.InvDueDateCalculation == 1 &&
                                     s.ScanDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.ScanDate.Value,
                                         s.ScanDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on CreatedDate
                                    (s.EntityProfile.InvDueDateCalculation == 2 &&
                                     s.CreatedDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.CreatedDate.Value,
                                         s.CreatedDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on InvoiceDate
                                    (s.EntityProfile.InvDueDateCalculation == 3 &&
                                     s.InvoiceDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.InvoiceDate.Value,
                                         s.InvoiceDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)))
                    .And(s => s.QueueType == InvoiceQueueType.RejectionQueue);

            var query =
                _dbcontext.Invoices.AsNoTracking().AsExpandable().Where(predicate);

            var dtoQuery = query.Select(i => new ExportRejectedInvoiceDto
            {
                
                SupplierName = i.SupplierInfo!.SupplierName,
                InvoiceDate = i.InvoiceDate!.Value.LocalDateTime.ToString("yyyy-MM-dd"),
                InvoiceNumber = i.InvoiceNo,
                PoNumber = i.PoNo,
                DueDate = i.DueDate!.Value.LocalDateTime.ToString("yyyy-MM-dd"),
                GrossAmount = i.TotalAmount,

                Reason = (i.StatusType == InvoiceStatusType.Rejected) ? (i.InvoiceActivityLog
                                     .Where(x => x.CurrentStatus == InvoiceStatusType.Rejected && x.Action.HasValue &&
                                                 new[] { InvoiceActionType.Reject,
                                                   InvoiceActionType.Import,
                                                   InvoiceActionType.Submit }
                                                     .Contains(x.Action.Value))
                                     .OrderByDescending(x => x.CreatedDate)
                                     .Select(x => x.Reason)
                                     .FirstOrDefault() ?? string.Empty) : string.Empty,
            });

            return dtoQuery.ToListAsync(token);
        }

        public async Task<PaginatedList<ArchiveInvoiceSearchDto>>
        GetArchiveInvoiceSearch(
            string? SupplierName, string? InvoiceNo, string? PONo, int pageNumber,
            int pageSize, string? sortField, int? sortOrder, int roleId, string? paymentTerm,
            string? supplierNo, string? suppABN, string? suppBankAccount,
            int? entityProfileID, string? grNo, DateTime? startInvoiceDate,
            DateTime? endInvoiceDate, DateTime? startDueDate, DateTime? endDueDate,
            int? daystillDue, decimal? netAmount, int? taxCodeID,
            decimal? taxAmount, string? currency, decimal? totalAmount,
            string? invRoutingFlowName, string? nextRole, string? keyword,
            string? mapID, DateTime? startScanDate, DateTime? endScanDate,
            string? invoiceID, CancellationToken token)
        {

            ExpressionStarter<InvoiceArchive> predicate = PredicateBuilder.New<InvoiceArchive>(true);

            var roleEntityIds = await _dbcontext.RoleEntities
            .Where(r => r.RoleID == roleId)
            .Select(r => r.EntityProfileID)
            .ToListAsync(token);

            if (roleEntityIds.Any())
            {
                predicate = predicate.And(i => i.EntityProfileID.HasValue && roleEntityIds.Contains(i.EntityProfileID.Value));
            }

            predicate =
                predicate
                    .AndIf(!string.IsNullOrEmpty(SupplierName),
                           s => s.SupplierInfo != null &&
                                s.SupplierInfo.SupplierName!.Contains(SupplierName!))
                    .AndIf(!string.IsNullOrEmpty(InvoiceNo),
                           s => s.InvoiceNo!.Contains(InvoiceNo!))
                    .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!))
                    .AndIf(!string.IsNullOrWhiteSpace(supplierNo),
                           s => s.SupplierInfo!.SupplierID!.Contains(supplierNo!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppABN),
                           s => s.SupplierInfo!.SupplierTaxID!.Contains(suppABN!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppBankAccount),
                           s => s.SuppBankAccount!.Contains(suppBankAccount!))

                    // Payment Term (string filter, not numeric)
                    .AndIf(!string.IsNullOrWhiteSpace(paymentTerm),
                           s => s.PaymentTerm != null && s.PaymentTerm == paymentTerm)

                    // Entity Profile
                    .AndIf(entityProfileID.HasValue && entityProfileID.Value != 0,
                           s => s.EntityProfileID == entityProfileID)

                    // GR No
                    .AndIf(!string.IsNullOrWhiteSpace(grNo),
                           s => s.GrNo!.Contains(grNo!))

                    // Invoice Date Range
                    .AndIf(startInvoiceDate.HasValue,
                           s => s.InvoiceDate >= startInvoiceDate)

                    .AndIf(endInvoiceDate.HasValue,
                           s => s.InvoiceDate <= endInvoiceDate)

                    // Due Date Range
                    .AndIf(startDueDate.HasValue, s => s.DueDate >= startDueDate)

                    .AndIf(endDueDate.HasValue, s => s.DueDate <= endDueDate)

                    // Scan Date Range
                    .AndIf(startScanDate.HasValue, s => s.ScanDate >= startScanDate)

                    .AndIf(endScanDate.HasValue, s => s.ScanDate <= endScanDate)

                    // Amount filters
                    .AndIf(netAmount.HasValue && netAmount.Value != 0,
                           s => s.NetAmount == netAmount)

                    .AndIf(taxCodeID.HasValue && taxCodeID.Value != 0,
                           s => s.TaxCodeID == taxCodeID)

                    .AndIf(taxAmount.HasValue && taxAmount.Value != 0,
                           s => s.TaxAmount == taxAmount)

                    .AndIf(!string.IsNullOrWhiteSpace(currency),
                           s => s.Currency == currency)

                    .AndIf(totalAmount.HasValue && totalAmount.Value != 0,
                           s => s.TotalAmount == totalAmount)

                    // Keyword
                    .AndIf(!string.IsNullOrWhiteSpace(keyword),
                           s => s.Keyword != null &&
                                s.Keyword.KeywordName.Contains(keyword!))

                    // Map ID
                    .AndIf(!string.IsNullOrWhiteSpace(mapID), s => s.MapID == mapID)

                    // Invoice ID
                    .AndIf(!string.IsNullOrWhiteSpace(invoiceID),
                           s => s.InvoiceID.ToString() == invoiceID)
                    /*
                       Daystill due
                       the value of invDueDateCalculation is base on web/assets/json
                     */
                    .AndIf(daystillDue.HasValue && daystillDue.Value != 0,
                           s => s.EntityProfile != null &&
                                (
                                    // Based on ScanDate
                                    (s.EntityProfile.InvDueDateCalculation == 1 &&
                                     s.ScanDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.ScanDate.Value,
                                         s.ScanDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on CreatedDate
                                    (s.EntityProfile.InvDueDateCalculation == 2 &&
                                     s.CreatedDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.CreatedDate.Value,
                                         s.CreatedDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on InvoiceDate
                                    (s.EntityProfile.InvDueDateCalculation == 3 &&
                                     s.InvoiceDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.InvoiceDate.Value,
                                         s.InvoiceDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)));

            var query =
                _dbcontext.InvoiceArchives.AsNoTracking().AsExpandable().Where(
                    predicate);

            var sortDictionary = new Dictionary<string, string>() {
        { "suppName", "suppName" },
        { "displayInvoiceDate", "invoiceDate" },
        { "invoiceNo", "invoiceNo" },
        { "poNo", "poNo" },
        { "displayDueDate", "dueDate" },
        { "displayGrossAmount", "grossAmount" },
        { "exceptionReason", "exceptionReason" },
        { "reason", "reason" }
      };

            sortField = sortDictionary.ContainsKey(sortField ?? string.Empty)
                            ? sortDictionary[sortField ?? string.Empty]
                            : null;

            if (string.IsNullOrEmpty(sortField))

            {
                query =
                    query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery =
                await query
                    .Select(i => new ArchiveInvoiceSearchDto
                    {
                        InvoiceID = i.InvoiceID,
                        Entity = i.EntityProfile != null ? i.EntityProfile.EntityName
                                                       : string.Empty,
                        SuppName = i.SupplierInfo != null ? i.SupplierInfo.SupplierName
                                                        : string.Empty,
                        InvoiceNo = i.InvoiceNo,
                        PoNo = i.PoNo,
                        InvoiceDate = i.InvoiceDate == null
                                        ? null
                                        : i.InvoiceDate.Value.UtcDateTime,
                        DueDate =
                          i.DueDate == null ? null : i.DueDate.Value.UtcDateTime,
                        GrossAmount = i.TotalAmount,
                        ExceptionReason = null,
                        IsSelected = false
                    })
                    .ToListAsync();

            var pagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                                 .ToPaginatedListAsync(pageNumber, pageSize, token);
            return pagination;
        }

        public async Task<PaginatedList<ExceptionInvoiceSearchDto>>
        GetExceptionInvoiceSearch(
            string? SupplierName, string? InvoiceNo, string? PONo, int pageNumber,
            int pageSize, string? sortField, int? sortOrder, int roleId, string? paymentTerm,
            string? supplierNo, string? suppABN, string? suppBankAccount,
            int? entityProfileID, string? grNo, DateTime? startInvoiceDate,
            DateTime? endInvoiceDate, DateTime? startDueDate, DateTime? endDueDate,
            int? daystillDue, decimal? netAmount, int? taxCodeID,
            decimal? taxAmount, string? currency, decimal? totalAmount,
            string? invRoutingFlowName, string? nextRole, string? keyword,
            string? mapID, DateTime? startScanDate, DateTime? endScanDate,
            string? invoiceID, CancellationToken token)
        {
            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(i => i.StatusType == InvoiceStatusType.Exception || i.QueueType == InvoiceQueueType.ExceptionQueue);

            var roleEntityIds = await _dbcontext.RoleEntities
            .Where(r => r.RoleID == roleId)
            .Select(r => r.EntityProfileID)
            .ToListAsync(token);

            if (roleEntityIds.Any())
            {
                predicate = predicate.And(i => i.EntityProfileID.HasValue && roleEntityIds.Contains(i.EntityProfileID.Value));
            }

            predicate =
                predicate
                    .AndIf(!string.IsNullOrEmpty(SupplierName),
                           s => s.SupplierInfo!.SupplierName!.Contains(SupplierName!))
                    .AndIf(!string.IsNullOrEmpty(InvoiceNo),
                           s => s.InvoiceNo!.Contains(InvoiceNo!))
                    .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!))

                    .AndIf(!string.IsNullOrWhiteSpace(supplierNo),
                           s => s.SupplierInfo!.SupplierID!.Contains(supplierNo!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppABN),
                           s => s.SupplierInfo!.SupplierTaxID!.Contains(suppABN!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppBankAccount),
                           s => s.SuppBankAccount!.Contains(suppBankAccount!))

                    // Payment Term (string filter, not numeric)
                    .AndIf(!string.IsNullOrWhiteSpace(paymentTerm),
                           s => s.PaymentTerm != null && s.PaymentTerm == paymentTerm)

                    // Entity Profile
                    .AndIf(entityProfileID.HasValue && entityProfileID.Value != 0,
                           s => s.EntityProfileID == entityProfileID)

                    // GR No
                    .AndIf(!string.IsNullOrWhiteSpace(grNo),
                           s => s.GrNo!.Contains(grNo!))

                    // Invoice Date Range
                    .AndIf(startInvoiceDate.HasValue,
                           s => s.InvoiceDate >= startInvoiceDate)

                    .AndIf(endInvoiceDate.HasValue,
                           s => s.InvoiceDate <= endInvoiceDate)

                    // Due Date Range
                    .AndIf(startDueDate.HasValue, s => s.DueDate >= startDueDate)

                    .AndIf(endDueDate.HasValue, s => s.DueDate <= endDueDate)

                    // Scan Date Range
                    .AndIf(startScanDate.HasValue, s => s.ScanDate >= startScanDate)

                    .AndIf(endScanDate.HasValue, s => s.ScanDate <= endScanDate)

                    // Amount filters
                    .AndIf(netAmount.HasValue && netAmount.Value != 0,
                           s => s.NetAmount == netAmount)

                    .AndIf(taxCodeID.HasValue && taxCodeID.Value != 0,
                           s => s.TaxCodeID == taxCodeID)

                    .AndIf(taxAmount.HasValue && taxAmount.Value != 0,
                           s => s.TaxAmount == taxAmount)

                    .AndIf(!string.IsNullOrWhiteSpace(currency),
                           s => s.Currency == currency)

                    .AndIf(totalAmount.HasValue && totalAmount.Value != 0,
                           s => s.TotalAmount == totalAmount)

                    // Routing Flow
                    .AndIf(!string.IsNullOrWhiteSpace(invRoutingFlowName),
                           s => s.InvRoutingFlow != null &&
                                s.InvRoutingFlow!.InvRoutingFlowName!.Contains(
                                    invRoutingFlowName!))

                    // Next Role
                    .AndIf(
                        !string.IsNullOrWhiteSpace(nextRole),
                        s => s.InvInfoRoutingLevels!.Where(x => x.InvFlowStatus == 0)
                                 .Select(x => x.Role!.RoleName)
                                 .FirstOrDefault()!.Contains(nextRole!))

                    // Keyword
                    .AndIf(!string.IsNullOrWhiteSpace(keyword),
                           s => s.Keyword != null &&
                                s.Keyword.KeywordName.Contains(keyword!))

                    // Map ID
                    .AndIf(!string.IsNullOrWhiteSpace(mapID), s => s.MapID == mapID)

                    // Invoice ID
                    .AndIf(!string.IsNullOrWhiteSpace(invoiceID),
                           s => s.InvoiceID.ToString() == invoiceID)
                    /*
                       Daystill due
                       the value of invDueDateCalculation is base on web/assets/json
                     */
                    .AndIf(daystillDue.HasValue && daystillDue.Value != 0,
                           s => s.EntityProfile != null &&
                                (
                                    // Based on ScanDate
                                    (s.EntityProfile.InvDueDateCalculation == 1 &&
                                     s.ScanDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.ScanDate.Value,
                                         s.ScanDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on CreatedDate
                                    (s.EntityProfile.InvDueDateCalculation == 2 &&
                                     s.CreatedDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.CreatedDate.Value,
                                         s.CreatedDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on InvoiceDate
                                    (s.EntityProfile.InvDueDateCalculation == 3 &&
                                     s.InvoiceDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.InvoiceDate.Value,
                                         s.InvoiceDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)));

            var query =
                _dbcontext.Invoices.AsNoTracking().AsExpandable().Where(predicate);

            var sortDictionary = new Dictionary<string, string>() {
        { "suppName", "suppName" },
        { "displayInvoiceDate", "invoiceDate" },
        { "invoiceNo", "invoiceNo" },
        { "poNo", "poNo" },
        { "displayDueDate", "dueDate" },
        { "displayGrossAmount", "grossAmount" },
        { "exceptionReason", "exceptionReason" },
        { "reason", "reason" }
      };

            sortField = sortDictionary.ContainsKey(sortField ?? string.Empty)
                            ? sortDictionary[sortField ?? string.Empty]
                            : null;

            if (string.IsNullOrEmpty(sortField))
            {
                query =
                    query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery =
                await query
                    .Select(i => new ExceptionInvoiceSearchDto
                    {
                        InvoiceID = i.InvoiceID,
                        Entity = i.EntityProfile!.EntityName,
                        SuppName = i.SupplierInfo!.SupplierName,
                        InvoiceNo = i.InvoiceNo,
                        PoNo = i.PoNo,
                        InvoiceDate = i.InvoiceDate == null
                                        ? null
                                        : i.InvoiceDate.Value.UtcDateTime,
                        DueDate =
                          i.DueDate == null ? null : i.DueDate.Value.UtcDateTime,
                        GrossAmount = i.TotalAmount,
                        ExceptionReason = string.Join(
                          "; ",
                          i.InvoiceActivityLog!
                                  .Where(
                                      a => a.InvoiceID == i.InvoiceID &&
                                           a.IsCurrentValidationContext == true &&
                                             //       (a.Action == InvoiceActionType.Validate ||
                                             //       a.Action == InvoiceActionType.Import) &&
                                             a.Action == InvoiceActionType.Validate &&
                                           !string.IsNullOrEmpty(a.Reason))
                                  .Select(a => a.Reason) ??
                              Enumerable.Empty<string>()),
                        IsSelected = false
                    })
                    .ToListAsync();

            var pagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                                 .ToPaginatedListAsync(pageNumber, pageSize, token);
            return pagination;
        }

        public async Task<PaginatedList<InvAllocLineDto>> GetInvAllocLinePerInvoice(
            long? invoiceID, int pageNumber, int pageSize, string? sortField,
            int? sortOrder, CancellationToken token)
        {
            ExpressionStarter<InvAllocLine> predicate =
                PredicateBuilder.New<InvAllocLine>();

            predicate =
                predicate.AndIf(invoiceID.HasValue, i => i.InvoiceID == invoiceID);

            var query = _dbcontext.InvoicesAllocLines.AsNoTracking()
                            .AsQueryable()
                            .AsExpandable()
                            .Where(predicate);

            if (string.IsNullOrEmpty(sortField))
            {
                query =
                    query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
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
                FreeFields =
                  ia.FreeFields != null
                      ? ia.FreeFields
                            .Select(f => new InvAllocLineFreeFieldDto
                            {
                                FieldKey = f.FieldKey,
                                FieldValue = f.FieldValue
                            })
                            .ToList()
                      : new List<InvAllocLineFreeFieldDto>(),

                Dimensions = ia.Dimensions != null
                               ? ia.Dimensions
                                     .Select(f => new InvAllocLineDimensionDto
                                     {
                                         DimensionKey = f.DimensionKey,
                                         DimensionValue = f.DimensionValue
                                     })
                                     .ToList()
                               : new List<InvAllocLineDimensionDto>(),
            });

            var invoiceAllocationPagination =
                await dtoQuery.OrderByDynamic(sortField, sortOrder)
                    .ToPaginatedListAsync(pageNumber, pageSize, token);
            return invoiceAllocationPagination;
        }

        public async Task<List<InvAllocEntryDto>> GetInvoiceAllocationInfo(
            long? invoiceID, CancellationToken token)
        {
            ExpressionStarter<InvAllocLine> predicate =
                PredicateBuilder.New<InvAllocLine>(alloc =>
                                                       alloc.InvoiceID == invoiceID!);

            predicate =
                predicate.AndIf(invoiceID.HasValue, i => i.InvoiceID == invoiceID);

            var query = _dbcontext.InvoicesAllocLines
                            .Include(m => m.PurchaseOrderMatchTrackings)
                            .AsNoTracking()
                            .AsQueryable()
                            .AsExpandable()
                            .Where(predicate);

            return await query
                .Select(dto => new InvAllocEntryDto
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
                    IsFromPOMatching = dto.PurchaseOrderMatchTrackings.Any(
                      a => a.InvAllocLineID == dto.InvAllocLineID),
                })
                .ToListAsync(token);
        }

        public async Task<InvoiceDto> GetInvoiceInfo(long invoiceID,
                                                     CancellationToken token)
        {
            var invoice =
                await _dbcontext.Invoices.AsNoTracking()
                    .Where(x => x.InvoiceID == invoiceID)
                    .Select(x => new InvoiceDto
                    {
                        InvoiceID = x.InvoiceID,
                        InvoiceNo = x.InvoiceNo,
                        InvoiceDate =
                          x.InvoiceDate.HasValue ? x.InvoiceDate!.Value : null,
                        MapID = x.MapID,
                        ScanDate = x.ScanDate,
                        CreatedDate = x.CreatedDate,
                        EntityProfileID = x.EntityProfile!.EntityProfileID,
                        SupplierInfoID = x.SupplierInfoID,
                        KeywordID = x.KeywordID,
                        Keyword =
                          x.Keyword != null ? x.Keyword.KeywordName : string.Empty,
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
                        ApproverRole = x.ApprovedUserInvoices != null
                                         ? x.ApprovedUserInvoices.UserID
                                         : string.Empty,
                        ApprovedUser = x.ApproverInvoices != null
                                         ? x.ApproverInvoices.RoleName
                                         : string.Empty,
                        QueueType = x.QueueType,
                        StatusType = x.StatusType,
                        RoutingFlowName = x.InvRoutingFlow != null
                                            ? x.InvRoutingFlow.InvRoutingFlowName
                                            : null,
                        InvRoutingFlowID = x.InvRoutingFlowID,
                        InvRoutingFlowName = x.InvRoutingFlow != null
                                               ? x.InvRoutingFlow.InvRoutingFlowName
                                               : null,
                        NextRole =
                          x.InvInfoRoutingLevels! != null
                              ? x.StatusType == InvoiceStatusType.ReadyForExport
                                    ? string.Empty
                                    : x.InvInfoRoutingLevels!
                                          .Where(i => i.InvFlowStatus == 0)
                                          .OrderBy(o => o.Level)
                                          .Select(s => s.Role.RoleName)
                                          .FirstOrDefault()
                              : "N/A",
                        Reason =
                          (x.StatusType == InvoiceStatusType.Rejected)
                              ? (x.InvoiceActivityLog
                                     .Where(i => i.CurrentStatus ==
                                                     InvoiceStatusType.Rejected &&
                                                 i.Action.HasValue &&
                                                 new[] { InvoiceActionType.Reject,
                                                   InvoiceActionType.Import,
                                                   InvoiceActionType.Submit }
                                                     .Contains(i.Action.Value))
                                     .OrderByDescending(i => i.CreatedDate)
                                     .Select(i => i.Reason)
                                     .FirstOrDefault() ??
                                 string.Empty)
                              : string.Empty,
                        InvoiceAllocationLines =
                          x.InvoiceAllocationLines!
                              .Select(dto => new InvAllocLineDto
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
                              })
                              .ToList(),
                        InvInfoRoutingLevels =
                          x.InvInfoRoutingLevels!
                              .Select(dto => new InvInfoRoutingLevelDto
                              {
                                  InvInfoRoutingLevelID = dto.InvInfoRoutingLevelID,
                                  InvoiceID = dto.InvoiceID,
                                  InvRoutingFlowID = dto.InvRoutingFlowID,
                                  RoleID = dto.RoleID,
                                  Level = dto.Level,

                              })
                              .ToList()

                    })
                    .FirstOrDefaultAsync();

            return invoice!;
        }

        public async Task<PaginatedList<InvMyInvoiceSearchDto>> GetMyInvoiceSearch(
            string? SupplierName, string? InvoiceNo, string? PONo, int pageNumber,
            int pageSize, string? sortField, int? sortOrder, int roleId,
            string? paymentTerm, string? supplierNo, string? suppABN,
            string? suppBankAccount, int? entityProfileID, string? grNo,
            DateTime? startInvoiceDate, DateTime? endInvoiceDate,
            DateTime? startDueDate, DateTime? endDueDate, int? daystillDue,
            decimal? netAmount, int? taxCodeID, decimal? taxAmount,
            string? currency, decimal? totalAmount, string? invRoutingFlowName,
            string? nextRole, string? keyword, string? mapID,
            DateTime? startScanDate, DateTime? endScanDate, string? invoiceID,
            CancellationToken token)
        {
            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(i => (i.StatusType == InvoiceStatusType.ForApproval || i.StatusType == InvoiceStatusType.ApprovalOnHold) && i.ApproverRole == roleId);

            var roleEntityIds = await _dbcontext.RoleEntities
            .Where(r => r.RoleID == roleId)
            .Select(r => r.EntityProfileID)
            .ToListAsync(token);

            if (roleEntityIds.Any())
            {
                predicate = predicate.And(i => i.EntityProfileID.HasValue && roleEntityIds.Contains(i.EntityProfileID.Value));
            }

            predicate =
                predicate
                    .AndIf(!string.IsNullOrEmpty(SupplierName),
                           s => s.SupplierInfo!.SupplierName!.Contains(SupplierName!))
                    .AndIf(!string.IsNullOrEmpty(InvoiceNo),
                           s => s.InvoiceNo!.Contains(InvoiceNo!))
                    .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!))

                    .AndIf(!string.IsNullOrWhiteSpace(supplierNo),
                           s => s.SupplierInfo!.SupplierID!.Contains(supplierNo!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppABN),
                           s => s.SupplierInfo!.SupplierTaxID!.Contains(suppABN!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppBankAccount),
                           s => s.SuppBankAccount!.Contains(suppBankAccount!))

                    // Payment Term (string filter, not numeric)
                    .AndIf(!string.IsNullOrWhiteSpace(paymentTerm),
                           s => s.PaymentTerm != null && s.PaymentTerm == paymentTerm)

                    // Entity Profile
                    .AndIf(entityProfileID.HasValue && entityProfileID.Value != 0,
                           s => s.EntityProfileID == entityProfileID)

                    // GR No
                    .AndIf(!string.IsNullOrWhiteSpace(grNo),
                           s => s.GrNo!.Contains(grNo!))

                    // Invoice Date Range
                    .AndIf(startInvoiceDate.HasValue,
                           s => s.InvoiceDate >= startInvoiceDate)

                    .AndIf(endInvoiceDate.HasValue,
                           s => s.InvoiceDate <= endInvoiceDate)

                    // Due Date Range
                    .AndIf(startDueDate.HasValue, s => s.DueDate >= startDueDate)

                    .AndIf(endDueDate.HasValue, s => s.DueDate <= endDueDate)

                    // Scan Date Range
                    .AndIf(startScanDate.HasValue, s => s.ScanDate >= startScanDate)

                    .AndIf(endScanDate.HasValue, s => s.ScanDate <= endScanDate)

                    // Amount filters
                    .AndIf(netAmount.HasValue && netAmount.Value != 0,
                           s => s.NetAmount == netAmount)

                    .AndIf(taxCodeID.HasValue && taxCodeID.Value != 0,
                           s => s.TaxCodeID == taxCodeID)

                    .AndIf(taxAmount.HasValue && taxAmount.Value != 0,
                           s => s.TaxAmount == taxAmount)

                    .AndIf(!string.IsNullOrWhiteSpace(currency),
                           s => s.Currency == currency)

                    .AndIf(totalAmount.HasValue && totalAmount.Value != 0,
                           s => s.TotalAmount == totalAmount)

                    // Routing Flow
                    .AndIf(!string.IsNullOrWhiteSpace(invRoutingFlowName),
                           s => s.InvRoutingFlow != null &&
                                s.InvRoutingFlow!.InvRoutingFlowName!.Contains(
                                    invRoutingFlowName!))

                    // Next Role
                    .AndIf(
                        !string.IsNullOrWhiteSpace(nextRole),
                        s => s.InvInfoRoutingLevels!.Where(x => x.InvFlowStatus == 0)
                                 .Select(x => x.Role!.RoleName)
                                 .FirstOrDefault()!.Contains(nextRole!))

                    // Keyword
                    .AndIf(!string.IsNullOrWhiteSpace(keyword),
                           s => s.Keyword != null &&
                                s.Keyword.KeywordName.Contains(keyword!))

                    // Map ID
                    .AndIf(!string.IsNullOrWhiteSpace(mapID), s => s.MapID == mapID)

                    // Invoice ID
                    .AndIf(!string.IsNullOrWhiteSpace(invoiceID),
                           s => s.InvoiceID.ToString() == invoiceID)
                    /*
                       Daystill due
                       the value of invDueDateCalculation is base on web/assets/json
                     */
                    .AndIf(daystillDue.HasValue && daystillDue.Value != 0,
                           s => s.EntityProfile != null &&
                                (
                                    // Based on ScanDate
                                    (s.EntityProfile.InvDueDateCalculation == 1 &&
                                     s.ScanDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.ScanDate.Value,
                                         s.ScanDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on CreatedDate
                                    (s.EntityProfile.InvDueDateCalculation == 2 &&
                                     s.CreatedDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.CreatedDate.Value,
                                         s.CreatedDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on InvoiceDate
                                    (s.EntityProfile.InvDueDateCalculation == 3 &&
                                     s.InvoiceDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.InvoiceDate.Value,
                                         s.InvoiceDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)));

            var query =
                _dbcontext.Invoices.AsNoTracking().AsExpandable().Where(predicate);

            var sortDictionary = new Dictionary<string, string>() {
        { "suppName", "suppName" },
        { "displayInvoiceDate", "invoiceDate" },
        { "invoiceNo", "invoiceNo" },
        { "poNo", "poNo" },
        { "displayDueDate", "dueDate" },
        { "displayGrossAmount", "grossAmount" },
        { "exceptionReason", "exceptionReason" },
        { "reason", "reason" }
      };

            sortField = sortDictionary.ContainsKey(sortField ?? string.Empty)
                            ? sortDictionary[sortField ?? string.Empty]
                            : null;

            if (string.IsNullOrEmpty(sortField))

            {
                query =
                    query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery =
                await query
                    .Select(i => new InvMyInvoiceSearchDto
                    {
                        InvoiceID = i.InvoiceID,
                        Entity = i.EntityProfile!.EntityName,
                        SuppName = i.SupplierInfo!.SupplierName,
                        InvoiceNo = i.InvoiceNo,
                        PoNo = i.PoNo,
                        InvoiceDate = i.InvoiceDate == null
                                        ? null
                                        : i.InvoiceDate.Value.UtcDateTime,
                        DueDate =
                          i.DueDate == null ? null : i.DueDate.Value.UtcDateTime,
                        GrossAmount = i.TotalAmount,
                        NextRole =
                          i.InvInfoRoutingLevels != null
                              ? i.StatusType == InvoiceStatusType.ReadyForExport
                                    ? string.Empty
                                    : i.InvInfoRoutingLevels!
                                          .Where(i => i.InvFlowStatus == 0)
                                          .OrderBy(o => o.Level)
                                          .Select(s => s.Role.RoleName)
                                          .FirstOrDefault()
                              : "N/A",
                        ExceptionReason = string.Join(
                          "; ",
                          i.InvoiceActivityLog!
                                  .Where(
                                      a => a.InvoiceID == i.InvoiceID &&
                                           a.IsCurrentValidationContext == true &&
                                         // (a.Action == InvoiceActionType.Validate ||
                                         //   a.Action == InvoiceActionType.Import) &&
                                         a.Action == InvoiceActionType.Validate &&
                                           !string.IsNullOrEmpty(a.Reason))
                                  .Select(a => a.Reason) ??
                              Enumerable.Empty<string>()),
                        IsSelected = false
                    })
                    .ToListAsync();

            var myInvoiceSearchPagination =
                await dtoQuery.OrderByDynamic(sortField, sortOrder)
                    .ToPaginatedListAsync(pageNumber, pageSize, token);
            return myInvoiceSearchPagination;
        }

        public async Task<PaginatedList<RejectedInvoiceSearchDto>>
        GetRejectedInvoiceSearch(
            string? SupplierName, string? InvoiceNo, string? PONo, int pageNumber,
            int pageSize, string? sortField, int? sortOrder, int roleId, string? paymentTerm,
            string? supplierNo, string? suppABN, string? suppBankAccount,
            int? entityProfileID, string? grNo, DateTime? startInvoiceDate,
            DateTime? endInvoiceDate, DateTime? startDueDate, DateTime? endDueDate,
            int? daystillDue, decimal? netAmount, int? taxCodeID,
            decimal? taxAmount, string? currency, decimal? totalAmount,
            string? invRoutingFlowName, string? nextRole, string? keyword,
            string? mapID, DateTime? startScanDate, DateTime? endScanDate,
            string? invoiceID, CancellationToken token)
        {
            ExpressionStarter<Invoice> predicate = PredicateBuilder.New<Invoice>(i => i.StatusType == InvoiceStatusType.Rejected || i.QueueType == InvoiceQueueType.RejectionQueue);

            var roleEntityIds = await _dbcontext.RoleEntities
            .Where(r => r.RoleID == roleId)
            .Select(r => r.EntityProfileID)
            .ToListAsync(token);

            if (roleEntityIds.Any())
            {
                predicate = predicate.And(i => i.EntityProfileID.HasValue && roleEntityIds.Contains(i.EntityProfileID.Value));
            }

            predicate =
                predicate
                    .AndIf(!string.IsNullOrEmpty(SupplierName),
                           s => s.SupplierInfo!.SupplierName!.Contains(SupplierName!))
                    .AndIf(!string.IsNullOrEmpty(InvoiceNo),
                           s => s.InvoiceNo!.Contains(InvoiceNo!))
                    .AndIf(!string.IsNullOrEmpty(PONo), s => s.PoNo!.Contains(PONo!))
                    .AndIf(!string.IsNullOrWhiteSpace(supplierNo),
                           s => s.SupplierInfo!.SupplierID!.Contains(supplierNo!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppABN),
                           s => s.SupplierInfo!.SupplierTaxID!.Contains(suppABN!))

                    .AndIf(!string.IsNullOrWhiteSpace(suppBankAccount),
                           s => s.SuppBankAccount!.Contains(suppBankAccount!))

                    // Payment Term (string filter, not numeric)
                    .AndIf(!string.IsNullOrWhiteSpace(paymentTerm),
                           s => s.PaymentTerm != null && s.PaymentTerm == paymentTerm)

                    // Entity Profile
                    .AndIf(entityProfileID.HasValue && entityProfileID.Value != 0,
                           s => s.EntityProfileID == entityProfileID)

                    // GR No
                    .AndIf(!string.IsNullOrWhiteSpace(grNo),
                           s => s.GrNo!.Contains(grNo!))

                    // Invoice Date Range
                    .AndIf(startInvoiceDate.HasValue,
                           s => s.InvoiceDate >= startInvoiceDate)

                    .AndIf(endInvoiceDate.HasValue,
                           s => s.InvoiceDate <= endInvoiceDate)

                    // Due Date Range
                    .AndIf(startDueDate.HasValue, s => s.DueDate >= startDueDate)

                    .AndIf(endDueDate.HasValue, s => s.DueDate <= endDueDate)

                    // Scan Date Range
                    .AndIf(startScanDate.HasValue, s => s.ScanDate >= startScanDate)

                    .AndIf(endScanDate.HasValue, s => s.ScanDate <= endScanDate)

                    // Amount filters
                    .AndIf(netAmount.HasValue && netAmount.Value != 0,
                           s => s.NetAmount == netAmount)

                    .AndIf(taxCodeID.HasValue && taxCodeID.Value != 0,
                           s => s.TaxCodeID == taxCodeID)

                    .AndIf(taxAmount.HasValue && taxAmount.Value != 0,
                           s => s.TaxAmount == taxAmount)

                    .AndIf(!string.IsNullOrWhiteSpace(currency),
                           s => s.Currency == currency)

                    .AndIf(totalAmount.HasValue && totalAmount.Value != 0,
                           s => s.TotalAmount == totalAmount)

                    // Routing Flow
                    .AndIf(!string.IsNullOrWhiteSpace(invRoutingFlowName),
                           s => s.InvRoutingFlow != null &&
                                s.InvRoutingFlow!.InvRoutingFlowName!.Contains(
                                    invRoutingFlowName!))

                    // Next Role
                    .AndIf(
                        !string.IsNullOrWhiteSpace(nextRole),
                        s => s.InvInfoRoutingLevels!.Where(x => x.InvFlowStatus == 0)
                                 .Select(x => x.Role!.RoleName)
                                 .FirstOrDefault()!.Contains(nextRole!))

                    // Keyword
                    .AndIf(!string.IsNullOrWhiteSpace(keyword),
                           s => s.Keyword != null &&
                                s.Keyword.KeywordName.Contains(keyword!))

                    // Map ID
                    .AndIf(!string.IsNullOrWhiteSpace(mapID), s => s.MapID == mapID)

                    // Invoice ID
                    .AndIf(!string.IsNullOrWhiteSpace(invoiceID),
                           s => s.InvoiceID.ToString() == invoiceID)
                    /*
                       Daystill due
                       the value of invDueDateCalculation is base on web/assets/json
                     */
                    .AndIf(daystillDue.HasValue && daystillDue.Value != 0,
                           s => s.EntityProfile != null &&
                                (
                                    // Based on ScanDate
                                    (s.EntityProfile.InvDueDateCalculation == 1 &&
                                     s.ScanDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.ScanDate.Value,
                                         s.ScanDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on CreatedDate
                                    (s.EntityProfile.InvDueDateCalculation == 2 &&
                                     s.CreatedDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.CreatedDate.Value,
                                         s.CreatedDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)

                                    ||

                                    // Based on InvoiceDate
                                    (s.EntityProfile.InvDueDateCalculation == 3 &&
                                     s.InvoiceDate != null &&
                                     !string.IsNullOrEmpty(s.PaymentTerm) &&
                                     EF.Functions.DateDiffDay(
                                         s.InvoiceDate.Value,
                                         s.InvoiceDate.Value.AddDays(
                                             int.Parse(s.PaymentTerm!))) -
                                             1 ==
                                         daystillDue.Value)));

            var query =
                _dbcontext.Invoices.AsNoTracking().AsExpandable().Where(predicate);

            var sortDictionary = new Dictionary<string, string>() {
        { "suppName", "suppName" },
        { "displayInvoiceDate", "invoiceDate" },
        { "invoiceNo", "invoiceNo" },
        { "poNo", "poNo" },
        { "displayDueDate", "dueDate" },
        { "displayGrossAmount", "grossAmount" },
        { "exceptionReason", "exceptionReason" },
        { "reason", "reason" }
      };
            sortField = sortDictionary.ContainsKey(sortField ?? string.Empty)
                            ? sortDictionary[sortField ?? string.Empty]
                            : null;

            if (string.IsNullOrEmpty(sortField))

            {
                query =
                    query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery =
                await query
                    .Select(i => new RejectedInvoiceSearchDto
                    {
                        InvoiceID = i.InvoiceID,
                        Entity = i.EntityProfile!.EntityName,
                        SuppName = i.SupplierInfo!.SupplierName,
                        InvoiceNo = i.InvoiceNo,
                        PoNo = i.PoNo,
                        InvoiceDate = i.InvoiceDate == null
                                        ? null
                                        : i.InvoiceDate.Value.UtcDateTime,
                        DueDate =
                          i.DueDate == null ? null : i.DueDate.Value.UtcDateTime,
                        GrossAmount = i.TotalAmount,
                        ArchiveDate = null,
                        InvoiceApprover = null,
                        Reason =
                          (i.StatusType == InvoiceStatusType.Rejected)
                              ? (i.InvoiceActivityLog
                                     .Where(x => x.CurrentStatus ==
                                                     InvoiceStatusType.Rejected &&
                                                 x.Action.HasValue &&
                                                 new[] { InvoiceActionType.Reject,
                                                   InvoiceActionType.Import,
                                                   InvoiceActionType.Submit }
                                                     .Contains(x.Action.Value))
                                     .OrderByDescending(x => x.CreatedDate)
                                     .Select(x => x.Reason)
                                     .FirstOrDefault() ??
                                 string.Empty)
                              : string.Empty,
                    })
                    .ToListAsync();

            var pagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                                 .ToPaginatedListAsync(pageNumber, pageSize, token);

            return pagination;
        }

        public async Task<PaginatedList<LoadInvoiceCommentsDto>>
        LoadInvoiceComments(long? InvoiceID, int pageNumber, int pageSize,
                            string? sortField, int? sortOrder,
                            CancellationToken token)
        {
            ExpressionStarter<InvoiceComment> predicate =
                PredicateBuilder.New<InvoiceComment>(true);

            predicate =
                predicate.AndIf(InvoiceID.HasValue, s => s.InvoiceID == InvoiceID);

            var query =
                _dbcontext.InvoiceComments.AsNoTracking().AsExpandable().Where(
                    predicate);

            if (string.IsNullOrEmpty(sortField))

            {
                query = query.OrderByDescending(p => p.CreatedDate);
            }

            var dtoQuery =
                query.AsEnumerable()
                    .Select(i => new LoadInvoiceCommentsDto
                    {
                        InvoiceCommentID = i.InvoiceCommentID,
                        Comment = i.Comment,
                        CreatedBy = i.CreatedBy!,
                        CreatedDate = i.CreatedDate?.ToLocalTime().ToString(
                          "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture)
                    })
                    .ToList();

            var pagination = await dtoQuery.OrderByDynamic(sortField, sortOrder)
                                 .ToPaginatedListAsync(pageNumber, pageSize, token);
            return pagination;
        }

        public async Task<InvoiceNavigationResultDto> GetAdjacentInvoiceId(
            long invoiceID, bool isNext, InvoiceStatusType? statusType,
            InvoiceQueueType? queueType, InvoiceSearchBaseDto? filter,
            PageDetailsDto? page, CancellationToken token)
        {
            var currentInvoiceState =
                await _dbcontext.Invoices.AsNoTracking()
                    .Where(i => i.InvoiceID == invoiceID)
                    .Select(i => new {
                        i.InvoiceID,
                        i.StatusType,
                        i.QueueType,
                        i.ApproverRole
                    })
                    .FirstOrDefaultAsync(token);

            if (currentInvoiceState == null)
            {
                return new InvoiceNavigationResultDto(false, null);
            }

            var statusFilter = statusType ?? currentInvoiceState.StatusType;
            var queueFilter = queueType ?? currentInvoiceState.QueueType;
            var searchFilter = filter;
            var pageDetails = page;

            ExpressionStarter<Invoice> predicate =
                PredicateBuilder.New<Invoice>(i => i.QueueType == queueFilter);

            predicate =
                predicate
                    .AndIf(!string.IsNullOrEmpty(searchFilter?.SuppName),
                           s => s.SupplierInfo!.SupplierName!.Contains(
                               searchFilter.SuppName!))
                    .AndIf(!string.IsNullOrEmpty(searchFilter?.InvoiceNo),
                           s => s.InvoiceNo!.Contains(searchFilter.InvoiceNo!))
                    .AndIf(!string.IsNullOrEmpty(searchFilter?.PoNo),
                           s => s.PoNo!.Contains(searchFilter.PoNo!));

            var query = _dbcontext.Invoices.AsNoTracking().Where(predicate);

            if (queueType == InvoiceQueueType.MyInvoices)
            {
                query = query.Where(w => w.ApproverRole ==
                                         currentInvoiceState.ApproverRole);
            }

            var dtoQuery =
                query
                    .Select(i => new InvoiceFilterDto
                    {
                        InvoiceID = i.InvoiceID,
                        Entity = i.EntityProfile!.EntityName,
                        SuppName = i.SupplierInfo!.SupplierName,
                        InvoiceNo = i.InvoiceNo,
                        PoNo = i.PoNo,
                        InvoiceDate = i.InvoiceDate,
                        DueDate = i.DueDate,
                        GrossAmount = i.TotalAmount.ToString("F2"),
                        NextRole =
                          i.InvInfoRoutingLevels != null
                              ? i.StatusType == InvoiceStatusType.ReadyForExport
                                    ? string.Empty
                                    : i.InvInfoRoutingLevels!
                                          .Where(i => i.InvFlowStatus == 0)
                                          .OrderBy(o => o.Level)
                                          .Select(s => s.Role.RoleName)
                                          .FirstOrDefault()
                              : "N/A",
                        ExceptionReason = string.Join(
                          "; ",
                          i.InvoiceActivityLog!
                                  .Where(
                                      a => a.InvoiceID == i.InvoiceID &&
                                           a.IsCurrentValidationContext == true &&
                                           (a.Action == InvoiceActionType.Validate ||
                                            a.Action == InvoiceActionType.Import) &&
                                           !string.IsNullOrEmpty(a.Reason))
                                  .Select(a => a.Reason) ??
                              Enumerable.Empty<string>()),
                        Reason =
                          (i.StatusType == InvoiceStatusType.Rejected)
                              ? (i.InvoiceActivityLog
                                     .Where(x => x.CurrentStatus ==
                                                     InvoiceStatusType.Rejected &&
                                                 x.Action.HasValue &&
                                                 new[] { InvoiceActionType.Reject,
                                                   InvoiceActionType.Import,
                                                   InvoiceActionType.Submit }
                                                     .Contains(x.Action.Value))
                                     .OrderByDescending(x => x.CreatedDate)
                                     .Select(x => x.Reason)
                                     .FirstOrDefault() ??
                                 string.Empty)
                              : string.Empty,
                        CreatedDate = i.CreatedDate,
                        LastUpdatedDate = i.LastUpdatedDate

                    })
                    .AsEnumerable();

            if (pageDetails == null)
            {
                dtoQuery =
                    dtoQuery.OrderByDescending(o => o.LastUpdatedDate ?? o.CreatedDate)
                        .ThenBy(o => o.InvoiceID);
            }
            else
            {
                dtoQuery = OrderByDynamic<InvoiceFilterDto>(
                    dtoQuery, pageDetails.SortField, pageDetails.SortOrder);
            }

            var result = dtoQuery.ToList();

            var orderedIds = result.Select(x => x.InvoiceID).ToList();

            var currentIndex = orderedIds.IndexOf(invoiceID);
            long? adjacentInvoiceID = null;
            if (currentIndex == -1)
            {
                if (result.Count == 0)
                    return new InvoiceNavigationResultDto(true, null);
                adjacentInvoiceID = orderedIds[0];

                return new InvoiceNavigationResultDto(true, adjacentInvoiceID);
            }

            if (isNext)
            {
                if (currentIndex + 1 < orderedIds.Count)
                    adjacentInvoiceID = orderedIds[currentIndex + 1];
            }
            else
            {
                if (currentIndex - 1 >= 0)
                    adjacentInvoiceID = orderedIds[currentIndex - 1];
            }

            return new InvoiceNavigationResultDto(true, adjacentInvoiceID);
        }

        private IEnumerable<T> OrderByDynamic<T>(IEnumerable<T> source,
                                                 string? sortField,
                                                 int? sortOrder)
        {
            if (string.IsNullOrWhiteSpace(sortField))
                return source;

            // Case-insensitive, unambiguous property lookup
            var property = typeof(T).GetProperties().FirstOrDefault(
                p => string.Equals(p.Name, sortField,
                                   StringComparison.OrdinalIgnoreCase));

            if (property == null)
                return source;  // No matching property → skip sorting

            var param = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(param, property);
            var lambda = Expression.Lambda<Func<T, object>>(
                Expression.Convert(propertyAccess, typeof(object)), param);

            return sortOrder == -1 ? source.OrderByDescending(lambda.Compile())
                                   : source.OrderBy(lambda.Compile());
        }

        public async Task<PaginatedList<InvSearchSupplierDto>>
        SearchSupplierWithPagination(string? SupplierID, string? SupplierName,
                                     int pageNumber, int pageSize,
                                     string? sortField, int? sortOrder,
                                     CancellationToken token)
        {
            ExpressionStarter<SupplierInfo> predicate =
                PredicateBuilder.New<SupplierInfo>(s => s.IsActive || !s.IsActive);

            predicate = predicate
                            .AndIf(!string.IsNullOrEmpty(SupplierID),
                                   s => s.SupplierID!.Contains(SupplierID!))
                            .AndIf(!string.IsNullOrEmpty(SupplierName),
                                   s => s.SupplierName!.Contains(SupplierName!));

            var query = _dbcontext.SupplierInfos.Include(s => s.Account)
                            .Include(s => s.EntityProfile)
                            .AsNoTracking()
                            .AsQueryable()
                            .AsExpandable()
                            .Where(predicate);

            if (string.IsNullOrEmpty(sortField))

            {
                query =
                    query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);
            }

            var dtoQuery = query.Select(s => new InvSearchSupplierDto
            {
                SupplierInfoID = s.SupplierInfoID,
                SupplierID = s.SupplierID,
                SupplierName = s.SupplierName,
                SupplierTaxID = s.SupplierTaxID,
                Entity = s.EntityProfile!.EntityName,
                IsActive = s.IsActive,
                InvoiceRoutingFlowID =
                  s.InvRoutingFlow != null ? s.InvRoutingFlow.InvRoutingFlowID : null,
                InvoiceRoutingFlowName = s.InvRoutingFlow != null
                                           ? s.InvRoutingFlow.InvRoutingFlowName ?? ""
                                           : null
            });

            var supplierPagination =
                await dtoQuery.OrderByDynamic(sortField, sortOrder)
                    .ToPaginatedListAsync(pageNumber, pageSize, token);
            return supplierPagination;
        }
        public async Task<GetInvoiceStatusDto?> GetInvoiceStatusAsync(
            long invoiceId, CancellationToken cancellationToken)
        {
            return await _dbcontext.Invoices.AsNoTracking()
                .Where(i => i.InvoiceID == invoiceId)
                .Select(i => new GetInvoiceStatusDto
                {
                    Status = i.StatusType,
                    Queue = i.QueueType
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> ChangeHoldStateAsync(
            InvStatusChangeDto dto, string updatedBy,
            CancellationToken cancellationToken)
        {
            var invoice = await _dbcontext.Invoices.FirstOrDefaultAsync(
                i => i.InvoiceID == dto.InvoiceID, cancellationToken);

            if (invoice == null)
                return false;

            var oldStatus = invoice.StatusType;

            bool isHold = dto.Status == InvoiceStatusType.ApprovalOnHold ||
                          dto.Status == InvoiceStatusType.ExceptionOnHold;

            invoice.StatusType = dto.Status;
            invoice.SetAuditFieldsOnUpdate(updatedBy);

            var reasonText = isHold ? $"ROUTE TO HOLD REASON: {dto.Reason}"
                                    : $"ROUTE TO UN-HOLD REASON: {dto.Reason}";

            var activityLog = new ActivityLog
            {
                InvoiceID = (int)dto.InvoiceID,
                ActionBy = updatedBy,
                Activity = "UPDATE",
                Module = invoice.QueueType.ToString(),
                ColumnName = "Reason",
                NewValue = reasonText,
                ActivityDate = DateTime.UtcNow
            };

            var invoiceActivityLog = InvoicActionLogFactory.CreateInvoiceActivityLog(
                dto, oldStatus,
                isHold ? InvoiceActionType.Hold : InvoiceActionType.Unhold);

            invoiceActivityLog.SetAuditFieldsOnCreate(updatedBy);

            await _dbcontext.ActivityLogs.AddAsync(activityLog, cancellationToken);
            await _dbcontext.InvoiceActivityLogs.AddAsync(invoiceActivityLog,
                                                          cancellationToken);

            await _dbcontext.SaveChangesAsync(cancellationToken);

            return true;
        }
        public async Task<Invoice?> GetByIdWithDetailsAsync(long invoiceId)
        {
            return await _dbcontext.Invoices
                .Include(i => i.InvoiceAllocationLines)
                .Include(i => i.InvoiceActivityLog)
                .FirstOrDefaultAsync(i => i.InvoiceID == invoiceId);
        }

        public async Task<InvValidationResponseDto> ValidateInvoiceAsync(
            Invoice invoice,
            string updatedBy,
            string environmentName,
            CancellationToken cancellationToken)
        {
            var currentQueueType = invoice.QueueType;

            // DATA QUERIES
            var invoiceDataToValidate = await _dbcontext.Invoices
                .AsNoTracking()
                .Where(i =>
                    i.InvoiceID != invoice.InvoiceID &&
                    i.StatusType != InvoiceStatusType.Rejected &&
                    i.StatusType != InvoiceStatusType.Archived)
                .ToListAsync(cancellationToken);

            var suppliers = await _dbcontext.SupplierInfos
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var taxCodes = await _dbcontext.TaxCodes
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var entities = await _dbcontext.EntityProfiles
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var purchaseOrders = await _dbcontext.PurchaseOrders
                .AsNoTracking()
                .Where(po => po.PoNo != null)
                .ToListAsync(cancellationToken);

            var poMatchingConfig = await _dbcontext.EntityMatchingConfigs
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.EntityProfileID == invoice.EntityProfileID &&
                    x.ConfigType == MatchingConfigType.PO,
                    cancellationToken);

            var matchedPurchaseOrders = await _dbcontext.PurchaseOrderMatchTrackings
                .AsNoTracking()
                .Include(x => x.PurchaseOrder)
                .Include(x => x.PurchaseOrderLine)
                .Where(x => x.InvoiceID == invoice.InvoiceID)
                .ToListAsync(cancellationToken);

            // RULE ENGINE
            var ruleFilePath = Path.Combine("rulesfiles", $"cbsap.{environmentName}.json");

            if (!File.Exists(ruleFilePath))
                throw new FileNotFoundException($"Validation rules file not found: {ruleFilePath}");

            var rules = ValidationRuleFactory.Load(ruleFilePath);
            var engine = new ValidationEngine(rules);

            var runtimeContext = new Dictionary<string, object>
            {
                ["InvoiceRecords"] = invoiceDataToValidate,
                ["SupplierInfos"] = suppliers,
                ["TaxCodes"] = taxCodes,
                ["EntityProfile"] = entities,
                ["PurchaseOrders"] = purchaseOrders,
                ["POMatchingConfig"] = poMatchingConfig!,
                ["MatchedPurchaseOrders"] = matchedPurchaseOrders
            };

            // VALIDATION EXECUTION
            var failures = engine.Validate(invoice, runtimeContext, out bool stopEarly);

            // LOGS
            var existingLogs = await _dbcontext.InvoiceActivityLogs
                .Where(l =>
        l.InvoiceID == invoice.InvoiceID &&
        (l.Action == InvoiceActionType.Validate ||
         l.Action == InvoiceActionType.Import))
    .ToListAsync(cancellationToken);

     

            foreach (var log in existingLogs)
                log.IsCurrentValidationContext = true;

            // SUCCESS CASE
            if (!failures.Any())
            {
                var log = new InvoiceActivityLog
                {
                    InvoiceID = invoice.InvoiceID,
                    PreviousStatus = invoice.StatusType,
                    CurrentStatus = invoice.StatusType,
                    Action = InvoiceActionType.Validate,
                    IsCurrentValidationContext = true
                };

                log.SetAuditFieldsOnCreate(updatedBy);

                _dbcontext.InvoiceActivityLogs.Add(log);

                await _dbcontext.SaveChangesAsync(cancellationToken);

                return new InvValidationResponseDto
                {
                    QueueType = currentQueueType!.Value,
                    InvoiceActionType = InvoiceActionType.Validate.ToString(),
                    FailureMessages = string.Empty
                };
            }

            // STOP EARLY (CRITICAL)
            if (stopEarly)
            {
                var critical = failures.First(x => x.Severity == EngineValidationSeverity.Critical);

                var matching = existingLogs
                    .Where(x => x.Reason == critical.ErrorMessage)
                    .ToList();

                if (!matching.Any())
                {
                    _dbcontext.InvoiceActivityLogs.Add(new InvoiceActivityLog
                    {
                        InvoiceID = invoice.InvoiceID,
                        PreviousStatus = invoice.StatusType,
                        CurrentStatus = invoice.StatusType,
                        Reason = critical.ErrorMessage,
                        Action = InvoiceActionType.Validate,
                        IsCurrentValidationContext = true
                    });
                }
                else
                {
                    matching.ForEach(x => x.IsCurrentValidationContext = true);
                }

                await _dbcontext.SaveChangesAsync(cancellationToken);

                return BuildResponse(invoice, failures);
            }

            // NORMAL FAILURES
            foreach (var failure in failures)
            {
                var exists = existingLogs
                    .Any(x => x.Reason == failure.ErrorMessage);

                if (!exists)
                {
                    _dbcontext.InvoiceActivityLogs.Add(new InvoiceActivityLog
                    {
                        InvoiceID = invoice.InvoiceID,
                        PreviousStatus = invoice.StatusType,
                        CurrentStatus = invoice.StatusType,
                        Reason = failure.ErrorMessage,
                        Action = InvoiceActionType.Validate,
                        IsCurrentValidationContext = true
                    });
                }
            }

            invoice.SetAuditFieldsOnCreate(updatedBy);

            await _dbcontext.SaveChangesAsync(cancellationToken);

            return BuildResponse(invoice, failures);
        }

        private static InvValidationResponseDto BuildResponse(Invoice invoice, List<EngineValidationResult> failures)
        {
            return new InvValidationResponseDto
            {
                QueueType = invoice.QueueType!.Value,
                InvoiceActionType = InvoiceActionType.Validate.ToString(),
                FailureMessages = failures.Any()
                    ? string.Join(";", failures.Select(x => x.ErrorMessage))
                    : string.Empty
            };
        }


        //  Task<GetInvoiceStatusDto?> IInvoiceRepository.GetInvoiceStatusAsync(long
        //  invoiceId, CancellationToken token)
        //  {
        //      throw new NotImplementedException();
        // }
    }
}