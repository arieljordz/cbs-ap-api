using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.Entity;
using CbsAp.Domain.Entities.Supplier;
using Mapster;

namespace CbsAp.Application.MapsterMappinngs.SystemSettings
{
    public class EntityMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
        
            config.NewConfig<EntityDto, EntityProfile>()
                .Map(dest => dest.EntityProfileID, src => src.EntityProfileID)
                .Map(dest => dest.EntityCode, src => src.EntityCode)
                .Map(dest => dest.EmailAddress, src => src.EmailAddress)
                .Map(dest => dest.TaxID, src => src.TaxID)
                .Map(dest => dest.ERPFinanceSystem, src => src.ERPFinanceSystem)
                .Map(dest => dest.DefaultInvoiceDueInDays, src => src.DefaultInvoiceDueInDays)
                .Map(dest => dest.InvAllowPresetAmount, src => src.InvAllowPresetAmount)
                .Map(dest => dest.InvAllowPresetDimension, src => src.InvAllowPresetDimension)
                .Map(dest => dest.TaxDollarAmt, src => src.TaxDollarAmt)
                .Map(dest => dest.TaxPercentageAmt, src => src.TaxPercentageAmt)
                .Map(dest => dest.CreatedDate, src => src.CreatedDate)
                .Map(dest => dest.MatchingConfigs, src => src.MatchingConfigs.Adapt<List<EntityMatchingConfigDto>>());

            config.NewConfig<EntityMatchingConfig, EntityMatchingConfigDto>()
                .Map(dest => dest.MatchingConfigID, src => src.MatchingConfigID)
                .Map(dest => dest.EntityProfileID, src => src.EntityProfileID)
                .Map(dest => dest.ConfigType, src => src.ConfigType.ToString())
                .Map(dest => dest.MatchingLevel, src => src.MatchingLevel)
                .Map(dest => dest.InvoiceMatchBasis, src => src.InvoiceMatchBasis)
              
                .Map(dest => dest.DollarAmt, src => src.DollarAmt) 
                .Map(dest => dest.PercentageAmt, src => src.PercentageAmt); 

            //Search Pagination
            config.NewConfig<PaginatedList<EntityProfile>, PaginatedList<EntitySearchDto>>()
                 .MapWith(entity => new PaginatedList<EntitySearchDto>
                 {
                     Data = MapPermissionSearchDTO(entity).Data,
                     CurrentPage = entity.CurrentPage,
                     PageSize = entity.PageSize,
                     TotalCount = entity.TotalCount,
                     TotalPages = entity.TotalPages,
                     IsSuccess = entity.IsSuccess,

                 });
        }

        private static PaginatedList<EntitySearchDto> MapPermissionSearchDTO(PaginatedList<EntityProfile> entity)
        {
            var entityDTO = entity.Data.Adapt<List<EntitySearchDto>>();
            return new PaginatedList<EntitySearchDto>(entityDTO);
        }
    }
}
