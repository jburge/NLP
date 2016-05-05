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
        int eventCount;
        Dictionary<string, Gram> model = new Dictionary<string, Gram>();
        public static string punctuation = ";,\\(\\)'\"\\*:";
        public static string terminators = ".!?";
        public static string wordBreak = " -_";
        public static string RegexPunctuation = "[" + punctuation + "]";
        public static string RegexTerminators = "[" + terminators + "]";
        public Model(int depth)
        {
            modelDepth = depth;
            eventCount = 0;
        }

        ///// Simple Utility Functions
        public int getModelDepth()
        {
            return modelDepth;
        }
        public bool HasKey(string s) //checks if a word is in dictionary
        {
            return model.ContainsKey(s);
        }
        public List<string> GetDictionary()
        {
            return model.Keys.ToList();
        }
        public int GetEventCount()
        {
            return eventCount;
        }
        public Gram getGramFromChain(Queue<string> chain)
        {
            return model[chain.First()].getGram(chain);
        }
        public int GetWordCount(string word)
        {
            if (model.ContainsKey(word))
            {
                return model[word].getCount();
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Adds a new word to the dictionary
        /// </summary>
        /// <param name="word"></param>
        public void AddWord(string word)
        {
            model[word] = new Gram(word);
            eventCount++;
        }

        /// <summary>
        /// Takes event chain and recursively increments states that occur
        /// </summary>
        /// <param name="chain"></param>
        public void AddEvent(Queue<string> chain)
        {
            string firstWord = chain.Dequeue();
            if (!model.ContainsKey(firstWord))
            {
                model.Add(firstWord, new Gram(firstWord));
            }
            if (chain.Count > 0)
            {
                model[firstWord].Add(chain);
            }
            model[firstWord].Increment();
        }
        /// <summary>
        /// Pushes the Queue through into the model
        /// Typical use case if when a terminator is hit, and thus a new queue will start for the next sentence
        /// </summary>
        /// <param name="chain"></param>
        public Queue<string> ChainPush(Queue<string> chain)
        {
            while (chain.Count > 0)
            {
                Queue<string> temp = new Queue<string>(chain.ToArray());
                AddEvent(temp);
                chain.Dequeue();
            }
            return chain;
        }

        /// <summary>
        /// This function takes in a predicate and next state and adds the instance to the model
        /// </summary>
        /// <param name="chain">This is the predicate</param>
        /// <param name="nextWord">This is the next state</param>
        /// <returns>Returns a queue as the predicate for the next state</returns>
        private Queue<string> ObserveEvent(Queue<string> chain, string nextWord)
        {
            eventCount++;

            chain.Enqueue(nextWord);
            if (chain.Count > modelDepth)
            {
                chain.Dequeue();
            }
            Queue<string> temp = new Queue<string>(chain.ToArray());
            AddEvent(temp);
            return chain;
        }



        /// <summary>
        /// Trains model off static corpus
        /// </summary>
        /// <param name="fileName"></param>
        public void TrainModel(string fileName)
        {
            Queue<string> chain = new Queue<string>();
            //string[] lines = System.IO.File.ReadAllLines("../../" + fileName);

            string[] lines = System.IO.File.ReadAllLines(fileName);
            for (int i = 0; i < lines.Count(); i++)
            {
                //Console.WriteLine(lines[i]);
                string stripped = Regex.Replace(lines[i], RegexPunctuation, "");
                //Console.WriteLine(stripped);
                string[] phrases = stripped.Split(' ', '-', '_');
                for (int j = 0; j < phrases.Count(); j++)
                {
                    //Console.WriteLine(phrases[j]);
                    string word = phrases[j].ToLower();
                    //Console.WriteLine(word);
                    string check = Regex.Replace(word, RegexTerminators, "");
                    if (check == "")
                        break;
                    //Console.WriteLine(check);
                    bool terminator = (check != word);
                    ObserveEvent(chain, check);
                    if (terminator)
                    {
                        chain.Dequeue();
                        chain = ChainPush(chain);
                    }
                }
            }
            Console.WriteLine("Trained on file " + fileName);
        }

        /// <summary>
        /// Given a predicate and currentWord, determine what words in the dictionary are likely to be intended by input
        /// </summary>
        /// <param name="predicate">previous k words</param>
        /// <param name="currentWord">user input word</param>
        /// <returns>List of tuples containing value weight and word string</returns>
        private List<Tuple<double, string>> EvaluateState(Queue<string> predicate, string currentWord)
        {
            return Analysis.EvaluateLikelihood(new Queue<string>(predicate.ToArray()), currentWord, this);
        }

        public Queue<string> DynamicRead(Queue<string> predicate, string currentWord)
        {
            List<Tuple<double, string>> valuation = EvaluateState(predicate, currentWord);
            // update model

            // print through writer

            Writer.WriteMetaData(predicate, currentWord);
            return predicate;
        }
        public Queue<string> DynamicObserve(Queue<string> chain, string nextWord)
        {
            eventCount++;
            chain.Enqueue(nextWord);
            if (chain.Count > modelDepth)
            {
                chain.Dequeue();
            }
            Queue<string> temp = new Queue<string>(chain.ToArray());
            ChainPush(temp);
            return chain;
        }
        /////////////////////////////////////////////
        /// Functions to Print Model distribution ///
        /////////////////////////////////////////////
        public void DisplayModel()
        {
            DisplayUnigrams();
            //DisplayBigrams();
            //DisplayTrigrams();
        }
        private void DisplayUnigrams()
        {
            List<Gram> list = model.Values.ToList().OrderByDescending(o => o.getCount()).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i].getWord() + ": " + list[i].getCount());
            }
        }
        private void DisplayBigrams()
        {
            List<Gram> list = model.Values.ToList().OrderByDescending(o => o.getCount()).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                List<Gram> bigramList = list[i].getChildren();
                for (int j = 0; j < bigramList.Count; j++)
                {
                    Console.WriteLine("(" + list[i].getWord() + ", " + bigramList[j].getWord() + "): " + bigramList[j].getCount());
                }
            }
        }
        private void DisplayTrigrams()
        {
            List<Gram> list = model.Values.ToList().OrderByDescending(o => o.getCount()).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                List<Gram> bigramList = list[i].getChildren();
                for (int j = 0; j < bigramList.Count; j++)
                {
                    List<Gram> trigramList = bigramList[j].getChildren();
                    for (int k = 0; k < trigramList.Count; k++)
                    {
                        Console.WriteLine("(" + list[i].getWord() + ", " + bigramList[j].getWord() + ", " + trigramList[k].getWord() + "): " + trigramList[k].getCount());
                    }
                }
            }
        }
    }
}
