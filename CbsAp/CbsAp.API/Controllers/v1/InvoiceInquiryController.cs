using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Features.InvoiceInquiry.Queries.Pagination;
using CbsAp.Application.Features.InvoiceInquiry.Queries.Reports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    /// <summary>
    /// Defines the <see cref="InvoiceInquiryController" />
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class InvoiceInquiryController : BaseAPIController
    {
        private readonly ISender _mediator;

        public InvoiceInquiryController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("invoice-inquiry/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchInvoiceInquiryPagination
            ([FromBody] SearchInvoiceInquiryQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("invoice-inquiry/download")]
        public async Task<IActionResult> DownloadInvoiceInquiry
           ([FromQuery] ExportInvoiceInquiryQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);

            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"InvoiceInquiry_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }


    }
}