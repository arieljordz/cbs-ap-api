using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.TaxCodesManagement;
using CbsAp.Application.Features.Dashboard.Queries.Common;
using CbsAp.Application.Features.TaxCodes.Command.Common;
using CbsAp.Application.Features.TaxCodesManagement.Queries.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TaxCodeController : BaseAPIController
    {
        private readonly ISender _mediator;

        public TaxCodeController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> CreateTaxCode([FromBody] CreateUpdateTaxCodeDTO createTaxCodeDTO)
        {
            var command = new CreateTaxCodeCommand(
               createTaxCodeDTO.EntityID,
               createTaxCodeDTO.TaxCodeName,
               createTaxCodeDTO.Code,
               createTaxCodeDTO.TaxRate,
               this.CurrentUser
            );
            var result = await _mediator.Send(command);

            return CreateResponse(result);
        }

        [HttpPut("{taxCodeID}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateTaxCode([FromRoute] long taxCodeID, [FromBody] CreateUpdateTaxCodeDTO updateTaxCodeDTO)
        {
            var command = new UpdateTaxCodeCommand(
               taxCodeID,
               updateTaxCodeDTO.EntityID,
               updateTaxCodeDTO.TaxCodeName,
               updateTaxCodeDTO.Code,
               updateTaxCodeDTO.TaxRate,
              this.CurrentUser
            );
            var result = await _mediator.Send(command);

            return CreateResponse(result);
        }

        [HttpGet("{taxCodeId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEntityByID(long taxCodeId)
        {
            var taxCodeByIdQuery = new GetTaxCodeByIDQuery(taxCodeId);
            var result = await _mediator.Send(taxCodeByIdQuery);

            return CreateResponse(result);
        }

        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TaxCodeDTO>> GetTaxCodes([FromQuery] GetTaxCodesQuery taxCodeQuery)
        {
            var taxCodes = await _mediator.Send(taxCodeQuery);
            return Ok(taxCodes);
        }

        [HttpGet("download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DownloadTaxCodes
           ([FromQuery] ExportTaxCodesQuery exportTaxCodesQuery)
        {
            var result = await _mediator.Send(exportTaxCodesQuery);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"TaxCodes_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
    }
}