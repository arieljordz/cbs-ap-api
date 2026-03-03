using FluentValidation;

namespace CbsAp.Application.Features.Dashboard.Command.Common
{
    public class UpdateNoticeCommandValidator : AbstractValidator<UpdateNoticeCommand>
    {
        public UpdateNoticeCommandValidator()
        {
            RuleFor(notice => notice.NoticeID)
              .NotEmpty()
              .WithMessage("NoticeID should not be zero");

            RuleFor(notice => notice.Heading)
               .NotEmpty()
               .WithMessage("Heading is required");

            RuleFor(notice => notice.Message)
                .NotEmpty()
                .WithMessage("Message is required");
        }
    }
}