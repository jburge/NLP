using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;


//// THings to add
// save model state
// write into text file
// auto complete
// mess with window settings
// allow scrolling
// need to figure out buffers


namespace NLP
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
        static void GetInfo(Model model)
        {
            string temp = Console.ReadLine();
            Console.WriteLine(model.GetWordCount(temp));
        }
        static void SetUpWindow()
        {
            Console.WindowWidth = Writer.windowW;
            Console.WindowHeight = Writer.windowH;
            Console.BufferHeight = Writer.windowH;
            Console.BufferWidth = Writer.windowW;
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
            string w1 = Console.ReadLine();
            string w2 = Console.ReadLine();

            double d = EditDistance.ComputeEditDistanceDP(w1, w2);
            Console.WriteLine(w1 + " , " + w2 + " : " + d);
        }
    }
}
