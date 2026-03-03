namespace CbsAp.Application.DTOs.Dashboard
{
    public class UpdateNoticeDTO
    {
        public long NoticeID { get; set; }
        public string Heading { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool SendNotification { get; set; }

    }
}