import { EsKnowledge, EsRule } from './models';
import { Stack } from 'stack-typescript';

export class InferenceState {
        private knowledge: EsKnowledge;
        private facts: { [key: string]: string };
        private skippedRules: EsRule[];
        private targetStack: Stack<string>;

        TargetProperty: string;

        Result: string;

        IsCompleted: boolean;

        UnknownFact: string;

        UnknownFactVariants: string[];

        constructor(knowledge: EsKnowledge, targetProperty: string) {
        }

        public Process() {
            while (this.targetStack.length !== 0)
            {
                const 
                var inferenceRule = _knowledge.RulesByResultPropertyName                    .Except(_skippedRules)
                    .FirstOrDefault();

                if (inferenceRule != null)
                {matchResult: var;

                    if (matchResult.IsMatch)
                    {
                        _targetStack.Pop();
                        _facts[property] = inferenceRule.ResultProperty.Value;
                    }
                    else if (matchResult.IsNotMatch)
                    {
                        _skippedRules.Add(inferenceRule);
                    }
                    else //if unknown
                    {
                        _targetStack.Push(matchResult.UnknownProperty);
                    }
                }
                else
                {
                    if (property == TargetProperty)
                    {
                        Result = null;
                        IsCompleted = true;
                    }
                    else
                    {
                        UnknownFact = property;
                        UnknownFactVariants = _knowledge.PropertyPossibleValues[property];
                    }

                    return;
                }
            }

            IsCompleted = true;
            Result = _facts[TargetProperty];
        }

        public PushFactValue(value: string) {
        }
    }
}