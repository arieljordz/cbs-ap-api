using System.Net.Mail;
using System.Text;

namespace CbsAp.Infrastracture.NotificationService
{
    public static class SmtpClientExtension
    {
        public static async Task<bool> SendNotificationEmailAsync(this SmtpClient smtpClient, MailMessage message,
            EmailConfiguration emailConfig)
        {
            try
            {
                if (message == null)  
                {
                    return false;
                }

                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;
                smtpClient.Port = emailConfig.Port;
                smtpClient.Host = emailConfig.Host;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = true;
                await smtpClient.SendMailAsync(message).ConfigureAwait(false);
                return true;
            }
            catch (SmtpException)
            {
                return false;
            }
        }
    }
}