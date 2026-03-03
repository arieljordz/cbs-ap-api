using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.SystemSettings.ArchiveInvoice.Command.Update
{
    public class UpdateArchSettingCommandHandler : ICommandHandler<UpdateArchSettingCommand, ResponseResult<bool>>
    {
        private readonly IUnitofWork _unitofWork;

        public UpdateArchSettingCommandHandler(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task<ResponseResult<bool>> Handle(UpdateArchSettingCommand request, CancellationToken cancellationToken)
        {
            var systemVariableRepo = _unitofWork.GetRepository<SystemVariable>();

            var archiveSetting = await systemVariableRepo.Query()
            .FirstOrDefaultAsync(sv => sv.Name == SystemVariableConstants.ARCH_INV_SETTING_NAME);

            if (archiveSetting == null)
                return ResponseResult<bool>.NotFound("Archive Invoice Setting Not Found");

            archiveSetting.Value = request.ArchiveInvSetting.Value;

            archiveSetting.SetAuditFieldsOnUpdate(request.UpdatedBy);

            var isSaved = await _unitofWork.SaveChanges(cancellationToken);
            if (!isSaved)
                return ResponseResult<bool>.BadRequest("Archive Invoice Setting error on saving");
            return ResponseResult<bool>.OK("Archive Invoice Setting successfully updated");
        }
    }
}