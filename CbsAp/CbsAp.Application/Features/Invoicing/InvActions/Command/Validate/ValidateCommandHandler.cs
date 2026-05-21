using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.ActivityLog;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Entities.TaxCodes;
using CbsAp.Domain.Enums;
using CBSAP.ValidationEngine;
using CBSAP.ValidationEngine.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.Validate
{
    public class ValidateCommandHandler : ICommandHandler<ValidateCommand, ResponseResult<InvValidationResponseDto>>
    {
        private readonly IUnitofWork _unitOfWork;

        private readonly IWebHostEnvironment _env;

        public ValidateCommandHandler(IUnitofWork unitofWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitofWork;
            _env = environment;
        }
        public async Task<ResponseResult<InvValidationResponseDto>> Handle(ValidateCommand request, CancellationToken cancellationToken)
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
            var poRepo = _unitOfWork.GetRepository<PurchaseOrder>();
            var matchedPoRepo = _unitOfWork.GetRepository<PurchaseOrderMatchTracking>();

            var activityLogRepo = _unitOfWork.GetRepository<InvoiceActivityLog>();

            var invoiceDataToValidate = (await invoiceRepo.ApplyPredicateAsync(i =>
              i.InvoiceID != invoice.InvoiceID &&
              i.StatusType != InvoiceStatusType.Rejected &&
              i.StatusType != InvoiceStatusType.Archived)).AsEnumerable();

            ///TODO : optimize by only selecting necessary fields for validation instead of whole entities
            var supplier = (await supplierRepo.GetAllAsync()).AsEnumerable();
            var taxCode = (await taxcodeRepo.GetAllAsync()).AsEnumerable();
            var entities = (await entityRepo.GetAllAsync()).AsEnumerable();

            var purchaseOrders = poRepo.Query()
                .AsNoTracking()
                .Where(po => po.PoNo != null)
                .AsEnumerable();
            
            var poMatchingConfig = _unitOfWork.GetRepository<EntityMatchingConfig>()
                .Query().AsNoTracking()
                .FirstOrDefault(x => x.EntityProfileID == invoice.EntityProfileID && x.ConfigType == MatchingConfigType.PO);

            var matchedPurchaseOrders = matchedPoRepo.Query()
                .AsNoTracking()
                .Include(p => p.PurchaseOrder)
                .Include(p => p.PurchaseOrderLine)
                .Where(p => p.InvoiceID == invoice.InvoiceID).AsEnumerable();
               

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
                ["PurchaseOrders"] = purchaseOrders,
                ["POMatchingConfig"]= poMatchingConfig!,
                ["MatchedPurchaseOrders"]=matchedPurchaseOrders
            };

            var failures = engine.Validate(invoice, runtimeContext, out bool stopEarly);

            var existingLogs = await activityLogRepo
                .Query()
                .Where(l => l.InvoiceID == invoice.InvoiceID &&
                            (l.Action == InvoiceActionType.Validate || l.Action == InvoiceActionType.Import))
                .ToListAsync(cancellationToken);

            foreach (var log in existingLogs)
                log.IsCurrentValidationContext = false;

            if (!failures.Any())
            {
                var approvedLog = new InvoiceActivityLog
                {
                    InvoiceID = invoice.InvoiceID,
                    PreviousStatus = invoice.StatusType,
                    CurrentStatus = invoice.StatusType,
                    IsCurrentValidationContext = true,
                    Action = InvoiceActionType.Validate,
                    Reason = null
                };

                approvedLog.SetAuditFieldsOnCreate(request.UpdatedBy);
                invoice.InvoiceActivityLog!.Add(approvedLog);

                var res = await _unitOfWork.SaveChanges(request.UpdatedBy,"Validate",cancellationToken);

                var response = new InvValidationResponseDto
                {
                    QueueType = currentQueueType!.Value,
                    InvoiceActionType = Enum.GetName(typeof(InvoiceActionType), InvoiceActionType.Validate)!,
                    FailureMessages = failures.Any() ? string.Join(";", failures.Select(f => f.ErrorMessage))
                    : string.Empty
                };




                if (!res)
                    return ResponseResult<InvValidationResponseDto>.BadRequest("Failed to save invoice changes.");

                return !failures.Any()
                    ? ResponseResult<InvValidationResponseDto>.OK(response, "No error on invoice")  //TODO : next US routing to next approver
                    : ResponseResult<InvValidationResponseDto>.BadRequest(response);
            }

            if (stopEarly)
            {
                var critical = failures.First(f => f.Severity == EngineValidationSeverity.Critical);

                var matching = existingLogs
                    .Where(x => x.Reason == critical.ErrorMessage && x.Action == InvoiceActionType.Validate)
                    .ToList();

                InvoiceActivityLog logToUse;

                if (!matching.Any())
                {
                    logToUse = new InvoiceActivityLog
                    {
                        InvoiceID = invoice.InvoiceID,
                        PreviousStatus = invoice.StatusType,
                        CurrentStatus = invoice.StatusType,
                        Reason = critical.ErrorMessage,
                        Action = InvoiceActionType.Validate,
                        IsCurrentValidationContext = true
                    };

                    logToUse.SetAuditFieldsOnCreate(request.UpdatedBy);

                    invoice.InvoiceActivityLog!.Add(logToUse);
                }
                else
                {
                    foreach (var log in matching)
                        log.IsCurrentValidationContext = true;

                    logToUse = matching.First();
                }

                var saved = await _unitOfWork.SaveChanges(request.UpdatedBy,"Validate",cancellationToken);

                var response = new InvValidationResponseDto
                {
                    QueueType = currentQueueType!.Value,
                    InvoiceActionType = Enum.GetName(typeof(InvoiceActionType), InvoiceActionType.Validate)!,
                    FailureMessages = failures.Any() ? string.Join(";", failures.Select(f => f.ErrorMessage))
                    : string.Empty
                };

                //return saved
                //    ? ResponseResult<InvValidationResponseDto>.OK(critical.ErrorMessage)
                //    : ResponseResult<InvValidationResponseDto>.BadRequest("Error saving critical validation result");

                return ResponseResult<InvValidationResponseDto>.BadRequest(response);
            }

            var newActivityLOg = new ActivityLog
            {
                InvoiceID = (int)invoice.InvoiceID,
                ActionBy = request.UpdatedBy,
                Activity = "VALIDATE",
                Module = invoice.QueueType.ToString(),
                OldValue = null,
                NewValue = string.Format("VALIDATION RESULT: {0}", failures.Any() ? string.Join(";", failures.Select(f => f.ErrorMessage)) : "No Validation Error"),
                ColumnName = null,
                metaDataOld = null,
                metaDataNew = null,
                MetaData = null,
                ActivityDate = DateTime.UtcNow,
                CreatedBy = null,
                CreatedDate = null,
                LastUpdatedBy = null,
                LastUpdatedDate = null
            };

            await _unitOfWork.GetRepository<ActivityLog>().AddAsync(newActivityLOg);

            foreach (var failure in failures)
            {
                var matching = existingLogs
                    .Where(x => x.Reason == failure.ErrorMessage && x.Action == InvoiceActionType.Validate)
                    .ToList();

                if (!matching.Any())
                {
                    var log = new InvoiceActivityLog
                    {
                        InvoiceID = invoice.InvoiceID,
                        PreviousStatus = invoice.StatusType,
                        CurrentStatus = invoice.StatusType,
                        Reason = failure.ErrorMessage,
                        Action = InvoiceActionType.Validate,
                        IsCurrentValidationContext = true
                    };

                    log.SetAuditFieldsOnCreate(request.UpdatedBy);

                    invoice.InvoiceActivityLog!.Add(log);
                }
                else
                {
                    foreach (var log in matching)
                        log.IsCurrentValidationContext = true;
                }
            }

            invoice.SetAuditFieldsOnCreate(request.UpdatedBy);

            var saveResult = await _unitOfWork.SaveChanges(request.UpdatedBy, "Validate", cancellationToken);

            var sendResponse = new InvValidationResponseDto
            {
                QueueType = currentQueueType!.Value,
                InvoiceActionType = Enum.GetName(typeof(InvoiceActionType), InvoiceActionType.Validate)!,
                FailureMessages = failures.Any() ? string.Join(";", failures.Select(f => f.ErrorMessage))
                : string.Empty
            };

            if (!saveResult)
                return ResponseResult<InvValidationResponseDto>.BadRequest("Failed to save invoice changes.");

            return !failures.Any()
                ? ResponseResult<InvValidationResponseDto>.OK(sendResponse, "No error on invoice")  //TODO : next US routing to next approver
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