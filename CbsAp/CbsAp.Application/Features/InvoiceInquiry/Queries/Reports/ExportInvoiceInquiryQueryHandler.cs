using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.InvoiceInquiry;
using CbsAp.Application.Abstractions.Services.Reports;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.InvoiceInquiry.Queries.Reports
{
    public class ExportInvoiceInquiryQueryHandler : IQueryHandler<ExportInvoiceInquiryQuery, ResponseResult<byte[]>>
    {
        private readonly IInvoiceInquiryService _invoiceInquiryService;
        private readonly IExcelService _excelService;

        public ExportInvoiceInquiryQueryHandler(IInvoiceInquiryService invoiceInquiryService, IExcelService excelService)
        {
            _invoiceInquiryService = invoiceInquiryService;
            _excelService = excelService;
        }

        public async Task<ResponseResult<byte[]>> Handle(ExportInvoiceInquiryQuery request, CancellationToken cancellationToken)
        {
            var result = await _invoiceInquiryService.ExportInvoiceInquiryToExcel(
                request.SupplierName,
                request.InvoiceNumber, 
                request.PONumber, 
                request.Status, 
                request.ScanDateFrom,
                request.ScanDateTo, 
                request.InvoiceDateFrom,
                request.InvoiceDateTo, 
                cancellationToken);

            if (!result.Any() || result == null)
            {
                return ResponseResult<byte[]>.NotFound(MessageConstants.Message("InvoiceInquiry", MessageOperationType.NotFound));
            }

            var excelBytes = await Task.Run(() => _excelService.GenerateExcel(result, "InvoiceInquiry"));

            return ResponseResult<byte[]>.Success(excelBytes, "Export excel data");
        }
    }
}