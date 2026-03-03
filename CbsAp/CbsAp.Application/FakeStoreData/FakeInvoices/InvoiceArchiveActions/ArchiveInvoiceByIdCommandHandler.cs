using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.InvoicingArchive;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace CbsAp.Application.FakeStoreData.FakeInvoices.InvoiceArchiveActions
{
    public class ArchiveInvoiceByIdCommandHandler : ICommandHandler<ArchiveInvoiceByIdCommand, ResponseResult<int>>
    {
        private readonly IUnitofWork _unitOfWork;

        public ArchiveInvoiceByIdCommandHandler(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseResult<int>> Handle(ArchiveInvoiceByIdCommand request, CancellationToken cancellationToken)
        {
            if (request.InvoiceId <= 0)
            {
                return ResponseResult<int>.BadRequest("Invalid invoice identifier supplied.");
            }

            var context = _unitOfWork.Context;

            var invoice = await context.Set<Invoice>()
                .Include(i => i.InvoiceAllocationLines)!
                    .ThenInclude(l => l.FreeFields)
                .Include(i => i.InvoiceAllocationLines)!
                    .ThenInclude(l => l.Dimensions)
                .Include(i => i.InvoiceComments)
                .Include(i => i.InvoiceAttachnments)
                .Include(i => i.InvoiceActivityLog)
                .FirstOrDefaultAsync(i => i.InvoiceID == request.InvoiceId, cancellationToken);

            if (invoice == null)
            {
                return ResponseResult<int>.NotFound($"Invoice {request.InvoiceId} not found or already archived.");
            }

            var purchaseOrderMatchTrackings = await context.Set<PurchaseOrderMatchTracking>()
                .Where(p => p.InvoiceID == invoice.InvoiceID)
                .ToListAsync(cancellationToken);

            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var aggregate = MapInvoice(invoice, purchaseOrderMatchTrackings, request.ArchivedBy);

                await InsertWithIdentityAsync(context, new[] { aggregate.Invoice }, "InvoiceArchive", cancellationToken);
                await InsertWithIdentityAsync(context, aggregate.InvoiceAllocationLines, "InvAllocLineArchive", cancellationToken);
                await InsertWithIdentityAsync(context, aggregate.AllocationLineFreeFields, "InvAllocLineFreeFieldArchive", cancellationToken);
                await InsertWithIdentityAsync(context, aggregate.AllocationLineDimensions, "InvAllocLineDimensionArchive", cancellationToken);
                await InsertWithIdentityAsync(context, aggregate.InvoiceComments, "InvoiceCommentArchive", cancellationToken);
                await InsertWithIdentityAsync(context, aggregate.InvoiceAttachments, "InvoiceAttachnmentArchive", cancellationToken);
                await InsertWithIdentityAsync(context, aggregate.InvoiceActivityLogs, "InvoiceActivityLogArchive", cancellationToken);
                await InsertWithIdentityAsync(context, aggregate.PurchaseOrderMatchTrackings, "PurchaseOrderMatchTrackingArchive", cancellationToken);

                if (purchaseOrderMatchTrackings.Any())
                {
                    context.Set<PurchaseOrderMatchTracking>().RemoveRange(purchaseOrderMatchTrackings);
                }

                context.Set<Invoice>().Remove(invoice);

                await _unitOfWork.SaveChanges(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return ResponseResult<int>.OK("Invoice archived successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ResponseResult<int>.InternalServerError($"Archiving failed: {ex.Message}");
            }
        }

        private async Task InsertWithIdentityAsync<T>(DbContext context, IEnumerable<T> entities, string tableName, CancellationToken cancellationToken)
            where T : class
        {
            var entityList = entities.ToList();
            if (!entityList.Any())
            {
                return;
            }

            var database = context.Database;
            var transactionInfrastructure = database.CurrentTransaction as IInfrastructure<DbTransaction>;
            if (transactionInfrastructure?.Instance?.Connection == null)
            {
                throw new InvalidOperationException("Archiving requires an active database transaction.");
            }

            var dbTransaction = transactionInfrastructure.Instance;
            var connection = dbTransaction.Connection;

            await ExecuteIdentityToggleAsync(connection, dbTransaction, tableName, true, cancellationToken);

            context.Set<T>().AddRange(entityList);
            try
            {
                await _unitOfWork.SaveChanges(cancellationToken);
            }
            finally
            {
                await ExecuteIdentityToggleAsync(connection, dbTransaction, tableName, false, cancellationToken);
            }
        }

        private static Task ExecuteIdentityToggleAsync(DbConnection connection, DbTransaction? transaction, string tableName, bool enable, CancellationToken cancellationToken)
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"SET IDENTITY_INSERT [CBSAP].[{tableName}] {(enable ? "ON" : "OFF")}";
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            // SQL client async pattern throws if EndExecuteNonQuery is invoked twice; use sync execution instead.
            command.ExecuteNonQuery();
            return Task.CompletedTask;
        }

        private static InvoiceArchiveAggregate MapInvoice(
            Invoice invoice,
            IEnumerable<PurchaseOrderMatchTracking> purchaseOrderMatchTrackings,
            string archivedBy)
        {
            var archiveInvoice = new InvoiceArchive
            {
                InvoiceID = invoice.InvoiceID,
                InvoiceNo = invoice.InvoiceNo,
                InvoiceDate = invoice.InvoiceDate,
                MapID = invoice.MapID,
                ImageID = invoice.ImageID,
                ScanDate = invoice.ScanDate,
                EntityProfileID = invoice.EntityProfileID,
                SupplierInfoID = invoice.SupplierInfoID,
                SuppBankAccount = invoice.SuppBankAccount,
                DueDate = invoice.DueDate,
                PoNo = invoice.PoNo,
                GrNo = invoice.GrNo,
                Currency = invoice.Currency,
                NetAmount = invoice.NetAmount,
                TaxAmount = invoice.TaxAmount,
                TotalAmount = invoice.TotalAmount,
                TaxCodeID = invoice.TaxCodeID,
                PaymentTerm = invoice.PaymentTerm,
                Note = invoice.Note,
                ApproverRole = invoice.ApproverRole,
                ApprovedUser = invoice.ApprovedUser,
                QueueType = InvoiceQueueType.ArchiveQueue,
                StatusType = InvoiceStatusType.Archived,
                FreeFields = new FreeFieldSets
                {
                    FreeField1 = invoice.FreeFields?.FreeField1,
                    FreeField2 = invoice.FreeFields?.FreeField2,
                    FreeField3 = invoice.FreeFields?.FreeField3
                },
                SpareAmount = new SpareAmountSets
                {
                    SpareAmount1 = invoice.SpareAmount?.SpareAmount1,
                    SpareAmount2 = invoice.SpareAmount?.SpareAmount2,
                    SpareAmount3 = invoice.SpareAmount?.SpareAmount3
                },
                CreatedBy = invoice.CreatedBy,
                CreatedDate = invoice.CreatedDate,
                LastUpdatedBy = archivedBy,
                LastUpdatedDate = DateTimeOffset.UtcNow
            };

            var lines = new List<InvAllocLineArchive>();
            var freeFields = new List<InvAllocLineFreeFieldArchive>();
            var dimensions = new List<InvAllocLineDimensionArchive>();
            var comments = new List<InvoiceCommentArchive>();
            var attachments = new List<InvoiceAttachnmentArchive>();
            var activityLogs = new List<InvoiceActivityLogArchive>();
            var trackingArchives = new List<PurchaseOrderMatchTrackingArchive>();

            var lineLookup = new Dictionary<long, InvAllocLineArchive>();

            foreach (var line in invoice.InvoiceAllocationLines ?? Enumerable.Empty<InvAllocLine>())
            {
                var lineArchive = new InvAllocLineArchive
                {
                    InvAllocLineID = line.InvAllocLineID,
                    InvoiceID = invoice.InvoiceID,
                    LineNo = line.LineNo,
                    PoNo = line.PoNo,
                    PoLineNo = line.PoLineNo,
                    AccountID = line.AccountID,
                    LineDescription = line.LineDescription,
                    Qty = line.Qty,
                    LineNetAmount = line.LineNetAmount,
                    LineTaxAmount = line.LineTaxAmount,
                    LineAmount = line.LineAmount,
                    Currency = line.Currency,
                    TaxCodeID = line.TaxCodeID,
                    LineApproved = line.LineApproved,
                    Note = line.Note,
                    CreatedBy = line.CreatedBy,
                    CreatedDate = line.CreatedDate,
                    LastUpdatedBy = line.LastUpdatedBy,
                    LastUpdatedDate = line.LastUpdatedDate,
                    Invoice = null
                };

                foreach (var freeField in line.FreeFields ?? Enumerable.Empty<InvAllocLineFreeField>())
                {
                    freeFields.Add(new InvAllocLineFreeFieldArchive
                    {
                        InvAllocLineFieldID = freeField.InvAllocLineFieldID,
                        InvAllocLineID = freeField.InvAllocLineID,
                        FieldKey = freeField.FieldKey,
                        FieldValue = freeField.FieldValue,
                        CreatedBy = freeField.CreatedBy,
                        CreatedDate = freeField.CreatedDate,
                        LastUpdatedBy = freeField.LastUpdatedBy,
                        LastUpdatedDate = freeField.LastUpdatedDate,
                        AllocationLine = null
                    });
                }

                foreach (var dimension in line.Dimensions ?? Enumerable.Empty<InvAllocLineDimension>())
                {
                    dimensions.Add(new InvAllocLineDimensionArchive
                    {
                        InvAllocLineDimensionID = dimension.InvAllocLineDimensionID,
                        InvAllocLineID = dimension.InvAllocLineID,
                        DimensionKey = dimension.DimensionKey,
                        DimensionValue = dimension.DimensionValue,
                        CreatedBy = dimension.CreatedBy,
                        CreatedDate = dimension.CreatedDate,
                        LastUpdatedBy = dimension.LastUpdatedBy,
                        LastUpdatedDate = dimension.LastUpdatedDate,
                        AllocationLine = null
                    });
                }

                lines.Add(lineArchive);
                if (line.InvAllocLineID != 0)
                {
                    lineLookup[line.InvAllocLineID] = lineArchive;
                }
            }

            foreach (var comment in invoice.InvoiceComments ?? Enumerable.Empty<InvoiceComment>())
            {
                comments.Add(new InvoiceCommentArchive
                {
                    InvoiceCommentID = comment.InvoiceCommentID,
                    Comment = comment.Comment,
                    InvoiceID = invoice.InvoiceID,
                    CreatedBy = comment.CreatedBy,
                    CreatedDate = comment.CreatedDate,
                    LastUpdatedBy = comment.LastUpdatedBy,
                    LastUpdatedDate = comment.LastUpdatedDate,
                    Invoice = null
                });
            }

            foreach (var attachment in invoice.InvoiceAttachnments ?? Enumerable.Empty<InvoiceAttachnment>())
            {
                attachments.Add(new InvoiceAttachnmentArchive
                {
                    InvoiceAttachnmentID = attachment.InvoiceAttachnmentID,
                    InvoiceID = invoice.InvoiceID,
                    OriginalFileName = attachment.OriginalFileName,
                    StorageFileName = attachment.StorageFileName,
                    FileType = attachment.FileType,
                    CreatedBy = attachment.CreatedBy,
                    CreatedDate = attachment.CreatedDate,
                    LastUpdatedBy = attachment.LastUpdatedBy,
                    LastUpdatedDate = attachment.LastUpdatedDate,
                    Invoice = null
                });
            }

            foreach (var activity in invoice.InvoiceActivityLog ?? Enumerable.Empty<InvoiceActivityLog>())
            {
                activityLogs.Add(new InvoiceActivityLogArchive
                {
                    ActivityLogID = activity.ActivityLogID,
                    InvoiceID = invoice.InvoiceID,
                    PreviousStatus = activity.PreviousStatus,
                    CurrentStatus = activity.CurrentStatus,
                    Reason = activity.Reason,
                    Action = activity.Action,
                    CreatedBy = activity.CreatedBy,
                    CreatedDate = activity.CreatedDate,
                    LastUpdatedBy = activity.LastUpdatedBy,
                    LastUpdatedDate = activity.LastUpdatedDate,
                    Invoice = null
                });
            }

            var distinctTrackings = purchaseOrderMatchTrackings
                .GroupBy(p => p.PurchaseOrderMatchTrackingID)
                .Select(g => g.First());

            foreach (var tracking in distinctTrackings)
            {
                var trackingArchive = new PurchaseOrderMatchTrackingArchive
                {
                    PurchaseOrderMatchTrackingID = tracking.PurchaseOrderMatchTrackingID,
                    PurchaseOrderLineID = tracking.PurchaseOrderLineID,
                    PurchaseOrderID = tracking.PurchaseOrderID,
                    InvoiceID = invoice.InvoiceID,
                    InvAllocLineID = tracking.InvAllocLineID,
                    Account = tracking.Account,
                    Qty = tracking.Qty,
                    RemainingQty = tracking.RemainingQty,
                    Amount = tracking.Amount,
                    NetAmount = tracking.NetAmount,
                    TaxAmount = tracking.TaxAmount,
                    MatchingDate = tracking.MatchingDate,
                    IsSystemMatching = tracking.IsSystemMatching,
                    MatchingStatus = tracking.MatchingStatus,
                    CreatedBy = tracking.CreatedBy,
                    CreatedDate = tracking.CreatedDate,
                    LastUpdatedBy = tracking.LastUpdatedBy,
                    LastUpdatedDate = tracking.LastUpdatedDate,
                    Invoice = null
                };

                if (tracking.InvAllocLineID.HasValue && lineLookup.TryGetValue(tracking.InvAllocLineID.Value, out var lineArchive))
                {
                    trackingArchive.InvAllocLine = lineArchive;
                    lineArchive.PurchaseOrderMatchTrackings.Add(trackingArchive);
                }

                trackingArchives.Add(trackingArchive);
            }

            return new InvoiceArchiveAggregate(
                archiveInvoice,
                lines,
                freeFields,
                dimensions,
                comments,
                attachments,
                activityLogs,
                trackingArchives);
        }

        private sealed record InvoiceArchiveAggregate(
            InvoiceArchive Invoice,
            List<InvAllocLineArchive> InvoiceAllocationLines,
            List<InvAllocLineFreeFieldArchive> AllocationLineFreeFields,
            List<InvAllocLineDimensionArchive> AllocationLineDimensions,
            List<InvoiceCommentArchive> InvoiceComments,
            List<InvoiceAttachnmentArchive> InvoiceAttachments,
            List<InvoiceActivityLogArchive> InvoiceActivityLogs,
            List<PurchaseOrderMatchTrackingArchive> PurchaseOrderMatchTrackings);
    }
}
