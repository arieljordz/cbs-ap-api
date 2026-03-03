using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.NotificationStrategy;
using CbsAp.Application.Abstractions.NotificationStrategy.NotificationContext;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Features.Dashboard.Command.Common;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Dashboard;
using Mapster;

namespace CbsAp.Application.Features.Dashboard.Command
{
    public class CreateNoticeCommandHandler : ICommandHandler<CreateNoticeCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitOfWork;
        private readonly INotificationContext _notificationContext;
        private readonly IUserManagementRepository _userManagementRepository;

        public CreateNoticeCommandHandler(IUnitofWork unitofWork, INotificationContext notificationContext,IUserManagementRepository userManagementRepository)
        {
            _unitOfWork = unitofWork;
            _notificationContext = notificationContext;
            _userManagementRepository = userManagementRepository;
        }

        public async Task<ResponseResult<string>> Handle(CreateNoticeCommand request, CancellationToken cancellationToken)
        {
            var notice = request.Adapt<Notice>();
            await _unitOfWork.GetRepository<Notice>().AddAsync(notice);
            bool success = await _unitOfWork.SaveChanges(cancellationToken);

            if (success && request.SendNotification)
            {
                var users = await _userManagementRepository.GetActiveUserAccounts();
                var recipientsList = users.Select(x=>x.EmailAddress).ToList();
                var recipients =  string.Join<string>(',', recipientsList);
                var emailBindData = new Dictionary<string, string> {
                        { "{heading}" , request.Heading },
                        { "{message}", request.Message}
                    };

                 await _notificationContext.GetNotificationTypeStrategy(NotificationType.NoticeNotification).SendNotificationAsync(recipients, emailBindData);
            }

            return  ResponseResult<string>.Created (notice.Heading,
                       MessageConstants.FormatMessage(MessageConstants.AddSuccess, notice.Heading));
        }
    }
}