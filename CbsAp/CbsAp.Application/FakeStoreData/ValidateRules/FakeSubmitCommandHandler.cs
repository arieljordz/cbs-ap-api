using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Entities.TaxCodes;
using CbsAp.Domain.Enums;
using CBSAP.ValidationEngine;
using CBSAP.ValidationEngine.Core;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.FakeStoreData.ValidateRules
{
    public class FakeSubmitCommandHandler : ICommandHandler<FakeSubmitCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;

        public FakeSubmitCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<bool>> Handle(FakeSubmitCommand request, CancellationToken cancellationToken)
        {
            var invoiceRepo = _unitofWork.GetRepository<Invoice>();
            var supplierRepo = _unitofWork.GetRepository<SupplierInfo>();
            var taxcodeRepo = _unitofWork.GetRepository<TaxCode>();

            var invoice = await invoiceRepo
                .Query()
                .FirstOrDefaultAsync(i => i.InvoiceID == request.InvoiceID);

            if (invoice == null)
                return ResponseResult<bool>.BadRequest("Invoice is not found");

            ////run validation engine
            ///
            var invoiceDataToValidate = (
                await invoiceRepo.ApplyPredicateAsync(i =>
            i.InvoiceID != request.InvoiceID
            && (i.StatusType != Domain.Enums.InvoiceStatusType.Rejected
            || i.StatusType != Domain.Enums.InvoiceStatusType.Exception))).AsEnumerable();

            var supplier = (await supplierRepo.GetAllAsync()).AsEnumerable();
            var taxCode = (await taxcodeRepo.GetAllAsync()).AsEnumerable();

            var rules = ValidationRuleFactory.Load("cbsap.validationrules.json");

            var engine = new ValidationEngine(rules);

            var runtimeContext = new Dictionary<string, object>
            {
                ["InvoiceRecords"] = invoiceDataToValidate,
                ["SupplierInfos"] = supplier,
                ["TaxCodes"] = taxCode,
            };

            var failures = engine.Validate(invoice!, runtimeContext, out bool stopEarly);

            if (!failures.Any())
                return ResponseResult<bool>.OK("No Error");

            if (stopEarly)
            {
                var critical = failures.First(f => f.Severity == EngineValidationSeverity.Critical);

                var actityLog = new InvoiceActivityLog
                {
                    InvoiceID = invoice.InvoiceID,
                    PreviousStatus = invoice.StatusType,
                    CurrentStatus = (Domain.Enums.InvoiceStatusType?)critical.NextStatus,
                    Reason = critical.ErrorMessage,
                    Action = InvoiceActionType.Submit
                };

                invoice.StatusType = (Domain.Enums.InvoiceStatusType?)critical.NextStatus;
                actityLog.SetAuditFieldsOnCreate(request.UpdateBy);
                await _unitofWork.GetRepository<InvoiceActivityLog>().AddAsync(actityLog);

                invoice.InvoiceID = request.InvoiceID;
                invoice.StatusType = Domain.Enums.InvoiceStatusType.Rejected;
                invoice.QueueType = Domain.Enums.InvoiceQueueType.RejectionQueue;

                invoice.SetAuditFieldsOnUpdate(request.UpdateBy);

                var saved = await _unitofWork.SaveChanges(cancellationToken);
                return ResponseResult<bool>.OK(critical.ErrorMessage);
            }

            var highest = failures.MaxBy(f => f.Severity);

            var actityLogMulti = new InvoiceActivityLog
            {
                InvoiceID = invoice.InvoiceID,
                PreviousStatus = invoice.StatusType,
                CurrentStatus = (Domain.Enums.InvoiceStatusType?)highest.NextStatus,
                Reason = highest.ErrorMessage,
                Action = InvoiceActionType.Submit
            };

            var logs = failures.Select(f => new InvoiceActivityLog
            {
                InvoiceID = invoice.InvoiceID,
                PreviousStatus = invoice.StatusType,
                CurrentStatus = (Domain.Enums.InvoiceStatusType?)f.NextStatus,
                Reason = f.ErrorMessage,
                Action = InvoiceActionType.Submit,
                CreatedBy = request.UpdateBy,
                CreatedDate = DateTimeOffset.Now
            });

            await _unitofWork.GetRepository<InvoiceActivityLog>().AddRangeAsync(logs);

            invoice.InvoiceID = request.InvoiceID;
            invoice.StatusType = (Domain.Enums.InvoiceStatusType?)highest.NextStatus;
            invoice.QueueType = (Domain.Enums.InvoiceQueueType?)highest.TargetQueue;
            invoice.SetAuditFieldsOnUpdate(request.UpdateBy);

            await _unitofWork.SaveChanges(cancellationToken);
            //   return ResponseResult<bool>.OK("No Error");

            // Optional: log non-critical failures
            // var logs = failures.Select(f => new ValidationLog
            // {
            //     EntityId = invoice.Id,
            //     Error = f.ErrorMessage,
            //     Code = f.ErrorCode,
            //     Severity = f.Severity.ToString(),
            //     CreatedAt = DateTime.UtcNow
            // });
            // await _logRepo.SaveAsync(logs);

            var combinedErrors = string.Join("; ", failures.Select(f => f.ErrorMessage));
            return ResponseResult<bool>.OK(combinedErrors);
        }
    }
}