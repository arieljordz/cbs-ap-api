using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Features.Invoicing.InvActions.Command;
using CbsAp.Application.Features.Invoicing.Reports;
using CbsAp.Application.Features.PO.Command.SavePO;
using CbsAp.Application.Features.PO.Command.UpdatePO;
using CbsAp.Application.Features.PO.Queries.GetPOLineUsage;
using CbsAp.Application.Features.PO.Queries.GetPOMatchingByInvID;
using CbsAp.Application.Features.PO.Queries.POSearch;
using CbsAp.Application.Features.PO.Queries.ReCalculateRemainingQty;
using CbsAp.Application.Features.PO.Queries.Reports;
using CbsAp.Application.Features.PO.Queries.SearchPOLines;
using CbsAp.Domain.Entities.Invoicing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PurchaseOrderController : BaseAPIController
    {
        private readonly ISender _mediator;

        public PurchaseOrderController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("searchPOLines")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchPOLinesPage([FromQuery] SearchPOLinesQuery query)
        {
            var result = await _mediator.Send(query);
            return CreateResponse(result);
        }

        [HttpPost("savePOMatching")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SavePOMatching([FromBody] SavePOMatchingDto dto) {


            var command = new SavePOCommand(dto, this.CurrentUser);

            var result = await _mediator.Send(command);
            return CreateResponse(result);
        }

        [HttpGet("{PONo}/{InvoiceID}/getPOMatchingByID")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPOMatchByInvID(string PONo, long InvoiceID)
        {
            var param = new GetPOMatchByInvIDQuery(PONo, InvoiceID);
            var result = await _mediator.Send(param);
            return CreateResponse(result);
        }

        [HttpPut("updatePOMatching")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePOMatching([FromBody] SavePOMatchingDto dto)
        {

            var command = new UpdatePOCommand(dto, this.CurrentUser);

            var result = await _mediator.Send(command);
            return CreateResponse(result);
        }

        [HttpPost("recalculatePOLine")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalculateRemainingQty([FromBody] SavePOMatchingDto dto)
        {

            var query = new ReCalculateRemainingQtyQuery(dto);
            var result = await _mediator.Send(query);
            return CreateResponse(result);
        }


        [HttpGet("{purchaseOrderLineID}/usage")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPurchaseOrderLineUsage(long purchaseOrderLineID)
        {
            var param = new GetPOLineUsageQuery(purchaseOrderLineID);

            var result = await _mediator.Send(param);
            return CreateResponse(result);
        }
        [HttpGet("searchPO/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchPO([FromQuery] POSearchQuery query)
        {
            var result = await _mediator.Send(query);
            return CreateResponse(result);

        }
        [HttpGet("export/download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportPurchaseOrder
         ([FromQuery] ExportPOSearchQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"PurchaseOrders_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
    }
}