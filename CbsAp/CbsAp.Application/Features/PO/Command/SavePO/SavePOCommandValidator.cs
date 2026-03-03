using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.PO.Command.SavePO
{
    public class SavePOCommandValidator :AbstractValidator<SavePOCommand>
    {
        public SavePOCommandValidator()
        {
            RuleFor(i => i.SavePOMatchingDto.InvoiceID)
                 .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Invoice ID is required");
        }
    }
}
