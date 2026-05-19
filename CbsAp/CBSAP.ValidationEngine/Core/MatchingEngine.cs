using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CBSAP.ValidationEngine.Core
{
    public class MatchingEngine<T1,T2>
    {
        private readonly List<IMatchingRule<T1,T2>> _rules = new();

        public void AddRule(IMatchingRule<T1,T2> rule) => _rules.Add(rule);
        public void ClearRule() => _rules.Clear();


        //var matchedHeaders = headerEngine.ExecuteMatch(poHeaders, invoiceHeaders, (po, inv) =>
        //po.PONumber == inv.RelatedPONumber &&
        //po.VendorId == inv.VendorId &&
        //po.TotalAmount == inv.TotalAmount);
        public List<(T1 Left, T2 Right)> ExecuteMatch(List<T1> leftSource,List<T2> rightSource,
        Func<T1, T2, bool> matchCriteria)
        {
            var matchedPairs = new List<(T1, T2)>();
            var remainingRight = rightSource.ToList(); 

            foreach (var left in leftSource)
            {
                var match = remainingRight.FirstOrDefault(right => matchCriteria(left, right));

                if (match != null)
                {
                    matchedPairs.Add((left, match));
                    remainingRight.Remove(match); 
                }
            }

            return matchedPairs;
        }

      
        public List<(T1 Left, T2 Right)> ExecuteMatch(List<T1> leftSource, List<T2> rightSource)
        {
            var matchedPairs = new List<(T1, T2)>();
            var availableRight = rightSource.Select((item, index) => new { item, index }).ToList();
            var remainingLeft = new List<(T1 Item, int Index)>();

            //Prioritize exact index matches
            for (int i = 0; i < leftSource.Count && i < rightSource.Count; i++)
            {
                var left = leftSource[i];
                var right = rightSource[i];

                if (_rules.All(rule => rule.IsMatch(left, right)))
                {
                    matchedPairs.Add((left, right));
                    availableRight.RemoveAll(r => r.index == i);
                }
                else
                {
                    remainingLeft.Add((left, i));
                }
            }

            // Greedily match remaining items
            foreach (var left in remainingLeft)
            {
                var match = availableRight.FirstOrDefault(r =>
                    _rules.All(rule => rule.IsMatch(left.Item, r.item)));

                if (match != null)
                {
                    matchedPairs.Add((left.Item, match.item));
                    availableRight.Remove(match);
                }
            }
            return matchedPairs;
        }
    }
}
