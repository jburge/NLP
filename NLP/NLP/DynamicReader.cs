using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public static class DynamicReader
    {
 
        public static void InputLoop(Model2 model)
        {
            Queue<string> chain = new Queue<string>();
            string word = "";
            while (true)
            {
                char key = Console.ReadKey().KeyChar;
                if (key == ' ')
                {
                    Console.Write(word + " ");
                    if (chain.Count == model.getModelDepth())
                        chain.Dequeue();
                    if (chain.Count > 0)
                    {
                        List<Tuple<double, string>> test = model.EvaluateState(chain, word);
                        foreach (Tuple<double, string> t in test)
                        {
                            Console.WriteLine(t.Item2 + ": " + t.Item1);
                        }
                    }
                    chain.Enqueue(word);
                    word = "";
                }
                else
                {
                    word += key;
                }
            }
        }
    }
}
