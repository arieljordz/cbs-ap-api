using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.Authentication.Queries.IsPasswordResetLinkValid
{
    public record IsPasswordResetLinkValidQuery(
        string Token
        )
        : IQuery<ResponseResult<bool>>
    {
    }
}
