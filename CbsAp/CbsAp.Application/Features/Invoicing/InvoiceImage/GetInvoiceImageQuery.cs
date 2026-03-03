using CbsAp.Application.Abstractions.Messaging;

namespace CbsAp.Application.Features.Invoicing.InvoiceImage
{
    public record GetInvoiceImageQuery(long InvoiceID) : IQuery<string>
    {
    }
}
