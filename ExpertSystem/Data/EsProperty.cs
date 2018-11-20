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
    }
}