using Bogus;
using CbsAp.Application.FakeStoreData.FakeDTO;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.InvoicingArchive;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.FakeStoreData
{
    public static class InvoiceFakeGenerator
    {
        public static Invoice GenerateInvoice(FakeInvoiceDto dto, string createdBy)
        {
            var faker = new Faker("en_AU");

            var invoice = new Invoice
            {
                InvoiceNo = dto.InvoiceNo,
                InvoiceDate = dto.InvoiceDate,
                MapID = faker.Random.Int(0, 99999).ToString("D5"),
                ScanDate = faker.Date.Recent(30),
                EntityProfileID = dto.EntityProfileID,
                SupplierInfoID = dto.SupplierInfoID,
                SuppBankAccount = faker.Finance.Account(),
                DueDate = faker.Date.Future(1),
                PoNo = dto.PurchaseOrderNo ??string.Empty,
                GrNo = dto.GoodsReceiptNo ?? string.Empty,
                Currency = dto.Currency,
                NetAmount = dto.NetAmount,
                TaxAmount = dto.TaxAmount,
                TotalAmount = dto.TotalAmount,
                TaxCodeID = dto.TaxCodeID,
                PaymentTerm = "30",
                Note = "-- FAKE GENERATED INVOICE --",
                ApproverRole = null,
                ApprovedUser = null,
                QueueType = dto.QueueType,
                StatusType = dto.StatusType,
                ImageID = null,
                KeywordID = dto.KeywordID,
            };
            invoice.SetAuditFieldsOnCreate(createdBy);
            return invoice;
        }

        public static InvoiceArchive GenerateArchiveInvoice(FakeInvoiceDto dto, string createdBy)
        {
            var faker = new Faker("en_AU");

            var invoice = new InvoiceArchive
            {
                InvoiceNo = dto.InvoiceNo,
                InvoiceDate = dto.InvoiceDate,
                MapID = faker.Random.Int(0, 99999).ToString("D5"),
                ScanDate = faker.Date.Recent(30),
                EntityProfileID = dto.EntityProfileID,
                SupplierInfoID = dto.SupplierInfoID,
                SuppBankAccount = faker.Finance.Account(),
                DueDate = faker.Date.Future(1),
                PoNo = "PO-" + faker.Random.AlphaNumeric(6).ToUpper(),
                GrNo = "GR-" + faker.Random.AlphaNumeric(6).ToUpper(),
                Currency = dto.Currency,
                NetAmount = dto.NetAmount,
                TaxAmount = dto.TaxAmount,
                TotalAmount = dto.TotalAmount,
                TaxCodeID = dto.TaxCodeID,
                PaymentTerm = "30",
                Note = "-- FAKE GENERATED ARCHIVE INVOICE --",
                ApproverRole = null,
                ApprovedUser = null,
                QueueType = dto.QueueType ?? InvoiceQueueType.ArchiveQueue,
                StatusType = dto.StatusType ?? InvoiceStatusType.Archived,
                ImageID = null
            };

            invoice.SetAuditFieldsOnCreate(createdBy);
            return invoice;
        }
    }
}