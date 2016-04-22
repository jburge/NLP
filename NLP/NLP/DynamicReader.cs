using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public static class DynamicReader
    {
 
        public static void InputLoop(Model model)
        {
            Writer.SetCursor(0, 1);
            Queue<string> chain = new Queue<string>();
            string word = "";
            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey(); 
                if (info.Key == ConsoleKey.Spacebar)
                {
                    Console.Write(" ");
                    if (!model.HasKey(word))
                    {
                        model.AddWord(word);
                    }
                    if (chain.Count == model.getModelDepth())
                        chain.Dequeue();
                    if (chain.Count > 0)
                    {
                        model.DynamicRead(chain, word);
                        
                    }
                    chain.Enqueue(word);
                    word = "";
                }
                else if(info.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else if (info.Key == ConsoleKey.Enter)
                { }
                else
                {
                   word += info.KeyChar;
                }
            }
        }
    }
}
