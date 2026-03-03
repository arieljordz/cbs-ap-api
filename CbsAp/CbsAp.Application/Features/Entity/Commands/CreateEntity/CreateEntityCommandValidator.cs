using FluentValidation;
using FluentValidation.Validators;

namespace CbsAp.Application.Features.Entity.Commands.CreateEntity
{
    public class CreateEntityCommandValidator : AbstractValidator<CreateEntityCommand>
    {
        public CreateEntityCommandValidator()
        {
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
