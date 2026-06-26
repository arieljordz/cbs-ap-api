using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.AccountDimension
{
    public class AccountDimensionSearchByIdDTO
    {
        public long EntityProfileID { get; set; }

        public string Entity { get; set; }

        public string Category { get; set; }

        public long DimensionID { get; set; }

        public string Assigned { get; set; }
    }
}