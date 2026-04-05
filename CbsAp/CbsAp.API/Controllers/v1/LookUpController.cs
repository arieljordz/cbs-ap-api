using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Features.Account.Export;
using CbsAp.Application.Features.DimensionsManagement.Export;
using CbsAp.Application.Features.DimensionsManagement.Queries.Common;
using CbsAp.Application.Features.Invoicing.Accounts.Queries.LookUps;
using CbsAp.Application.Features.Invoicing.Accounts.Queries.SearchLookUp;
using CbsAp.Application.Features.Invoicing.InvRoutingFlows.Queries.Lookups;
using CbsAp.Application.Features.Invoicing.LookUps;
using CbsAp.Application.Features.GoodReceiptsManagement.Export;
using CbsAp.Application.Features.GoodReceiptsManagement.Queries.Common;
using CbsAp.Application.Features.Roles.Queries.GetRolesLookUp;
using CbsAp.Application.Features.Supplier.Queries.GetSupplierLookUp;
using CbsAp.Application.Features.TaxCodesManagement.Queries.Lookups;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    // [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class LookUpController : BaseAPIController
    {
        private readonly ISender _mediator;

        public LookUpController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActiveRolesAsync()
        {
            var query = new GetRolesLookUpQuery();
            var result = await _mediator.Send(query);
            return CreateResponse(result);
        }

        [HttpGet("accounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccountsLookUpAsync()
        {
            var query = new GetAccountsLookUpQuery();
            var result = await _mediator.Send(query);
            return CreateResponse(result);
        }

        [HttpGet("inv-routing-flows")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInvRoutingFlowLookUpAsync()
        {
            var query = new GetRoutingFlowLookUpQuery();
            var result = await _mediator.Send(query);
            return CreateResponse(result);
        }

        [HttpGet("inv-routing-flows/{entityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInvRoutingFlowLookUpByEntityIdAsync(long entityId)
        {
            var query = new GetRoutingFlowLookUpByEntityIdQuery(entityId);
            var result = await _mediator.Send(query);
            return CreateResponse(result);
        }


        [HttpGet("tax-codes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTaxCodesLookupAsync()
        {
            var query = new GetTaxCodesLookupQuery();
            var result = await _mediator.Send(query);
            return CreateResponse(result);
        }

        [HttpGet("suppliers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSuppliersLookupAsync()
        {
            var query = new GetSupplierLookUpQuery();
            var result = await _mediator.Send(query);
            return CreateResponse(result);
        }

        [HttpGet("suppliers/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchSupplierPagination
         ([FromQuery] InvSupplierLookUpQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("routingflow/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchRoutingFlowPagination
          ([FromQuery] GetRoutingFlowLookUpNewQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("accounts/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchAccountsPagination
        ([FromQuery] SearchAccountLookUpQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("dimensions/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchDimensionsPagination
        ([FromQuery] GetDimensionsQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("dimensions/download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DownloadDimensions
          ([FromQuery] ExportDimensionsQuery exportDimensionsQuery)
        {
            var result = await _mediator.Send(exportDimensionsQuery);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"Dimensions_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }

        [HttpGet("accounts/download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DownloadAccounts
          ([FromQuery] ExportAccountsQuery exportAccountsQuery)
        {
            var result = await _mediator.Send(exportAccountsQuery);
            if (!result.IsSuccess)
                return CreateResponse(result);

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"Accounts_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }

        [HttpGet("goods-receipts/paged")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchGoodsReceiptsPagination([FromQuery] GetGoodsReceiptsQuery paramQuery)
        {
            var result = await _mediator.Send(paramQuery);
            return CreateResponse(result);
        }

        [HttpGet("goods-receipts/download")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DownloadGoodsReceipts([FromQuery] ExportGoodsReceiptsQuery exportGoodsReceiptsQuery)
        {
            var result = await _mediator.Send(exportGoodsReceiptsQuery);
            if (!result.IsSuccess)
            {
                return CreateResponse(result);
            }

            return File(result.ResponseData,
                        ReportTypeConstants.excelContentType,
                        $"GoodsReceipts_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
    }
}