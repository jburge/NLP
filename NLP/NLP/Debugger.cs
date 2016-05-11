using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public static class Debugger
    {
        private static List<string> logList;
        private static string output = "..\\..\\log.txt";
        private static string resultDir = "..\\..\\TestResults\\";
        private static Dictionary<Tuple<string, string>, MatchTracker> TestData;
        public static void InitializeLog()
        {
            logList = new List<string>();
            TestData = new Dictionary<Tuple<string, string>, MatchTracker>(); ;
        }
        public static void Log(string item)
        {
            logList.Add(item);
        }
        public static void StartTest(Model model, string fileName)
        {
            Console.WriteLine(String.Format("Testing {0} Model on {1}", model.getAuthor(), fileName));
            TestData.Add(new Tuple<string, string>(model.getAuthor(), fileName), new MatchTracker());
        }
        public static void FinishTest(Model model, string fileName)
        {
            Console.WriteLine(String.Format("Finished Testing {0} Model on {1}", model.getAuthor(), fileName));
            
            WriteFile(new Tuple<string, string>(model.getAuthor(), fileName));
        }
        private static void WriteFile(Tuple<string, string> t)
        {
            MatchTracker mt = TestData[t];
            List<Tuple<int, string>> wordList = new List<Tuple<int, string>>();
            foreach (string w in mt.matches.Keys)
            {
                wordList.Add(new Tuple<int, string>(mt.matches[w], w));
            }
            wordList.Sort();
            wordList.Reverse();
            List<string> result = new List<string>();
            for (int i = 0; i < wordList.Count; i++)
            {
                 result.Add(String.Format("{0}: {1}",wordList[i].Item2, wordList[i].Item1));
            }
            System.IO.File.WriteAllLines(resultDir + t.Item1 + "\\" + t.Item2, result.ToArray());
        }
        public static void LogMatch(Model model, string fileName, string word)
        {
            Tuple<string, string> test = new Tuple<string,string>(model.getAuthor(), fileName);
            TestData[test].AddMatch(word);
        }
        public static void WriteLog()
        {
            System.IO.File.WriteAllLines(output, logList.ToArray());
        }

    }
    public class MatchTracker
    {
        public Dictionary<string, int> matches;
        public MatchTracker()
        {
            matches = new Dictionary<string, int>(); ;
        }
        public void AddMatch(string w)
        {
            if(!matches.ContainsKey(w))
            {
                matches[w] = 1;
            }
            else
            {
                matches[w] += 1;
            }
        }
    }
}
