using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Domain.Entities.Invoicing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.Invoicing.InvActions.Helpers
{
    internal static class AllocationHelper
    {
        public static void UpdateInvoiceAllocations(IUnitofWork unitOfWork, Invoice invoice, InvoiceDto dto)
        {
            var existingAllocations = invoice.InvoiceAllocationLines!.ToList();

            var mappedAllocations = dto.InvoiceAllocationLines
                .Select(x => new InvAllocLine
                {
                    InvAllocLineID = x.InvAllocLineID,
                    InvoiceID = x.InvoiceID,
                    LineNo = x.LineNo,
                    PoNo = x.PoNo,
                    PoLineNo = x.PoLineNo,
                    Qty = x.Qty,
                    LineDescription = x.LineDescription,
                    Note = x.Note,
                    LineNetAmount = x.LineNetAmount,
                    LineTaxAmount = x.LineTaxAmount,
                    LineAmount = x.LineAmount,
                    TaxCodeID = x.TaxCodeID,
                    AccountID = x.Account,
                }).ToList();

            var incomingAllocIds = mappedAllocations
                .Where(x => x.InvAllocLineID != 0)
                .Select(x => x.InvAllocLineID)
                .ToHashSet();

            var toAdd = mappedAllocations
                .Where(x => x.InvAllocLineID == 0 || !existingAllocations.Any(e => e.InvAllocLineID == x.InvAllocLineID))
                .ToList();

            var toUpdate = mappedAllocations
                .Where(x => x.InvAllocLineID != 0 && existingAllocations.Any(e => e.InvAllocLineID == x.InvAllocLineID))
                .ToList();

            var toDelete = existingAllocations
                .Where(x => !incomingAllocIds.Contains(x.InvAllocLineID))
                .ToList();

            if (toDelete.Any())
                unitOfWork.GetRepository<InvAllocLine>().RemoveRangeAsync(toDelete).Wait();

            UpdateAllocations(existingAllocations, toUpdate);
            existingAllocations.AddRange(toAdd);

            invoice.InvoiceAllocationLines = existingAllocations;
        }

        private static void UpdateAllocations(List<InvAllocLine> existing, List<InvAllocLine> updated)
        {
            foreach (var update in updated)
            {
                var existingItem = existing.First(e => e.InvAllocLineID == update.InvAllocLineID);
                existingItem.LineNo = update.LineNo;
                existingItem.PoNo = update.PoNo;
                existingItem.PoLineNo = update.PoLineNo;
                existingItem.Qty = update.Qty;
                existingItem.LineDescription = update.LineDescription;
                existingItem.Note = update.Note;
                existingItem.LineNetAmount = update.LineNetAmount;
                existingItem.LineTaxAmount = update.LineTaxAmount;
                existingItem.LineAmount = update.LineAmount;
                existingItem.TaxCodeID = update.TaxCodeID;
                existingItem.AccountID = update.AccountID;
            }
        }
    }

}