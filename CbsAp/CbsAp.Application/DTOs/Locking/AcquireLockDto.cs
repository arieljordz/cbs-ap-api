using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.Locking
{
    public record AcquireLockDto(long Id,bool OverrideLock);
    
}
