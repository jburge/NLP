using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace NLP
{
    public static class TestFunctions
    {
        public static void TestEdit(Model model)
        {
            string w1 = Console.ReadLine();
            string w2 = Console.ReadLine();

            double d = EditDistance.ComputeEditDistance(model, w1, w2);
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
            CountFile(txtFolderPath + "ATaleOfTwoCities.txt");
            //}
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
