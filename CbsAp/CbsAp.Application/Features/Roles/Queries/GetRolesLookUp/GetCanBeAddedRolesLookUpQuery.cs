using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Shared.ResultPatten;


namespace CbsAp.Application.Features.Roles.Queries.GetRolesLookUp
{
    public record GetCanBeAddedRolesLookUpQuery() : IQuery<ResponseResult<IEnumerable<RoleDTO>>>
        {

    }


}