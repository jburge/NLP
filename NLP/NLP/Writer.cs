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
        private static int promptCount = 6;
        private static List<Tuple<double, string>> promptList; 
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
        public static void PrintPostWriteEvaluation(List<Tuple<double, string>> valuation)
        {
            promptList = valuation;
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            SetCursorCorner();
            for(int i = 0; i < promptCount && i < valuation.Count; i++)
            {
                Console.Write(String.Format("{2}){0}: {1:0.00}  ", valuation[i].Item2, valuation[i].Item1*100, (i+1)%10));
            }
            SetCursor(x,y);
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
                SetCursor(Console.WindowWidth - 1, currLoc.Item2 - 1);
                Console.Write(" ");
                SetCursor(Console.WindowWidth - 1, currLoc.Item2 - 1);
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


    }
}
