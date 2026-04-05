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
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.ForApproval
{
    public class InvApproveCommandHandler : ICommandHandler<InvApproveCommand, ResponseResult<InvValidationResponseDto>>
    {
        private readonly IUnitofWork _unitOfWork;

        private readonly IWebHostEnvironment _env;

        public InvApproveCommandHandler(IUnitofWork unitofWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitofWork;
            _env = environment;
        }

        public async Task<ResponseResult<InvValidationResponseDto>> Handle(InvApproveCommand request, CancellationToken cancellationToken)
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


            //invoice allocation line
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

            UpdateItems(existingInvAllocationLine, itemsToUpdate, request.UpdatedBy);
            AddItems(invoice, itemstoAdd, request.UpdatedBy);
            DeleteItems(itemsToDelete);


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

            if (!failures.Any())
            {
                var approvedLog = new InvoiceActivityLog
                {
                    InvoiceID = invoice.InvoiceID,
                    PreviousStatus = invoice.StatusType,
                    CurrentStatus = InvoiceStatusType.ReadyForExport,
                    Reason = null,
                    Action = InvoiceActionType.Approve
                };
                approvedLog.SetAuditFieldsOnCreate(request.UpdatedBy);
                invoice.InvoiceActivityLog!.Add(approvedLog);
                var prevQueue = invoice.QueueType;
                invoice.StatusType = InvoiceStatusType.ReadyForExport;
                invoice.QueueType = null;
                var approver = invoice.InvInfoRoutingLevels?.Where(w => w.InvFlowStatus == 1).FirstOrDefault();
                invoice.InvInfoRoutingLevels.ForEach(f => 
                {
                    if (prevQueue == InvoiceQueueType.MyInvoices)
                    {
                        if (f.Level - approver?.Level == 0)
                        {
                            f.InvFlowStatus = (int?)InvFlowStatus.Submitted;
                            invoice.ApprovedUser = f.RoleID.ToString();
                        }
                    }
                });
            }
            else
            {
                if (stopEarly)
                {
                    var critical = failures
                        .First(f => f.Severity == EngineValidationSeverity.Critical);

                    var logExists = await activityLogRepo.AnyAsync(x => x.Reason == critical.ErrorMessage &&
                     x.InvoiceID == invoice.InvoiceID);

                    var criticalLog = new InvoiceActivityLog
                    {
                        InvoiceID = invoice.InvoiceID,
                        PreviousStatus = invoice.StatusType,
                        CurrentStatus = invoice.StatusType,
                        Reason = critical.ErrorMessage,
                        Action = InvoiceActionType.Approve,
                    };

                    if (!logExists)
                    {
                        invoice.InvoiceActivityLog!.Add(criticalLog);
                    }

                    criticalLog.SetAuditFieldsOnCreate(request.UpdatedBy);
                    var _module = Enum.GetValues(typeof(InvoiceQueueType)).Cast<InvoiceQueueType>().FirstOrDefault(s => s == invoice.QueueType);
                    var success = await _unitOfWork.SaveChanges(request.UpdatedBy, _module.ToString(),cancellationToken);
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

                    if (!logExists)
                    {
                        invoice.InvoiceActivityLog!.Add(new InvoiceActivityLog
                        {
                            InvoiceID = invoice.InvoiceID,
                            PreviousStatus = invoice.StatusType,
                            CurrentStatus = invoice.StatusType,
                            Reason = failure.ErrorMessage,
                            Action = InvoiceActionType.Submit,
                            CreatedBy = request.UpdatedBy,
                            CreatedDate = DateTimeOffset.UtcNow,
                        });
                        invoice.InvoiceActivityLog!.SetAuditFieldsOnCreate(request.UpdatedBy);
                    }
                }

                invoice.SetAuditFieldsOnCreate(request.UpdatedBy);
            }
            var module = Enum.GetValues(typeof(InvoiceQueueType)).Cast<InvoiceQueueType>().FirstOrDefault(s => s == invoice.QueueType);
            var saveResult = await _unitOfWork.SaveChanges(request.UpdatedBy,module.ToString(),cancellationToken);

            var sendResponse = new InvValidationResponseDto
            {
                QueueType = currentQueueType!.Value,
                InvoiceActionType = Enum.GetName(typeof(InvoiceActionType), InvoiceActionType.Approve)!,
                FailureMessages = string.Join(";", failures.Select(f => f.ErrorMessage))
            };

            if (!saveResult)
                return ResponseResult<InvValidationResponseDto>.BadRequest("Failed to approved invoice");

            return !failures.Any()
                ? ResponseResult<InvValidationResponseDto>.OK("Invoice passed all the validations")  //TODO : next US routing to next approver
                : ResponseResult<InvValidationResponseDto>.OK(sendResponse);
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