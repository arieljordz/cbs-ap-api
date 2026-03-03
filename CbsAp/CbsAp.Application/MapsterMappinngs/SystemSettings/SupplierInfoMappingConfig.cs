using CbsAp.Application.DTOs.Supplier;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.Supplier;
using Mapster;

namespace CbsAp.Application.MapsterMappinngs.SystemSettings
{
    public class SupplierInfoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<SupplierInfo, SupplierInfoDto>()
             .MapWith(src => new SupplierInfoDto
             {
                 SupplierInfoID = src.SupplierInfoID,
                 SupplierID = src.SupplierID,
                 SupplierTaxID = src.SupplierTaxID,
                 EntityProfileID = src.EntityProfileID,
                 SupplierName = src.SupplierName,
                 IsActive = src.IsActive,
                 Telephone = src.Telephone,
                 EmailAddress = src.EmailAddress,
                 Contact = src.Contact,
                 AddressLine1 = src.AddressLine1,
                 AddressLine2 = src.AddressLine2,
                 AddressLine3 = src.AddressLine3,
                 AddressLine4 = src.AddressLine4,
                 AddressLine5 = src.AddressLine5,
                 AddressLine6 = src.AddressLine6,
                 AccountID = src.AccountID,
                 TaxCodeID = src.TaxCodeID,
                 Currency = src.Currency,
                 PaymentTerms = src.PaymentTerms,
                 InvRoutingFlowID = src.InvRoutingFlowID,
                 FreeField1 = src.FreeField1,
                 FreeField2 = src.FreeField2,
                 FreeField3 = src.FreeField3,
                 Notes = src.Notes,
                 CreatedBy = src.CreatedBy,
                 CreatedDate = src.CreatedDate,
                 LastUpdatedBy = src.LastUpdatedBy,
                 LastUpdatedDate = src.LastUpdatedDate,
             });

            config.NewConfig<SupplierInfoDto, SupplierInfo>()
             .MapWith(src => new SupplierInfo
             {
                 SupplierInfoID = src.SupplierInfoID,
                 SupplierID = src.SupplierID,
                 SupplierTaxID = src.SupplierTaxID,
                 EntityProfileID = src.EntityProfileID,
                 SupplierName = src.SupplierName,
                 IsActive = src.IsActive,
                 Telephone = src.Telephone,
                 EmailAddress = src.EmailAddress,
                 Contact = src.Contact,
                 AddressLine1 = src.AddressLine1,
                 AddressLine2 = src.AddressLine2,
                 AddressLine3 = src.AddressLine3,
                 AddressLine4 = src.AddressLine4,
                 AddressLine5 = src.AddressLine5,
                 AddressLine6 = src.AddressLine6,
                 AccountID = src.AccountID,
                 TaxCodeID = src.TaxCodeID,
                 Currency = src.Currency,
                 PaymentTerms = src.PaymentTerms,
                 InvRoutingFlowID = src.InvRoutingFlowID,
                 FreeField1 = src.FreeField1,
                 FreeField2 = src.FreeField2,
                 FreeField3 = src.FreeField3,
                 Notes = src.Notes,
                 CreatedBy = src.CreatedBy,
                 CreatedDate = src.CreatedDate,
                 LastUpdatedBy = src.LastUpdatedBy,
                 LastUpdatedDate = src.LastUpdatedDate,
             });

            config.NewConfig<PaginatedList<SupplierInfo>, PaginatedList<SupplierSearchDto>>()
               .MapWith(supplier => new PaginatedList<SupplierSearchDto>
               {
                   Data = MapSupplierPagedDto(supplier).Data,
                   CurrentPage = supplier.CurrentPage,
                   PageSize = supplier.PageSize,
                   TotalCount = supplier.TotalCount,
                   TotalPages = supplier.TotalPages,
                   IsSuccess = supplier.IsSuccess,
               });
        }

        private static PaginatedList<SupplierSearchDto> MapSupplierPagedDto(PaginatedList<SupplierInfo> supplier)
        {
            var searchDto = supplier.Data.Adapt<List<SupplierSearchDto>>();
            return new PaginatedList<SupplierSearchDto>(searchDto);
        }
    }
}