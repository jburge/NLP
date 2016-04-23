using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;

namespace NLP
{
    class Program
    {
        static void Main(string[] args)
        {
            Model model = RunModel();
            Console.Clear();
            DynamicReader.InputLoop(model);
        }
        public static Model RunModel()
        {
            Model myModel = new Model(3);
            string txtFolderPath = "\\\\cs1\\cs_students\\jburge16\\CS457\\NLP\\NLP\\NLP\\TextFiles\\";
            string[] files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (string fileName in files)
            {
                myModel.ReadInputCorpus(fileName);
            }
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
