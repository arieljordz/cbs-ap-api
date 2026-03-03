namespace CbsAp.Application.DTOs.Dashboard
{
    public class CreateNoticeDTO
    {

        public string Heading { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
       // public string CreatedBy { get; set; } = string.Empty;
        public bool SendNotification  { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public string LastUpdatedBy { get; set; } = string.Empty;
        //public DateTime LastUpdatedDate { get; set; }
    }
}