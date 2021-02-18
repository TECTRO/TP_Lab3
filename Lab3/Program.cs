using Standard.Data.StringMetrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab3
{
    class Program
    {
        static string wrong_symb = "./,=-+!&?_";
        const int tablePadding = 12;
        static void Main(string[] args)
        {
            var strings = new[] {
                "мама мыла раму",
                "мама мыла раму",
                "мама мыла раму мылом",
                "мамы мыли рамы",
                "папы мыли рамы",
                "мыла мама раму",
                "мыла мама раму мылом, мелом и порошком"
            };
            strings = strings.Select(t => new string(t.ToLower().Where(c => !wrong_symb.Contains(c)).ToArray())).ToArray();
            var corpus = string.Join(" ", strings).Split(" ".ToCharArray());
            var terms = corpus.GroupBy(word => word).Select(gr => (gr.Key, val: (float)gr.Count() / corpus.Length)).ToList();

            Console.WriteLine(string.Join("\n", terms.Select(t => $" {t.Key,-10} частота: {t.val,0:f3}")));

            (new WordCloud.WordCloud(800, 600)).Draw(terms.Select(t => t.Key).ToList(), terms.Select(t => (int)t.val * corpus.Length).ToList()).Save("Cloud.jpg");
            Console.WriteLine("\n Облако слов создано\n");

            var tf_idf = terms.Select(term => (term.Key, term.val * (strings.Length / (float)strings.Where(doc => doc.Contains(term.Key)).Count())));
            Console.WriteLine(string.Join("\n", tf_idf.Select(val => $" werb:{val.Key,-10} TF-IDF:{val.Item2,0:f3}")));

            Console.WriteLine("\n" + string.Join("\n", terms.OrderByDescending(t => t.val).Take(10)));

            var similarity = new CosineSimilarity();
            var simMatrix = strings.Select(t => strings.Select(tt => similarity.GetSimilarity(tt, t)).ToList()).ToList();
            Console.WriteLine(string.Join("\n", simMatrix.Select((row,ind) => $"{$"документ {ind + 1}",tablePadding}" + string.Join("", row.Select(item => $"{item,tablePadding:f3}"))).Prepend(string.Join("", strings.Select((val, ind) => $"{$"Документ {ind + 1}",tablePadding}").Prepend($"{"",tablePadding}")))));
            Console.ReadKey();
        }
    }
}
