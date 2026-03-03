using FluentValidation;

namespace CbsAp.Application.Features.Invoicing.InvActions.Command
{
    public class UpdateInvoiceCommandValidator : AbstractValidator<UpdateInvoiceCommand>
    {
        public UpdateInvoiceCommandValidator()
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
               .NotEmpty()
              .WithMessage("Invoice Date is required");

            RuleFor(i => i.invoiceDto.NetAmount)
              .NotEmpty()
             .WithMessage("Net Amount  is required");

            RuleFor(i => i.invoiceDto.TaxAmount)
             .NotEmpty()
            .WithMessage("Tax Amount is required");

            RuleFor(i => i.invoiceDto.TotalAmount)
             .NotEmpty()
            .WithMessage("Total Amount is required");

            RuleFor(i => i.invoiceDto.TaxCodeID)
             .NotEmpty()
            .WithMessage("Tax Code   is required");
        }
    }
}