using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.FakeStoreData.ValidateRules
{
    public record FakeSubmitCommand(long InvoiceID, string UpdateBy)
        : ICommand<ResponseResult<bool>>
    {
    }
}