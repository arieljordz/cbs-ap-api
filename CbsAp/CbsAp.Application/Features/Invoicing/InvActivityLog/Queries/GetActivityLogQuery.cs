using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.ActivityLog;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActivityLog.Queries
{
    public record GetActivityLogQuery(long InvoiceID) : IQuery<ResponseResult<List<ActivityLogEntryDto>>>
    {
    }
}
