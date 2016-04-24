using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPRefactored
{
    public static class Writer
    {
        public static int windowW = 120;
        public static int windowH = 50;
        private static int promptCount = 6;                     // number of prompts words to display
        private static List<Tuple<double, string>> promptList = new List<Tuple<double,string>>();
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
            return new Tuple<int, int> (Console.CursorLeft, Console.CursorTop);
        }
        public static void ClearLine()
        {
            Tuple<int, int> pos = getCursorLoc();
            string whiteSpace = "";
            int x = pos.Item1;
            while(x++<windowW)
            {
                whiteSpace+=" ";
            }
            Console.Write(whiteSpace);
            SetCursor(pos);
        }
        /// <summary>
        /// Takes List of optimal alternitives to consider
        /// Writes these to top of screen
        /// </summary>
        /// <param name="valuation"></param>
        public static void PrintPostWriteEvaluation(List<Tuple<double, string>> valuation)
        {
            Tuple<int, int> currLoc = new Tuple<int,int>(Console.CursorLeft, Console.CursorTop);
            SetCursorCorner();
            ClearLine();
            for (int i = 0; i < promptCount && i < valuation.Count; i++)
            {
                promptList.Add(valuation[i]);
                Console.Write(String.Format("{2}){0}: {1:0.00}  ", valuation[i].Item2, valuation[i].Item1 * 100, (i + 1) % 10));
            }
            SetCursor(currLoc.Item1, currLoc.Item2);
        }
        public static void PrintProbabilityDistribution(List<Tuple<double, string>> values, Dictionary<string, double> dist)
        {
            Tuple<int, int> currLoc = new Tuple<int, int>(Console.CursorLeft, Console.CursorTop);
            SetCursor(0,1);
            ClearLine();
            for (int i = 0; i < promptCount && i < values.Count; i++)
            {
                Console.Write(String.Format("{2}){0}: {1:0.00}  ", values[i].Item2, dist[values[i].Item2] * 100, (i + 1) % 10));
            }
            SetCursor(currLoc.Item1, currLoc.Item2);
        }
        public static void PrintEditDistance(List<Tuple<double, string>> values, List<Tuple<double, string>> edList, string word)
        {
            Tuple<int, int> currLoc = new Tuple<int, int>(Console.CursorLeft, Console.CursorTop);
            SetCursor(0, 2);
            ClearLine();
            for (int i = 0; i < promptCount && i < values.Count; i++)
            {
                Console.Write(String.Format("{2}){0}: {1:0.00}  ", values[i].Item2, EditDistance.ComputeEditDistanceDP(values[i].Item2, word), (i + 1) % 10));
            }
            SetCursor(currLoc.Item1, currLoc.Item2);
        }
        public static void Backspace()
        {
            Tuple<int, int> currLoc = new Tuple<int, int>(Console.CursorLeft, Console.CursorTop);
            Console.Write(currLoc);
            if (currLoc.Item1 != 0) // not on edge
            {
                SetCursor(currLoc.Item1, currLoc.Item2);
                Console.Write(" ");
                SetCursor(currLoc.Item1, currLoc.Item2);
            }
            else //if on edge now
            {
                SetCursor(windowW - 1, currLoc.Item2 - 1);
                Console.Write(" ");
                SetCursor(windowW - 1, currLoc.Item2 - 1);
            }
        }
        public static void ReWriteWord(int keyNumber, Tuple<int, int> start)
        {
            Tuple<int, int> currLoc = new Tuple<int, int>(Console.CursorLeft, Console.CursorTop);
            SetCursor(start.Item1, start.Item2);
            while (Console.CursorTop != currLoc.Item2 || Console.CursorLeft != currLoc.Item1)
            {
                Console.Write(" ");
            }
            SetCursor(start.Item1, start.Item2);
            string newWord = promptList[(keyNumber + 9) % 10].Item2;
            Console.Write(newWord);
        }

        public static void WriteMetaData(Queue<string> chain, string word)
        {
            Tuple<int, int> currLoc = getCursorLoc();
            SetCursor(0, windowH - 2);
            ClearLine();
            
            for(int i = 0; i < chain.Count; i++)
            {
                Console.Write(String.Format("{0}, ", chain.ElementAt(i)));
            }
            Console.Write(String.Format("current word: {0}", word));
            //Console.WriteLine(String.Format("pLoc = {0}, {1}; cLoc = {2}, {3}", pLoc.Item1, pLoc.Item2, cLoc.Item1, cLoc.Item2));
            SetCursor(currLoc);
        }
    }
}
