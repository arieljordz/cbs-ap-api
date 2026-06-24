using FluentValidation;

namespace CbsAp.Application.Features.DimensionSetup.Commands.DeleteDimensionSetup
{
    public class DeleteDimensionSetupCommandValidator : AbstractValidator<DeleteDimensionSetupCommand>
    {
        public DeleteDimensionSetupCommandValidator()
        {
            RuleFor(e => e.dimensionSetupId)
              .NotEmpty()
              .WithMessage("DimensionSetup ID is required");
        }
    }
}