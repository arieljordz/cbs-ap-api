using Asp.Versioning;
using CbsAp.Application.DTOs.Import;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CbsAp.Application.Features.Import;

namespace CbsAp.API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ImportController : BaseAPIController
    {
        private readonly ISender _mediator;

        public ImportController(ISender mediator)
        {
            _mediator = mediator;
        }



        [HttpPost("Import-invoice")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ImportInvoice([FromBody] List<ImportInvoiceDto> command)
        {
            var param = new ImportQuery(command);
            var result = await _mediator.Send(param);
            return CreateResponse(result);
        }

    }
}
