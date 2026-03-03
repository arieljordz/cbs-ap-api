using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.SystemSettings;
using CbsAp.Application.Shared.ResultPatten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.SystemSettings.ArchiveInvoice.Queries.GetArchieveInvSettings
{
    public record GetArchieveInvSettingQuery() : IQuery<ResponseResult<ArchiveInvSettingDto>>
    {
    }
}
