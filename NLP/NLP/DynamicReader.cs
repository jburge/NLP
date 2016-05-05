using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Text.RegularExpressions;

namespace NLP
{
    public static class DynamicReader
    {
        private static int topLine = 5;

        private static Tuple<int, int> lastLoc = new Tuple<int, int>(0, 0);
        private static Tuple<int, int> currLoc = new Tuple<int, int>(0, 0);

        private static Queue<string> chain = new Queue<string>();

        private static string word = "";
        private static string wordBuffer = "";

        private static bool writingWord = false;
        //need sentence term bool to wipe quee and buffer

        public static void InputLoop(Model model)
        {
            Writer.SetCursor(0, topLine);
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
                    EndSentence(model);
                }
                else if ((Char.IsPunctuation(info.KeyChar) || info.Key == ConsoleKey.Spacebar) && writingWord)              //end word
                {
                    EndWord(model);
                }
                else if (info.Key == ConsoleKey.Tab) { }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    BackSpace();
                }
                else if (Char.IsLetter(info.KeyChar))
                {
                    if (!writingWord)
                    {
                        lastLoc = currLoc;
                        writingWord = true;
                    }
                    word += info.KeyChar;
                }
                ///////////////////
                if (ConsoleKey.Enter == info.Key)
                {
                    //Console.WriteLine();
                }
            }
        }
        private static void EndSentence(Model model)
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
        private static void EndWord(Model model)
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
        private static void BackSpace()
        {
            if (word.Length > 0)
                word = word.Remove(word.Length - 1);
            else
                wordBuffer = "";
            Writer.Backspace();
        }
    }
}
