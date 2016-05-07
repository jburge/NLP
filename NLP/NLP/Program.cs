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
        private static string author = "Austen";
        private static int depth = 3;
        static void Main(string[] args)
        {
            Debugger.InitializeLog();
            //EditAttempt();
            SetUpWindow();
            //Model model = InitializeModel();
            //TestEdit();
            //model.DisplayModel();
            //DynamicReader.InputLoop(model);
            TestModel();
            //CountWords();
            //Debugger.WriteLog();
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
            Model m = new Model(depth);
            string txtFolderPath = "..\\..\\TextFiles\\TrainingCorpus\\"+author+"\\";
            string[] files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (string fileName in files)
            {
                m.TrainModel(fileName);
            }
            //Console.Clear();
            return m;
        }
        static void TestModel()
        {
            author = "Austen";
            Debugger.Log(String.Format("Model Trained on {0}", author));
            string txtFolderPath = "..\\..\\TextFiles\\TestCorpus\\";
            string[] files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            for(int i = 3; i < files.Count(); i++)
            {
                ModelTestManager mtm = new ModelTestManager(InitializeModel(), files[i]);
                mtm.TestModel();
            }
            author = "Dickens";
            Debugger.Log(String.Format("Model Trained on {0}", author));
            for (int i = 3; i < files.Count(); i++)
            {
                ModelTestManager mtm = new ModelTestManager(InitializeModel(), files[i]);
                mtm.TestModel();
            }
            author = "Twain";
            Debugger.Log(String.Format("Model Trained on {0}", author));
            for (int i = 0; i < files.Count(); i++)
            {
                ModelTestManager mtm = new ModelTestManager(InitializeModel(), files[i]);
                mtm.TestModel();
            }
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
            string author = "Twain";
            string txtFolderPath = "..\\..\\TextFiles\\TrainingCorpus\\" + author + "\\";
            string[] files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                Console.WriteLine(file);
                string content = "";
                string[] lines = System.IO.File.ReadAllLines(file);
                for (int i = 0; i < lines.Count(); i++)
                {
                    if (lines[i] == "")
                        content += "\n";
                    else if (lines[i][lines[i].Length - 1] != ' ')
                        content += lines[i] + " ";
                    else if (lines[i][lines[i].Length - 1] == ' ')
                        content += lines[i];
                }
                System.IO.File.WriteAllText(file, content);
            }
        }
        public static void CountWords()
        {
            string txtFolderPath;
            string[] files;
            List<string> authors = new List<string> { "Shakespeare", "Austen", "Dickens", "Twain" };
            Debugger.Log("Training Corpus:");
            for (int a = 0; a < authors.Count; a++)
            {
                txtFolderPath = "..\\..\\TextFiles\\TrainingCorpus\\" + authors[a] + "\\";
                files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                    int phrases = RegexLogic.GetPhrasesFromFile(file).Count();
                    Debugger.Log(String.Format("{0}: {1} words", file, phrases));
                }
            }
            txtFolderPath = "..\\..\\TextFiles\\TestCorpus\\";
            files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            Debugger.Log("Test Corpus:");
            for(int i = 0; i < files.Count(); i++)
            {
                Console.WriteLine(files[i]);
                string[] phrases = RegexLogic.GetPhrasesFromFile(files[i]);
                Debugger.Log(String.Format("{0}: {1} words", files[i], phrases.Count()));
            }
        }
    }
}
