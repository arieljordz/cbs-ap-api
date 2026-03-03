using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Invoicing.InvRoutingFlows.Commands.CreateRoutingFlow;
using CbsAp.Application.Invoicing.InvRoutingFlows.Commands.DeleteRoutingFlow;
using CbsAp.Application.Invoicing.InvRoutingFlows.Commands.UpdateRoutingFlow;
using CbsAp.Application.Invoicing.InvRoutingFlows.Queries.GetInvRoutingFlowByID;
using CbsAp.Application.Invoicing.InvRoutingFlows.Queries.Pagination;
using CbsAp.Application.Invoicing.InvRoutingFlows.Queries.Reports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class InvoiceRoutingFlowController : BaseAPIController
    {
        private readonly ISender _mediator;

        public InvoiceRoutingFlowController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateInvRoutingFlow([FromBody] InvRoutingFlowDto dto)
        {
            var invRoutingFlowCommand =
               new CreateRoutingFlowCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(invRoutingFlowCommand);

            return CreateResponse(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateInvRoutingFlow([FromBody] InvRoutingFlowDto dto)
        {
            var invRoutingFlowCommand =
               new UpdateRoutingFlowCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(invRoutingFlowCommand);

            return CreateResponse(result);
        }

        [HttpDelete("{invRoutingFlowID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteInvRoutingFlow(long invRoutingFlowID)
        {
            var param = new DeleteRoutingFlowCommand(invRoutingFlowID);
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpGet("{invRoutingFlowID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetInvRoutingFlowByID(long invRoutingFlowID)
        {
            var param = new GetInvRoutingFlowByIDQuery(invRoutingFlowID);
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchInvRoutingFlowPagination
          ([FromQuery] SearchInvRoutingFlowQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("users/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchInvRoutingFlowByUserPagination
        ([FromQuery] SearchInvRoutingFlowByUserQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("roles/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchInvRoutingFlowByRolesPagination
       ([FromQuery] SearchInvRoutingFlowByRolesQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DownloadInvRoutingFlow
          ([FromQuery] InvRoutingFlowExportQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"InvoiceRoutingFlow_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
    }
}
