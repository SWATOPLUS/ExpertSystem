using System.Linq;

namespace ExpertSystem.Data
{
    public class EsProperty
    {
        public EsProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }

        public static EsProperty Parse(string s)
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