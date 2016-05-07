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
            Debugger.InitializeLog();
            EditAttempt();
            SetUpWindow();
            Model model = InitializeModel();
            //TestEdit();
            //model.DisplayModel();
            //DynamicReader.InputLoop(model);
            //TestModel(model);
            Debugger.WriteLog();
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
            string txtFolderPath = "..\\..\\TextFiles\\TrainingCorpus\\Dickens\\";
            string[] files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (string fileName in files)
            {
                m.TrainModel(fileName);
            }
            //Console.Clear();
            return m;
        }
        static void TestModel(Model model)
        {
            ModelTestManager.InitializeManager(model, "..\\..\\TextFiles\\TestFiles\\MansfieldPark.txt");
            ModelTestManager.TestModel();
        }
        public static void TestEdit()
        {
            string w1 = Console.ReadLine();
            string w2 = Console.ReadLine();

            double d = EditDistance.ComputeEditDistanceDP(w1, w2);
            Console.WriteLine(w1 + " , " + w2 + " : " + d);
        }
        public static void EditAttempt()
        {
            string txtFolderPath = "..\\..\\TextFiles\\TrainingCorpus\\Dickens\\";
            string[] files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                Console.WriteLine(file);
                string[] lines = System.IO.File.ReadAllLines(file);
                string content = "";
                for (int i = 0; i < lines.Count(); i++)
                {
                    if (lines[i] == "")
                        content += "\n";
                    else if (lines[i] == lines[i].ToUpper())
                        content += lines[i] + "~ ";
                    else if (lines[i][lines[i].Length - 1] != ' ')
                        content += lines[i] + " ";
                    else if (lines[i][lines[i].Length - 1] == ' ')
                        content += lines[i];
                }
                System.IO.File.WriteAllText(file, content);
            }
        }
    }
}
