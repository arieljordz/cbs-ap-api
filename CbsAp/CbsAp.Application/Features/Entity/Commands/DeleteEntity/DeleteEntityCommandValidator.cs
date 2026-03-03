using FluentValidation;

namespace CbsAp.Application.Features.Entity.Commands.DeleteEntity
{
    public class DeleteEntityCommandValidator : AbstractValidator<DeleteEntityCommand>
    {
        public DeleteEntityCommandValidator()
        {
            RuleFor(e => e.EntityProfileID)
              .NotEmpty()
              .WithMessage("Entity ID is required");
        }
    }
}