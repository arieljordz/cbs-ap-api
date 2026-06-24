using CbsAp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.DTOs.DimensionSetup
{
    public class DimensionSetupDto
    {
        public long DimensionSetupId { get; set; }

        public string? DimensionSetupName { get; set; }

        public short DisplayOrder { get; set; }

        public string? DimensionName { get; set; }

        public long? DimensionValueId { get; set; }

        public bool? Required { get; set; }

        public bool? Show { get; set; }
    }
}
