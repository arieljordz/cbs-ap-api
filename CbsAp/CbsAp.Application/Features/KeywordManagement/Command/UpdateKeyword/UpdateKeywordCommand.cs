using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.KeywordManagement.Command
{
    public record UpdateKeywordCommand(
        long KeywordID,
        string KeywordName,
        long? EntityProfileID,
        long InvoiceRoutingFlowID,
        bool IsActive,
        string LastUpdatedBy
        ) : ICommand<ResponseResult<string>>;
}