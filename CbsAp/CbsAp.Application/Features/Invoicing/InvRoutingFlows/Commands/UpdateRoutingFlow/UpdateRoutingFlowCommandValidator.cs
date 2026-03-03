using FluentValidation;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Commands.UpdateRoutingFlow
{
    public class UpdateRoutingFlowCommandValidator : AbstractValidator<UpdateRoutingFlowCommand>
    {
        public UpdateRoutingFlowCommandValidator()
        {
            RuleFor(x => x.InvRoutingFlowDto.InvRoutingFlowID)
               .NotEmpty()
               .GreaterThan(0)
               .WithMessage("Invoice Routing Flow ID is required");

            RuleFor(x => x.InvRoutingFlowDto.SupplierInfoID)
             .NotEmpty()
             .WithMessage("Linked Supplier is required");

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