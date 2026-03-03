using FluentValidation;

namespace CbsAp.Application.Features.Supplier.Commands.CreateSupplier
{
    public class CreateSupplierValidator : AbstractValidator<CreateSupplierCommand>
    {
        public CreateSupplierValidator()
        {
            RuleFor(v => v.Supplier.SupplierID)
               .NotEmpty()
               .WithMessage("Supplier ID is required");

            RuleFor(v => v.Supplier.SupplierName)
                .NotEmpty()
                .WithMessage("Supplier Name is required");
        }
    }
}
