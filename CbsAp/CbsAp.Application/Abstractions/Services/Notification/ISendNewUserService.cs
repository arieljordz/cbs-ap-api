using System.Net.Mail;

namespace CbsAp.Application.Abstractions.Services.Notification
{
    public interface ISendNewUserService
    {
        Task<bool> SendNewUserEmailAsync(string recipient, Dictionary<string, string> bindData);
    }
}