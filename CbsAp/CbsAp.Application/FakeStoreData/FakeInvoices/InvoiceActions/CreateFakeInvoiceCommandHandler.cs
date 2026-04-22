using System.Runtime.CompilerServices;
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.ActivityLog;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Entities.TaxCodes;
using CbsAp.Domain.Enums;
using CBSAP.ValidationEngine;
using CBSAP.ValidationEngine.Core;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.FakeStoreData.FakeInvoices.InvoiceActions
{
    public class CreateFakeInvoiceCommandHandler : ICommandHandler<CreateFakeInvoiceCommand, ResponseResult<int>>
    {
        private readonly IUnitofWork _unitOfWork;

        private readonly IWebHostEnvironment _env;

        public CreateFakeInvoiceCommandHandler(IUnitofWork unitofWork, IWebHostEnvironment environment)
        {
            _unitOfWork = unitofWork;
            _env = environment;
        }

        public async Task<ResponseResult<int>> Handle(CreateFakeInvoiceCommand request, CancellationToken cancellationToken)
        {
            var newInvoice = InvoiceFakeGenerator.GenerateInvoice(request.Dto, request.CreatedBy);

            var invoiceRepo = _unitOfWork.GetRepository<Invoice>();
            await invoiceRepo.AddAsync(newInvoice);

            // Step 2:
            var supplierRepo = _unitOfWork.GetRepository<SupplierInfo>();
            var taxcodeRepo = _unitOfWork.GetRepository<TaxCode>();
            var entityRepo = _unitOfWork.GetRepository<EntityProfile>();
            var invRoutingFlowRepo = _unitOfWork.GetRepository<InvRoutingFlow>();
            var invKeywordRepo = _unitOfWork.GetRepository<Keyword>();
            var poRepo = _unitOfWork.GetRepository<PurchaseOrder>();
            var matchedPoRepo = _unitOfWork.GetRepository<PurchaseOrderMatchTracking>();

            var invoiceDataToValidate = (await invoiceRepo.ApplyPredicateAsync(i =>
                i.InvoiceID != newInvoice.InvoiceID &&
                i.StatusType != Domain.Enums.InvoiceStatusType.Rejected &&
                i.StatusType != Domain.Enums.InvoiceStatusType.Archived)).AsEnumerable();

            var supplier = (await supplierRepo.GetAllAsync()).AsEnumerable();
            var taxCode = (await taxcodeRepo.GetAllAsync()).AsEnumerable();
            var entities = (await entityRepo.GetAllAsync()).AsEnumerable();
            var routingFlows = (await invRoutingFlowRepo.GetAllAsync()).AsEnumerable();
            var keywords = (await invKeywordRepo.GetAllAsync()).AsEnumerable();

            var purchaseOrders = poRepo.Query()
                .AsNoTracking()
                .Where(po => po.PoNo != null)
                .AsEnumerable();

            var poMatchingConfig = _unitOfWork.GetRepository<EntityMatchingConfig>()
                .Query().AsNoTracking()
                .FirstOrDefault(x => x.EntityProfileID == newInvoice.EntityProfileID && x.ConfigType == MatchingConfigType.PO);

            var matchedPurchaseOrders = matchedPoRepo.Query()
                .AsNoTracking()
                .Include(p => p.PurchaseOrder)
                .Include(p => p.PurchaseOrderLine)
                .Where(p => p.InvoiceID == newInvoice.InvoiceID).AsEnumerable();

            string ruleFilePath = Path.Combine("rulesfiles", $"cbsap.{_env.EnvironmentName}.json");

            if (!File.Exists(ruleFilePath))
            {
                return ResponseResult<int>.BadRequest($"Validation rules file not found: {ruleFilePath}");
            }

            var rules = ValidationRuleFactory.Load(ruleFilePath);
            var engine = new ValidationEngine(rules);

            var runtimeContext = new Dictionary<string, object>
            {
                ["InvoiceRecords"] = invoiceDataToValidate,
                ["SupplierInfos"] = supplier,
                ["TaxCodes"] = taxCode,
                ["EntityProfile"] = entities,
                ["InvRoutingFlow"] = routingFlows,
                ["Keyword"] = keywords,
                ["PurchaseOrders"] = purchaseOrders,
                ["POMatchingConfig"] = poMatchingConfig!,
                ["MatchedPurchaseOrders"] = matchedPurchaseOrders
            };

            var failures = engine.Validate(newInvoice, runtimeContext, out bool stopEarly);

            var activityLogs = new List<InvoiceActivityLog>();


            var filteredFailures = failures
                .Where(f => f.RelatedRelationshipIds == null || !f.RelatedRelationshipIds.Any())
                .ToList();

            /// routing flow levels 
            /// 
            var invRoutingFlowLevelRepo = _unitOfWork.GetRepository<InvRoutingFlowLevels>();

            // check for invalidKeyword
            var validKeyWord = keywords.Any(a => a.KeywordID == newInvoice.KeywordID);
            if (!validKeyWord)
            {
                newInvoice.KeywordID = null;
            }

            // 1st hierarchy : Keywords

            var resultInvRoutingFlowIDLinkedtoKeyword = failures
               .Where(i => i.RelatedRelationshipIds.ContainsKey("InvoiceRoutingFlowIDLinkedToKeyword"))
              .Select(item => new { Value = item.RelatedRelationshipIds["InvoiceRoutingFlowIDLinkedToKeyword"] })
              .FirstOrDefault();



            /// note : change the 2nd hierachy to else if added the keyword
            // 2nd hierarchy : Linked to Supplier

            // Linked to Supplier
            var resultInvRoutingFlowIDLinkedtoSupplier = failures
                .Where(i => i.RelatedRelationshipIds.ContainsKey("InvoiceRoutingFlowIDLinkedToSupplier"))
               .Select(item => new { Value = item.RelatedRelationshipIds["InvoiceRoutingFlowIDLinkedToSupplier"] })
               .FirstOrDefault();

            if (resultInvRoutingFlowIDLinkedtoKeyword != null && (resultInvRoutingFlowIDLinkedtoKeyword!.Value != null || resultInvRoutingFlowIDLinkedtoKeyword!.Value != 0))
            {
                var routingFlowLevel = await invRoutingFlowLevelRepo.Query()
           .Where(x => x.InvRoutingFlowID == resultInvRoutingFlowIDLinkedtoKeyword!.Value).ToListAsync(cancellationToken);

                foreach (var level in routingFlowLevel)
                {
                    var invoiceInfoRoutingLevel = new InvInfoRoutingLevel
                    {
                        InvoiceID = newInvoice.InvoiceID,
                        InvRoutingFlowID = resultInvRoutingFlowIDLinkedtoKeyword!.Value,
                        Level = level.Level,
                        RoleID = level.RoleID,
                        KeywordID = request.Dto.KeywordID,
                        InvFlowStatus = (int?)InvFlowStatus.Pending
                    };
                    newInvoice.InvInfoRoutingLevels!.Add(invoiceInfoRoutingLevel);
                }

                newInvoice.InvRoutingFlowID = resultInvRoutingFlowIDLinkedtoKeyword!.Value;
            }

            else if (resultInvRoutingFlowIDLinkedtoSupplier != null && (resultInvRoutingFlowIDLinkedtoSupplier!.Value != null || resultInvRoutingFlowIDLinkedtoSupplier!.Value != 0))
            {
                var routingFlowLevel = await invRoutingFlowLevelRepo.Query()
                .Where(x => x.InvRoutingFlowID == resultInvRoutingFlowIDLinkedtoSupplier!.Value).ToListAsync(cancellationToken);

                foreach (var level in routingFlowLevel)
                {
                    var invoiceInfoRoutingLevel = new InvInfoRoutingLevel
                    {
                        InvoiceID = newInvoice.InvoiceID,
                        InvRoutingFlowID = resultInvRoutingFlowIDLinkedtoSupplier!.Value,
                        Level = level.Level,
                        RoleID = level.RoleID,
                        SupplierInfoID = request.Dto.SupplierInfoID,
                        InvFlowStatus = (int?)InvFlowStatus.Pending
                    };
                    newInvoice.InvInfoRoutingLevels!.Add(invoiceInfoRoutingLevel);
                }

                newInvoice.InvRoutingFlowID = resultInvRoutingFlowIDLinkedtoSupplier!.Value;

            }

            var supplierDetails = supplier
                .FirstOrDefault(s => s.SupplierInfoID == newInvoice.SupplierInfoID);


            var paymentTerm = supplierDetails?.PaymentTerms;
            var paymentDays = int.TryParse(paymentTerm, out var days) ? days : 0;

            newInvoice.PaymentTerm = paymentTerm ?? string.Empty;

            var entityDetails = entities
                .FirstOrDefault(e => e.EntityProfileID == newInvoice.EntityProfileID);

            DateTimeOffset baseDate = entityDetails.InvDueDateCalculation switch
            {
                1 => newInvoice.ScanDate ?? newInvoice.CreatedDate!.Value,
                2 => newInvoice.CreatedDate!.Value,
                3 => newInvoice.InvoiceDate ?? newInvoice.CreatedDate!.Value,
                _ => newInvoice.CreatedDate!.Value
            };

            // Compute due date
            newInvoice.DueDate = entityDetails.InvDueDateCalculation is 1 or 2 or 3
                ? baseDate.AddDays(paymentDays)
                : baseDate.AddDays((int)entityDetails.DefaultInvoiceDueInDays);

            //remove routing flow validation message after inserting routing flow level --- if not inserted validation message remains
            if (newInvoice.InvInfoRoutingLevels?.Count > 0)
            {
                int indexFiltered = filteredFailures.Select((w, i) => new { Item = w, Index = i })
                            .Where(x => string.Equals(x.Item.ErrorMessage,
                                                      "Invoice has a missing routing flow",
                                                      StringComparison.Ordinal))
                            .Select(x => x.Index)
                            .DefaultIfEmpty(-1)
                            .First();
                int indexFailure = failures.Select((w, i) => new { Item = w, Index = i })
                        .Where(x => string.Equals(x.Item.ErrorMessage,
                                                  "Invoice has a missing routing flow",
                                                  StringComparison.Ordinal))
                        .Select(x => x.Index)
                        .DefaultIfEmpty(-1)
                        .First();
                filteredFailures.RemoveAt(indexFiltered);
                failures.RemoveAt(indexFailure);

            }

            if (!filteredFailures.Any() || failures.All(a => string.IsNullOrEmpty(a.ErrorMessage)))
            {
                //Set Assigned status to level 1 role if no error encounters
                newInvoice.InvInfoRoutingLevels.ForEach(f =>
                {
                    if (f.Level == 1)
                    {
                        f.InvFlowStatus = (int?)InvFlowStatus.Assigned;
                        newInvoice.ApproverRole = f.RoleID;
                    }
                });
                // No validation errors - set invoice to approved
                var approvedLog = new InvoiceActivityLog
                {
                    InvoiceID = newInvoice.InvoiceID,
                    PreviousStatus = newInvoice.StatusType,
                    CurrentStatus = InvoiceStatusType.ForApproval,
                    Reason = null,
                    Action = InvoiceActionType.Import
                };
                approvedLog.SetAuditFieldsOnCreate(request.CreatedBy);

                newInvoice.InvoiceActivityLog!.Add(approvedLog);

                newInvoice.StatusType = InvoiceStatusType.ForApproval;
                newInvoice.QueueType = InvoiceQueueType.MyInvoices;
            }
            else
            {
                if (stopEarly)
                {
                    var critical = failures
                        .First(f => f.Severity == EngineValidationSeverity.Critical);

                    var criticalLog = new InvoiceActivityLog
                    {
                        InvoiceID = newInvoice.InvoiceID,
                        PreviousStatus = newInvoice.StatusType,
                        CurrentStatus = (InvoiceStatusType?)critical.NextStatus,
                        Reason = critical.ErrorMessage,
                        Action = InvoiceActionType.Import,
                        IsCurrentValidationContext = true
                    };

                    newInvoice.StatusType = InvoiceStatusType.Rejected;
                    newInvoice.QueueType = InvoiceQueueType.RejectionQueue;

                    criticalLog.SetAuditFieldsOnCreate(request.CreatedBy);
                    newInvoice.InvoiceActivityLog!.Add(criticalLog);

                    var success = await _unitOfWork.SaveChanges("Admin", "import", cancellationToken);
                    return success
                        ? ResponseResult<int>.OK(critical.ErrorMessage)
                        : ResponseResult<int>.BadRequest("Error saving critical validation result");
                }

                // Non-critical failures (exception queue)
                var highest = failures.MaxBy(f => f.Severity);
                foreach (var failure in filteredFailures)
                {
                    newInvoice.InvoiceActivityLog!.Add(new InvoiceActivityLog
                    {
                        InvoiceID = newInvoice.InvoiceID,
                        PreviousStatus = newInvoice.StatusType,
                        CurrentStatus = (InvoiceStatusType?)failure.NextStatus,
                        Reason = failure.ErrorMessage,
                        Action = InvoiceActionType.Import,
                        CreatedBy = request.CreatedBy,
                        CreatedDate = DateTimeOffset.UtcNow,
                        IsCurrentValidationContext = true
                    });
                }
                newInvoice.InvoiceActivityLog!.SetAuditFieldsOnCreate(request.CreatedBy);

                newInvoice.StatusType = (InvoiceStatusType?)highest!.NextStatus;
                newInvoice.QueueType = (InvoiceQueueType?)highest.TargetQueue;
                newInvoice.SetAuditFieldsOnCreate(request.CreatedBy);
            }

            //insert log to validation result
            var saveResult = await _unitOfWork.SaveChanges("System", "import", cancellationToken);
            var newActivityLOg = new ActivityLog
            {
                InvoiceID = (int)newInvoice.InvoiceID,
                ActionBy = "System",
                Activity = "VALIDATE",
                Module = newInvoice.QueueType.ToString(),
                OldValue = null,
                NewValue = string.Format("VALIDATION RESULT: {0}", failures.Any() ? string.Join(";", failures.Select(f => f.ErrorMessage)) : "No Validation Error"),
                ColumnName = "Import",
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
            await _unitOfWork.SaveChanges("", "", cancellationToken);
            if (!saveResult)
                return ResponseResult<int>.BadRequest("Failed to save invoice changes.");

            return !failures.Any()
                ? ResponseResult<int>.OK("Invoice is now for approval state")
                : ResponseResult<int>.OK(string.Join("; ", failures.Select(f => f.ErrorMessage)));
        }
    }
}