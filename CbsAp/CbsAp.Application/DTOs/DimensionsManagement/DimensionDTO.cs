namespace CbsAp.Application.DTOs.DimensionsManagement
{
    public class DimensionDTO
    {
        public long DimensionID { get; set; }

        public string Entity { get; set; } = string.Empty;

        public string Dimension { get; set; } = string.Empty;

        public string DimensionName { get; set; } = string.Empty;

        public bool Active { get; set; }

        public string ActiveStatus => Active ? "Active" : "Inactive";
    }
}
