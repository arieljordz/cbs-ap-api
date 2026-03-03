using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;


namespace CbsAp.Application.Features.Invoicing.InvAllocationLine.Queries
{
    public record GetInvAllocByInvIDQuery(long InvoiceID) : IQuery<ResponseResult<List<InvAllocEntryDto>>>
    {
    }
}
