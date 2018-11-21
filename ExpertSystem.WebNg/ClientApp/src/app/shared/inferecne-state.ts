import { EsKnowledge, EsRule } from './models';
import { Stack } from 'stack-typescript';
import { List } from 'linqts';

export class InferenceState {
    private facts: { [key: string]: string };
    private skippedRules: List<EsRule>;
    private targetStack: Stack<string>;

    TargetProperty: string;

    Result: string;

    IsCompleted: boolean;

    UnknownFact: string;

    UnknownFactVariants: string[];

    constructor(private knowledge: EsKnowledge, private targetProperty: string) {

        this.facts = {};
        this.skippedRules = new List<EsRule>();
        this.targetStack = new Stack<string>();
        this.targetStack.push(targetProperty);
        this.TargetProperty = this.targetProperty;
    }

    public Process() {
        while (this.targetStack.length !== 0) {
            const property = this.targetStack.pop();
            this.targetStack.push(property);
            const inferenceRule = new List(this.knowledge.RulesByResultPropertyName[property])
                .Except(this.skippedRules)
                .FirstOrDefault();
            if (inferenceRule) {
                const matchResult = inferenceRule.IsMatch(this.facts);
                if (matchResult.IsMatch) {
                    this.targetStack.pop();
                    this.facts[property] = inferenceRule.ResultProperty.Value;
                } else if (matchResult.IsNotMatch) {
                    this.skippedRules.Add(inferenceRule);
                } else {
                    this.targetStack.push(matchResult.UnknownProperty);
                }
            } else {
                if (property === this.TargetProperty) {
                    this.Result = null;
                    this.IsCompleted = true;
                } else {
                    this.UnknownFact = property;
                    this.UnknownFactVariants = this.knowledge.PropertyPossibleValues[property];
                }
                return;
            }
        }

        this.IsCompleted = true;
        this.Result = this.facts[this.TargetProperty];
    }

    public PushFactValue(value: string) {
        const property = this.targetStack.pop();
        this.facts[property] = value;
    }
}

