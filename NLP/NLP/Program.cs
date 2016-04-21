using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace NLP
{
    class Program
    {
        static void Main(string[] args)
        {
            TestEdit();
        }
        public void RunModel()
        {
            Model2 myModel = new Model2(3);
            myModel.ReadInputCorpus("Input.txt");
            //myModel.DisplayModel();
            string tt = "lorem";
            Queue<string> temp = new Queue<string>();
            temp.Enqueue(tt);
            List<Tuple<double, string>> test = myModel.Evaluate(temp);
            foreach (Tuple<double, string> t in test)
            {
                Console.WriteLine(t.Item2 + ": " + t.Item1);
            }
        }
        public static void TestEdit()
        {
            string w1 = "sunday";
            string w2 = "saturday";

            int d = EditDistance.ComputeEditDistanceDP(w1, w2, w1.Length, w2.Length);
            Console.WriteLine(w1 + " , " + w2 + " : " + d);
        }
    }
}
