using FluentValidation;

namespace CbsAp.Application.Features.Dashboard.Command.Common
{
    public class CreateNoticeCommandValidator : AbstractValidator<CreateNoticeCommand>
    {
        public CreateNoticeCommandValidator()
        {
            RuleFor(notice => notice.Heading)
               .NotEmpty()
               .WithMessage("Heading is required");

            RuleFor(notice => notice.Message)
                .NotEmpty()
                .WithMessage("Message is required");
        }
    }
}