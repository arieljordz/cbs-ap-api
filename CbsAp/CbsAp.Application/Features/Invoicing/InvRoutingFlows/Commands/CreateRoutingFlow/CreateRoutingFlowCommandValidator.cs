using FluentValidation;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Commands.CreateRoutingFlow
{
    public class CreateRoutingFlowCommandValidator : AbstractValidator<CreateRoutingFlowCommand>
    {
        public CreateRoutingFlowCommandValidator()
        {
            RuleFor(x => x.InvRoutingFlowDto.IsActive)
               .NotEmpty()
               .WithMessage("Active is required");

            RuleFor(x => x.InvRoutingFlowDto.InvRoutingFlowName)
                .NotEmpty()
                .WithMessage("Routing Name is required");

            RuleFor(x => x.InvRoutingFlowDto.MatchReference)
               .NotEmpty()
               .WithMessage("Match Reference is required");
        }
    }
}