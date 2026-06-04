using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.Validate
{
    public record ValidateCommand(InvoiceDto invoiceDto, string UpdatedBy)
        : ICommand<ResponseResult<InvValidationResponseDto>>
    {
    }
}