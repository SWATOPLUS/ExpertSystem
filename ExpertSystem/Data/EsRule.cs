using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpertSystem.Data
{
    public class EsRule
    {
        public IReadOnlyList<EsProperty> SourceProperties { get; }
        public EsProperty ResultProperty { get; }

        public EsRule(IEnumerable<EsProperty> sourceProperties, EsProperty resultProperty)
        {
            SourceProperties = sourceProperties.ToArray();
            ResultProperty = resultProperty;
        }

        public static EsRule Parse(string s)
        {
            var parts = s.Split(new[] { "<то/>" }, StringSplitOptions.None);

            var sourceProperties = parts[0]
                .Split(new[] {";", "\n", ","}, StringSplitOptions.None)
                .Select(EsProperty.Parse)
                .Where(x => x != null);

            var resultProperty = EsProperty.Parse(parts[1]);

            return new EsRule(sourceProperties, resultProperty);
        }
    }
}