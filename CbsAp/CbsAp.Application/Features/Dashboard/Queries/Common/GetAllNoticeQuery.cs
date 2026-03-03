using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Dashboard;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Dashboard.Queries.Common
{
    public record GetAllNoticeQuery() : IQuery<ResponseResult<IEnumerable<NoticeDTO>>>;
}