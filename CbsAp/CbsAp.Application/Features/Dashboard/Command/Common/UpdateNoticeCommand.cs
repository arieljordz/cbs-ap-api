using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Dashboard.Command.Common
{
    public record  UpdateNoticeCommand(
        long NoticeID, 
        string Heading, 
        string Message,
        bool SendNotification,
        string LastUpdatedBy
        ) : ICommand<ResponseResult<string>>;
}