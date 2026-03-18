using CbsAp.Application.DTOs.Entity;
using CbsAp.Domain.Entities.Entity;
using Mapster;
using System.Collections.Generic;

namespace CbsAp.Application.Configurations
{
    public class MapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<EntityDto, EntityProfile>()
                  .Ignore(dest => dest.MatchingConfigs)
                  .IgnoreNullValues(true);
        }
    }
}