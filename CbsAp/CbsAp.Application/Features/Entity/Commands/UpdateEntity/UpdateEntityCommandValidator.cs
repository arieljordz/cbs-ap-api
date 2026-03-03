using FluentValidation;
using FluentValidation.Validators;

namespace CbsAp.Application.Features.Entity.Commands.UpdateEntity
{
    public class UpdateEntityCommandValidator : AbstractValidator<UpdateEntityCommand>
    {
        public UpdateEntityCommandValidator()
        {
            RuleFor(e => e.Entity.EntityProfileID)
               .NotEmpty()
               .WithMessage("Entity ID is required");
            RuleFor(e => e.Entity.EntityName)
               .NotEmpty()
               .WithMessage("Entity Name is required");

            RuleFor(e => e.Entity.EntityCode)
                .NotEmpty()
                .WithMessage("Entity Code is required");

            RuleFor(e => e.Entity.EmailAddress)
             .NotEmpty().WithMessage("Email is required")
             .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("Invalid email format");

            RuleFor(e => e.Entity.TaxID)
                .NotEmpty()
                .WithMessage("Tax ID is required");

            RuleFor(e => e.Entity.DefaultInvoiceDueInDays)
                .NotEmpty()
                .WithMessage("Default Invoice Due Date in Days is required");
        }
    }
}