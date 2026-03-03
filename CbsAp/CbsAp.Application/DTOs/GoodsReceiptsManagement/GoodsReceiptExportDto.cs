namespace CbsAp.Application.DTOs.GoodsReceiptsManagement
{
    public class GoodsReceiptExportDto
    {
        public string Entity { get; set; } = string.Empty;

        public string Supplier { get; set; } = string.Empty;

        public string GoodsReceiptNumber { get; set; } = string.Empty;

        public string DeliveryNote { get; set; } = string.Empty;

        public DateTimeOffset? DeliveryDate { get; set; }

        public string ActiveStatus { get; set; } = string.Empty;
    }
}
