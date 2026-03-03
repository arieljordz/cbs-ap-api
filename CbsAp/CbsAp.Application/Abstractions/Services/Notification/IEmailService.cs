using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Abstractions.Services.Notification
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string recipient, string subject, string templateName,  Dictionary<string, string> bindData);
    }
}
