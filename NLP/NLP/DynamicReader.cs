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
            Tuple<int, int> lastLoc = new Tuple<int, int>(0,0);
            Tuple<int, int> currLoc = new Tuple<int, int>(0,0);
            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey(); 
                if (info.Key == ConsoleKey.Spacebar)
                {
                    //Console.Write(" ");
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
                    lastLoc = currLoc;
                    currLoc = new Tuple<int, int>(Console.CursorLeft, Console.CursorTop);
                }
                else if(info.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else if (Char.IsNumber(info.KeyChar))
                {
                    Writer.ReWriteWord(Convert.ToInt32(info.KeyChar.ToString()), lastLoc);
                    currLoc = new Tuple<int, int>(Console.CursorLeft, Console.CursorTop);

                }
                else if (info.Key == ConsoleKey.Enter || info.Key == ConsoleKey.Tab)
                { }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if(word.Length > 0)
                        word = word.Remove(word.Length - 1);
                    Writer.Backspace();
                }
                else if (Char.IsLetter((info.KeyChar)))
                {
                   word += info.KeyChar;
                }
            }
        }
    }
}
