using CbsAp.Application.Abstractions.Services.Notification;
using CbsAp.Infrastracture.NotificationService.Constants;
using System.Net.Mail;

namespace CbsAp.Infrastracture.NotificationService
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public async Task<(string HtmlBody, List<Attachment> Attachments)> LoadEmailTemplateAsync(
            string templateName,
            Dictionary<string, string> placeholders)
        {
            string templatePath = $"{templateName}.mjml";
            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Template {templateName} not found!");

            string templateContent = await File.ReadAllTextAsync(templatePath);

            placeholders.Add("{{logo}}", "EmailNotificationTemplates/assets/cbsap-grey.png");
            placeholders.Add("{{base64bsanzpoweredBy}}", "EmailNotificationTemplates/assets/poweredby-cbsanz.png");

            foreach (var placeholder in placeholders)
            {
                var x = $"{{{{{placeholder.Key}}}}}";
                templateContent = templateContent.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }

            foreach (var placeholder in placeholders)
            {
                if (placeholder.Key.Equals("{{logo}}") || placeholder.Key.Equals("{{base64bsanzpoweredBy}}"))
                {
                    string imageBase64 = await ConvertImageToBase64Async(placeholder.Value);
                    templateContent = templateContent.Replace(placeholder.Key, imageBase64);
                }
            }

            return (templateContent, new List<Attachment>());
        }

        private static async Task<string> ConvertImageToBase64Async(string imagePath)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException($"Image {imagePath} not found!");

            byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
            return Convert.ToBase64String(imageBytes);
        }
    }
}