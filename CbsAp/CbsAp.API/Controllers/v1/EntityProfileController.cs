using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Features.Entity.Commands.CreateEntity;
using CbsAp.Application.Features.Entity.Commands.DeleteEntity;
using CbsAp.Application.Features.Entity.Commands.UpdateEntity;
using CbsAp.Application.Features.Entity.Queries.GetEntityByID;
using CbsAp.Application.Features.Entity.Queries.Pagination;
using CbsAp.Application.Features.Entity.Queries.Reports;
using CbsAp.Application.Features.EntityProfileManagement.Queries.EntityProfileSearchActions;
using CbsAp.Application.Features.EntityProfileManagement.Queries.GetAllEntities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    /// <summary>
    /// Defines the <see cref="EntityProfileController" />
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class EntityProfileController : BaseAPIController
    {
        private readonly ISender _mediator;

        public EntityProfileController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Return All  list of  Entity Profile
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEntityProfileAsync()
        {
            var query = new GetAllEntityQuery();
            var result =
                await _mediator.Send(query);

            return result.IsSuccess
               ? Ok(result)
               : BadRequest(result.Messages);
        }

        /// <summary>
        /// Return collections of Entity Profile By Role
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>

        [HttpGet("{roleID}/entity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEntityProfileByRoleAsync(long roleID)
        {
            var query = new EntityProfileByRoleQuery(roleID);
            var result =
                await _mediator.Send(query);

            return CreateResponse(result);
        }

        /// <summary>
        /// Return all available entity profiles
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>

        [HttpGet("{roleID}/available-entities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEntityProfileNotInRoleAsync(long roleID)
        {
            var query = new EntityProfileNotInRoleQuery(roleID);
            var result =
                await _mediator.Send(query);

            return CreateResponse(result);
        }

        [HttpPost("entity")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateEntity([FromBody] EntityDto entityDTO)
        {
            var entityCreateCommand =
               new CreateEntityCommand(entityDTO, this.CurrentUser);
            var result = await _mediator.Send(entityCreateCommand);

            return CreateResponse(result);
        }

        [HttpPut("entity")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateEntity([FromBody] EntityDto entityDTO)
        {
            var entityCreateCommand =
               new UpdateEntityCommand(entityDTO, this.CurrentUser);
            var result = await _mediator.Send(entityCreateCommand);

            return CreateResponse(result);
        }

        [HttpGet("{entityProfileID}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEntityByID(long entityProfileID)
        {
            var param = new GetEntityByIDQuery(entityProfileID);
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpGet("entity/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchPermissionPagination
            ([FromQuery] SearchEntityQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("entity/download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DownloadEntity
           ([FromQuery] ExportEntityQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"Entity_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }

        [HttpDelete("{entityProfileID}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteEntity(long entityProfileID)
        {
            var userCommand = new DeleteEntityCommand(entityProfileID);
            var result = await _mediator.Send(userCommand);

            return CreateResponse(result);
        }
    }
}