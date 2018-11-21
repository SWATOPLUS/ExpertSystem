import { List } from 'linqts';

export class EsKnowledge {
    Rules: EsRule[];
    PropertyNames: string[];
    RulesByResultPropertyName: { [key: string]: EsRule[] };
    PropertyPossibleValues: { [key: string]: string[] };

    constructor(rules: EsRule[]) {
        this.Rules = rules;
        this.PropertyNames = new List(Object.assign([], rules))
            .SelectMany(x => {
                const list = new List<EsProperty>(Object.assign([], x.SourceProperties)).Select(y => y.Name);
                list.Add(x.ResultProperty.Name);
                return list;
            })
            .Distinct()
            .ToArray();

        const rulesByResultPropertyName = new List(Object.assign([], rules))
            .GroupBy(x => x.ResultProperty.Name, x => x);

        new List(this.PropertyNames)
            .Except(new List(Object.keys(rulesByResultPropertyName)))
            .ToList()
            .ForEach(x => rulesByResultPropertyName[x] = []);

        this.RulesByResultPropertyName = rulesByResultPropertyName;

        const relatedProperties = new List(Object.assign([], rules))
            .SelectMany(x => {
                const list = new List<EsProperty>(Object.assign([], x.SourceProperties));
                list.Add(x.ResultProperty);
                return list;
            })
            .GroupBy(x => x.Name, x => x);

        this.PropertyPossibleValues = {};

        for (const property in relatedProperties) {
            if (relatedProperties.hasOwnProperty(property)) {
                this.PropertyPossibleValues[property] =
                    new List<EsProperty>(Object.assign([], relatedProperties[property])).Select(y => y.Value).Distinct().ToArray();
            }
        }
    }
}

export class EsRule {
    constructor(public SourceProperties: EsProperty[], public ResultProperty: EsProperty) {
    }

    IsMatch(facts: { [key: string]: string; }): any {
        for (let i = 0; i < this.SourceProperties.length; i++) {
            const property = this.SourceProperties[i];

            const realValue = facts[property.Name];

            if (realValue === null || realValue === undefined) {
                return MatchResult.Undefined(property.Name);
            }

            if (realValue !== property.Value) {
                return MatchResult.NotMatch;
            }
        }

        return MatchResult.Match;
    }
}

export class EsProperty {
    constructor(public Name: string, public Value: string) {
    }
}

export class EsParser {
    public static ParseKnowledge(s: string): EsKnowledge {
        const rules = new List(s.split('<если/>'))
            .Skip(1)
            .Select(x => this.ParseRule(x))
            .Where(x => x !== null || x !== undefined)
            .ToArray();
        return new EsKnowledge(rules);
    }

    public static ParseRule(s: string): EsRule {
        const parts = s.split('<то/>');
        const sourceProperties = new List(parts[0].replace(/\n/g, ';').split(';'))
            .Where(x => !!x)
            .Select(x => this.ParseProperty(x))
            .Where(x => x !== null || x !== undefined)
            .ToList();
        const resultProperty = EsParser.ParseProperty(parts[1]);

        const sources = [];
        sourceProperties.ForEach( (x, i) => {
            if (i < sourceProperties.Count()) {
             sources.push(x);
            }
        });
        return new EsRule(sources, resultProperty);
    }

    public static ParseProperty(s: string): EsProperty {
        if (s.indexOf('=') < 0) {
            return null;
        }
        const parts = new List(s.split('='))
            .Select(x => x.trim())
            .ToArray();
        return new EsProperty(parts[0], parts[1]);
    }
}

export class MatchResult {
    public static get Match(): MatchResult {
        return new MatchResult(true);
    }
    public static get NotMatch(): MatchResult {
        return new MatchResult(false);
    }
    public static Undefined(unknownProperty: string): MatchResult {
        return new MatchResult(null, unknownProperty);
    }
    public constructor(private matching: any, private unknownProperty: string = null) {
    }
    public get Matching(): boolean {
        return this.matching;
    }
    public get UnknownProperty(): string {
        return this.unknownProperty;
    }
    public get IsMatch(): boolean {
        return this.matching === true;
    }
    public get IsNotMatch(): boolean {
        return this.matching === false;
    }
    public IsUndefined(unknownProperty: string): boolean {
        return this.matching === null;
    }
}
