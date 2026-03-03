namespace CbsAp.Application.DTOs.Invoicing
{
    public class InvActivityLogDto
    {
        public string ActionName { get; set; } = string.Empty;

        public List<InvActivityLogEntriesDto> InvActivityLogEntries { get; set; } =
             new List<InvActivityLogEntriesDto>();
    }

    public class InvActivityLogEntriesDto
    {
        public long ActivityLogID { get; set; }

        public long InvoiceID { get; set; }

        public string? PreviousStatus { get; set; }

        public string? CurrentStatus { get; set; }

        public string? Reason { get; set; }

        public string? Action { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public string CreatedDate { get; set; } = string.Empty;
    }
}
