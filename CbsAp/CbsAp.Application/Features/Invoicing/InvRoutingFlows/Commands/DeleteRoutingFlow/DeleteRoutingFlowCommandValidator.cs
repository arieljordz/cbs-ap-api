using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Invoicing.InvRoutingFlows.Commands.DeleteRoutingFlow
{
    public class DeleteRoutingFlowCommandValidator : AbstractValidator<DeleteRoutingFlowCommand>
    {
        public DeleteRoutingFlowCommandValidator()
        {
            RuleFor(v => v.InvRoutingFlowID )
          .NotEmpty()
          .WithMessage("Invoice Routing flow ID is required");
        }
    }
}
