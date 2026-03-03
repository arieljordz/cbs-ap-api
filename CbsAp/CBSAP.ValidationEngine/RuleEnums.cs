namespace CBSAP.ValidationEngine
{
    public enum EngineInvoiceQueueType
    {
        ExceptionQueue = 101,
        RejectionQueue = 102,
        MyInvoices = 103,
        ArchiveQueue = 104,
        ExportedQueue = 105,
        ApproverQueue = 106,
    }

    public enum EngineInvoiceStatusType
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

    public enum EngineValidationSeverity
    {
        Info = 1,
        Warning = 5,
        Error = 10,
        Critical = 20
    }
}