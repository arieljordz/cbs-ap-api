using CbsAp.Application.Abstractions.Services.Notification;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Infrastracture.NotificationService.Constants;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace CbsAp.Infrastracture.NotificationService
{
    public class SendNewUserService : ISendNewUserService
    {
        private readonly EmailConfiguration _emailConfig;

        public SendNewUserService(IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task<bool> SendNewUserEmailAsync(string recipient, Dictionary<string, string> bindData)
        {
            string bindDataToHtmlTemplate = File.ReadAllText(NoticationConfiguration.NewUserNotificationTemplate);

            bindData.Add("{base64CBSlogo}", NoticationConfiguration.HeaderLogo.ConvertImageToBase64());
            bindData.Add("{base64bsanzpoweredBy}", NoticationConfiguration.FooterLogo.ConvertImageToBase64());
            bindDataToHtmlTemplate =
                  bindData.Aggregate(bindDataToHtmlTemplate,
                  (current, kvp) =>
                  current.Replace(kvp.Key, kvp.Value));

            MailMessage message =
                new MailMessage(
                    _emailConfig.From!,
                    recipient,
                    "[CBS AP - New User Notification]",
                    bindDataToHtmlTemplate
                );

            message.ReplyToList.Add(new MailAddress("no-reply@cbsap.canon.com.au"));

            using (SmtpClient smtpClient = new SmtpClient())
            {
                return await smtpClient.SendNotificationEmailAsync(message, _emailConfig);
            };
        }

        private MailMessage CreateEmailMessage(string recipient, string emailBody, List<Attachment> attachments)
        {
            var message = new MailMessage(
                _emailConfig.From!,
                recipient,
                "{Notice]", // Placeholder, replace with actual subject if needed
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