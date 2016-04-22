using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public static class Writer
    {
        private static int promptCount = 10;
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
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            SetCursorCorner();
            for(int i = 0; i < promptCount && i < valuation.Count; i++)
            {
                Console.Write(String.Format("{0}: {1:0.00}\t", valuation[i].Item2, valuation[i].Item1*100));
            }
            SetCursor(x,y);
        }
    }
}
