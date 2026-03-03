using CbsAp.Application.DTOs.GoodsReceiptsManagement;
using CbsAp.Domain.Entities.GoodReceipts;
using Mapster;

namespace CbsAp.Application.MapsterMappinngs.SystemMaintenance
{
    public class GoodsReceiptMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<GoodReceipt, GoodsReceiptDTO>()
                .MapWith(gr => new GoodsReceiptDTO
                {
                    GoodsReceiptID = gr.GoodsReceiptID,
                    Entity = gr.EntityProfile != null ? gr.EntityProfile.EntityName : string.Empty,
                    Supplier = gr.Supplier != null && gr.Supplier.SupplierName != null ? gr.Supplier.SupplierName : string.Empty,
                    GoodsReceiptNumber = gr.GoodsReceiptNumber,
                    DeliveryNote = gr.DeliveryNote ?? string.Empty,
                    DeliveryDate = gr.DeliveryDate,
                    Active = gr.Active
                });
        }
    }
}
