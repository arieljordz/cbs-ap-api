using Asp.Versioning;
using CbsAp.Application.DTOs.UserAuthentication;
using CbsAp.Application.Features.Authentication.Commands.ActivateUser;
using CbsAp.Application.Features.Authentication.Commands.ForgotPassword;
using CbsAp.Application.Features.Authentication.Commands.SetNewPassword;
using CbsAp.Application.Features.Authentication.Commands.SwitchRole;
using CbsAp.Application.Features.Authentication.Common;
using CbsAp.Application.Features.Authentication.Queries;
using CbsAp.Application.Features.Authentication.Queries.IsActivateNewUserLinkValid;
using CbsAp.Application.Features.Authentication.Queries.IsPasswordResetLinkValid;
using CbsAp.Application.Features.Authentication.Queries.topbar;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var authenticateQuery = _mapper.Map<LoginQuery>(request);
            var result = await _mediator.Send(authenticateQuery);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("forgotpassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("setnewpassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("activatenewuser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ActivateNewUser([FromBody] ActivateUserCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize]
        [HttpPost("thumbnail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetThumbnailInfo([FromBody] GetThumbnailInfoQuery request)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("isPasswordResetLinkValid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> IsPasswordResetLinkValid(string token)
        {
            var query = new IsPasswordResetLinkValidQuery(token);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("isActivateNewUserLinkValid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> IsActivateNewUserLinkValid(string token)
        {
            var query = new IsActivateNewUserLinkValidQuery(token);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("switchrole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SwitchRole([FromBody] SwitchRoleDto switchRoleDto)
        {
            var currentUser = HttpContext.User?.Identity?.Name ?? "";
            var switchRoleCommand = new SwitchRoleCommand(switchRoleDto.RoleId, currentUser);

            var result = await _mediator.Send(switchRoleCommand);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}