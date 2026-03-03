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
                request.InvoiceNo,
                request.PONo,
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