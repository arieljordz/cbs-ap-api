using FluentValidation;



namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Commands.AssignRoleRoutingFlow
{
    public class AssignRoleRoutingFlowValidator : AbstractValidator<AssignRoleRoutingFlowCommand>
    {
        public AssignRoleRoutingFlowValidator()
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