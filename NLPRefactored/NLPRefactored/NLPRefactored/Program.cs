using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;

namespace NLPRefactored
{
    class Program
    {
       
        static void Main(string[] args)
        {
            SetUpWindow();
            Model model = InitializeModel();
            //TestEdit();
            //model.DisplayModel();
            DynamicReader.InputLoop(model);
        }
        static void GetInfo(Model model){
            string temp = Console.ReadLine();
            Console.WriteLine(model.GetWordCount(temp)); 
        }
        static void SetUpWindow()
        {
            Console.WindowWidth = Writer.windowW;
            Console.WindowHeight = Writer.windowH;
        }
        static Model InitializeModel()
        {
            Model m = new Model(3);
            string txtFolderPath = "..\\..\\TextFiles\\";
            string[] files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (string fileName in files)
            {
                m.TrainModel(fileName);
            }
            Console.Clear();
            return m;
        }
        public static void TestEdit()
        {
            string w1 = "confustion";
            string w2 = "hopefully";

            double d = EditDistance.ComputeEditDistanceDP(w1, w2);
            Console.WriteLine(w1 + " , " + w2 + " : " + d);
        }
    }
}
