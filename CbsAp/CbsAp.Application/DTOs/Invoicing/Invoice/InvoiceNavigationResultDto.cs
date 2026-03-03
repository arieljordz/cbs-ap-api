namespace CbsAp.Application.DTOs.Invoicing.Invoice
{
    public record InvoiceNavigationResultDto(bool CurrentInvoiceExists, long? InvoiceId);
}
