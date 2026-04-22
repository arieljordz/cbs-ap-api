using FluentValidation;



namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Commands.RemoveRoleRoutingFlow
{
    public class RemoveRoleRoutingFlowValidator : AbstractValidator<RemoveRoleRoutingFlowCommand>
    {
        public RemoveRoleRoutingFlowValidator()
        {
            RuleFor(role => role.RoleRoutingFlowDTO.RoleID)
            .NotEmpty()
            .WithMessage("RoleId is required");
            RuleFor(role => role.RoleRoutingFlowDTO.InvoiceID)
            .NotEmpty()
            .WithMessage("InvoiceId is required");
        }
    }
}