using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Entities.TaxCodes;
using CbsAp.Domain.Enums;
using CBSAP.ValidationEngine;
using CBSAP.ValidationEngine.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.Submit
{
    public class SubmitCommandHandler : ICommandHandler<SubmitCommand, ResponseResult<InvValidationResponseDto>>
    {
        private readonly IUnitofWork _unitOfWork;

        private readonly IWebHostEnvironment _env;

        public SubmitCommandHandler(IUnitofWork unitofWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitofWork;
            _env = environment;
        }

        public async Task<ResponseResult<InvValidationResponseDto>> Handle(SubmitCommand request, CancellationToken cancellationToken)
        {
            var dto = request.invoiceDto;

            var invoiceRepo = _unitOfWork.GetRepository<Invoice>();
            var invoice = await invoiceRepo
                .Query()
                .FirstOrDefaultAsync(i => i.InvoiceID == dto.InvoiceID);
            if (invoice == null)
                return ResponseResult<InvValidationResponseDto>.BadRequest("Invoice is not found");

            invoice.InvoiceID = dto.InvoiceID;
            invoice.InvoiceNo = dto.InvoiceNo;
            invoice.InvoiceDate = dto.InvoiceDate!.Value.ToLocalTime();
            invoice.MapID = dto.MapID;
            invoice.ScanDate = dto.ScanDate;
            invoice.EntityProfileID = dto.EntityProfileID;
            invoice.SupplierInfoID = dto.SupplierInfoID;
            invoice.DueDate = dto.DueDate;
            invoice.PoNo = dto.PoNo;
            invoice.GrNo = dto.GrNo;
            invoice.Currency = dto.Currency;
            invoice.NetAmount = dto.NetAmount;
            invoice.TaxAmount = dto.TaxAmount;
            invoice.TotalAmount = dto.TotalAmount;
            invoice.TaxCodeID = dto.TaxCodeID;
            invoice.PaymentTerm = dto.PaymentTerm;
            invoice.Note = dto.Note;

            //invoice line 
            //
            var existingInvAllocationLine = invoice.InvoiceAllocationLines!.ToList();

            var mapIncomingInvAllocationItems = dto.InvoiceAllocationLines
                .Select(dto => new InvAllocLine
                {
                    InvAllocLineID = dto.InvAllocLineID,
                    InvoiceID = dto.InvoiceID,
                    LineNo = dto.LineNo,
                    PoNo = dto.PoNo,
                    Qty = dto.Qty,
                    LineDescription = dto.LineDescription,
                    Note = dto.Note,
                    LineNetAmount = dto.LineNetAmount,
                    LineTaxAmount = dto.LineTaxAmount,
                    LineAmount = dto.LineAmount,
                    TaxCodeID = dto.TaxCodeID,

                });

            var itemsToUpdate = mapIncomingInvAllocationItems
                .Where(i => i.InvAllocLineID != 0 &&
                existingInvAllocationLine.Any(e => e.InvAllocLineID == i.InvAllocLineID))
                .ToList();

            var itemstoAdd = mapIncomingInvAllocationItems
                .Where(i => i.InvAllocLineID == 0 ||
                !existingInvAllocationLine.Any(e => e.InvAllocLineID == i.InvAllocLineID))
                .ToList();

            var incominginvAllocItemsIds = mapIncomingInvAllocationItems
                .Where(i => i.InvAllocLineID != 0)
                .Select(i => i.InvAllocLineID)
                .ToHashSet();


            var itemsToDelete = existingInvAllocationLine
                .Where(i => !incominginvAllocItemsIds.Contains(i.InvAllocLineID))
                .ToList();

          

            var currentQueueType = invoice.QueueType;

            var supplierRepo = _unitOfWork.GetRepository<SupplierInfo>();
            var taxcodeRepo = _unitOfWork.GetRepository<TaxCode>();
            var entityRepo = _unitOfWork.GetRepository<EntityProfile>();

            var activityLogRepo = _unitOfWork.GetRepository<InvoiceActivityLog>();

            var invoiceDataToValidate = (await invoiceRepo.ApplyPredicateAsync(i =>
              i.InvoiceID != invoice.InvoiceID &&
              i.StatusType != InvoiceStatusType.Rejected &&
              i.StatusType != InvoiceStatusType.Archived)).AsEnumerable();

            var supplier = (await supplierRepo.GetAllAsync()).AsEnumerable();
            var taxCode = (await taxcodeRepo.GetAllAsync()).AsEnumerable();
            var entities = (await entityRepo.GetAllAsync()).AsEnumerable();

            string ruleFilePath = Path.Combine("rulesfiles", $"cbsap.{_env.EnvironmentName}.json");

            if (!File.Exists(ruleFilePath))
            {
                return ResponseResult<InvValidationResponseDto>.BadRequest($"Validation rules file not found: {ruleFilePath}");
            }

            var rules = ValidationRuleFactory.Load(ruleFilePath);
            var engine = new ValidationEngine(rules);

            var runtimeContext = new Dictionary<string, object>
            {
                ["InvoiceRecords"] = invoiceDataToValidate,
                ["SupplierInfos"] = supplier,
                ["TaxCodes"] = taxCode,
                ["EntityProfile"] = entities,
            };

            var failures = engine.Validate(invoice, runtimeContext, out bool stopEarly);

            var existingLogs = await activityLogRepo
               .Query()
               .Where(l => l.InvoiceID == invoice.InvoiceID &&
                           l.Action == InvoiceActionType.Validate)
               .ToListAsync(cancellationToken);

            foreach (var log in existingLogs)
                log.IsCurrentValidationContext = false;

            if (!failures.Any())
            {
                /// exception no  error for approval state
                ///
                // my invoices and for approaval remaain on the current state approval
                //
                //validate button show error and insert into  activitylog

                var approvedLog = new InvoiceActivityLog
                {
                    InvoiceID = invoice.InvoiceID,
                    PreviousStatus = invoice.StatusType,
                    CurrentStatus = InvoiceStatusType.ForApproval,
                    Reason = null,
                    Action = InvoiceActionType.Submit
                };
                approvedLog.SetAuditFieldsOnCreate(request.UpdatedBy);
                invoice.InvoiceActivityLog!.Add(approvedLog);
                invoice.StatusType = InvoiceStatusType.ForApproval;
                invoice.QueueType = InvoiceQueueType.MyInvoices;
            }
            else
            {
                if (stopEarly)
                {
                    var critical = failures
                        .First(f => f.Severity == EngineValidationSeverity.Critical);

                    var logExists = await activityLogRepo.AnyAsync(x => x.Reason == critical.ErrorMessage &&
                     x.InvoiceID == invoice.InvoiceID);

                    var matching = existingLogs
                        .Where(x => x.Reason == critical.ErrorMessage)
                        .ToList();

                    var criticalLog = new InvoiceActivityLog
                    {
                        InvoiceID = invoice.InvoiceID,
                        PreviousStatus = invoice.StatusType,
                        CurrentStatus = invoice.StatusType,
                        Reason = critical.ErrorMessage,
                        IsCurrentValidationContext = true,
                        Action = InvoiceActionType.Submit,
                    };

                    if (!logExists)
                    {
                        invoice.InvoiceActivityLog!.Add(criticalLog);
                    }
                    else
                    {
                        foreach (var match in matching)
                        {
                            match.IsCurrentValidationContext = true;
                        }
                    }

                    invoice.StatusType = invoice.QueueType != InvoiceQueueType.MyInvoices ?
                            InvoiceStatusType.ForApproval : invoice.StatusType;
                    invoice.QueueType = invoice.QueueType != InvoiceQueueType.MyInvoices
                        ? InvoiceQueueType.MyInvoices : invoice.QueueType;
                    criticalLog.SetAuditFieldsOnCreate(request.UpdatedBy);

                    var success = await _unitOfWork.SaveChanges(cancellationToken);
                    return success
                        ? ResponseResult<InvValidationResponseDto>.OK(critical.ErrorMessage)
                        : ResponseResult<InvValidationResponseDto>.BadRequest("Error saving critical validation result");
                }

                // Non-critical failures (exception queue)
                var highest = failures.MaxBy(f => f.Severity);
                foreach (var failure in failures)
                {
                    var logExists = await activityLogRepo.AnyAsync(x => x.Reason == failure.ErrorMessage &&
                    x.InvoiceID == invoice.InvoiceID);

                    var matching = existingLogs
                        .Where(x => x.Reason == failure.ErrorMessage)
                        .ToList();

                    if (!logExists)
                    {
                        invoice.InvoiceActivityLog!.Add(new InvoiceActivityLog
                        {
                            InvoiceID = invoice.InvoiceID,
                            PreviousStatus = invoice.StatusType,
                            CurrentStatus = invoice.StatusType,
                            Reason = failure.ErrorMessage,
                            Action = InvoiceActionType.Submit,
                            IsCurrentValidationContext = true,
                            CreatedBy = request.UpdatedBy,
                            CreatedDate = DateTimeOffset.UtcNow,
                        });
                        invoice.InvoiceActivityLog!.SetAuditFieldsOnCreate(request.UpdatedBy);
                    }
                    else
                    { 
                        foreach (var match in matching)
                        {
                            match.IsCurrentValidationContext = true;
                        }
                    }
                }

                invoice.SetAuditFieldsOnCreate(request.UpdatedBy);
            }



            UpdateItems(existingInvAllocationLine, itemsToUpdate, request.UpdatedBy);
            AddItems(invoice, itemstoAdd, request.UpdatedBy);
            DeleteItems(itemsToDelete);


            var saveResult = await _unitOfWork.SaveChanges(cancellationToken);

            var sendResponse = new InvValidationResponseDto
            {
                QueueType = currentQueueType!.Value,
                InvoiceActionType = Enum.GetName(typeof(InvoiceActionType), InvoiceActionType.Submit)!,
                FailureMessages = failures.Any() ? string.Join(";", failures.Select(f => f.ErrorMessage))
                : string.Empty
            };

            if (!saveResult)
                return ResponseResult<InvValidationResponseDto>.BadRequest("Failed to submit invoice.");

            return !failures.Any()
                ? ResponseResult<InvValidationResponseDto>.OK("Successfully submitted")  //TODO : next US routing to next approver
                : ResponseResult<InvValidationResponseDto>.BadRequest(sendResponse);
        }

        private static void UpdateItems(List<InvAllocLine> existingAllocLines,
          List<InvAllocLine> updatedLines,
          string updatedBy)
        {
            foreach (var updated in updatedLines)
            {
                var existing = existingAllocLines
                    .First(e => e.InvAllocLineID == updated.InvAllocLineID);

                existing.LineNo = updated.LineNo;
                existing.PoNo = updated.PoNo;
                existing.Qty = updated.Qty;
                existing.LineDescription = updated.LineDescription;
                existing.Note = updated.Note;
                existing.LineNetAmount = updated.LineNetAmount;
                existing.LineTaxAmount = updated.LineTaxAmount;
                existing.LineAmount = updated.LineAmount;
                existing.TaxCodeID = updated.TaxCodeID;
                existing.SetAuditFieldsOnUpdate(updatedBy);
            }

        }

        private static void AddItems(Invoice invoice, List<InvAllocLine> newItems, string createdBy)
        {
            foreach (var item in newItems)
            {
                invoice.InvoiceAllocationLines!.Add(new InvAllocLine
                {
                    InvoiceID = item.InvoiceID,
                    LineNo = item.LineNo,
                    PoNo = item.PoNo,
                    Qty = item.Qty,
                    LineDescription = item.LineDescription,
                    Note = item.Note,
                    LineNetAmount = item.LineNetAmount,
                    LineTaxAmount = item.LineTaxAmount,
                    LineAmount = item.LineAmount,
                    TaxCodeID = item.TaxCodeID,

                });
                invoice.InvoiceAllocationLines!.SetAuditFieldsOnCreate(createdBy);



            }
        }

        private void DeleteItems(List<InvAllocLine> todeletelines)
        {
            _unitOfWork.GetRepository<InvAllocLine>().RemoveRangeAsync(todeletelines);
        }
    }
}