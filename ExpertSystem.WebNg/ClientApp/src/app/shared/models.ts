import { List } from 'linqts';

export class EsKnowledge {
    Rules: EsRule[];
    PropertyNames: string[];
    RulesByResultPropertyName: { [key: string]: EsRule[] };
    PropertyPossibleValues: { [key: string]: string[] };

    constructor(rules: EsRule[]) {
        this.Rules = rules;
        this.PropertyNames = new List(rules)
            .SelectMany(x => {
                const list = new List(x.SourceProperties).Select(y => y.Name);
                list.Add(x.ResultProperty.Name);
                return list;
            })
            .Distinct()
            .ToArray();

        const rulesByResultPropertyName = new List(rules)
            .GroupBy(x => x.ResultProperty.Name, x => x)
            .ToDictionary(x => x.Key, x => x.ToArray());

        new List(this.PropertyNames)
            .Except(rulesByResultPropertyName.Keys)
            .ToList()
            .ForEach(x => rulesByResultPropertyName.Add(x, []));

        this.RulesByResultPropertyName = rulesByResultPropertyName;

        this.PropertyPossibleValues = new List(rules)
            .SelectMany(x => {
                const list = new List(x.SourceProperties);
                list.Add(x.ResultProperty);
                return list;
            })
            .GroupBy(x => x.Name, x => x)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Value).Distinct().ToArray());
    }
}

export class EsRule {
    SourceProperties: EsProperty[];
    ResultProperty: EsProperty;
}

export class EsProperty {
    Name: string;
    Value: string;
}

export class EsParser {
    public static ParseKnowledge(s: string): EsKnowledge {
        const rules = new List(s.split('<если/>'))
            .Skip(1)
            .Select(this.ParseRule)
            .Where(x => x != null)
            .ToArray();
        return new EsKnowledge(rules);
    }

    public static ParseRule(s: string): EsRule {
        const parts = s.split('<то/>');
        const sourceProperties = new List(parts[0].replace(/\n/g, ';').split(';'))
        .Select(this.ParseProperty)
        .Where(x => x != null)
        .ToArray();
        const resultProperty = EsParser.ParseProperty(parts[1]);

        const result = new EsRule();
        result.ResultProperty = resultProperty;
        result.SourceProperties = sourceProperties;
        return result;
    }

    public static ParseProperty(s: string): EsProperty {
        if (s.indexOf('=') > -1) {
            return null;
        }

        const parts = new List(s.split('='))
        .Select(x => x.trim())
        .ToArray();

        const result = new EsProperty();
        result.Name = parts[0];
        result.Value = parts[1];
        return result;
    }
}