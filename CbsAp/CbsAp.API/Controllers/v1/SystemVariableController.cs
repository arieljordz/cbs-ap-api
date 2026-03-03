using Asp.Versioning;
using CbsAp.Application.DTOs.SystemSettings;
using CbsAp.Application.Features.SystemSettings.ArchiveInvoice.Command.Update;
using CbsAp.Application.Features.SystemSettings.ArchiveInvoice.Queries.GetArchieveInvSettings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SystemVariableController : BaseAPIController
    {
        private readonly ISender _mediator;

        public SystemVariableController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetArchiveInvoiceSetting()
        {
            var param = new GetArchieveInvSettingQuery();
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateArchiveInvoiceSetting([FromBody] ArchiveInvSettingDto dto)
        {
            var param = new UpdateArchSettingCommand(dto, this.CurrentUser);
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }
    }
}
