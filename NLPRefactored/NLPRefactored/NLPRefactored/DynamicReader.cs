using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Text.RegularExpressions;

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
                currLoc = Writer.getCursorLoc();
                Writer.PrintCursorInfo(currLoc, lastLoc);
                ConsoleKeyInfo info = Console.ReadKey();
                if (info.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else if (Char.IsNumber(info.KeyChar))
                {
                    if (!writingWord)
                    {
                        wordBuffer = Writer.ReWriteWord(Convert.ToInt32(info.KeyChar.ToString()), lastLoc, wordBuffer != wordBuffer.ToLower());
                    }
                }
                else if ((Model.terminators.Contains(info.KeyChar) || info.Key == ConsoleKey.Enter) && writingWord)         //end sentence
                {
                    writingWord = false;
                    if (wordBuffer != "")
                    {
                        chain = model.DynamicObserve(chain, wordBuffer.ToLower());
                        wordBuffer = "";
                    }
                    chain = model.DynamicRead(chain, word.ToLower());
                    //reset
                    if (word != "")
                    {
                        chain = model.DynamicObserve(chain, word.ToLower());
                        word = "";
                    }
                    chain = model.ChainPush(chain);
                }
                else if ((Char.IsPunctuation(info.KeyChar) || info.Key == ConsoleKey.Spacebar) && writingWord)              //end word
                {
                    //do space stuff
                    writingWord = false;
                    if (wordBuffer != "")
                    {
                        chain = model.DynamicObserve(chain, wordBuffer.ToLower());
                    }
                    chain = model.DynamicRead(chain, word.ToLower());
                    //reset
                    wordBuffer = word;
                    word = "";
                }
                else if (info.Key == ConsoleKey.Tab) { }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (word.Length > 0)
                        word = word.Remove(word.Length - 1);
                    else
                        wordBuffer = "";
                    Writer.Backspace();
                }
                else if (Char.IsLetter(info.KeyChar))
                {
                    if(!writingWord)
                    {
                        lastLoc = currLoc;
                        writingWord = true;
                    }
                    word += info.KeyChar;
                }
                ///////////////////
                if (ConsoleKey.Enter == info.Key)
                {
                    Console.WriteLine();
                }
            }
        }
    }
}
