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
        public static void TestEdit()
        {
            string w1 = Console.ReadLine();
            string w2 = Console.ReadLine();

            double d = EditDistance.ComputeEditDistanceDP(w1, w2);
            Console.WriteLine(w1 + " , " + w2 + " : " + d);
        }
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
}
