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
    }

    public static class EsParser
    {
        public static EsKnowledge ParseKnowledge(string s)
        {
            var rules = s.Split(new[] {"<если/>"}, StringSplitOptions.None)
                .Skip(1)
                .Select(ParseRule)
                .Where(x => x != null);

            return new EsKnowledge(rules);
        }

        public static EsRule ParseRule(string s)
        {
            var parts = s.Split(new[] { "<то/>" }, StringSplitOptions.None);

            var sourceProperties = parts[0]
                .Split(new[] { ";", "\n", "," }, StringSplitOptions.None)
                .Select(ParseProperty)
                .Where(x => x != null);

            var resultProperty = ParseProperty(parts[1]);

            return new EsRule(sourceProperties, resultProperty);
        }

        public static EsProperty ParseProperty(string s)
        {
            if (!s.Contains("="))
            {
                return null;
            }

            var parts = s.Split('=')
                .Select(x => x.Trim())
                .ToArray();

            return new EsProperty(parts[0], parts[1]);
        }
    }
}
