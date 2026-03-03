using CBSAP.ValidationEngine.Rules;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CBSAP.ValidationEngine.Core
{
    public static class ValidationRuleFactory
    {
        public static List<IValidationRule> Load(string jsonPath)
        {
            var json = File.ReadAllText(jsonPath);
            var root = JsonDocument.Parse(json).RootElement;

            JsonElement array = root.TryGetProperty("rules", out var rulesProp)
                    ? rulesProp
                    : throw new Exception("Missing 'rules' array in JSON.");
            var rules = new List<IValidationRule>();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };

            foreach (var item in array.EnumerateArray())
            {
                var type = item.GetProperty("type").GetString();

                switch (type)
                {
                    case "RejectInvoiceRule":
                        rules.Add(JsonSerializer.Deserialize<RejectDuplicatesRules>(item.GetRawText(), options)!);
                        Console.WriteLine(item.GetRawText());
                        break;

          

                    case "PotentialDuplicateRules":
                        rules.Add(JsonSerializer.Deserialize<PotentialDuplicateRules>(item.GetRawText(), options)!);
                        break;

                    case "RequiredFieldRule":
                        rules.Add(JsonSerializer.Deserialize<RequiredFieldRule>(item.GetRawText(), options)!);
                        break;

                    case "SupplierIDActiveDeleteRule":
                        rules.Add(JsonSerializer.Deserialize<SupplierActiveDeleteRule>(item.GetRawText(), options)!);
                        break;

                    case "SupplierInValidForEntityRule":
                        rules.Add(JsonSerializer.Deserialize<SupplierInvalidForEntityRule>(item.GetRawText(), options)!);
                        break;

                    case "InvoiceDateNotnFutureRule":
                        rules.Add(JsonSerializer.Deserialize<InvoiceDateNotnFutureRule>(item.GetRawText(), options)!);
                        break;

                    case "ZeroEmptyTotalAmountRule":
                        rules.Add(JsonSerializer.Deserialize<ZeroEmptyTotalAmountRule>(item.GetRawText(), options)!);
                        break;

                    case "SupplierTaxCodeMatchRule":
                        rules.Add(JsonSerializer.Deserialize<SupplierTaxCodeMatchRule>(item.GetRawText(), options)!);
                        break;

                    case "SupplierTaxCodeRateMatchRule":
                        rules.Add(JsonSerializer.Deserialize<SupplierTaxCodeRateMatchRule>(item.GetRawText(), options)!);
                        break;


                    case "RoutingFlowLinkToSupplierRule":
                        rules.Add(JsonSerializer.Deserialize<RoutingFlowLinkToSupplierRule>(item.GetRawText(), options)!);
                        break;

                    case "RoutingFlowLinkToKeywordRule":
                        rules.Add(JsonSerializer.Deserialize<RoutingFlowLinkToKeywordRule>(item.GetRawText(), options)!);
                        break;
                    case "MissingRoutingLevelRule":
                        rules.Add(JsonSerializer.Deserialize<MissingRoutingLevelRule>(item.GetRawText(), options)!);
                        break;
                }
            }

            return rules;
        }
    }
}