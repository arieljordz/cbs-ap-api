
using FluentValidation;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.RouteToException
{
    public class InvoiceRouteToExceptionCommandValidator : AbstractValidator<InvoiceRouteToExceptionCommand>
    {
        public InvoiceRouteToExceptionCommandValidator()
        {
            RuleFor(i => i.dto.InvoiceID)
                .NotEmpty()
                .WithMessage("Invoice ID is required"); 
        }
    }
}
