using System.Text.Json;
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared.ResultPatten;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CbsAp.Application.Features.Invoicing.InvActions.Queries.GetAdjacentInvoice
{
    public class GetAdjacentInvoiceIdQueryHandler : IQueryHandler<GetAdjacentInvoiceIdQuery, ResponseResult<long?>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public GetAdjacentInvoiceIdQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResponseResult<long?>> Handle(GetAdjacentInvoiceIdQuery request, CancellationToken cancellationToken)
        {
            var filter = request.gridFilter == null ? null : JsonSerializer.Deserialize<InvoiceSearchBaseDto>(request.gridFilter, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var page = request.gridRowDetails == null ? null : JsonSerializer.Deserialize<PageDetailsDto>(request.gridRowDetails, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var navigationResult = await _invoiceRepository.GetAdjacentInvoiceId(
                request.InvoiceID,
                request.IsNext,
                request.StatusType,
                request.QueueType,
                filter,
                page,
                cancellationToken);

            if (!navigationResult.CurrentInvoiceExists)
            {
                return ResponseResult<long?>.NotFound("Invoice not found.");
            }

            return ResponseResult<long?>.OK(navigationResult.InvoiceId);
        }
    }
}
