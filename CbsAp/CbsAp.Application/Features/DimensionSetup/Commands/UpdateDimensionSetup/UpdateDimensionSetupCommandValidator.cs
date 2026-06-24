using FluentValidation;
using FluentValidation.Validators;

namespace CbsAp.Application.Features.DimensionSetup.Commands.UpdateDimensionSetup
{
    public class UpdateDimensionSetupCommandValidator : AbstractValidator<UpdateDimensionSetupCommand>
    {
        public UpdateDimensionSetupCommandValidator()
        {
            RuleFor(e => e.dimensionSetup.DimensionSetupId)
               .NotEmpty()
               .WithMessage("DimensionSetup ID is required");

            RuleFor(e => e.dimensionSetup.DimensionSetupName)
                .NotEmpty()
                .WithMessage("dzimensionSetup Code is required");

            RuleFor(e => e.dimensionSetup.Required)
             .NotEmpty()
             .WithMessage("Dimension Status is required");

            RuleFor(e => e.dimensionSetup.Show)
            .NotEmpty()
            .WithMessage("Show Status is required");
        }
    }
}