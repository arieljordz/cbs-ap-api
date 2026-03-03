using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.Features.PermissionManagement.Commands.CreatePermission;
using CbsAp.Application.Features.PermissionManagement.Commands.UpdatePermission;
using CbsAp.Application.Features.PermissionManagement.Queries.GetAllOperations;
using CbsAp.Application.Features.PermissionManagement.Queries.RolePermissionActions;
using CbsAp.Application.Features.PermissionManagement.Queries.SearchActions;
using CbsAp.Application.Features.PermissionManagement.Reports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PermissionManagementController : BaseAPIController
    {
        private readonly IMediator _mediator;

        public PermissionManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("operations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllOperations()
        {
            var query = new GetAllOperationQuery();
            var result = await _mediator.Send(query);

            return CreateResponse(result);
        }

        [Authorize]
        [HttpGet("permission/{permissionID}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPermissionByID(long permissionID)
        {
            var paramQuery = new SearchPermissionByIDQuery(permissionID);
            var result = await _mediator.Send(paramQuery);

            return CreateResponse(result);
        }

        [Authorize]
        [HttpGet("permission/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchPermissionPagination
            ([FromQuery] SearchPermissionParamQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [Authorize]
        [HttpGet("permission/download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DownloadPermissions
           ([FromQuery] ExportPermissionQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"PermissionGroup_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }

        [Authorize]
        [HttpGet("permission/active-permissions")]
        public async Task<IActionResult> GetAllPermissions()
        {
            var query = new GetPermissionQuery();
            var result = await _mediator.Send(query);
            return CreateResponse(result);
        }

        [Authorize]
        [HttpPost("permission")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionDto permissionDTO)
        {
            var permissionCommand = new PermissionCommand(permissionDTO, this.CurrentUser);
            var result = await _mediator.Send(permissionCommand);

            return CreateResponse(result);
        }

        [Authorize]
        [HttpPut("permission")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePermission([FromBody] UpdatePermissionDTO permissionDTO)
        {
            var permissionCommand = new UpdatePermissionCommand(permissionDTO, this.CurrentUser);
            var result = await _mediator.Send(permissionCommand);
            return CreateResponse(result);
        }

        [Authorize]
        [HttpGet("permission/{roleID}/role-permissions")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPermissionByRole(long roleID)
        {
            var paramQuery = new GetPermissionByRoleQuery(roleID);
            var result =
                await _mediator.Send(paramQuery);

            return CreateResponse(result);
        }

        [Authorize]
        [HttpGet("permission/{roleID}/available-permissions")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPermissionNotInRole(long roleID)
        {
            var paramQuery = new GetPermissionNotInRoleQuery(roleID);
            var result =
                await _mediator.Send(paramQuery);

            return CreateResponse(result);
        }
    }
}