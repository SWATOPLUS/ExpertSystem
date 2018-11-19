using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpertSystem.Data
{
    public class EsKnowledge
    {
        public IReadOnlyList<EsRule> Rules { get; }

        public IReadOnlyList<string> PropertyNames { get; }

        public IReadOnlyDictionary<string, IReadOnlyList<EsRule>> RulesByResultPropertyName { get; }

        public IReadOnlyDictionary<string, IReadOnlyList<string>> PropertyPossibleValues { get; }

        public EsKnowledge(IEnumerable<EsRule> rules)
        {
            Rules = rules.ToArray();
            PropertyNames = Rules
                .SelectMany(x => x.SourceProperties.Select(y => y.Name).Append(x.ResultProperty.Name))
                .Distinct()
                .ToArray();

            var rulesByResultPropertyName = Rules
                .GroupBy(x => x.ResultProperty.Name)
                .ToDictionary(x => x.Key, x => x.ToArray() as IReadOnlyList<EsRule>);

            PropertyNames
                .Except(rulesByResultPropertyName.Keys)
                .ToList()
                .ForEach(x => rulesByResultPropertyName.Add(x, Array.Empty<EsRule>()));

            RulesByResultPropertyName = rulesByResultPropertyName;

            PropertyPossibleValues = Rules
                .SelectMany(x => x.SourceProperties.Append(x.ResultProperty))
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Value).Distinct().ToArray() as IReadOnlyList<string>);
        }

        public static EsKnowledge Parse(string s)
        {
            var rules = s.Split(new []{"<если/>"}, StringSplitOptions.None).Skip(1).Select(EsRule.Parse).Where(x => x != null);

            return new EsKnowledge(rules);
        }
    }
}
