using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSAP.ValidationEngine.Rules
{
    public class RoutingFlowLinkToSupplierRule : IValidationRule
    {
        public string Name => "RoutingFlowLinkToSupplierRule";


        public string? EntityProfileIDField { get; set; }
        public string? InvRoutingFlowIDField { get; set; }
        public string? SupplierKeyField { get; set; }

        public string? SupplierReferenceSourceKey { get; set; }
        public string? EntityContextKey { get; set; }

        public string? RoutingFlowContextKey { get; set; }
        public string? InvoiceReferenceSourceKey { get; set; }


        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            var result = new EngineValidationResult();

            if (context == null || runtimeContext == null || string.IsNullOrWhiteSpace(InvoiceReferenceSourceKey))
                throw new ArgumentNullException(nameof(context));

            var type = context.GetType();

            var invoiceEntityProfileID = type.GetProperty(EntityProfileIDField!)?.GetValue(context);
            var invoiceSupplierInfoID = type.GetProperty(SupplierKeyField!)?.GetValue(context);

            if (!runtimeContext.TryGetValue(InvoiceReferenceSourceKey!, out var invoiceData))

                return EngineValidationResult.Success();
            var invoices = invoiceData as IEnumerable<object>;

            if (!runtimeContext.TryGetValue(SupplierReferenceSourceKey!, out var supplierData))

                return EngineValidationResult.Success();

            var suppliers = supplierData as IEnumerable<object>;
            if (suppliers == null) return EngineValidationResult.Success();



            if (!runtimeContext.TryGetValue(EntityContextKey!, out var entityProfileData))

                return EngineValidationResult.Success();

            var entityProfile = entityProfileData as IEnumerable<object>;
            if (entityProfile == null) return EngineValidationResult.Success();


            if (!runtimeContext.TryGetValue(RoutingFlowContextKey!, out var invRoutingFlowData))

                return EngineValidationResult.Success();

            var invRoutingFlow = invRoutingFlowData as IEnumerable<object>;
            if (invRoutingFlow == null) return EngineValidationResult.Success();



            var routingFlow = invRoutingFlow.FirstOrDefault(s =>
            {
                var supplierInfoId = s.GetType().GetProperty(SupplierKeyField!)?.GetValue(s);

                if (supplierInfoId == null) return false;

                var entityProfileID = s.GetType().GetProperty(EntityProfileIDField!)?.GetValue(s);

                if (supplierInfoId == null) return false;

                if (entityProfileID == null) return false;

                return supplierInfoId!.Equals(invoiceSupplierInfoID) && entityProfileID!.Equals(invoiceEntityProfileID);

            }

          ) as InvRoutingFlow;

            if (routingFlow != null && suppliers != null)
            {
                var supplierInfo = suppliers
                    .OfType<SupplierInfo>() // ensure correct type
                    .FirstOrDefault(s =>
                    {
                        // Ensure field names are not null
                        if (string.IsNullOrWhiteSpace(SupplierKeyField) ||
                            string.IsNullOrWhiteSpace(EntityProfileIDField) ||
                            string.IsNullOrWhiteSpace(InvRoutingFlowIDField))
                            return false;

                        // Get property values safely
                        var supplierInfoIdProp = s.GetType().GetProperty(SupplierKeyField);
                        var entityProfileIdProp = s.GetType().GetProperty(EntityProfileIDField);
                        var invRoutingFlowIdProp = s.GetType().GetProperty(InvRoutingFlowIDField);

                        if (supplierInfoIdProp == null || entityProfileIdProp == null || invRoutingFlowIdProp == null)
                            return false; // property does not exist

                        var supplierInfoId = supplierInfoIdProp.GetValue(s);
                        var entityProfileID = entityProfileIdProp.GetValue(s);
                        var invRoutingFlowID = invRoutingFlowIdProp.GetValue(s);

                        // null checks for values
                        if (supplierInfoId == null || entityProfileID == null || invRoutingFlowID == null)
                            return false;

                        // final comparison
                        return supplierInfoId.Equals(invoiceSupplierInfoID) &&
                               entityProfileID.Equals(invoiceEntityProfileID) &&
                               invRoutingFlowID.Equals(routingFlow.InvRoutingFlowID);
                    });

                if (supplierInfo != null)
                {
                    // assign safely
                    result.RelatedRelationshipIds["InvoiceRoutingFlowIDLinkedToSupplier"] = supplierInfo.InvRoutingFlowID;
                }
            }

            return result;
        }
    }
}
