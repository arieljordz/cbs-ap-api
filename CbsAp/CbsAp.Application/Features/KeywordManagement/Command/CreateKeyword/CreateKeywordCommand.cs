using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.KeywordManagement.Command
{
    public record CreateKeywordCommand(
        string KeywordName,
        long? EntityProfileID,
        long InvoiceRoutingFlowID,
        bool IsActive,
        string CreatedBy
        ) :
        ICommand<ResponseResult<string>>;
}