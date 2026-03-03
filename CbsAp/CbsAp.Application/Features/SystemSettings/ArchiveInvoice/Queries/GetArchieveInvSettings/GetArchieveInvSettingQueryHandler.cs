using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.SystemSettings;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Application.Features.SystemSettings.ArchiveInvoice.Queries.GetArchieveInvSettings
{
    public class GetArchieveInvSettingQueryHandler : IQueryHandler<GetArchieveInvSettingQuery, ResponseResult<ArchiveInvSettingDto>>
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ISystemVariableRepository _systemVariableRepository;

        public GetArchieveInvSettingQueryHandler(IUnitofWork unitofWork, ISystemVariableRepository systemVariableRepository)
        {
            _unitofWork = unitofWork;
            _systemVariableRepository = systemVariableRepository;
        }
        public async Task<ResponseResult<ArchiveInvSettingDto>> Handle(GetArchieveInvSettingQuery request, CancellationToken cancellationToken)
        {

            var archiveSetting = await _systemVariableRepository.Query()
            .AsNoTracking()
            .Where(sv => sv.Name == SystemVariableConstants.ARCH_INV_SETTING_NAME)
            .Select(sv => new ArchiveInvSettingDto
            {

                SystemVariableID = sv.SystemVariableID,
                Name = sv.Name,
                Description = sv.Description,
                Value = sv.Value,

            }).FirstOrDefaultAsync();

            if (archiveSetting == null)
                return ResponseResult<ArchiveInvSettingDto>.NotFound("Archive Invoice Setting Not Found");

            return ResponseResult<ArchiveInvSettingDto>.OK(archiveSetting);
        }
    }
}
