using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Services.Entity;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.Entity.Commands.CreateEntity
{
    public class CreateEntityCommandHandler : ICommandHandler<CreateEntityCommand, ResponseResult<bool>>
    {
        private readonly IEntityService _entityService;
        private readonly ILogger<CreateEntityCommandHandler> _logger;

        public CreateEntityCommandHandler(
           IEntityService entityService,
           ILogger<CreateEntityCommandHandler> logger
            )
        {
            _entityService = entityService;
           _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(CreateEntityCommand request, CancellationToken cancellationToken)
        {
            var entity = request.Entity.Adapt<EntityProfile>();
            entity.SetAuditFieldsOnCreate(request.CreatedBy);

            if (await _entityService.IsEntityExist(entity.EntityName!, entity.EntityCode!))
            {
                _logger.LogWarning("Entity with name : {Name} and {EntityCode} is already existed",
                     entity.EntityName, entity.EntityCode);

                return ResponseResult<bool>.BadRequest("Entity  is already existed");
            }

            if (!await _entityService.CreateEntity(entity, cancellationToken))
            {
                _logger.LogError("Error in adding new entity : {Name}", entity.EntityName);
                return ResponseResult<bool>.BadRequest("Error adding new entity");
            }

            return ResponseResult<bool>.Created(MessageConstants.Message(MessageOperationType.Create, "entity"));
        }
    }
}
