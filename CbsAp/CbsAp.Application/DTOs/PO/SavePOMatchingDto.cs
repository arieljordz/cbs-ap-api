namespace CbsAp.Application.DTOs.PO
{
    public class SavePOMatchingDto
    {
        public long InvoiceID { get; set; }
        public List<PoLinesDto>? PoLines { get; set; } = new List<PoLinesDto>();
    }
}