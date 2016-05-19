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
        private static string wordoutput = "..\\..\\wordcounts.txt";
        private static List<string> MyList;
        private static Dictionary<Tuple<string, string>, MatchTracker> TestData;
        private static Dictionary<string, int> wordCounts = new Dictionary<string,int>();
        public static void InitializeLog()
        {
            logList = new List<string>();
            TestData = new Dictionary<Tuple<string, string>, MatchTracker>();
            MyList = new List<string> { "the", "and", "of", "to", "be", "a", "been", "was", "he", "had", "not", "own", "were", "at", "have", "much", "same", "dear", "you", "more", "shop", "than", "time", "very", "little", "are", "lady", "that", "as", "sure", "i", "down", "house", "it", "voice", "for", "marquis", "in", "other", "tree", "well", "say", "deal", "from", "is", "minutes", "world", "door", "hundred", "man", "prisoner", "room", "saw", "all", "five", "hand", "her", "night", "old", "one", "people", "seven", "should", "subject", "who", "with", "court", "head", "if", "know", "ship", "wine" };
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
        public static void Count(string w)
        {
            if (!wordCounts.ContainsKey(w))
            {
                wordCounts.Add(w, 1);
            }
            else
                wordCounts[w] += 1;
        }
        public static void PrintCount()
        {
            List<KeyValuePair<string, int>> temp = wordCounts.ToList();
            List<string> result = new List<string>();
            foreach (KeyValuePair<string, int> kvp in temp)
            {
                if(MyList.Contains(kvp.Key))
                result.Add(kvp.Key +": "+ kvp.Value);
            }
            System.IO.File.WriteAllLines(wordoutput, result.ToArray());
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
