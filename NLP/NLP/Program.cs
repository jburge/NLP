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
            //TestEdit();
            Model model = RunModel();
            DynamicReader.InputLoop(model);
        }
        public static Model RunModel()
        {
            Model myModel = new Model(3);
            myModel.ReadInputCorpus("Input.txt");
            return myModel;
        }
        public static void TestEdit()
        {
            string w1 = "sunday";
            string w2 = "saturday";

            int d = EditDistance.ComputeEditDistanceDP(w1, w2);
            Console.WriteLine(w1 + " , " + w2 + " : " + d);
        }
    }
}
