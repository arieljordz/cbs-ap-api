using CbsAp.Application.Features.Invoicing.InvActions.Command.ForReject;
using FluentValidation;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.Reactivate
{
    public class InvoiceReactivateCommandValidator : AbstractValidator<InvoiceReactivateCommand>
    {
        public InvoiceReactivateCommandValidator()
        {
            RuleFor(i => i.dto)
                .NotNull()
                .WithMessage("Request payload is required");

            RuleFor(i => i.dto.InvoiceID)
                .NotEmpty()
                .WithMessage("Invoice ID is required");

            RuleFor(i => i.dto.Reason)
                .NotEmpty()
                .WithMessage("Reason is required when reactivating an invoice");
        }
    }
}
