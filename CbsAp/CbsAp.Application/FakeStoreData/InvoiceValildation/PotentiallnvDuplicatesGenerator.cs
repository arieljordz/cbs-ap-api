using Bogus;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.FakeStoreData.InvoiceValildation
{
    public static class PotentiallnvDuplicatesGenerator
    {
        public static List<Invoice> GenerateInvoice()
        {
            var faker = new Faker("en_AU");

            var commonSupplierId = 26;
            var duplicateInvoiceNo = "DUPINV001";
            var duplicateDate = new DateTime(2024, 10, 10);
            var duplicateAmount = 1234.56m;

            var invoices = new List<Invoice>();

            //Combo 1: SupplierInfoID + InvoiceNo + InvoiceDate
            invoices.Add(new Invoice
            {
                SupplierInfoID = commonSupplierId,
                InvoiceNo = duplicateInvoiceNo,
                InvoiceDate = duplicateDate,
                TotalAmount = 3725.55m,

                ApproverRole = "Administrator",
                ApprovedUser = "User 1",
                QueueType = InvoiceQueueType.MyInvoices,
                StatusType = InvoiceStatusType.ForApproval,
                TaxCodeID = faker.Random.Long(1, 10),
                PaymentTerm = "30 Days",
                Note = faker.Lorem.Sentence(),
                SuppBankAccount = faker.Finance.Account(),
                DueDate = faker.Date.FutureOffset(1),
                PoNo = "PO-" + faker.Random.AlphaNumeric(6).ToUpper(),
                GrNo = "GR-" + faker.Random.AlphaNumeric(6).ToUpper(),
                Currency = "AUD",
                NetAmount = faker.Finance.Amount(1000, 5000),
                TaxAmount = faker.Finance.Amount(100, 500),
                MapID = faker.Random.Int(0, 99999).ToString("D5"),
                ScanDate = faker.Date.RecentOffset(30),
                EntityProfileID = faker.Random.Long(34, 35),
            });
            invoices.Add(new Invoice
            {
                SupplierInfoID = commonSupplierId,
                InvoiceNo = duplicateInvoiceNo,
                InvoiceDate = duplicateDate,
                TotalAmount = 3725.51m,

                ApproverRole = "Administrator",
                ApprovedUser = "User 1",
                QueueType = InvoiceQueueType.MyInvoices,
                StatusType = InvoiceStatusType.ForApproval,
                TaxCodeID = faker.Random.Long(1, 10),
                PaymentTerm = "30 Days",
                Note = faker.Lorem.Sentence(),
                SuppBankAccount = faker.Finance.Account(),
                DueDate = faker.Date.FutureOffset(1),
                PoNo = "PO-" + faker.Random.AlphaNumeric(6).ToUpper(),
                GrNo = "GR-" + faker.Random.AlphaNumeric(6).ToUpper(),
                Currency = "AUD",
                NetAmount = faker.Finance.Amount(1000, 5000),
                TaxAmount = faker.Finance.Amount(100, 500),
                MapID = faker.Random.Int(0, 99999).ToString("D5"),
                ScanDate = faker.Date.RecentOffset(30),
                EntityProfileID = faker.Random.Long(34, 35),
            });

            // ✅ Combo 2: SupplierInfoID + InvoiceNo + TotalAmount
            invoices.Add(new Invoice
            {
                SupplierInfoID = commonSupplierId,
                InvoiceNo = duplicateInvoiceNo,
                InvoiceDate = faker.Date.Past(),
                TotalAmount = duplicateAmount,

                ApproverRole = "Administrator",
                ApprovedUser = "User 1",
                QueueType = InvoiceQueueType.MyInvoices,
                StatusType = InvoiceStatusType.ForApproval,
                TaxCodeID = faker.Random.Long(1, 10),
                PaymentTerm = "30 Days",
                Note = faker.Lorem.Sentence(),
                SuppBankAccount = faker.Finance.Account(),
                DueDate = faker.Date.FutureOffset(1),
                PoNo = "PO-" + faker.Random.AlphaNumeric(6).ToUpper(),
                GrNo = "GR-" + faker.Random.AlphaNumeric(6).ToUpper(),
                Currency = "AUD",
                NetAmount = faker.Finance.Amount(1000, 5000),
                TaxAmount = faker.Finance.Amount(100, 500),
                MapID = faker.Random.Int(0, 99999).ToString("D5"),
                ScanDate = faker.Date.RecentOffset(30),
                EntityProfileID = faker.Random.Long(34, 35),
            });
            invoices.Add(new Invoice
            {
                SupplierInfoID = commonSupplierId,
                InvoiceNo = duplicateInvoiceNo,
                InvoiceDate = faker.Date.Past(),
                TotalAmount = duplicateAmount,

                ApproverRole = "Administrator",
                ApprovedUser = "User 1",
                QueueType = InvoiceQueueType.MyInvoices,
                StatusType = InvoiceStatusType.ForApproval,
                TaxCodeID = faker.Random.Long(1, 10),
                PaymentTerm = "30 Days",
                Note = faker.Lorem.Sentence(),
                SuppBankAccount = faker.Finance.Account(),
                DueDate = faker.Date.FutureOffset(1),
                PoNo = "PO-" + faker.Random.AlphaNumeric(6).ToUpper(),
                GrNo = "GR-" + faker.Random.AlphaNumeric(6).ToUpper(),
                Currency = "AUD",
                NetAmount = faker.Finance.Amount(1000, 5000),
                TaxAmount = faker.Finance.Amount(100, 500),
                MapID = faker.Random.Int(0, 99999).ToString("D5"),
                ScanDate = faker.Date.RecentOffset(30),
                EntityProfileID = faker.Random.Long(34, 35),
            });

            // ✅ Combo 3: SupplierInfoID + InvoiceDate + TotalAmount
            invoices.Add(new Invoice
            {
                SupplierInfoID = commonSupplierId,
                InvoiceNo = faker.Random.String2(6),
                InvoiceDate = duplicateDate,
                TotalAmount = duplicateAmount,

                ApproverRole = "Administrator",
                ApprovedUser = "User 1",
                QueueType = InvoiceQueueType.MyInvoices,
                StatusType = InvoiceStatusType.ForApproval,
                TaxCodeID = faker.Random.Long(1, 10),
                PaymentTerm = "30 Days",
                Note = faker.Lorem.Sentence(),
                SuppBankAccount = faker.Finance.Account(),
                DueDate = faker.Date.FutureOffset(1),
                PoNo = "PO-" + faker.Random.AlphaNumeric(6).ToUpper(),
                GrNo = "GR-" + faker.Random.AlphaNumeric(6).ToUpper(),
                Currency = "AUD",
                NetAmount = faker.Finance.Amount(1000, 5000),
                TaxAmount = faker.Finance.Amount(100, 500),
                MapID = faker.Random.Int(0, 99999).ToString("D5"),
                ScanDate = faker.Date.RecentOffset(30),
                EntityProfileID = faker.Random.Long(34, 35),
            });
            invoices.Add(new Invoice
            {
                SupplierInfoID = commonSupplierId,
                InvoiceNo = faker.Random.String2(6),
                InvoiceDate = duplicateDate,
                TotalAmount = duplicateAmount,

                ApproverRole = "Administrator",
                ApprovedUser = "User 1",
                QueueType = InvoiceQueueType.MyInvoices,
                StatusType = InvoiceStatusType.ForApproval,
                TaxCodeID = faker.Random.Long(1, 10),
                PaymentTerm = "30 Days",
                Note = faker.Lorem.Sentence(),
                SuppBankAccount = faker.Finance.Account(),
                DueDate = faker.Date.FutureOffset(1),
                PoNo = "PO-" + faker.Random.AlphaNumeric(6).ToUpper(),
                GrNo = "GR-" + faker.Random.AlphaNumeric(6).ToUpper(),
                Currency = "AUD",
                NetAmount = faker.Finance.Amount(1000, 5000),
                TaxAmount = faker.Finance.Amount(100, 500),
                MapID = faker.Random.Int(0, 99999).ToString("D5"),
                ScanDate = faker.Date.RecentOffset(30),
                EntityProfileID = faker.Random.Long(34, 35),
            });

            return invoices;

            //var invoice = new Invoice
            //{
            //    InvoiceNo = "INV-" + faker.Random.AlphaNumeric(10).ToUpper(),
            //    InvoiceDate = faker.Date.PastOffset(1),
            //    MapID = faker.Random.Int(0, 99999).ToString("D5"),
            //    ScanDate = faker.Date.RecentOffset(30),
            //    EntityProfileID = faker.Random.Long(34, 35),
            //    SupplierInfoID = 26,
            //    SuppBankAccount = faker.Finance.Account(),
            //    DueDate = faker.Date.FutureOffset(1),
            //    PoNo = "PO-" + faker.Random.AlphaNumeric(6).ToUpper(),
            //    GrNo = "GR-" + faker.Random.AlphaNumeric(6).ToUpper(),
            //    Currency = "AUD",
            //    NetAmount = faker.Finance.Amount(1000, 5000),
            //    TaxAmount = faker.Finance.Amount(100, 500),
            //    TotalAmount = faker.Finance.Amount(1100, 5500),
            //    TaxCodeID = faker.Random.Long(1, 10),
            //    PaymentTerm = "30 Days",
            //    Note = faker.Lorem.Sentence(),
            //    ApproverRole = "Administrator",
            //    ApprovedUser = "User 1",
            //    QueueType = EngineInvoiceQueueType.MyInvoices,
            //    StatusType = EngineInvoiceStatusType.ForApproval,

            //};

            //return invoice;
        }
    }
}