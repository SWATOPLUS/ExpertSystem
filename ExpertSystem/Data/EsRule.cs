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
    }
}