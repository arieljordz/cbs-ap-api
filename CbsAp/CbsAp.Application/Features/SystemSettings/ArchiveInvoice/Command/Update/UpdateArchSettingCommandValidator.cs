using FluentValidation;

namespace CbsAp.Application.Features.SystemSettings.ArchiveInvoice.Command.Update
{
    public class UpdateArchSettingCommandValidator : AbstractValidator<UpdateArchSettingCommand>
    {
        public UpdateArchSettingCommandValidator()
        {
            RuleFor(v => v.ArchiveInvSetting.Name)
               .NotEmpty()
               .WithMessage("Name is required");

            RuleFor(v => v.ArchiveInvSetting.Value)
              .NotEmpty()
              .WithMessage("Value is required");

            RuleFor(v => v.ArchiveInvSetting.Description)
              .NotEmpty()
              .WithMessage("Value is required");
        }
    }
}
