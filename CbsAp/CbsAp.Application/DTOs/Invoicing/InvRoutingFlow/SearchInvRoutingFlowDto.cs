namespace CbsAp.Application.DTOs.Invoicing.InvRoutingFlow
{
    public class SearchInvRoutingFlowDto
    {
        public long InvRoutingFlowID { get; set; }
        public string? Entity { get; set; }
        public string? InvoiceRoutingFlowName { get; set; }
        public string? SuppliersLinked { get; set; }
        public string? MatchReference { get; set; }
    }

    public class SearchInvRoutingFlowUserDto
    {
        public string? UserID { get; set; }
    }

    public class SearchInvRoutingFlowRolesDto
    {
        public string RoleName { get; set; }
    }
}