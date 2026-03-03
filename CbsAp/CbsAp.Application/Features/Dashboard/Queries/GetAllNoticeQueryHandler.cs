using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Dashboard;
using CbsAp.Application.Features.Dashboard.Queries.Common;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using Mapster;

namespace CbsAp.Application.Features.Dashboard.Queries
{
    public class GetAllNoticeQueryHandler : IQueryHandler<GetAllNoticeQuery, ResponseResult<IEnumerable<NoticeDTO>>>
    {
        private readonly INoticeRepository _noticeRepository;

        public GetAllNoticeQueryHandler(INoticeRepository noticeRepository)
        {
            _noticeRepository = noticeRepository;
        }

        public async Task<ResponseResult<IEnumerable<NoticeDTO>>> Handle(GetAllNoticeQuery request, CancellationToken cancellationToken)
        {
            //var roles = await _roleManagementRepository.GetAllRolesSearchAsync();
            var notices = await _noticeRepository.GetAllNotice();
            var noticesDto = notices?.Adapt<IEnumerable<NoticeDTO>>();

            return noticesDto == null ?
               ResponseResult<IEnumerable<NoticeDTO>>.NotFound(MessageConstants.FormatMessage(MessageConstants.GetNotFound))
            : ResponseResult<IEnumerable<NoticeDTO>>.Success(noticesDto);
        }
    }
}