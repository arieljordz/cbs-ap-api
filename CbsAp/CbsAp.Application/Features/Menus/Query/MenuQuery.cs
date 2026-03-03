using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Menus;
using CbsAp.Application.Shared.ResultPatten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.Menus.Query
{
    public record MenuQuery(long roleID) : IQuery<ResponseResult<List<MenuListDto>>>
    {
    }
}
