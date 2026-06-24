using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.DimensionSetup;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Features.DimensionSetup.Commands.CreateDimensionSetup;
using CbsAp.Application.Features.DimensionSetup.Commands.DeleteDimensionSetup;
using CbsAp.Application.Features.DimensionSetup.Commands.UpdateDimensionSetup;
using CbsAp.Application.Features.DimensionSetup.Queries.GetAllEntities;
using CbsAp.Application.Features.DimensionSetup.Queries.GetDimensionSetupByID;
using CbsAp.Application.Features.Entity.Commands.CreateEntity;
using CbsAp.Application.Features.Entity.Commands.DeleteEntity;
using CbsAp.Application.Features.Entity.Commands.UpdateEntity;
using CbsAp.Application.Features.Entity.Queries.GetEntityByID;
using CbsAp.Application.Features.Entity.Queries.GetEntityByRoleID;
using CbsAp.Application.Features.Entity.Queries.Pagination;
using CbsAp.Application.Features.Entity.Queries.Reports;
using CbsAp.Domain.Entities.DimensionSetup;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    /// <summary>
    /// Defines the <see cref="DimensionSetupController" />
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class DimensionSetupController : BaseAPIController
    {
        private readonly ISender _mediator;

        public DimensionSetupController(ISender mediator)
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
        public async Task<IActionResult> GetDimensionSetupAsync()
        {
            var query = new GetAllDimensionSetupQuery();
            var result =
                await _mediator.Send(query);

            return result.IsSuccess
               ? Ok(result)
               : BadRequest(result.Messages);
        }

        [HttpPost("dimensionSetup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateDimensionSetup([FromBody] DimensionSetupDto dimensionSetupDto)
        {
            var dimensionSetupCreateCommand =
               new CreateDimensionSetupCommand(dimensionSetupDto, this.CurrentUser);
            var result = await _mediator.Send(dimensionSetupCreateCommand);

            return CreateResponse(result);
        }

        [HttpPut("DimensionSetup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateDimensionSetup([FromBody] DimensionSetupDto dimensionSetupDTO)
        {
            var entityCreateCommand =
               new UpdateDimensionSetupCommand(dimensionSetupDTO, this.CurrentUser);
            var result = await _mediator.Send(entityCreateCommand);

            return CreateResponse(result);
        }

        [HttpGet("{DimensionSetupID}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDimensionSetupByID(long dimensionSetupId)
        {
            var param = new GetDimensionSetupByIDQuery(dimensionSetupId);
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpGet("dimensionSetup/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchDimensionSetupPagination
            ([FromQuery] SearchDimensionSetupQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpDelete("{dimensionSetupId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteDimensionSetup(long DimensionSetupID)
        {
            var userCommand = new DeleteDimensionSetupCommand(DimensionSetupID);
            var result = await _mediator.Send(userCommand);

            return CreateResponse(result);
        }
    }
}