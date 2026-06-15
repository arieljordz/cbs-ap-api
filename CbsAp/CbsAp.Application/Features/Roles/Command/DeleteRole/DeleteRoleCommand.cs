using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Roles.Command.DeleteRole
{
    public record DeleteRoleCommand(long roleID) : ICommand<ResponseResult<bool>>
    {
    }
}
