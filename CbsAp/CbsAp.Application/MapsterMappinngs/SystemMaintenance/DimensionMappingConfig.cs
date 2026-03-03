using CbsAp.Application.DTOs.DimensionsManagement;
using CbsAp.Domain.Entities.Dimensions;
using Mapster;

namespace CbsAp.Application.MapsterMappinngs.SystemMaintenance
{
    public class DimensionMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Dimension, DimensionDTO>()
                .MapWith(dimension => new DimensionDTO
                {
                    DimensionID = dimension.DimensionID,
                    Entity = dimension.EntityProfile != null ? dimension.EntityProfile.EntityName : string.Empty,
                    Dimension = dimension.DimensionCode,
                    DimensionName = dimension.Name,
                    Active = dimension.IsActive,
                });
        }
    }
}
