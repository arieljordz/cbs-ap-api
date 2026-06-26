using CbsAp.Application.DTOs.AccountDimension;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.PermissionManagement;
using System.Linq;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IAccountDimensionRepository
    {
        Task<PaginatedList<AccountDimensionSearchDTO>> GetSearchAccountDimensionAsync(
            long? roleId,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token
            );

    }
}