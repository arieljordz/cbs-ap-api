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
    public class ValidateByIdsCommandHandler
        : ICommandHandler<ValidateByIdsCommand, ResponseResult<List<InvValidationResponseDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IWebHostEnvironment _env;

        public ValidateByIdsCommandHandler(
            IInvoiceRepository invoiceRepository,
            IWebHostEnvironment env)
        {
            _invoiceRepository = invoiceRepository;
            _env = env;
        }

        public async Task<ResponseResult<List<InvValidationResponseDto>>> Handle(ValidateByIdsCommand request, CancellationToken cancellationToken)
        {
            var results = new List<InvValidationResponseDto>();

            foreach (var invoiceId in request.InvoiceIds)
            {
                var result = await ValidateInvoiceAsync(invoiceId, request.UpdatedBy, cancellationToken);
                results.Add(result);
            }

            return ResponseResult<List<InvValidationResponseDto>>.OK(results);
        }

        private async Task<InvValidationResponseDto> ValidateInvoiceAsync(long invoiceId, string updatedBy, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetByIdWithDetailsAsync(invoiceId);

            if (invoice == null)
            {
                return new InvValidationResponseDto
                {
                    QueueType = null,
                    InvoiceActionType = "Validate",
                    FailureMessages = $"Invoice {invoiceId} not found"
                };
            }

            // 👉 call your existing validation pipeline (unchanged logic conceptually)
            var result = await _invoiceRepository.ValidateInvoiceAsync(
                invoice,
                updatedBy,
                _env.EnvironmentName,
                cancellationToken
            );

            return new InvValidationResponseDto
            {
                QueueType = invoice.QueueType!.Value,
                InvoiceActionType = "Validate",
                FailureMessages = result.FailureMessages.Any()
                    ? string.Join(";", result.FailureMessages)
                    : string.Empty
            };
        }
    }
}