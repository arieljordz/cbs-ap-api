using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.FakeStoreData.FakeDTO;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.FakeStoreData.FakeInvoices.InvoiceActions
{
    public record CreateFakeInvoiceCommand(FakeInvoiceDto Dto, string CreatedBy)
        : ICommand<ResponseResult<int>>
    {
    }
}