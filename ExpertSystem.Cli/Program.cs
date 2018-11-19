using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExpertSystem.Data;

namespace ExpertSystem.Cli
{
    internal static class Program
    {
        private static void Main()
        {
            var knowledge = EsKnowledge.Parse(File.ReadAllText("knowledge.txt"));

            var engine = new InferenceEngine(knowledge, (x, y) => Ask(x, y));

            Console.WriteLine("Ваш вопрос:");
            var target = Console.ReadLine();

            var result = engine.AnalyzeAsync(target).GetAwaiter().GetResult();

            if (result == null)
            {
                Console.WriteLine("Невозможно установить результат!");
            }
            else
            {
                Console.WriteLine($"Результат: {result}. ({target})");
            }
        }

        private static string Ask(string property, IReadOnlyList<string> variants)
        {
            Console.WriteLine($"Укажите: {property}");
            Console.WriteLine(string.Join("\n", variants.Select((x, i) => $"{i + 1}. {x}")));
            Console.Write(">");
            int.TryParse(Console.ReadLine(), out var index);
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            if (index == 0)
            {
                Console.WriteLine("Ваш ответ не определен");
                return string.Empty;
            }

            Console.WriteLine($"Ваш ответ: {variants[index - 1]}");
            return variants[index - 1];
        }
    }
}
