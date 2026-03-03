using CbsAp.Application.Abstractions.Services.Notification;
using CbsAp.Application.Abstractions.Services.Shared;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace CbsAp.Infrastracture.NotificationService
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IMjmlService _mjmlService;

        public EmailService(IEmailTemplateService emailTemplateService, IMjmlService mjmlService, IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig.Value;
            _emailTemplateService = emailTemplateService;
            _mjmlService = mjmlService;
        }

        public async Task<bool> SendEmailAsync(
            string recipient,
            string subject,
            string templateName,
            Dictionary<string, string> bindData)
        {
            var (emailBody, attachments) =
                await _emailTemplateService.LoadEmailTemplateAsync(templateName, bindData);

            string htmlEmailBody = _mjmlService.ConvertMjmlToHtml(emailBody);

            MailMessage message = CreateEmailMessage(recipient, subject, htmlEmailBody, attachments);

            using (SmtpClient smtpClient = new SmtpClient())
            {
                return await smtpClient.SendNotificationEmailAsync(message, _emailConfig);
            }
        }

        private MailMessage CreateEmailMessage(string recipient, string subject, string emailBody, List<Attachment> attachments)
        {
            var message = new MailMessage(
                _emailConfig.From!,
                recipient,
                subject,
                emailBody
            );

            foreach (var attachment in attachments)
            {
                message.Attachments.Add(attachment);
            }

            message.ReplyToList.Add(new MailAddress("no-reply@cbsap.canon.com.au"));
            return message;
        }
    }
}