using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.Keywords;
using CbsAp.Domain.Entities.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSAP.ValidationEngine.Rules
{
    public class RoutingFlowLinkToKeywordRule : IValidationRule
    {
        public string Name => "RoutingFlowLinkToKeywordRule";

     
        public string? EntityProfileIDField { get; set; }
        public string? InvRoutingFlowIDField { get; set; }
        public string? SupplierKeyField { get; set; }
        public string? InvKeywordIDField { get; set; }

        public string? SupplierReferenceSourceKey { get; set; }
        public string? EntityContextKey { get; set; }
       
        public string? RoutingFlowContextKey { get; set; }
        public string? KeywordContextKey { get; set; }
        public string? InvoiceReferenceSourceKey { get; set; }


        public EngineValidationResult Validate(object context, IDictionary<string, object>? runtimeContext = null)
        {
            var result = new EngineValidationResult();

            if (context == null || runtimeContext == null || string.IsNullOrWhiteSpace(InvoiceReferenceSourceKey))
                throw new ArgumentNullException(nameof(context));

            var type = context.GetType();

            var invoiceEntityProfileID = type.GetProperty(EntityProfileIDField!)?.GetValue(context);
            var invoiceSupplierInfoID = type.GetProperty(SupplierKeyField!)?.GetValue(context);
            var invoiceKeywordID = type.GetProperty(InvKeywordIDField!)?.GetValue(context);


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



            if (!runtimeContext.TryGetValue(KeywordContextKey!, out var keywordData))

                return EngineValidationResult.Success();

            var invKeyword = keywordData as IEnumerable<object>;
            if (invKeyword == null) return EngineValidationResult.Success();


            var keyword = invKeyword.FirstOrDefault(s =>
            {
                var keywordID = s.GetType().GetProperty(InvKeywordIDField!)?.GetValue(s);
                if (keywordID == null) return false;

                return  keywordID!.Equals(invoiceKeywordID);

            }

          ) as Keyword;

            if (keyword != null)
            {
                result.RelatedRelationshipIds["InvoiceRoutingFlowIDLinkedToKeyword"] = keyword.InvoiceRoutingFlowID;
            }

            return result;
        }
    }
}
