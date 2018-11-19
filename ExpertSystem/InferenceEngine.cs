using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpertSystem.Data;

namespace ExpertSystem
{
    public class InferenceEngine
    {
        private readonly EsKnowledge _knowledge;
        private readonly Func<string, IReadOnlyList<string>, Task<string>> _questionFuncAsync;

        public InferenceEngine(EsKnowledge knowledge, Func<string, IReadOnlyList<string>, string> questionFunc)
        {
            _knowledge = knowledge;
            _questionFuncAsync = (x, y) => Task.FromResult(questionFunc(x, y));
        }

        public InferenceEngine(EsKnowledge knowledge, Func<string, IReadOnlyList<string>, Task<string>> questionFunc)
        {
            _knowledge = knowledge;
            _questionFuncAsync = questionFunc;
        }

        public async Task<string> AnalyzeAsync(string targetProperty)
        {
            var skippedRules = new HashSet<EsRule>(EqualityComparer<EsRule>.Default);
            var facts = new Dictionary<string, string>();
            var targetStack = new Stack<string>();
            targetStack.Push(targetProperty);

            while (targetStack.Any())
            {
                var property = targetStack.Peek();

                var inferenceRule = _knowledge.RulesByResultPropertyName[property]
                    .Except(skippedRules)
                    .FirstOrDefault();

                if (inferenceRule != null)
                {
                    var matchResult = inferenceRule.IsMatch(facts);

                    if (matchResult.IsMatch)
                    {
                        targetStack.Pop();
                        facts[property] = inferenceRule.ResultProperty.Value;
                    }
                    else if (matchResult.IsNotMatch)
                    {
                        skippedRules.Add(inferenceRule);
                    }
                    else //if unknown
                    {
                        targetStack.Push(matchResult.UnknownProperty);
                    }
                }
                else
                {
                    if (property == targetProperty)
                    {
                        return null;
                    }

                    facts[property] = await _questionFuncAsync(property, _knowledge.PropertyPossibleValues[property]);
                    targetStack.Pop();
                }
            }

            return facts[targetProperty];
        }
    }
}


