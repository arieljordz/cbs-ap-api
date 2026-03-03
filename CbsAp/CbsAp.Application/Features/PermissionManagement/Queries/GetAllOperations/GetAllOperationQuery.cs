using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.PermissionManagement.OperationDTO;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.PermissionManagement.Queries.GetAllOperations
{
    public record GetAllOperationQuery : IQuery<ResponseResult<IQueryable<GroupPanelDTO>>>
    {
        
    }
}