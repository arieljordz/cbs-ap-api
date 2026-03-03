using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Abstractions.Services.Authentication
{
    public interface IUserService
    {
        Task<bool> userExist(string userName);

    }
}
