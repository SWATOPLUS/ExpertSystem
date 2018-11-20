using System.Collections.Generic;
using System.Linq;
using ExpertSystem.Data;

namespace ExpertSystem
{
    public class InferenceState
    {
        private readonly EsKnowledge _knowledge;
        private readonly Dictionary<string, string> _facts;
        private readonly HashSet<EsRule> _skippedRules;
        private readonly Stack<string> _targetStack;

        public string TargetProperty { get; }

        public string Result { get; private set; }

        public bool IsCompleted { get; private set; }

        public string UnknownFact { get; private set; }

        public IReadOnlyList<string> UnknownFactVariants { get; private set; }

        public InferenceState(EsKnowledge knowledge, string targetProperty)
        {
            _facts = new Dictionary<string, string>();
            _knowledge = knowledge;
            _skippedRules = new HashSet<EsRule>(EqualityComparer<EsRule>.Default);
            _targetStack = new Stack<string>();
            _targetStack.Push(targetProperty);
            TargetProperty = targetProperty;
        }

        public void Process()
        {
            while (_targetStack.Any())
            {
                var property = _targetStack.Peek();

                var inferenceRule = _knowledge.RulesByResultPropertyName[property]
                    .Except(_skippedRules)
                    .FirstOrDefault();

                if (inferenceRule != null)
                {
                    var matchResult = inferenceRule.IsMatch(_facts);

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

        public void PushFactValue(string value)
        {
            var property = _targetStack.Pop();
            _facts[property] = value;
        }
    }
}