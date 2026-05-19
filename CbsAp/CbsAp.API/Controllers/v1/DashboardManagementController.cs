using Asp.Versioning;
using CbsAp.Application.DTOs.Dashboard;
using CbsAp.Application.Features.Dashboard.Command.Common;
using CbsAp.Application.Features.Dashboard.Queries;
using CbsAp.Application.Features.Dashboard.Queries.Common;
using CbsAp.Application.Features.Users.Queries.Common;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class DashboardManagementController : BaseAPIController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public DashboardManagementController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("assigned-invoices")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AssignedInvoiceResultDTO>> GetAssignedInvoices(string filterType)
        {
            int.TryParse( this.CurrentRole, out var roleId);
            var query = new GetAssignedInvoiceQuery(this.CurrentUser,roleId,filterType);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("notices")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> CreateNotice([FromBody] CreateNoticeDTO createNoticeDTO)
        {
            var command = new CreateNoticeCommand(
               createNoticeDTO.Heading,
               createNoticeDTO.Message,
               createNoticeDTO.SendNotification,
              this.CurrentUser
            );
            var result = await _mediator.Send(command);

            return CreateResponse(result);
        }

        [Authorize]
        [HttpPut("notices")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateNotice([FromBody] UpdateNoticeDTO updateNoticeDTO)
        {
            var command = new UpdateNoticeCommand(
               updateNoticeDTO.NoticeID,
               updateNoticeDTO.Heading,
               updateNoticeDTO.Message,
               updateNoticeDTO.SendNotification,
              this.CurrentUser
            );
            var result = await _mediator.Send(command);

            return CreateResponse(result);
        }

        [Authorize]
        [HttpGet("notices")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<NoticeDTO>> GetNotices()
        {
            var notices = await _mediator.Send(new GetAllNoticeQuery());
            return Ok(notices);
        }
    }
}