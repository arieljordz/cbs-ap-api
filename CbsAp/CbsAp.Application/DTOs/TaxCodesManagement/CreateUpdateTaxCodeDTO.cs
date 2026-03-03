namespace CbsAp.Application.DTOs.TaxCodesManagement
{
    public class CreateUpdateTaxCodeDTO
    {
        public long EntityID { get; set; }
        public string TaxCodeName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal TaxRate { get; set; }
    }
}