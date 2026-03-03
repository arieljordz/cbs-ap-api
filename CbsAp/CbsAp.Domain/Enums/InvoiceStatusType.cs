namespace CbsAp.Domain.Enums
{
    public enum InvoiceStatusType
    {
        Validation = 10,
        Exception = 50,
        ExceptionOnHold = 55,
        ForApproval = 60,
        ApprovalOnHold = 65,
        Rejected = 70,
        ReadyForExport = 80,
        Exported = 85,
        Approved = 90,
        Archived = 95,
    }
}