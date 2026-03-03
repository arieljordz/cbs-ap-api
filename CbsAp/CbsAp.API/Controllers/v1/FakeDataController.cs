using Asp.Versioning;
using CbsAp.Application.DTOs.Dashboard;
using CbsAp.Application.FakeStoreData;
using CbsAp.Application.FakeStoreData.FakeDTO;
using CbsAp.Application.FakeStoreData.FakeInvoices.InvoiceArchiveActions;
using CbsAp.Application.FakeStoreData.FakeInvoices.InvoiceActions;
using CbsAp.Application.FakeStoreData.ValidateRules;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
   //[ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class FakeDataController : BaseAPIController
    {
        private readonly ISender _mediator;

        public FakeDataController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("userspage")]
        public async Task<IActionResult> GetUserlists()
        {
            var fakeUser = new UserSearchFaker();
            var result = ResponseResult<List<FakeSearchUser>>
                .Success(fakeUser.returnFakeUser());

            return Ok(result);
        }

        [HttpGet("assignedinvoices")]
        public async Task<IActionResult> GetAssignedInvoices()
        {
            List<AssignedInvoiceDTO> invoices = new List<AssignedInvoiceDTO>();
            invoices.Add(new AssignedInvoiceDTO()
            {
                Amount = 5000,
                AssignedRole = "Admistrator",
                DueDate = DateTime.Now,
                InvoiceDate = DateTime.Now,
                SupplierName = "Converga Asia Inc"
            });

            var result = ResponseResult<List<AssignedInvoiceDTO>>.Success(invoices);
            result.ResponseData = invoices;
            result.IsSuccess = true;

            return Ok(result);
        }

        [HttpPost("{invoiceID}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SubmitFake(long invoiceID)
        {
            var cmd =
           new FakeSubmitCommand(invoiceID, this.CurrentUser);

            var result = await _mediator.Send(cmd);

            return CreateResponse(result);
        }

        [HttpPost("Import-GenerateFakeInvoice")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateFakeInvoice([FromBody] FakeInvoiceDto cmd)
        {
            var genfakeInvoiceCommand =
                new CreateFakeInvoiceCommand(cmd, this.CurrentUser);
            var result = await _mediator.Send(genfakeInvoiceCommand);

            return CreateResponse(result);
        }

        [HttpPost("Import-GenerateFakeArchiveInvoice")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateFakeArchiveInvoice([FromBody] FakeInvoiceDto cmd)
        {
            var generateArchiveInvoiceCommand =
                new CreateFakeInvoiceArchiveCommand(cmd, this.CurrentUser);
            var result = await _mediator.Send(generateArchiveInvoiceCommand);

            return CreateResponse(result);
        }

        [HttpPost("Import-ArchiveInvoiceById/{invoiceID:long}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ArchiveInvoiceById(long invoiceID)
        {
            var command = new ArchiveInvoiceByIdCommand(invoiceID, this.CurrentUser);
            var result = await _mediator.Send(command);

            return CreateResponse(result);
        }
    }
}