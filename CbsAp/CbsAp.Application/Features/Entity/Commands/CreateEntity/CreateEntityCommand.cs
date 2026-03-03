using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Entity.Commands.CreateEntity
{
    public record CreateEntityCommand(EntityDto Entity, string CreatedBy) : ICommand<ResponseResult<bool>>
    {
    }
}
