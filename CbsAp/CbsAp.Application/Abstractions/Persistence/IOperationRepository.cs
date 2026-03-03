using CbsAp.Application.DTOs.PermissionManagement.OperationDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IOperationRepository
    {
        Task<IQueryable<OperationsDTO>> GetAllOperationAsyc();
    }
}
