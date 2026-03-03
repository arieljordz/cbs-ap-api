using System.Net.Mail;

namespace CbsAp.Application.Abstractions.Services.Notification
{
    public interface IEmailTemplateService
    {
        Task<(string HtmlBody, List<Attachment> Attachments)> LoadEmailTemplateAsync(string templateName, Dictionary<string, string> placeholders);
    }
}