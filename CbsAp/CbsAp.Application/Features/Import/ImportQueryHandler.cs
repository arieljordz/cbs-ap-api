using Bogus;
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.DimensionsManagement;
using CbsAp.Application.DTOs.Import;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.ActivityLog;
using CbsAp.Domain.Entities.Dimensions;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.GoodReceipts;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Entities.PO;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Entities.System;
using CbsAp.Domain.Entities.TaxCodes;
using CbsAp.Domain.Enums;
using CBSAP.ValidationEngine;
using CBSAP.ValidationEngine.Core;
using CBSAP.ValidationEngine.Rules;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace CbsAp.Application.Features.Import
{
    public class ImportQueryHandler : IQueryHandler<ImportQuery, ResponseResult<List<ImportInvoiceResponseDto>>>
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public ImportQueryHandler(IUnitofWork unitOfWork, IWebHostEnvironment env)
        {
            this._unitOfWork = unitOfWork;
            this._env = env;
        }

        public async Task<ResponseResult<List<ImportInvoiceResponseDto>>> Handle(ImportQuery request, CancellationToken cancellationToken)
        {
            var result = new List<ImportInvoiceResponseDto>();
            var invoicesData = request.dto;

            foreach (var data in invoicesData)
            {
                try
                {
                    var newInvoice = DataMapper(data);

                    if (!Path.Exists(data.PdfFilePath))
                    {
                        result.Add(new ImportInvoiceResponseDto { InvoiceNo = data?.InvoiceNo, InvoiceId = (int)newInvoice.InvoiceID, PdfFilePath = data.PdfFilePath, ErrorMessage = "File not exists." });
                        continue;
                    }


                    var invoiceRepo = _unitOfWork.GetRepository<Invoice>();
                    await invoiceRepo.AddAsync(newInvoice);


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

                    var purchaseOrders = poRepo
                        .Query()
                        .AsNoTracking()
                        .Where(po => po.PoNo != null)
                        .AsEnumerable();

                    var poMatchingConfig = _unitOfWork.GetRepository<EntityMatchingConfig>()
                        .Query()
                        .AsNoTracking()
                        .FirstOrDefault(x => x.EntityProfileID == newInvoice.EntityProfileID && x.ConfigType == MatchingConfigType.PO);

                    var matchedPurchaseOrders = matchedPoRepo
                        .Query()
                        .AsNoTracking()
                        .Include(p => p.PurchaseOrder)
                        .Include(p => p.PurchaseOrderLine)
                        .Where(p => p.InvoiceID == newInvoice.InvoiceID).AsEnumerable();

                    var grRepo = _unitOfWork.GetRepository<GoodReceipt>();
                    var goodsReceipts = grRepo.Query()
                       .AsNoTracking()
                       .Include(x => x.GoodsReceiptLines)
                       .Where(p => p.GoodsReceiptNumber == newInvoice.GrNo).AsEnumerable();

                    string ruleFilePath = Path.Combine("rulesfiles", $"cbsap.{_env.EnvironmentName}.json");

                    if (!File.Exists(ruleFilePath))
                    {
                        result.Add(new ImportInvoiceResponseDto { InvoiceNo = data.InvoiceNo, InvoiceId = (int)newInvoice.InvoiceID, PdfFilePath = data.PdfFilePath, ErrorMessage = $"Validation rules file not found: {ruleFilePath}" });
                        continue;
                    }

                    var rules = ValidationRuleFactory.Load(ruleFilePath);
                    //add rule specific for import only
                    //todo: refactor
                    /*
                     *   "type": "InvoiceImportPOValidationRule",
                          "poNoField": "PoNo",
                          "errorCode": "17",
                          "severity": "Error",
                          "nextStatus": "Exception",
                          "targetQueue": "ExceptionQueue"
                     * */
                    rules.Add(new InvoiceImportPOValidationRule()
                    {
                        ErrorCode="17",
                        Severity= EngineValidationSeverity.Error,
                        NextStatus = EngineInvoiceStatusType.Exception,
                        TargetQueue = EngineInvoiceQueueType.ExceptionQueue
                    });

                    var poValidation =  rules.FirstOrDefault(x => x is InvoicePOValidationRule);
                    if (poValidation != null)
                    {
                        rules.Remove(poValidation);
                    }

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
                        ["MatchedPurchaseOrders"] = matchedPurchaseOrders,
                        ["GoodsReceipts"] = goodsReceipts
                    };

                    var validationResults = engine.Validate(newInvoice, runtimeContext, out bool stopEarly);
                    var failures = validationResults.Where(x => x.Severity != EngineValidationSeverity.Info).ToList();
                    var activityLogs = new List<InvoiceActivityLog>();

                    var filteredFailures = failures
                        .Where(f => f.RelatedRelationshipIds == null || !f.RelatedRelationshipIds.Any())
                        .ToList();

                    var invRoutingFlowLevelRepo = _unitOfWork.GetRepository<InvRoutingFlowLevels>();
                    var validKeyWord = keywords.Any(a => a.KeywordID == newInvoice.KeywordID);
                    if (!validKeyWord)
                    {
                        newInvoice.KeywordID = null;
                    }

                    var resultInvRoutingFlowIDLinkedtoKeyword = failures
                      .Where(i => i.RelatedRelationshipIds.ContainsKey("InvoiceRoutingFlowIDLinkedToKeyword"))
                      .Select(item => new { Value = item.RelatedRelationshipIds["InvoiceRoutingFlowIDLinkedToKeyword"] })
                      .FirstOrDefault();

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
                                KeywordID = newInvoice.KeywordID,
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
                                SupplierInfoID = newInvoice.SupplierInfoID,
                                InvFlowStatus = (int?)InvFlowStatus.Pending
                            };
                            newInvoice.InvInfoRoutingLevels!.Add(invoiceInfoRoutingLevel);
                        }

                        newInvoice.InvRoutingFlowID = resultInvRoutingFlowIDLinkedtoSupplier!.Value;

                    }

                    var entityDetails = entities
                        .FirstOrDefault(e => e.EntityProfileID == newInvoice.EntityProfileID);

                    DateTimeOffset baseDate = entityDetails?.InvDueDateCalculation switch
                    {
                        1 => newInvoice.ScanDate ?? newInvoice.CreatedDate!.Value,
                        2 => newInvoice.CreatedDate!.Value,
                        3 => newInvoice.InvoiceDate ?? newInvoice.CreatedDate!.Value,
                        _ => newInvoice.CreatedDate!.Value
                    };

                    var paymentDays = int.TryParse(newInvoice.PaymentTerm, out var days) ? days : 0;

                    newInvoice.DueDate = entityDetails == null ? null : entityDetails.InvDueDateCalculation is 1 or 2 or 3
                    ? baseDate.Date.AddDays(paymentDays)
                    : baseDate.Date.AddDays((int)entityDetails.DefaultInvoiceDueInDays);


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
                        approvedLog.SetAuditFieldsOnCreate("System");

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

                            criticalLog.SetAuditFieldsOnCreate("System");
                            newInvoice.InvoiceActivityLog!.Add(criticalLog);
                            newInvoice.ImageID = await CopyInvoiceFileAsync(data.PdfFilePath,cancellationToken);
                            var success = await _unitOfWork.SaveChanges("Admin", "import", cancellationToken);
                            result.Add(new ImportInvoiceResponseDto { InvoiceNo = data.InvoiceNo, InvoiceId = (int)newInvoice.InvoiceID, PdfFilePath = data.PdfFilePath, ErrorMessage = string.Empty });
                            continue;
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
                                CreatedBy = "System",
                                CreatedDate = DateTimeOffset.UtcNow,
                                IsCurrentValidationContext = true
                            });
                        }
                        newInvoice.InvoiceActivityLog!.SetAuditFieldsOnCreate("System");

                        newInvoice.StatusType = (InvoiceStatusType?)highest!.NextStatus;
                        newInvoice.QueueType = (InvoiceQueueType?)highest.TargetQueue;
                        newInvoice.SetAuditFieldsOnCreate("System");
                    }


                    //insert log to validation result
                    newInvoice.ImageID = await CopyInvoiceFileAsync(data.PdfFilePath, cancellationToken);
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
                        result.Add(new ImportInvoiceResponseDto { InvoiceNo = data.InvoiceNo, InvoiceId = (int)newInvoice.InvoiceID, PdfFilePath = data.PdfFilePath, ErrorMessage = string.Empty });

                    result.Add(new ImportInvoiceResponseDto { InvoiceNo = data.InvoiceNo, InvoiceId = (int)newInvoice.InvoiceID, PdfFilePath = data.PdfFilePath, ErrorMessage = string.Empty });
                }

                catch (Exception ex)
                {
                    MoveFile(data.PdfFilePath,"Error");
                    result.Add(new ImportInvoiceResponseDto { InvoiceNo = data?.InvoiceNo, InvoiceId = 0, PdfFilePath = data.PdfFilePath, ErrorMessage = ex.Message });
                    continue;
                }
                finally
                {
                    if (Path.Exists(data.PdfFilePath))
                    {
                        MoveFile(data.PdfFilePath, "Archive");
                    }
                }
            }
            return ResponseResult<List<ImportInvoiceResponseDto>>.Success(result);
        }

        private Invoice DataMapper(ImportInvoiceDto dto)
        {
            var entityRepo = _unitOfWork.GetRepository<EntityProfile>();
            var supplierRepo = _unitOfWork.GetRepository<SupplierInfo>();
            var keywordRepo = _unitOfWork.GetRepository<Keyword>();

            var entity = entityRepo.Query().FirstOrDefault(f => f.EntityCode == dto.CompanyCode);
            var supplier = supplierRepo.Query().FirstOrDefault(f => f.SupplierID == dto.SupplierId);
            var keyword = keywordRepo.Query().FirstOrDefault(f => f.KeywordName == dto.Keyword);

            var invoice = new Invoice
            {
                InvoiceNo = dto.InvoiceNo,
                InvoiceDate = string.IsNullOrEmpty(dto.InvoiceDate) ? null : DateTime.Parse(dto.InvoiceDate),
                MapID = dto.MapId.ToString(),
                ScanDate = string.IsNullOrEmpty(dto.ScanDate) ? null : DateTime.Parse(dto.ScanDate),
                EntityProfileID = entity == null ? null : entity.EntityProfileID,
                SupplierInfoID = supplier == null ? null : supplier.SupplierInfoID,
                SuppBankAccount = string.Empty,
                PoNo = dto.PoNo ?? string.Empty,
                GrNo = dto.GrNo ?? string.Empty,
                TaxCodeID = supplier?.TaxCodeID ?? null,
                PaymentTerm = supplier?.PaymentTerms ?? string.Empty,
                Currency = dto.Currency,
                NetAmount = dto.NetAmount,
                TaxAmount = dto.TaxAmount,
                TotalAmount = dto.TotalAmount,
                Note = "-- INVOICE IMPORT --",
                ApproverRole = null,
                ApprovedUser = null,
                QueueType = InvoiceQueueType.ExceptionQueue,
                StatusType = InvoiceStatusType.Validation,
                ImageID = null,
                KeywordID = keyword?.KeywordID,
            };
            invoice.SetAuditFieldsOnCreate("System");
            return invoice;
        }

        private async Task<string> CopyInvoiceFileAsync(string filePath, CancellationToken ct)
        {
            var systemVariableRepo = _unitOfWork.GetRepository<SystemVariable>();
            var rootPath = systemVariableRepo.Query().FirstOrDefault(f => f.Name.Equals("DEV1"))?.Value;
            var imageId = systemVariableRepo.Query().FirstOrDefault(f => f.Name.Equals("NextImageID"))?.Value;

            var fileName = $"{imageId.PadLeft(8, '0')}.pdf";

            string level1 = fileName.Length >= 2 ? fileName[..2] : fileName;
            string level2 = fileName[2..5];

            var basePath = Path.Combine(rootPath, level1, level2);
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            var imagePath = Path.Combine(basePath, fileName);
            var image = systemVariableRepo.Query().FirstOrDefault(f => f.Name.Equals("NextImageID"));
            image.Value = (Convert.ToInt32(image.Value) + 1).ToString();

            systemVariableRepo.UpdateAsync(image.SystemVariableID, image);

            filePath = Path.ChangeExtension(filePath, ".pdf");
            await using (var source = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read))
            await using (var dest = new FileStream(
                imagePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None))
            {
                await source.CopyToAsync(dest, ct);
                await dest.FlushAsync(ct);
            }


            var imageFileNameOnly = Path.GetFileNameWithoutExtension(imagePath);
            return $"{systemVariableRepo.Query().FirstOrDefault(f => f.Name.Equals("DEV1"))?.Name}:{imageFileNameOnly}PDF";
        }

        private void MoveFile(string filePath,string folder)
        {

            var originalFileName = Path.GetFileNameWithoutExtension(filePath);
            var originalPdfFile = $"{originalFileName}.pdf";
            var originalXmlFile = $"{originalFileName}.xml";

            string directoryPath = Path.GetDirectoryName(filePath);


            var archivePdfFilePath = Path.Combine(directoryPath, folder, originalPdfFile);
            if (Path.Exists(archivePdfFilePath))
            {
                var _originalPdfFile = $"{originalFileName}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                File.Move(Path.Combine(directoryPath, originalPdfFile), Path.Combine(directoryPath, folder, _originalPdfFile));
            }
            else
            {
                File.Move(Path.Combine(directoryPath, originalPdfFile), Path.Combine(directoryPath, folder, originalPdfFile));
            }


            var archiveXmlFilePath = Path.Combine(directoryPath, folder, originalXmlFile);
            if (Path.Exists(archiveXmlFilePath))
            {
                var _originalXmlFile = $"{originalFileName}_{DateTime.Now:yyyyMMddHHmmss}.xml";
                File.Move(Path.Combine(directoryPath, originalXmlFile), Path.Combine(directoryPath, folder, _originalXmlFile));
            }
            else
            {
                File.Move(Path.Combine(directoryPath, originalXmlFile), Path.Combine(directoryPath, folder, originalXmlFile));
            }
        }
    }
}
