using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.InvInfoRoutingLevel;
using CbsAp.Application.Shared.ResultPatten;


namespace CbsAp.Application.Features.Invoicing.InvInfoRouting.Queries
{
    public record GetRoutingFlowLinkedLevelQuery(long InvoiceID, long? SupplierInfoID, long? KeywordID) 
        : IQuery<ResponseResult<List<InvInfoRoutingLevelDto>>>
    {
    }
}
