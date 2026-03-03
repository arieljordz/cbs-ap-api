using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.SystemSettings;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.SystemSettings.ArchiveInvoice.Command.Update
{
    public record UpdateArchSettingCommand(ArchiveInvSettingDto ArchiveInvSetting, string UpdatedBy)
        : ICommand<ResponseResult<bool>>
    {
    }
}