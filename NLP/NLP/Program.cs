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
            Debugger.InitializeLog();
            //EditAttempt();
            SetUpWindow();
            Model model = InitializeModel();
            Console.Clear();
            //TestEdit();
            //model.DisplayModel();
<<<<<<< HEAD
            DynamicReader.InputLoop(model);
            //TestModel();
=======
            //DynamicReader.InputLoop(model);
            TestModel();
>>>>>>> 80b203456216172a040095455e6a11d7f2f72b85
            //CountWords();
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
        static void TestModel()
        {
            Debugger.Log(String.Format("{0}, {1}, {2}", Analysis.getWeights()[0], Analysis.getWeights()[1], Analysis.getWeights()[2]));
            author = "Austen";
            Debugger.Log(String.Format("Model Trained on {0}", author));
            string txtFolderPath = "..\\..\\TextFiles\\TestCorpus\\";
            string[] files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            for(int i = 0; i < files.Count(); i++)
            {
                ModelTestManager mtm = new ModelTestManager(InitializeModel(), files[i]);
                mtm.TestModelValuation();
            }
            author = "Dickens";
            Debugger.Log(String.Format("Model Trained on {0}", author));
            for (int i = 0; i < files.Count(); i++)
            {
                ModelTestManager mtm = new ModelTestManager(InitializeModel(), files[i]);
                mtm.TestModelValuation();
            }
            author = "Twain";
            Debugger.Log(String.Format("Model Trained on {0}", author));
            for (int i = 0; i < files.Count(); i++)
            {
                ModelTestManager mtm = new ModelTestManager(InitializeModel(), files[i]);
                mtm.TestModelValuation();
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
<<<<<<< HEAD
            //List<string> authors = new List<string> {"Austen", "Dickens", "Twain" };
            //Debugger.Log("Training Corpus:");
            //for (int a = 0; a < authors.Count; a++)
            //{
            //    txtFolderPath = "..\\..\\TextFiles\\TrainingCorpus\\" + authors[a] + "\\";
            //    files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            //    foreach (string file in files)
            //    {
            //        CountFile(file);
            //    }
            //}
            txtFolderPath = "..\\..\\TextFiles\\TestCorpus\\";
            //files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            //Debugger.Log("Test Corpus:");
            //for(int i = 0; i < files.Count(); i++)
            //{
                CountFile(txtFolderPath+"ATaleOfTwoCities.txt");
            //}
        }
        private static void CountFile(string file)
        {
            Console.WriteLine(file);
            List<string> phrases = new List<string>(RegexLogic.GetPhrasesFromFile(file));
            phrases.RemoveAll(item => Regex.Replace(item, "[\\.\\?\\!;~]", "") == "");
            foreach(string p in phrases){
                Debugger.Count(p.ToLower());
=======
            List<string> authors = new List<string> {"Austen", "Dickens", "Twain" };
            Debugger.Log("Training Corpus:");
            for (int a = 0; a < authors.Count; a++)
            {
                txtFolderPath = "..\\..\\TextFiles\\TrainingCorpus\\" + authors[a] + "\\";
                files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    CountFile(file);
                }
            }
            txtFolderPath = "..\\..\\TextFiles\\TestCorpus\\";
            files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            Debugger.Log("Test Corpus:");
            for(int i = 0; i < files.Count(); i++)
            {
                CountFile(files[i]);
>>>>>>> 80b203456216172a040095455e6a11d7f2f72b85
            }
            Debugger.PrintCount();
            Debugger.Log(String.Format("{0}: {1} words", file, phrases.Count));
        }
        private static void CountFile(string file)
        {
            Console.WriteLine(file);
            List<string> phrases = new List<string>(RegexLogic.GetPhrasesFromFile(file));
            phrases.RemoveAll(item => Regex.Replace(item, "[\\.\\?\\!;~]", "") == "");
            Debugger.Log(String.Format("{0}: {1} words", file, phrases.Count));
        }
    }
}
