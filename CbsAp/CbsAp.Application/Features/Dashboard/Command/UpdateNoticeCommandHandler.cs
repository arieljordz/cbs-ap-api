using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.NotificationStrategy.NotificationContext;
using CbsAp.Application.Abstractions.NotificationStrategy;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using Mapster;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Application.Features.Dashboard.Command.Common;
using CbsAp.Domain.Entities.Dashboard;
using CbsAp.Application.Shared.Extensions;

namespace CbsAp.Application.Features.Dashboard.Command
{
    public class UpdateNoticeCommandHandler : ICommandHandler<UpdateNoticeCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly IUserManagementRepository _userManagementRepository;
        private readonly INotificationContext _notificationContext;

        public UpdateNoticeCommandHandler(IUnitofWork unitofWork, IUserManagementRepository userManagementRepository,INotificationContext notificationContext)
        {
            _unitOfWork = unitofWork;
            _userManagementRepository = userManagementRepository;
            _notificationContext = notificationContext;
        }

        public async Task<ResponseResult<string>> Handle(UpdateNoticeCommand request, CancellationToken cancellationToken)
        {
            var noticeRepository = _unitOfWork.GetRepository<Notice>();

            var noticeQuery = await noticeRepository.ApplyPredicateAsync(x => x.NoticeID == request.NoticeID);
            var noticeEntity = noticeQuery.FirstOrDefault();
            if (noticeEntity == null)
            {
                return ResponseResult<string>.NotFound("Notice not found");
            }

            var updatedNotice = request.Adapt<Notice>();
            updatedNotice.CreatedBy = noticeEntity?.CreatedBy;
            updatedNotice.CreatedDate = noticeEntity?.CreatedDate; 
            updatedNotice.SetAuditFieldsOnUpdate(request.LastUpdatedBy);
            await noticeRepository.UpdateAsync(updatedNotice.NoticeID,updatedNotice);
            bool success = await _unitOfWork.SaveChanges(string.Empty,string.Empty,cancellationToken);

            if (success && request.SendNotification)
            {
                var users = await _userManagementRepository.GetActiveUserAccounts();
                var recipientsList = users.Select(x => x.EmailAddress).ToList();
                var recipients = string.Join<string>(',', recipientsList);
                var emailBindData = new Dictionary<string, string> {
                    { "{heading}" , request.Heading },
                    { "{message}", request.Message}
                };

                await _notificationContext.GetNotificationTypeStrategy(NotificationType.NoticeNotification).SendNotificationAsync(recipients, emailBindData);
            }

            return  ResponseResult<string>.Success(MessageConstants.FormatMessage(MessageConstants.UpdateSuccess, updatedNotice?.Heading));
        }
    }
}