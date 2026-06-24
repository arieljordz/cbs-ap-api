using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.PO;
using CbsAp.Application.Features.Entity.Queries.GetEntityByID;
using CbsAp.Application.Features.Invoicing.InvActions.Command;
using CbsAp.Application.Features.Invoicing.Reports;
using CbsAp.Application.Features.PO.Command.SavePO;
using CbsAp.Application.Features.PO.Command.UpdatePO;
using CbsAp.Application.Features.PO.Queries.BatchListPurchaseOrder;
using CbsAp.Application.Features.PO.Queries.GetPOLineUsage;
using CbsAp.Application.Features.PO.Queries.GetPOMatchingByInvID;
using CbsAp.Application.Features.PO.Queries.GetPurchaseOrderByID;
using CbsAp.Application.Features.PO.Queries.GetPurchaseOrderListByID;
using CbsAp.Application.Features.PO.Queries.POSearch;
using CbsAp.Application.Features.PO.Queries.ReCalculateRemainingQty;
using CbsAp.Application.Features.PO.Queries.ReportDetail;
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

        [HttpGet("exportpodetail/download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Exportpodetail
    ([FromQuery] ExportPODetailSearchQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"PurchaseOrders_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }


        [HttpGet("GetPurchaseOrderByID/{purchaseOrderId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPurchaseOrderByID(long purchaseOrderId)
        {
            var param = new GetPurchaseOrderByIDQuery(purchaseOrderId);
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpGet("GetPurchaseOrderListByID/paged")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPurchaseOrderListByID([FromQuery] GetPurchaseOrderListByIDQuery query)
        {

            var result = await _mediator.Send(query);

            return CreateResponse(result);
        }

        [HttpGet("PurchaseOrderNext/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PurchaseOrderNext([FromQuery] POSearchQuery query)
        {
            var result = await _mediator.Send(query);
            return CreateResponse(result);

        }


        // intent to add the next batch of list base on pagination
        [HttpGet("BatchListPurchaseOrder/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchListPurchaseOrder([FromQuery] BatchListPurchaseOrderQuery query)
        {
            var result = await _mediator.Send(query);
            return CreateResponse(result);

        }
    }
}