namespace CbsAp.Application.DTOs.DimensionsManagement
{
    public class CreateUpdateDimensionDTO
    {
        public long EntityProfileID { get; set; }

        public string Dimension { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool Active { get; set; }

        public string? FreeField1 { get; set; }

        public string? FreeField2 { get; set; }

        public string? FreeField3 { get; set; }
    }
}
