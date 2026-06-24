using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.Reports
{
    public class ExportRejectedInvoiceQueryHandler : IQueryHandler<ExportRejectedInvoiceQuery, ResponseResult<byte[]>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        private readonly IExcelService _excelService;

        public ExportRejectedInvoiceQueryHandler(IInvoiceRepository invoiceRepository, IExcelService excelService)
        {
            _invoiceRepository = invoiceRepository;
            _excelService = excelService;
        }

        public async Task<ResponseResult<byte[]>> Handle(ExportRejectedInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = await _invoiceRepository.ExportRejectedInvoice(
                request.SupplierName,
                request.InvoiceNumber,
                request.PONumber,
                request.RoleId,
                // Supplier Info
                request.PaymentTerm,
                request.SupplierNo,
                request.SuppABN,
                request.SuppBankAccount,

                // Invoice Detail
                request.EntityProfileID,
                request.GrNo,
                request.StartInvoiceDate,
                request.EndInvoiceDate,
                request.StartDueDate,
                request.EndDueDate,
                request.DaystillDue,

                // Amounts
                request.NetAmount,
                request.TaxCodeID,
                request.TaxAmount,
                request.Currency,
                request.TotalAmount,

                // Routing Flow
                request.InvRoutingFlowName,
                request.NextRole,
                request.Keyword,

                // Transaction Info
                request.MapID,
                request.StartScanDate,
                request.EndScanDate,
                request.InvoiceID,
                cancellationToken
                );
            if (result.Count == 0 || result == null)
            {
                return ResponseResult<byte[]>.NotFound(MessageConstants.Message("RejectedQueue", MessageOperationType.NotFound));
            }
            var excelBytes = await Task.Run(() => _excelService.GenerateExcel(result, "RejectedQueue", true));

            return ResponseResult<byte[]>.Success(excelBytes, "Export RejecedQueue excel data");
        }
    }
}