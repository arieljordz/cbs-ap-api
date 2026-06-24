

using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.AutoMatching
{
    public record UpdatePOMatchStatusCommand(List<long> PurchaseOrderIDs) : ICommand<ResponseResult<bool>>
    {
    }
}
