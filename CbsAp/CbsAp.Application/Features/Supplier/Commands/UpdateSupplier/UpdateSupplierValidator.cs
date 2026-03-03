using FluentValidation;

namespace CbsAp.Application.Features.Supplier.Commands.UpdateSupplier
{
    public class UpdateSupplierValidator : AbstractValidator<UpdateSupplierCommand>
    {
        public UpdateSupplierValidator()
        {
            RuleFor(v => v.Supplier.SupplierInfoID)
           .NotEmpty()
           .WithMessage("Supplier Info ID is required");


            RuleFor(v => v.Supplier.SupplierID)
             .NotEmpty()
             .WithMessage("Supplier ID is required");

            RuleFor(v => v.Supplier.SupplierName)
                .NotEmpty()
                .WithMessage("Supplier Name is required");
        }
    }
}
