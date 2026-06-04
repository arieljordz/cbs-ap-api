using CBSAP.ValidationEngine.Rules;

namespace CBSAP.ValidationEngine.Core
{
    public class ValidationEngine
    {
        private readonly IEnumerable<IValidationRule> _rules;

        public ValidationEngine(IEnumerable<IValidationRule> rules)
        {
            _rules = rules;
        }

        public List<EngineValidationResult> Validate(object context, IDictionary<string, object> runtimeContext, out bool stopEarly)
        {
            var allFailures = new List<EngineValidationResult>();
            stopEarly = false;
          

            foreach (var rule in _rules)
            {
                var result = rule.Validate(context, runtimeContext);

                foreach (var kvp in result.RelatedRelationshipIds) {

                    result.RelatedRelationshipIds[kvp.Key] = kvp.Value;
                }

                if (!result.IsSuccess)
                {
                    allFailures.Add(result);

                    if (result.Severity == EngineValidationSeverity.Critical)
                    {
                        stopEarly = true;
                        break;
                    }
                }
            }
            return allFailures;
        }
    }
}