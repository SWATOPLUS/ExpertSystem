using System;
using System.Collections.Generic;
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
            var state = new InferenceState(_knowledge, targetProperty);

            while (true)
            {
                state.Process();

                if (state.IsCompleted)
                {
                    return state.Result;
                }

                var factValue = await _questionFuncAsync(state.UnknownFact, state.UnknownFactVariants);

                state.PushFactValue(factValue);
            }
        }
    }
}


