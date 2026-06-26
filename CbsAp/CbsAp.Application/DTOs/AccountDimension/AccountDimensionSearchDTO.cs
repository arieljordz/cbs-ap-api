using CbsAp.Domain.Common.Interfaces;

namespace CbsAp.Application.DTOs.AccountDimension
{
    public class AccountDimensionSearchDTO
    {
        public long RoleId { get; set; }
        public long? EntityProfileId { get; set; }
        public string Entity { get; set; }
        public string Category { get; set; }
        public string Assigned { get; set; }
        public bool IsAssigned { get; set; }
    }

    public class SearchAccountDimensionParamQuery
    {
        public long RoleId { get; set; }

        public long? EntityProfileId { get; set; }

        public string? Category { get; set; }

        public string? Search { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}