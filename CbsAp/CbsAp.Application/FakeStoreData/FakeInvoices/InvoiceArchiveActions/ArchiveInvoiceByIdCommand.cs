using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.FakeStoreData.FakeInvoices.InvoiceArchiveActions
{
    public record ArchiveInvoiceByIdCommand(long InvoiceId, string ArchivedBy)
        : ICommand<ResponseResult<int>>
    {
    }
}
