using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Queries.Reports
{
    public record InvRoutingFlowExportQuery(
        string? EntityName,
        string? InvRoutingFlowName,
        string? LinkSupplier,
        string? Roles,
        string? MatchReference) :
        IQuery<ResponseResult<byte[]>>

    {
    }
}