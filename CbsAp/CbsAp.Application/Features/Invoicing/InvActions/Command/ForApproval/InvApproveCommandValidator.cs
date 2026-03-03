using CbsAp.Application.DTOs.Invoicing.Invoice;
using FluentValidation;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.ForApproval
{
    public class InvApproveCommandValidator : AbstractValidator<InvApproveCommand>
    {
        public InvApproveCommandValidator()
        {
            RuleFor(i => i.invoiceDto.InvoiceID)
                .NotEmpty()
                .WithMessage("Invoice ID is required");
        }
    }
}