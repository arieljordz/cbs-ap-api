using CbsAp.Application.Features.Invoicing.InvActions.Command.Reactivate;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.ForForceToSubmit
{
    public class ForceToSubmitCommandValidator : AbstractValidator<ForceToSubmitCommand>
    {
        public ForceToSubmitCommandValidator()
        {
            RuleFor(i => i.dto)
               .NotNull()
               .WithMessage("Request payload is required");

            RuleFor(i => i.dto.InvoiceID)
                .NotEmpty()
                .WithMessage("Invoice ID is required");

        }
    }
}
