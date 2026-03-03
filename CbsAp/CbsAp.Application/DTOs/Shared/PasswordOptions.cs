using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.Shared
{
    public sealed record PasswordOptions(
     int Length = 16,
     bool IncludeUppercase = true,
     bool IncludeLowercase = true,
     bool IncludeDigits = true,
     bool IncludeSymbols = true);
}
