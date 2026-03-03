using FluentValidation;

namespace CbsAp.Application.Features.DimensionsManagement.Command.Common
{
    public class CreateDimensionCommandValidator : AbstractValidator<CreateDimensionCommand>
    {
        public CreateDimensionCommandValidator()
        {
            RuleFor(dimension => dimension.EntityProfileID)
                .GreaterThan(0)
                .WithMessage("Entity profile is required.");

            RuleFor(dimension => dimension.Dimension)
                .NotEmpty()
                .WithMessage("Dimension is required.")
                .MaximumLength(15)
                .WithMessage("Dimension must not exceed 15 characters.");

            RuleFor(dimension => dimension.Name)
                .NotEmpty()
                .WithMessage("Dimension name is required.")
                .MaximumLength(50)
                .WithMessage("Dimension name must not exceed 50 characters.");

            RuleFor(dimension => dimension.FreeField1)
                .MaximumLength(90)
                .WithMessage("Free field 1 must not exceed 90 characters.")
                .When(dimension => !string.IsNullOrEmpty(dimension.FreeField1));

            RuleFor(dimension => dimension.FreeField2)
                .MaximumLength(90)
                .WithMessage("Free field 2 must not exceed 90 characters.")
                .When(dimension => !string.IsNullOrEmpty(dimension.FreeField2));

            RuleFor(dimension => dimension.FreeField3)
                .MaximumLength(90)
                .WithMessage("Free field 3 must not exceed 90 characters.")
                .When(dimension => !string.IsNullOrEmpty(dimension.FreeField3));
        }
    }
}
