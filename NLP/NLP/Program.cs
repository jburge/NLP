using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;


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
        private static string author = "Austen";
        private static int depth = 3;
        static void Main(string[] args)
        {
            //Debugger.InitializeLog();
            //EditAttempt();
            SetUpWindow();
            Model model = ComprehensiveModel();
            Console.Clear();
            //TestFunctions.TestEdit();
            //model.DisplayModel();
            DynamicReader.InputLoop(model);
            //TestModel();
            //CountWords();
            //Debugger.WriteLog();
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
            Model m = new Model(depth, author);
            string txtFolderPath = "..\\..\\TextFiles\\TrainingCorpus\\"+author+"\\";
            string[] files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (string fileName in files)
            {
                m.TrainModel(fileName);
            }
            //Console.Clear();
            return m;
        }
        static Model ComprehensiveModel()
        {
            Model m = new Model();
            string directoryPath = "..\\..\\TextFiles\\TrainingCorpus\\";
            string[] directories = Directory.GetDirectories(directoryPath);
            for(int i = 0; i < directories.Count(); i++)
            {
                string[] files = Directory.GetFiles(directories[i], "*.txt", SearchOption.TopDirectoryOnly);
                foreach (string fileName in files)
                {
                    m.TrainModel(fileName);
                }
            }
            return m;
        }
        static void TestModel()
        {
           // Debugger.Log(String.Format("{0}, {1}, {2}", Analysis.getWeights()[0], Analysis.getWeights()[1], Analysis.getWeights()[2]));
            author = "Austen";
            //Debugger.Log(String.Format("Model Trained on {0}", author));
            string txtFolderPath = "..\\..\\TextFiles\\TestCorpus\\";
            string[] files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            for(int i = 0; i < files.Count(); i++)
            {
                ModelTestManager mtm = new ModelTestManager(InitializeModel(), files[i]);
                //mtm.TestModelValuation();
            }
            author = "Dickens";
            //Debugger.Log(String.Format("Model Trained on {0}", author));
            for (int i = 0; i < files.Count(); i++)
            {
                ModelTestManager mtm = new ModelTestManager(InitializeModel(), files[i]);
                //mtm.TestModelValuation();
            }
            author = "Twain";
            //Debugger.Log(String.Format("Model Trained on {0}", author));
            for (int i = 0; i < files.Count(); i++)
            {
                ModelTestManager mtm = new ModelTestManager(InitializeModel(), files[i]);
                //mtm.TestModelValuation();
            }
        }
    }
}
