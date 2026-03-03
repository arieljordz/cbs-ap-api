using CbsAp.Application.DTOs.TaxCodesManagement;
using CbsAp.Domain.Entities.TaxCodes;
using Mapster;

namespace CbsAp.Application.MapsterMappinngs.Dashboard
{
    public class TaxCodeMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {

            config.NewConfig<TaxCode, TaxCodeDTO>()
             .MapWith(taxcode => new TaxCodeDTO
             {
                 EntityID = taxcode.EntityID,
                 Code = taxcode.Code ?? "",
                 TaxCodeID = taxcode.TaxCodeID,
                 TaxCodeName = taxcode.TaxCodeName ?? "",
                 TaxRate = taxcode.TaxRate,
                 EntityName = taxcode.EntityProfile.EntityName
             });
        }
    }
}
