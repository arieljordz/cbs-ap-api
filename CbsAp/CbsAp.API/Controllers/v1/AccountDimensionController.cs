using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.PermissionManagement;
using CbsAp.Application.Features.AccountDimension.Queries.SearchActions;
using CbsAp.Application.Features.PermissionManagement.Queries.SearchActions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AccountDimensionController : BaseAPIController
    {
        private readonly IMediator _mediator;

        public AccountDimensionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("account-dimensions/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchAccountDimensionPagination
            ([FromQuery] SearchAccountDimensionParamQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }


    }
}