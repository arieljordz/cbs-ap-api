using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Services.Entity;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.Entity.Commands.UpdateEntity
{
    public class UpdateEntityCommandHandler : ICommandHandler<UpdateEntityCommand, ResponseResult<bool>>
    {
        private readonly IEntityService _entityService;

        private readonly IUnitofWork _unitofWork;

        private readonly ILogger<UpdateEntityCommandHandler> _logger;

        public UpdateEntityCommandHandler(IEntityService entityService, IUnitofWork unitofWork, ILogger<UpdateEntityCommandHandler> logger)
        {
            _entityService = entityService;
            _unitofWork = unitofWork;
            _logger = logger;
        }

        public async Task<ResponseResult<bool>> Handle(UpdateEntityCommand request, CancellationToken cancellationToken)
        {
            var entity = await GetEntity(request.Entity.EntityProfileID);
            entity.CreatedBy = entity.CreatedBy!;
            entity.CreatedDate = entity.CreatedDate!;

            var mapUpdateEntity = request.Entity.Adapt(entity);

            mapUpdateEntity.AutomaticGoodsDelivered = request.Entity.AutomaticGoodsDelivered;

            mapUpdateEntity.SetAuditFieldsOnUpdate(request.updatedBy);

            if (await _entityService.IsEntityExist(entity.EntityName!, entity.EntityCode, entity.EntityProfileID))
            {
                _logger.LogWarning("Entity with  : {Name} and  {EntityCode} is already existed",
                    entity.EntityName, entity.EntityCode);

                return ResponseResult<bool>.BadRequest("Entity Name is already existed");
            }
            if (!await _entityService.UpdateEntity(mapUpdateEntity, cancellationToken))
            {
                _logger.LogWarning("Error in updating entity  : {Name}", entity.EntityName);
                return ResponseResult<bool>.BadRequest("Error on updating entity");
            }

            return ResponseResult<bool>.OK(MessageConstants.Message(MessageOperationType.Update, "entity"));
        }

        private async Task<EntityProfile> GetEntity(long entityProfileID)
        {
            return await _unitofWork.GetRepository<EntityProfile>().GetByIdAsync(entityProfileID);
        }
    }
}