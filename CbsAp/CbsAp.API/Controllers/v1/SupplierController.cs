using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Features.Supplier.Commands.CreateSupplier;
using CbsAp.Application.Features.Supplier.Commands.DeleteSupplier;
using CbsAp.Application.Features.Supplier.Commands.UpdateSupplier;
using CbsAp.Application.Features.Supplier.Queries.GetSupplierInfoByID;
using CbsAp.Application.Features.Supplier.Queries.Pagination;
using CbsAp.Application.Features.Supplier.Queries.Reports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SupplierController : BaseAPIController
    {
        private readonly ISender _mediator;

        public SupplierController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierInfoDto supplierInfoDto)
        {
            var supplierCommand =
                new CreateSupplierCommand(supplierInfoDto, this.CurrentUser);
            var result = await _mediator.Send(supplierCommand);
            return CreateResponse(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateSupplier([FromBody] SupplierInfoDto supplier)
        {
            var supplierCommand =
              new UpdateSupplierCommand(supplier, this.CurrentUser);
            var result = await _mediator.Send(supplierCommand);
            return CreateResponse(result);
        }

        [HttpGet("{supplierInfoID}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEntityByID(long supplierInfoID)
        {
            var param = new GetSupplierInfoByIDQuery(supplierInfoID);
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }

        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchSupplierPagination
           ([FromQuery] SearchSupplierQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DownloadSupplier
          ([FromQuery] SupplierExportQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"Supplier_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }

        [HttpDelete("{supplierInfoID}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteSupplier(long supplierInfoID)
        {
            var userCommand = new DeleteSupplierCommand(supplierInfoID);
            var result = await _mediator.Send(userCommand);

            return CreateResponse(result);
        }
    }
}