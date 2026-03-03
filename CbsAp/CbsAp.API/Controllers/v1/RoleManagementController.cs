using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Features.Roles.Command.CreateRole;
using CbsAp.Application.Features.Roles.Command.UpdateRole;
using CbsAp.Application.Features.Roles.Queries.Common;
using CbsAp.Application.Features.Roles.Queries.SearchActions;
using CbsAp.Application.Features.Roles.Queries.SearchHandler;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RoleManagementController : BaseAPIController
    {
        private readonly ISender _mediator;

        public RoleManagementController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListofRoles()
        {
            var query = new GetAllRolesQuery();
            var result = await _mediator.Send(query);

            return CreateResponse(result);
        }

        [HttpGet("roles/role-managers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetListofRoleManager([FromQuery] GetRoleManagerQuery param)
        {
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpGet("roles/search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetActiveRoles([FromQuery] GetActiveRolesQuery param)
        {
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpGet("roles/{roleID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRoleByID(long roleID)
        {
            var query = new SearchRoleByIdQuery(roleID);
            var result = await _mediator.Send(query);

            return CreateResponse(result);
        }

        [HttpGet("roles/reminder/{roleID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReminderByRoleID(long roleID)
        {
            var query = new GetRoleReminderByIdQuery(roleID);
            var result =
                await _mediator.Send(query);

            return CreateResponse(result);
        }

        [HttpPost("roles")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO roleDTO)
        {
            var command =
                new CreateRoleCommand(roleDTO, this.CurrentUser);
            var result = await _mediator.Send(command);

            return CreateResponse(result);
        }

        [HttpPut("roles")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDTO roleDTO)
        {
            var command =
                new UpdateRoleCommand(roleDTO, this.CurrentUser);
            var result = await _mediator.Send(command);

            return CreateResponse(result);
        }

        [HttpGet("roles/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchRolesPagination(
            [FromQuery] SearchRolePaginationParamQuery searchRolePaginationParam)
        {
            var result =
                await _mediator.Send(searchRolePaginationParam);

            return CreateResponse(result);
        }

        [HttpGet("roles/active/{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRoleByID(string userName)
        {
            var query = new GetRoleByUserNameQuery(userName);
            var result = await _mediator.Send(query);

            return CreateResponse(result);
        }

        [HttpGet("roles/{roleId}/permissions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRolePermissionsByRoleID(int roleId)
        {
            var query = new GetRolePermissionsByRoleId(roleId);
            var result = await _mediator.Send(query);

            return CreateResponse(result);
        }

        [HttpGet("roles/download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DownloadRoles
           ([FromQuery] ExportRolesQuery exportRoles)
        {
            var result = await _mediator.Send(exportRoles);

            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"Roles_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
    }
}