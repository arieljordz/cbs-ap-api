using FluentValidation;

namespace CbsAp.Application.Features.PO.Command.UpdatePO
{
    public class UpdatePOCommandValidator : AbstractValidator<UpdatePOCommand>
    {
        public UpdatePOCommandValidator()
        {
            RuleFor(i => i.SavePOMatchingDto.InvoiceID)
                 .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Invoice ID is required");
        }
    }
}
