using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Dashboard.Command.Common
{
    public record CreateNoticeCommand(
        string Heading,
        string Message,
        bool SendNotification,
        string CreatedBy
        ) :
        ICommand<ResponseResult<string>>;
}