namespace CbsAp.Application.DTOs.Dashboard
{
    public class NoticeDTO
    {
        public long NoticeID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        public DateTime MessageDate { get; set; }
        public string Heading { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsNew { get; set; }
    }


}