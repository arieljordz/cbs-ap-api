using System.Net.Mail;

namespace CbsAp.Application.Abstractions.Services.Notification
{
    public interface ISendNoticeService
    {
        Task<bool> SendNoticeEmailAsync(string recipient, Dictionary<string, string> bindData);
    }
}