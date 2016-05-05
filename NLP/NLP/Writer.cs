using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public static class Writer
    {
        public static int windowW = 120;
        public static int windowH = 50;
        private static int promptCount = 6;                     // number of prompts words to display

        private static List<Tuple<double, string>> promptList = new List<Tuple<double, string>>();
        private static Tuple<int, int> pos = new Tuple<int, int>(0, 0);


        public static void SetCursorCorner()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
        }
        public static void SetCursor(int x, int y)
        {
            Console.CursorLeft = x;
            Console.CursorTop = y;
        }
        public static void SetCursor(Tuple<int, int> coord)
        {
            SetCursor(coord.Item1, coord.Item2);
        }
        public static Tuple<int, int> getCursorLoc()
        {
            return new Tuple<int, int>(Console.CursorLeft, Console.CursorTop);
        }
        public static void ClearLine()
        {
            Tuple<int, int> position = getCursorLoc();
            string whiteSpace = "";
            int x = position.Item1;
            while (x++ < windowW - 1)
            {
                whiteSpace += " ";
            }
            Console.Write(whiteSpace);
            SetCursor(position);
        }
        /// <summary>
        /// Takes List of optimal alternitives to consider
        /// Writes these to top of screen
        /// </summary>
        /// <param name="valuation"></param>
        public static void PrintPostWriteEvaluation(List<Tuple<double, string>> valuation)
        {
            promptList = valuation;
            pos = new Tuple<int, int>(Console.CursorLeft, Console.CursorTop);
            SetCursorCorner();
            ClearLine();
            for (int i = 0; i < promptCount && i < valuation.Count; i++)
            {
                promptList.Add(valuation[i]);
                Console.Write(String.Format("{2}){0}: {1:0.00}  ", valuation[i].Item2, valuation[i].Item1 * 100, (i + 1) % 10));
            }
            SetCursor(pos.Item1, pos.Item2);
        }
        public static void PrintProbabilityDistribution(List<Tuple<double, string>> values, Dictionary<string, double> dist)
        {
            pos = new Tuple<int, int>(Console.CursorLeft, Console.CursorTop);
            SetCursor(0, 1);
            ClearLine();
            for (int i = 0; i < promptCount && i < values.Count; i++)
            {
                Console.Write(String.Format("{2}){0}: {1:0.00}  ", values[i].Item2, dist[values[i].Item2] * 100, (i + 1) % 10));
            }
            SetCursor(pos.Item1, pos.Item2);
        }
        public static void PrintEditDistance(List<Tuple<double, string>> values, List<Tuple<double, string>> edList, string word)
        {
            pos = new Tuple<int, int>(Console.CursorLeft, Console.CursorTop);
            SetCursor(0, 2);
            ClearLine();
            for (int i = 0; i < promptCount && i < values.Count; i++)
            {
                Console.Write(String.Format("{2}){0}: {1:0.00}  ", values[i].Item2, EditDistance.ComputeEditDistanceDP(values[i].Item2, word), (i + 1) % 10));
            }
            SetCursor(pos.Item1, pos.Item2);
        }
        public static void Backspace()
        {
            pos = getCursorLoc();
            if (pos.Item1 != 0) // not on edge
            {
                SetCursor(pos.Item1, pos.Item2);
                Console.Write(" ");
                SetCursor(pos.Item1, pos.Item2);
            }
            else //if on edge now
            {
                SetCursor(windowW - 1, pos.Item2 - 1);
                Console.Write(" ");
                SetCursor(windowW - 1, pos.Item2 - 1);
            }
        }
        public static string ReWriteWord(int keyNumber, Tuple<int, int> start, bool capitalize)
        {
            pos = getCursorLoc();
            SetCursor(start.Item1, start.Item2);
            ClearLine();
            string newWord = promptList[(keyNumber + 9) % 10].Item2;
            if (capitalize)
            {
                newWord = newWord.Substring(0, 1).ToUpper() + newWord.Substring(1, newWord.Length - 1);
            }
            Console.Write(newWord);
            return newWord;
        }

        public static void WriteMetaData(Queue<string> chain, string word)
        {
            pos = getCursorLoc();
            SetCursor(0, windowH - 3);
            ClearLine();

            for (int i = 0; i < chain.Count; i++)
            {
                Console.Write(String.Format("{0}, ", chain.ElementAt(i)));
            }
            Console.Write(String.Format("current word: {0}", word));
            SetCursor(pos);
        }
        public static void PrintCursorInfo(Tuple<int, int> currLoc, Tuple<int, int> lastLoc)
        {
            pos = getCursorLoc();
            SetCursor(0, windowH - 2);
            ClearLine();
            Console.Write(String.Format("pLoc = {0}, {1}", lastLoc.Item1, lastLoc.Item2));
            SetCursor(0, windowH - 1);
            ClearLine();
            Console.Write(String.Format("cLoc = {0}, {1}", currLoc.Item1, currLoc.Item2));
            SetCursor(pos);

        }
    }
}
