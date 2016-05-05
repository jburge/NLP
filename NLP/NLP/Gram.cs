using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public class Gram
    {
        private string gram;
        public Dictionary<string, Gram> children = new Dictionary<string, Gram>();
        private int count;

        public Gram(string word)
        {
            gram = word;
            count = 0;
        }

        public void Add(Queue<string> chain)
        {
            string nextWord = chain.Dequeue();
            if (!children.ContainsKey(nextWord))
            {
                children.Add(nextWord, new Gram(nextWord));
            }
            if (chain.Count > 0)
            {
                children[nextWord].Add(chain);
            }
            children[nextWord].Increment();
        }
        /// <summary>
        /// Gets the frequency of word w appearing as the next word given this state
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public int NextWordCount(string word)
        {
            if (children.ContainsKey(word))
            {
                return children[word].getCount();
            }
            else
            {
                return 0;
            }
        }
        public void Increment() { count++; }
        /// <summary>
        /// Returns all children (observed words from current state) ordered by count
        /// </summary>
        /// <returns></returns>
        public List<Gram> getChildrenGrams()
        {
            return new List<Gram>(children.Values.ToList().OrderBy(o => o.getCount()).ToList());
        }
        /// <summary>
        /// Returns all strings from children grams
        /// </summary>
        /// <returns></returns>
        public List<string> getChildren()
        {
            List<string> words = new List<string>();
            foreach (Gram g in children.Values)
            {
                words.Add(g.getWord());
            }
            words.Sort();
            return words;
        }
        public Gram getGram(Queue<string> chain)
        {
            chain.Dequeue();
            if (chain.Count > 0)
            {
                return children[chain.First()].getGram(chain);
            }
            else
            {
                return this;
            }
        }

        //////////////////////////////////
        public int getCount() { return count; }
        public string getWord() { return gram; }
        public bool ContainsChild(string child)
        {
            return children.ContainsKey(child);
        }
        public Gram this[string key]
        {
            get
            {
                Gram child;
                if (!this.children.TryGetValue(key, out child))
                {
                   children[key] = new Gram(key);
                }
                return child;
            }
        }
    }
}
