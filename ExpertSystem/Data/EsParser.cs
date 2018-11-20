using System;
using System.Linq;

namespace ExpertSystem.Data
{
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