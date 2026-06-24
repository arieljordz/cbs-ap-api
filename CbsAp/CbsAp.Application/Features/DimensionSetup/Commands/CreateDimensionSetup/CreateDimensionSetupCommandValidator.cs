using FluentValidation;
using FluentValidation.Validators;

namespace CbsAp.Application.Features.DimensionSetup.Commands.CreateDimensionSetup
{
    public class CreateDimensionSetupCommandValidator : AbstractValidator<CreateDimensionSetupCommand>
    {
        public CreateDimensionSetupCommandValidator()
        {
            RuleFor(e => e.dimensionSetup.DimensionSetupId)
             .NotEmpty()
             .WithMessage("DimensionSetup ID is required");
            RuleFor(e => e.dimensionSetup.DimensionSetupName)
               .NotEmpty()
               .WithMessage("DimensionSetup Name is required");

            RuleFor(e => e.dimensionSetup.DimensionSetupName)
                .NotEmpty()
                .WithMessage("DimensionSetup Code is required");

            RuleFor(e => e.dimensionSetup.DimensionName)
                .NotEmpty()
                .WithMessage("Dimension Name is required");


            RuleFor(e => e.dimensionSetup.Required)
                .NotEmpty()
                .WithMessage("Dimension Status is required");

            RuleFor(e => e.dimensionSetup.Show)
                .NotEmpty()
                .WithMessage("Show Status is required");

        }
    }
}
