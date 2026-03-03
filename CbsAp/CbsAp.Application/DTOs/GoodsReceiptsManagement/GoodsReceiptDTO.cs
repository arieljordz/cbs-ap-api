namespace CbsAp.Application.DTOs.GoodsReceiptsManagement
{
    public class GoodsReceiptDTO
    {
        public long GoodsReceiptID { get; set; }

        public string Entity { get; set; } = string.Empty;

        public string Supplier { get; set; } = string.Empty;

        public string GoodsReceiptNumber { get; set; } = string.Empty;

        public string DeliveryNote { get; set; } = string.Empty;

        public DateTimeOffset? DeliveryDate { get; set; }

        public bool Active { get; set; }

        public string ActiveStatus => Active ? "Yes" : "No";
    }
}
