using FluentValidation;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command.Validate
{
    public class ValidateCommandValidator : AbstractValidator<ValidateCommand>
    {
        public ValidateCommandValidator()
        {
            RuleFor(i => i.invoiceDto.SupplierInfoID)
            .NotEmpty()
           .GreaterThan(0)
           .WithMessage("Supplier Info ID is required");

            RuleFor(i => i.invoiceDto.InvoiceID)
               .NotEmpty()
              .GreaterThan(0)
              .WithMessage("Invoice ID is required");

            RuleFor(i => i.invoiceDto.InvoiceNo)
               .NotEmpty()
              .WithMessage("Invoice No is required");

            RuleFor(i => i.invoiceDto.InvoiceDate)
               .NotNull()
              .WithMessage("Invoice Date is required");

            RuleFor(i => i.invoiceDto.NetAmount)
              .NotNull()
             .WithMessage("Net Amount  is required");

            RuleFor(i => i.invoiceDto.TaxAmount)
            .NotNull()
            .WithMessage("Tax Amount is required");

            RuleFor(i => i.invoiceDto.TotalAmount)
             .NotNull()
            .WithMessage("Total Amount is required");

            RuleFor(i => i.invoiceDto.TaxCodeID)
             .NotNull()
            .WithMessage("Tax Code   is required");
        }
    }
}