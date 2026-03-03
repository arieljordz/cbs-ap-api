using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CbsAp.Application.Features.Authentication.Commands.ForgotPassword
{
    public record ForgotPasswordCommand(string EmailAddress) : ICommand<ResponseResult<bool>>
    {
    }
}
