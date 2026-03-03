using FluentValidation;

namespace CbsAp.Application.Features.Supplier.Commands.DeleteSupplier
{
    public class DeleteSupplierCommandValidator : AbstractValidator<DeleteSupplierCommand>
    {
        public DeleteSupplierCommandValidator()
        {

            RuleFor(v => v.SupplierInfoID)
          .NotEmpty()
          .WithMessage("Supplier Info ID is required");
        }
    }
}
 