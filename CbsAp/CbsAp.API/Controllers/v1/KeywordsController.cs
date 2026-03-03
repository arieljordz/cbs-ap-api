using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Keyword;
using CbsAp.Application.Features.KeywordManagement.Command;
using CbsAp.Application.Features.KeywordManagement.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class KeywordsController : BaseAPIController
    {
        private readonly ISender _mediator;

        public KeywordsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("{keywordID}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteKeyword(long keywordID)
        {
            var userCommand = new DeleteKeywordCommand(keywordID);
            var result = await _mediator.Send(userCommand);

            return CreateResponse(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> CreateKeyword([FromBody] CreateUpdateKeywordDTO createKeywordDto)
        {
            var command = new CreateKeywordCommand(
               createKeywordDto.KeywordName,
               createKeywordDto.EntityProfileID,
               createKeywordDto.InvoiceRoutingFlowID,
               createKeywordDto.IsActive,
               this.CurrentUser
            );
            var result = await _mediator.Send(command);

            return CreateResponse(result);
        }


        [HttpGet("{keywordId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetKeywordById(long keywordId)
        {
            var keywordByIdQuery = new GetKeywordByIdQuery(keywordId);
            var result = await _mediator.Send(keywordByIdQuery);

            return CreateResponse(result);
        }

        [HttpPut("{keywordID}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateKeyword([FromRoute] long keywordID, [FromBody] CreateUpdateKeywordDTO updateKeywordDto)
        {
            var command = new UpdateKeywordCommand(keywordID,
               updateKeywordDto.KeywordName,
               updateKeywordDto.EntityProfileID,
               updateKeywordDto.InvoiceRoutingFlowID,
               updateKeywordDto.IsActive,
              this.CurrentUser
            );
            var result = await _mediator.Send(command);

            return CreateResponse(result);
        }

        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<KeywordDTO>> GetKeywords([FromQuery] GetKeywordsQuery keywordsQuery)
        {
            var keywords = await _mediator.Send(keywordsQuery);
            return Ok(keywords);
        }

        [HttpGet("download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DownloadKeywords ([FromQuery] ExportKeywordsQuery exportKeywordsQuery)
        {
            var result = await _mediator.Send(exportKeywordsQuery);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"Keywords_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }


    }
}