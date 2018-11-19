using System.Collections.Generic;
using ExpertSystem.Data;

namespace ExpertSystem
{
    public static class EsExtensions
    {
        public static MatchResult IsMatch(this EsRule rule, IReadOnlyDictionary<string, string> facts)
        {
            foreach (var property in rule.SourceProperties)
            {
                if (facts.TryGetValue(property.Name, out var realValue))
                {
                    if (realValue != property.Value)
                    {
                        return MatchResult.NotMatch;
                    }
                }
                else
                {
                    return MatchResult.Undefined(property.Name);
                }
            }

            return MatchResult.Match;
        }
    }
}