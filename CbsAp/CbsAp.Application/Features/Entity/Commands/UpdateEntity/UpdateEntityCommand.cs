using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Entity.Commands.UpdateEntity
{
    public record UpdateEntityCommand(EntityDto Entity, string updatedBy) : ICommand<ResponseResult<bool>>
    {
    }
}
