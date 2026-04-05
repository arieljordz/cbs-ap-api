using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Features.Invoicing.InvActions.Command;
using CbsAp.Application.Features.Invoicing.InvActions.Command.AddComment;
using CbsAp.Application.Features.Invoicing.InvActions.Command.ForApproval;
using CbsAp.Application.Features.Invoicing.InvActions.Command.ForForceToSubmit;
using CbsAp.Application.Features.Invoicing.InvActions.Command.ForHold;
using CbsAp.Application.Features.Invoicing.InvActions.Command.ForReject;
using CbsAp.Application.Features.Invoicing.InvActions.Command.Reactivate;
using CbsAp.Application.Features.Invoicing.InvActions.Command.RouteToException;
using CbsAp.Application.Features.Invoicing.InvActions.Command.Submit;
using CbsAp.Application.Features.Invoicing.InvActions.Command.Validate;
using CbsAp.Application.Features.Invoicing.InvActions.Queries.ArchiveSearch;
using CbsAp.Application.Features.Invoicing.InvActions.Queries.ExceptionsSearch;
using CbsAp.Application.Features.Invoicing.InvActions.Queries.GetAdjacentInvoice;
using CbsAp.Application.Features.Invoicing.InvActions.Queries.GetInvoiceByID;
using CbsAp.Application.Features.Invoicing.InvActions.Queries.GetInvoiceStatus;
using CbsAp.Application.Features.Invoicing.InvActions.Queries.LoadInvoiceComment;
using CbsAp.Application.Features.Invoicing.InvActions.Queries.MyInvoiceSearch;
using CbsAp.Application.Features.Invoicing.InvActions.Queries.RejectedSearch;
using CbsAp.Application.Features.Invoicing.InvActivityLog.Queries;
using CbsAp.Application.Features.Invoicing.InvAllocationLine.Commands;
using CbsAp.Application.Features.Invoicing.InvAllocationLine.Queries;
using CbsAp.Application.Features.Invoicing.InvAttachments.Command.Upload;
using CbsAp.Application.Features.Invoicing.InvAttachments.Queries.Download;
using CbsAp.Application.Features.Invoicing.InvAttachments.Queries.GetAttachments;
using CbsAp.Application.Features.Invoicing.InvInfoRouting.Queries;
using CbsAp.Application.Features.Invoicing.InvoiceImage;
using CbsAp.Application.Features.Invoicing.Reports;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
  //  [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class InvoiceController : BaseAPIController
    {
        private readonly ISender _mediator;

        public InvoiceController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{invoiceID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInvoiceByInvID(long invoiceID)
        {
            var param = new GetInvoiceQuery(invoiceID);
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpGet("{invoiceID}/getStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInvoiceStatus(long invoiceID)
        {
            var param = new GetInvoiceStatusQuery(invoiceID);
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpGet("{invoiceID}/next")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNextInvoiceId(
            long invoiceID,
            [FromQuery] InvoiceStatusType? statusType = null,
            [FromQuery] InvoiceQueueType? queueType = null)
        {
            var result = await _mediator.Send(new GetAdjacentInvoiceIdQuery(invoiceID, true, statusType, queueType));

            return CreateResponse(result);
        }

        [HttpGet("{invoiceID}/previous")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPreviousInvoiceId(
            long invoiceID,
            [FromQuery] InvoiceStatusType? statusType = null,
            [FromQuery] InvoiceQueueType? queueType = null)
        {
            var result = await _mediator.Send(new GetAdjacentInvoiceIdQuery(invoiceID, false, statusType, queueType));

            return CreateResponse(result);
        }

        [HttpGet("invAllocationLine/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchInvAllocationLine
         ([FromQuery] GetInvAllocLineQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateInvoice([FromBody] InvoiceDto dto)
        {
            var updateInvoiceCommand =
               new UpdateInvoiceCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(updateInvoiceCommand);

            return CreateResponse(result);
        }

        

        [HttpGet("myInvoiceSearch/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> MyInvoiceSearchPage
        ([FromQuery] MyInvoiceSearchQuery paramQuery)
        {
            int.TryParse(this.CurrentRole, out var roleId);
            paramQuery.RoleId = roleId;
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("rejectQueueSearch/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RejectedQueueSearchPage
        ([FromQuery] RejectedSearchQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("exceptionQueueSearch/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExceptionQueueSearchPage
        ([FromQuery] ExceptionsSearchQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("archiveQueueSearch/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ArchiveQueueSearchPage
        ([FromQuery] ArchiveSearchQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("myinvoice/download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportMyInvoices
         ([FromQuery] ExportMyInvoiceQuery paramQuery)
        {

            int.TryParse(this.CurrentRole, out var roleId);
            paramQuery.RoleId = roleId;
            var result = await _mediator.Send(paramQuery);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"MyInvoices_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }

        [HttpGet("rejectedinvoice/download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportRejectedInvoice
         ([FromQuery] ExportRejectedInvoiceQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"RejectedQueue_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }

        [HttpGet("exceptioninvoice/download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ExceptionInvoice
         ([FromQuery] ExportExceptionInvoiceQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"ExceptionQueue_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }

        [HttpGet("archiveinvoice/download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ArchiveInvoice
        ([FromQuery] ExportArchiveInvoiceQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"ArchiveQueue_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }

        [HttpPut("forApproval")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ForInvoiceApproval([FromBody] InvoiceDto dto)
        {
            var forApprovedCommand =
               new InvApproveCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(forApprovedCommand);

            return CreateResponse(result);
        }

        [HttpPut("ForReject")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ForInvoiceReject([FromBody] InvStatusChangeDto dto)
        {
            var forApprovedCommand =
               new InvRejectCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(forApprovedCommand);

            return CreateResponse(result);
        }

        [HttpPut("ForHold")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ForHold([FromBody] InvStatusChangeDto dto)
        {
            var forApprovedCommand =
               new InvHoldCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(forApprovedCommand);

            return CreateResponse(result);
        }

        [HttpPut("RouteToException")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RouteToException([FromBody] InvStatusChangeDto dto)
        {
            var routeToExceptionCommand =
               new InvoiceRouteToExceptionCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(routeToExceptionCommand);

            return CreateResponse(result);
        }

        [HttpPut("Reactivate")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Reactivate([FromBody] InvStatusChangeDto dto)
        {
            var reactivateInvoice =
               new InvoiceReactivateCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(reactivateInvoice);

            return CreateResponse(result);
        }

        [HttpPut("ForceToSubmit")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ForceToSubmit([FromBody] InvStatusChangeDto dto)
        {
            var forceToSubmit =
               new ForceToSubmitCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(forceToSubmit);

            return CreateResponse(result);
        }

        [HttpPost("addInvoiceComment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddInvoiceComment([FromBody] InvoiceCommentDto dto)
        {
            var invCommentCommand =
               new InvCommentCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(invCommentCommand);

            return CreateResponse(result);
        }

        [HttpGet("loadinvoicecomments/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LoadInvoiceComments
        ([FromQuery] LoadInvoiceCommentQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(InvAttachmentFromDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload([FromForm] InvAttachmentFromDto dto)
        {
            var invAttachment =
              new UploadAttachmentComand(dto, this.CurrentUser);
            var result = await _mediator.Send(invAttachment);

            return CreateResponse(result);
        }

        [HttpGet("{invoiceAttachnmentID}/downloadAttachment")]
        [EnableCors(PolicyConstants.CorsAttachmentPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DownloadAttachment(long invoiceAttachnmentID)
        {
            var param = new DownloadAttachmentQuery(invoiceAttachnmentID);
            var result = await _mediator.Send(param);

            if (!result.IsSuccess)
                return CreateResponse(result);

            return result.ResponseData;
        }

        [HttpGet("getAllattachment/{invoiceID}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAttachment
        (long invoiceID)
        {
            var param = new GetInvAttachmentsQuery(invoiceID);
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpGet("{invoiceId}/image")]
        [EnableCors(PolicyConstants.CorsAttachmentPolicy)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetImage(long invoiceId)
        {
            var filePath = await _mediator.Send(new GetInvoiceImageQuery(invoiceId));

            if (string.IsNullOrWhiteSpace(filePath))
            {
                return NotFound();
            }

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var contentType = "application/pdf";

            return File(stream, contentType, enableRangeProcessing: true);
        }

        [HttpPost("Submit")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SubmitInvoice([FromBody] InvoiceDto dto)
        {
            var updateInvoiceCommand =
               new SubmitCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(updateInvoiceCommand);

            return CreateResponse(result);
        }

        [HttpPost("Validate")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ValidateInvoice([FromBody] InvoiceDto dto)
        {
            var updateInvoiceCommand =
               new ValidateCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(updateInvoiceCommand);
            return CreateResponse(result);
        }

        [HttpGet("{invoiceID}/allocationlines")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInvoiceAllocationByInvID(long invoiceID)
        {
            var param = new GetInvAllocByInvIDQuery(invoiceID);
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpPost("{invoiceID}/allocationlines")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddAllocationLine(long invoiceID,[FromBody] InvAllocLineDto dto)
        {
            dto.InvoiceID = invoiceID;
            var command = new AddAllocationLineCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(command);

            return CreateResponse(result);
        }

        [HttpPut("{invoiceID}/allocationlines")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateAllocationLine(long invoiceID,[FromBody] InvAllocLineDto dto)
        {
            dto.InvoiceID = invoiceID;
            var command = new UpdateAllocationLineCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(command);

            return CreateResponse(result);
        }

        [HttpDelete("{invoiceID}/allocationlines/{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteAllocationLine(long invoiceID,long id)
        {
            var command = new DeleteAllocationLineCommand(id, this.CurrentUser);
            var result = await _mediator.Send(command);

            return CreateResponse(result);
        }

        [HttpGet("{invoiceID}/invoiceActivityLog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInvoiceActivityLogByInvID(long invoiceID)
        {
            var param = new GetInvActivityLogQuery(invoiceID);
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpGet("{invoiceID}/ActivityLog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetIActivityLogByInvID(long invoiceID)
        {
            var param = new GetActivityLogQuery(invoiceID);
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpGet("invoiceInfoLinkedRoutingLevels")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInvoiceInfoRoutingLevels([FromQuery] GetRoutingFlowLinkedLevelQuery query)
        {
            var result = await _mediator.Send(query);
            return CreateResponse(result);
        }

    }
}