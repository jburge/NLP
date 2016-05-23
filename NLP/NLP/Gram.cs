using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public class Gram
    {
        private string gramWord;
        private Dictionary<string, Gram> children;
        private int count;

        public Gram(string word)
        {
            gramWord = word;
            children = new Dictionary<string,Gram>();
            count = 0;
        }

        public void Add(Queue<string> chain)
        {
            string nextWord = chain.Dequeue();
            if (!Contains(nextWord))
            {
                children.Add(nextWord, new Gram(nextWord));
            }
            
            if(chain.Count() > 0)
            {
                children[nextWord].Add(chain);
            }
            children[nextWord].Increment();
        }

        public string getMostFrequentChild()
        {
            string mostFrequentChild = "";
            if(children.Count() != 0)
                mostFrequentChild = children.OrderByDescending(x => x.Value.getCount()).First().Key;
            return mostFrequentChild;
        }

        public Gram getGram(Queue<string> chain)
        {
            if (chain.Count() == 0)
                return this;
            else
                return this[chain.Dequeue()].getGram(chain);
        }

        public int NextWordCount(string word)
        {
            if (Contains(word))
                return children[word].getCount();
            else
                return 0;
        }

        public List<string> getChildren() { return new List<string>(children.Keys.ToList().OrderBy(o => children[o].getCount()).ToList());
 }

        //public List<Gram2> getChildren() { return new List<Gram2>(children.Values.ToList().OrderBy(x => x.getCount()).ToList()); }

        public void Increment() { count++; }

        /// GENERAL UTILITY ///
        public int getCount() { return count; }
        public string getWord() { return gramWord; }
        public Gram this[string key]
        {
            get
            {
                Gram child;
                if(!this.children.TryGetValue(key, out child))
                {
                    children[key] = new Gram(key);
                    child = children[key];
                }
                return child;
            }
        }
        public bool Contains(string child) { return children.ContainsKey(child); }



    }
}
