using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPRefactored
{
    public static class DynamicReader
    {

        public static void InputLoop(Model model)
        {
            Writer.SetCursor(0, 5);
            Queue<string> chain = new Queue<string>();
            string word = "";
            Tuple<int, int> lastLoc = new Tuple<int, int>(0, 0);
            Tuple<int, int> currLoc = new Tuple<int, int>(0, 0);
            string wordBuffer = "";
            bool writingWord = false;
            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey();
                if (info.Key == ConsoleKey.Spacebar)
                {
                    if (!model.HasKey(wordBuffer) && wordBuffer != "")
                    {
                        model.AddWord(wordBuffer);
                    }
                    if (chain.Count == model.getModelDepth())
                        chain.Dequeue();
                    word.ToLower();
                    chain = model.DynamicRead(chain, word);
                    chain.Enqueue(word);
                    wordBuffer = word;
                    word = "";
                    lastLoc = currLoc;
                    currLoc = new Tuple<int, int>(Console.CursorLeft, Console.CursorTop);
                }
                else if (info.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else if (Char.IsNumber(info.KeyChar))
                {
                    Writer.ReWriteWord(Convert.ToInt32(info.KeyChar.ToString()), lastLoc);
                    currLoc = Writer.getCursorLoc();

                }
                else if (info.Key == ConsoleKey.Enter) { }
                else if (info.Key == ConsoleKey.Tab) { }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    //if (word.Length > 0)
                    //    word = word.Remove(word.Length - 1);
                    //Writer.Backspace();
                }
                else if (Char.IsLetter((info.KeyChar)))
                {
                    if(!writingWord)
                    {
                        currLoc = Writer.getCursorLoc();
                    }
                    writingWord = true;
                    word += info.KeyChar;
                }
            }
        }
    }
}
