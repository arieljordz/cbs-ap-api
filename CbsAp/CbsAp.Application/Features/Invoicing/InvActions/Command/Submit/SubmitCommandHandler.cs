using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.ActivityLog;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.PermissionManagement;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Entities.TaxCodes;
using CbsAp.Domain.Entities.UserManagement;
using CbsAp.Domain.Enums;
using CBSAP.ValidationEngine;
using CBSAP.ValidationEngine.Core;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

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

            if (invoice.QueueType == InvoiceQueueType.MyInvoices)
            {
                var lastUserCanApprovedInvoice = CheckApproverPermission(invoice, request.UpdatedBy);
                if (lastUserCanApprovedInvoice != string.Empty)
                {
                    var _sendResponse = new InvValidationResponseDto
                    {
                        QueueType = null,
                        InvoiceActionType = Enum.GetName(typeof(InvoiceActionType), InvoiceActionType.Submit)!,
                        FailureMessages = lastUserCanApprovedInvoice
                    };

                    return ResponseResult<InvValidationResponseDto>.BadRequest(_sendResponse);
                }
            }


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
            var poRepo = _unitOfWork.GetRepository<PurchaseOrder>();
            var matchedPoRepo = _unitOfWork.GetRepository<PurchaseOrderMatchTracking>();

            var activityLogRepo = _unitOfWork.GetRepository<InvoiceActivityLog>();

            var invoiceDataToValidate = (await invoiceRepo.ApplyPredicateAsync(i =>
              i.InvoiceID != invoice.InvoiceID &&
              i.StatusType != InvoiceStatusType.Rejected &&
              i.StatusType != InvoiceStatusType.Archived)).AsEnumerable();

            var supplier = (await supplierRepo.GetAllAsync()).AsEnumerable();
            var taxCode = (await taxcodeRepo.GetAllAsync()).AsEnumerable();
            var entities = (await entityRepo.GetAllAsync()).AsEnumerable();

            var purchaseOrders = poRepo.Query()
              .AsNoTracking()
              .Where(po => po.PoNo != null)
              .AsEnumerable();

            var poMatchingConfig = _unitOfWork.GetRepository<EntityMatchingConfig>()
                .Query()
                .AsNoTracking()
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
                ["POMatchingConfig"] = poMatchingConfig!,
                ["MatchedPurchaseOrders"] = matchedPurchaseOrders
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
                var prevQueue = invoice.QueueType;
                invoice.StatusType = InvoiceStatusType.ForApproval;
                invoice.QueueType = InvoiceQueueType.MyInvoices;


                var approver = invoice.InvInfoRoutingLevels?.Where(w => w.InvFlowStatus == 1).FirstOrDefault();
                //remove to add validation later
                //var roleRepo = await _unitOfWork.GetRepository<Role>().Query().FirstOrDefaultAsync(f => f.RoleID == approver.RoleID);

                invoice.InvInfoRoutingLevels.ForEach(f =>
                {
                    //f.InvFlowStatus = invoice.QueueType == InvoiceQueueType.MyInvoices ? f.Level == 1 ? (int?)InvFlowStatus.Assigned : (int)InvFlowStatus.Pending
                    //    : (int?)InvFlowStatus.Pending;
                    if (prevQueue == InvoiceQueueType.MyInvoices)
                    {
                        if (f.Level - approver?.Level == 0)
                        {
                            f.InvFlowStatus = (int?)InvFlowStatus.Submitted;
                        }
                        else if (f.Level - approver?.Level == 1)
                        {
                            f.InvFlowStatus = (int?)InvFlowStatus.Assigned;
                            invoice.ApproverRole = f.RoleID;
                        }
                        else
                        {
                            if (f.InvFlowStatus != 2)
                                f.InvFlowStatus = (int?)InvFlowStatus.Pending;
                        }
                    }
                    else
                    {
                        f.InvFlowStatus = invoice.QueueType == InvoiceQueueType.MyInvoices ? f.Level == 1 ? (int?)InvFlowStatus.Assigned : (int)InvFlowStatus.Pending : (int?)InvFlowStatus.Pending;
                        invoice.ApproverRole = invoice.InvInfoRoutingLevels?.Where(w => w.Level == 1).Select(s => s.RoleID).FirstOrDefault();
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

                    var success = await _unitOfWork.SaveChanges(request.UpdatedBy, "Submit", cancellationToken);
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


            var saveResult = await _unitOfWork.SaveChanges(request.UpdatedBy, "Submit", cancellationToken);


            //validate button show error and insert into  activitylog
            var newActivityLOg = new ActivityLog
            {
                InvoiceID = (int)invoice.InvoiceID,
                ActionBy = request.UpdatedBy,
                Activity = "SUBMIT",
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
            await _unitOfWork.SaveChanges(request.UpdatedBy, "Submit", cancellationToken);

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

        private string CheckApproverPermission(Invoice invoice, string currentUser)
        {
            string message = string.Empty;
            int roleId = Convert.ToInt32(invoice.ApproverRole);
            var routingLevelRepo = _unitOfWork.GetRepository<InvInfoRoutingLevel>();
            var userRepo = _unitOfWork.GetRepository<UserAccount>();
            var permissionGroupRepo = _unitOfWork.GetRepository<RolePermissionGroup>();




            var routingLevels = routingLevelRepo.Query()
                .Where(r => r.InvoiceID == invoice.InvoiceID)
                .ToList();

            var maxLevel = routingLevels.Max(r => r.Level);
            var userMaxLevel = routingLevels
                .Where(r => r.RoleID == roleId && r.InvFlowStatus == 1 && r.Level == maxLevel).FirstOrDefault();
            //.Select(r => r.Level)
            //.DefaultIfEmpty()
            //.Max();

            if (userMaxLevel == null)
            {
                return string.Empty;
            }

            var permissionGroups = permissionGroupRepo.Query()
                .Where(pg => userMaxLevel.RoleID == pg.RoleID)
                .SelectMany(pg => pg.Permission.PermissionGroups)
                .ToList();

            var canEditInvoiceRoutingFlow = permissionGroups.Any(a => a.OperationID == 23 && a.Access);




            if (canEditInvoiceRoutingFlow)
                message = "You’ve reached the end of the approval flow, Please attach a Role with the permission to approve the invoice.";
            else
                message = "You’ve reached the end of the approval flow, but your permissions don’t allow approval of this invoice. Please contact an administrator for access.";


            return message;
        }
    }
}