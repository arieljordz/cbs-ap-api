namespace CbsAp.Application.DTOs.TaxCodesManagement
{
    public class TaxCodeDTO
    {
        public long TaxCodeID { get; set; }
        public long EntityID { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string TaxCodeName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal TaxRate { get; set; }
    }
}