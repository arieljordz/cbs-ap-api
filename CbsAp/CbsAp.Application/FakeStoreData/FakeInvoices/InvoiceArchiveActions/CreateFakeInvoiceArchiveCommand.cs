using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.FakeStoreData.FakeDTO;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.FakeStoreData.FakeInvoices.InvoiceArchiveActions
{
    public record CreateFakeInvoiceArchiveCommand(FakeInvoiceDto Dto, string CreatedBy)
        : ICommand<ResponseResult<int>>
    {
    }
}
