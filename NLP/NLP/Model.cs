using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace NLP
{
    class Model
    {
        Dictionary<string, int> dictionary;
        Dictionary<Tuple<string, string>, int> bigrams;
        Dictionary<Tuple<string, string, string>, int> trigrams;

        static string punctuation = "[\";,]";
        static string terminators = "[.!?]";

        public Model() 
        {
            dictionary = new Dictionary<string, int>();
            bigrams = new Dictionary<Tuple<string, string>, int>();
            trigrams = new Dictionary<Tuple<string, string, string>, int>();

        }
        public void ReadInputCorpus(string fileName)
        {
            string[] lines = System.IO.File.ReadAllLines("../../" + fileName);
            foreach (string line in lines)
            {
                string stripped = Regex.Replace(line, punctuation, "");
                //Console.WriteLine(stripped);
                string[] phrases = stripped.Split(' ');
                int preceding = 1;
                Queue<string> history = new Queue<string>();
                history.Enqueue(".");
                foreach (string phrase in phrases)
                {
                    //string word = phrase.Trim(new Char[] { ',' });
                    string word = phrase.ToLower();
                    string check = Regex.Replace(word, terminators, "");
                    bool terminator = (check != word );
                    if (preceding > 1)
                    {
                        PutInTrigrams(history.ElementAt(1), history.ElementAt(0), check);
                    }
                    PutInBigrams(history.ElementAt(0), check);
                    PutInDictionary(check);
                    if(terminator)
                    {
                        preceding = 1;
                        history.Clear();
                        history.Enqueue(".");
                    }
                    else
                    {
                        history.Enqueue(check);
                        if (history.Count == 3)
                        {
                            history.Dequeue();
                        }
                    }
                }
            }
            //PrintDictionary();
            //PrintBigrams();
            //PrintTrigrams();
        }
        public void PutInDictionary(string entry)
        {
            if (dictionary.ContainsKey(entry))
            {
                dictionary[entry]++;
            }
            else
            {
                dictionary.Add(entry, 1);
            }
        }
        public void PutInBigrams(string w1, string w2)
        {
            Tuple<string, string> entry = new Tuple<string, string>(w1, w2);
            if (bigrams.ContainsKey(entry))
            {
                bigrams[entry]++;
            }
            else
            {
                bigrams.Add(entry, 1);
            }
        }
        public void PutInTrigrams(string w1, string w2, string w3)
        {
            Tuple<string, string, string> entry = new Tuple<string, string, string>(w1, w2, w3);
            if (trigrams.ContainsKey(entry))
            {
                trigrams[entry]++;
            }
            else
            {
                trigrams.Add(entry, 1);
            }
        }
        public void PrintDictionary()
        {
            foreach (KeyValuePair<string, int> kvp in dictionary)
            {
                Console.WriteLine(kvp.Key + ": " + kvp.Value);
            }
        }
        public void PrintBigrams()
        {
            foreach (KeyValuePair<Tuple<string, string>, int> kvp in bigrams)
            {
                Console.WriteLine("(" + kvp.Key.Item1 + ", " + kvp.Key.Item2+"): " + kvp.Value);
            }
        }
        public void PrintTrigrams()
        {
            foreach (KeyValuePair<Tuple<string, string, string>, int> kvp in trigrams)
            {
                Console.WriteLine("("+kvp.Key.Item1+", "+kvp.Key.Item2+", "+kvp.Key.Item3+"): "+kvp.Value);
            }
        }
        public void DisplayModel() {
            PrintDictionary();
        }
    }
}
