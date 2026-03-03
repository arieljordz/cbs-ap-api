namespace CbsAp.Application.DTOs.Invoicing.InvRoutingFlow
{
    public class ExportInvRoutingFlowDto
    {
        public string? Entity { get; set; }
        public string? InvoiceRoutingFlowName { get; set; }
        public string? SuppliersLinked { get; set; }
        public string? Roles { get; set; }
        public string? Users { get; set; }
        public string? MatchReference { get; set; }
    }
}