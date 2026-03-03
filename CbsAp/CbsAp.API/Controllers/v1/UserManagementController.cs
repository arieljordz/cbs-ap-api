using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Features.TaxCodesManagement.Queries.Common;
using CbsAp.Application.Features.Users.Commands.CreateNewUser;
using CbsAp.Application.Features.Users.Commands.DeleteUser;
using CbsAp.Application.Features.Users.Commands.LockUnlockUser;
using CbsAp.Application.Features.Users.Commands.UpdateUser;
using CbsAp.Application.Features.Users.Queries.Common;
using CbsAp.Application.Features.Users.Queries.ExportUserQuery;
using CbsAp.Application.Features.Users.Queries.UserRoleActions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserManagementController : BaseAPIController
    {
        private readonly ISender _mediator;

        private readonly IMapper _mapper;

        public UserManagementController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<UserSearchPaginationDTO>>> GetSearchUserAccount()
        {
            var query = new GetUserSearchQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("users/active-users")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetActiveUsers()
        {
            var query
                = new ActiveUsersQuery();
            var result = await _mediator.Send(query);
            return CreateResponse(result);
        }

        [HttpGet("users/{userAccountID}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserAccountByUserAccountID(long userAccountID)
        {
            var query
                = new SearchUserByUserAccountIDQuery(userAccountID);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("users")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateUserAccount([FromBody] CreateUserDTO userDTO)
        {
            var userCommand =
               new CreateUserCommand(userDTO, this.CurrentUser);
            var result = await _mediator.Send(userCommand);

            return CreateResponse(result);
        }

        [HttpPut("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserAccount([FromBody] UpdateUserDTO userDTO)
        {
            var updateUserCommand = new UpdateUserCommand(userDTO, this.CurrentUser);
            var result = await _mediator.Send(updateUserCommand);
            return CreateResponse(result);
        }

        [HttpGet("users/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchUsers([FromQuery] GetUserSearchParameterQuery parameterQuery)
        {
            var result = await _mediator.Send(parameterQuery);
            return CreateResponse(result);
        }

        [HttpDelete("users/{userAccountID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeactivateUserAccount(long userAccountID)
        {
            var userCommand = new DeactivateUserCommand(userAccountID, this.CurrentUser);
            var result = await _mediator.Send(userCommand);

            return CreateResponse(result);
        }

        [HttpGet("users/{roleID}/user-roles")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserByRole(long roleID)
        {
            var paramQuery = new GetUsersByRoleQuery(roleID);
            var result =
                await _mediator.Send(paramQuery);

            return CreateResponse(result);
        }

        [HttpGet("users/{roleID}/available-users")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserNotInRole(long roleID)
        {
            var paramQuery = new GetUserNotInRoleQuery(roleID);
            var result =
                await _mediator.Send(paramQuery);

            return CreateResponse(result);
        }

        [HttpPost("users/lock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetThumbnailInfo([FromBody] LockUnlockUserCommand request)
        {
            var result = await _mediator.Send(request);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("users/download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DownloadTaxCodes
           ([FromQuery] ExportUserQuery exportUser)
        {
            var result = await _mediator.Send(exportUser);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"Users_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
    }
}