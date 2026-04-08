using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.InvInfoRoutingLevel;
using CbsAp.Application.DTOs.Invoicing.Invoice;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Entities.Supplier;
using CbsAp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbsAp.Application.Features.Invoicing.InvActions.Helpers
{
    internal static class RoutingFlowHelper
    {
        public static void ApplyRoutingFlowLogic(IUnitofWork unitOfWork, Invoice invoice, InvoiceDto dto)
        {
            if (dto.InvRoutingFlowID != invoice.InvRoutingFlowID && invoice.QueueType != InvoiceQueueType.MyInvoices)
            {
                invoice.InvRoutingFlowID = dto.InvRoutingFlowID;
            }
            else if (string.IsNullOrEmpty(dto.InvRoutingFlowName) && invoice.QueueType != InvoiceQueueType.MyInvoices)
            {
                // Try keyword routing
                var keywordRouting = unitOfWork.GetRepository<Keyword>()
                    .Query()
                    .FirstOrDefault(a => a.KeywordID == dto.KeywordID);

                if (keywordRouting != null)
                    invoice.InvRoutingFlowID = keywordRouting.InvoiceRoutingFlowID;
                else
                {
                    // Try supplier routing
                    var supplierRouting = unitOfWork.GetRepository<SupplierInfo>()
                        .Query()
                        .FirstOrDefault(w => w.SupplierInfoID == dto.SupplierInfoID);

                    if (supplierRouting != null)
                        invoice.InvRoutingFlowID = supplierRouting.InvRoutingFlowID;
                }
            }
        }

        public static void UpdateRoutingFlowLevels(IUnitofWork unitOfWork, Invoice invoice, InvoiceDto dto, InvoiceQueueType? prevQueue, bool routingFlowChanged)
        {
            var existingRoutingLevels = invoice.InvInfoRoutingLevels!.ToList();

            // Regenerate only if routing flow changed
            if (routingFlowChanged)
            {
                dto.InvInfoRoutingLevels = unitOfWork.GetRepository<InvRoutingFlowLevels>()
                    .Query()
                    .Where(w => w.InvRoutingFlowID == invoice.InvRoutingFlowID)
                    .Select(s => new InvInfoRoutingLevelDto
                    {
                        InvRoutingFlowID = s.InvRoutingFlowID,
                        InvoiceID = invoice.InvoiceID,
                        SupplierInfoID = invoice.SupplierInfoID,
                        KeywordID = invoice.KeywordID,
                        RoleID = s.RoleID,
                        Level = s.Level
                    }).ToList();
            }

            var mappedRoutingLevels = dto.InvInfoRoutingLevels
                .Select(x => new InvInfoRoutingLevel
                {
                    InvoiceID = x.InvoiceID,
                    InvInfoRoutingLevelID = x.InvInfoRoutingLevelID,
                    InvRoutingFlowID = x.InvRoutingFlowID,
                    Level = x.Level,
                    RoleID = x.RoleID,
                }).ToList();

            var incomingRoutingIds = mappedRoutingLevels
                .Where(x => x.InvInfoRoutingLevelID != 0)
                .Select(x => x.InvInfoRoutingLevelID)
                .ToHashSet();

            var routingToAdd = mappedRoutingLevels
                .Where(x => x.InvInfoRoutingLevelID == 0 || !existingRoutingLevels.Any(e => e.InvInfoRoutingLevelID == x.InvInfoRoutingLevelID))
                .ToList();

            var routingToUpdate = mappedRoutingLevels
                .Where(x => x.InvInfoRoutingLevelID != 0 && existingRoutingLevels.Any(e => e.InvInfoRoutingLevelID == x.InvInfoRoutingLevelID))
                .ToList();

            var routingToDelete = existingRoutingLevels
                .Where(x => !incomingRoutingIds.Contains(x.InvInfoRoutingLevelID))
                .ToList();

            UpdateRoutingLevels(existingRoutingLevels, routingToUpdate);

            if (prevQueue != InvoiceQueueType.MyInvoices)
            {
                AddRoutingLevels(invoice, routingToAdd);
                unitOfWork.GetRepository<InvInfoRoutingLevel>().RemoveRangeAsync(routingToDelete).Wait();
            }
        }

        private static void UpdateRoutingLevels(List<InvInfoRoutingLevel> existingRoutingLevels, List<InvInfoRoutingLevel> updatedRoutingLevels)
        {
            foreach (var updated in updatedRoutingLevels)
            {
                var existing = existingRoutingLevels.First(e => e.InvInfoRoutingLevelID == updated.InvInfoRoutingLevelID);
                existing.InvRoutingFlowID = updated.InvRoutingFlowID == 0 ? null : updated.InvRoutingFlowID;
                existing.Level = updated.Level;
                existing.RoleID = updated.RoleID;
            }
        }

        private static void AddRoutingLevels(Invoice invoice, List<InvInfoRoutingLevel> newItems)
        {
            if (invoice.InvInfoRoutingLevels == null)
                invoice.InvInfoRoutingLevels = new List<InvInfoRoutingLevel>();

            foreach (var item in newItems)
            {
                invoice.InvInfoRoutingLevels.Add(new InvInfoRoutingLevel
                {
                    InvoiceID = invoice.InvoiceID,
                    InvRoutingFlowID = invoice.InvRoutingFlowID,
                    Level = item.Level,
                    RoleID = item.RoleID,
                    KeywordID = invoice.KeywordID,
                    SupplierInfoID = invoice.SupplierInfoID,
                    InvFlowStatus = invoice.QueueType == InvoiceQueueType.MyInvoices
                        ? item.Level == 1 ? (int?)InvFlowStatus.Assigned : (int)InvFlowStatus.Pending
                        : (int?)InvFlowStatus.Pending
                });
            }
        }
    }

}