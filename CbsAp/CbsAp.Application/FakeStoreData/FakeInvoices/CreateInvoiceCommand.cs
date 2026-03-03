using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.FakeStoreData.FakeInvoices
{
    public record CreateInvoiceCommand(int requestCount) : ICommand<ResponseResult<int>>
    {
    }
}