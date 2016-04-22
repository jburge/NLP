using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Text.RegularExpressions;

namespace NLP
{
    public class Model
    {
        int modelDepth;
        Dictionary<string, Gram> model = new Dictionary<string, Gram>();
        static string punctuation = "[\";,]";
        static string terminators = "[.!?]";
        public Model(int depth) { modelDepth = depth; }
        public void AddWord(string word)
        {
            model[word] = new Gram(word);
        }
        public void AddEvent(Queue<string> chain)
        {
            string firstWord = chain.Dequeue();
            if (model.ContainsKey(firstWord))
            {
                model[firstWord].Increment(); ;
            }
            else
            {
                model.Add(firstWord, new Gram(firstWord));
            }
            if (chain.Count > 0)
            {
                model[firstWord].Add(chain);
            }
        }
        public Queue<string> ObserveEvent(Queue<string> chain, string nextWord)
        {
            chain.Enqueue(nextWord);
            if (chain.Count > modelDepth)
            {
                chain.Dequeue();
            }
            if (chain.Count == modelDepth)
            {
                Queue<string> temp = new Queue<string>(chain.ToArray());
                AddEvent(temp);
            }
            return chain;
        }
        private void ChainPush(Queue<string> chain)
        {
            chain.Dequeue();
            while(chain.Count > 0)
            {
                Queue<string> temp = new Queue<string>(chain.ToArray());
                AddEvent(temp);
                chain.Dequeue();
            }
        }
        public void ReadInputCorpus(string fileName)
        {
            Queue<string> chain = new Queue<string>();
            string[] lines = System.IO.File.ReadAllLines("../../" + fileName);
            foreach (string line in lines)
            {
                string stripped = Regex.Replace(line, punctuation, "");
                //Console.WriteLine(stripped);
                string[] phrases = stripped.Split(' ');
                foreach (string phrase in phrases)
                {
                    string word = phrase.ToLower();
                    string check = Regex.Replace(word, terminators, "");
                    if (check == "")
                        break;
                    //Console.WriteLine(check);
                    bool terminator = (check != word);
                    ObserveEvent(chain, check);
                    if (terminator)
                    {
                        ChainPush(chain);
                    }
                }
            }
        }
        public void DynamicRead(Queue<string> predicate, string currentWord)
        {
            List<Tuple<double, string>> valuation = Analysis.Evaluate(NGramProbability(getGramFromChain(predicate)), currentWord);
            // update model
            predicate.Enqueue(currentWord);
            ChainPush(predicate);
            // print through writer
            Writer.PrintPostWriteEvaluation(valuation);
        }
        public void DisplayModel()
        {
            DisplayUnigrams();
            DisplayBigrams();
            DisplayTrigrams();
        }
        private void DisplayUnigrams()
        {
            List<Gram> list = model.Values.ToList().OrderBy(o=>o.getWord()).ToList();
            for(int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i].getWord() + ": " + list[i].getCount());
            }
        }
        private void DisplayBigrams()
        {
            List<Gram> list = model.Values.ToList().OrderBy(o => o.getWord()).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                List<Gram> bigramList = list[i].getChildren();
                for (int j = 0; j < bigramList.Count; j++)
                {
                    Console.WriteLine("("+list[i].getWord() +", " + bigramList[j].getWord() +"): " + bigramList[j].getCount());
                }
            }
        }
        private void DisplayTrigrams()
        {
            List<Gram> list = model.Values.ToList().OrderBy(o => o.getWord()).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                List<Gram> bigramList = list[i].getChildren();
                for (int j = 0; j < bigramList.Count; j++)
                {
                    List<Gram> trigramList = bigramList[j].getChildren();
                    for (int k = 0; k < trigramList.Count; k++)
                    {
                        Console.WriteLine("(" + list[i].getWord() + ", " + bigramList[j].getWord() +", "+ trigramList[k].getWord()+"): " + trigramList[k].getCount());
                    }
                }
            }
        }
        public List<Tuple<double, string>> EvaluateState(Queue<string> predicate, string currentWord)
        {
            return Analysis.Evaluate(NGramProbability(getGramFromChain(predicate)), currentWord);
        }
        private List<Tuple<double,string>> NGramProbability(Gram predicate)
        {
            List<Tuple<double, string>> distribution = new List<Tuple<double, string>>();
            double predicateInstances = predicate.getCount();
            //List<Gram> predicateChildren = predicate.getChildren();
            foreach (string s in model.Keys)
            {
                int freqCount = 1;
                if(predicate.children.ContainsKey(s))
                {
                    freqCount = predicate.children[s].getCount();
                }
                distribution.Add(new Tuple<double, string>( freqCount/ predicateInstances, s));
            }
            distribution.Sort();
            return distribution;
        }
        private Gram getGramFromChain(Queue<string> chain)
        {
            return model[chain.First()].getGram(chain);
        }

        public int getModelDepth() { return modelDepth; }
        public bool HasKey(string s)
        {
            return model.ContainsKey(s);
        }
    }
}
