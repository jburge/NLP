using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Text.RegularExpressions;

//make it so captial words not at beginning of sentence stay cappitalised
namespace NLP
{
    public class Model
    {
        int modelDepth;
        int eventCount;
        Gram model;
        string author = "";
        public static string terminators = ".!?";
        public static string wordBreak = " -_";
        public static string RegexCharRemoval = "[^a-zA-Z0-9\\.\\?\\!;\' ]";
        public static string RegexTerminators = "[" + terminators + "]";
        public static List<string> exceptionList = new List<string> { "mr.", "mrs.", "dr." };
        public Model(int depth)
        {
            model = new Gram("");
            modelDepth = depth;
            eventCount = 0;
        }
        public Model(int depth, string _author)
        {
            model = new Gram("");
            modelDepth = depth;
            eventCount = 0;
            author = _author;
        }

        ///// Simple Utility Functions
        public int getModelDepth()
        {
            return modelDepth;
        }
        public bool HasKey(string s) //checks if a word is in dictionary
        {
            return model.ContainsChild(s);
        }
        public List<string> GetDictionary()
        {
            return model.getChildren();
        }
        public int GetEventCount()
        {
            return eventCount;
        }
        public Gram getGramFromChain(Queue<string> chain)
        {
            return model.getGram(chain);
        }
        public int GetWordCount(string word)
        {
            if (model.ContainsChild(word))
            {
                return model[word].getCount();
            }
            else
            {
                return 0;
            }
        }
        public string getAuthor() { return author; }
        public Gram this[string key]
        {
            get
            {
                Gram child = model[key];
                return child;
            }
        }
        /// <summary>
        /// Takes event chain and recursively increments states that occur
        /// </summary>
        /// <param name="chain"></param>
        public void AddEvent(Queue<string> chain)
        {
            string firstWord = chain.Dequeue();
            Gram temp = model[firstWord];
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

        // want to keep 0-9, a-z, A-Z, '.', '?', '!', ';', '''
        // [^a-zA-Z0-9\.\?\!;' ]
        /// <summary>
        /// Trains model off static corpus
        /// </summary>
        /// <param name="fileName"></param>
        public void TrainModel(string fileName)
        {
            Queue<string> chain = new Queue<string>();
            //string[] lines = System.IO.File.ReadAllLines("../../" + fileName);

            string[] phrases = RegexLogic.GetPhrasesFromFile(fileName);
            for (int i = 0; i < phrases.Count(); i++)
            {
                string phrase = phrases[i];
                string word = phrase.ToLower();
                if(!exceptionList.Contains(word))
                    word = Regex.Replace(word, "[\\.\\?\\!;~]", "").ToLower();
                if (word == "")
                    break;
                //if (word.Contains('\'') && !(phrase.Substring(0, 1) == phrase.Substring(0, 1).ToUpper() && phrase.Substring(phrase.Length - 2, 2) == "'s"))
                //    Debugger.Log(String.Format("{0}: {1} ({2})",Regex.Split(fileName, "\\\\").Last(), word, i+1));
                //Console.WriteLine(check);
                bool terminator = (phrase != word);
                ObserveEvent(chain, word);
                if (terminator)
                {
                    chain.Dequeue();
                    chain = ChainPush(chain);
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
            List<Gram> list = model.getChildrenGrams();
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i].getWord() + ": " + list[i].getCount());
            }
        }
        private void DisplayBigrams()
        {
            List<Gram> list = model.getChildrenGrams();
            for (int i = 0; i < list.Count; i++)
            {
                List<Gram> bigramList = list[i].getChildrenGrams();
                for (int j = 0; j < bigramList.Count; j++)
                {
                    Console.WriteLine("(" + list[i].getWord() + ", " + bigramList[j].getWord() + "): " + bigramList[j].getCount());
                }
            }
        }
        private void DisplayTrigrams()
        {
            List<Gram> list = model.getChildrenGrams();
            for (int i = 0; i < list.Count; i++)
            {
                List<Gram> bigramList = list[i].getChildrenGrams();
                for (int j = 0; j < bigramList.Count; j++)
                {
                    List<Gram> trigramList = bigramList[j].getChildrenGrams();
                    for (int k = 0; k < trigramList.Count; k++)
                    {
                        Console.WriteLine("(" + list[i].getWord() + ", " + bigramList[j].getWord() + ", " + trigramList[k].getWord() + "): " + trigramList[k].getCount());
                    }
                }
            }
        }
    }
}
