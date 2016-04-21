using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public class Gram
    {
        string gram;
        public Dictionary<string, Gram> children = new Dictionary<string, Gram>();
        int count;
       
        public void Add(Queue<string> chain)
        {
            string nextWord = chain.Dequeue();
            if (children.ContainsKey(nextWord))
            {
                children[nextWord].Increment(); ;
            }
            else
            {
                children.Add(nextWord, new Gram(nextWord));
            }
            if(chain.Count > 0)
            {
                children[nextWord].Add(chain);
            }
        }

        public Gram(string word)
        {
            gram = word;
            count = 1;
        }
        public void Increment() { count++; }
        public int getCount() { return count;}
        public string getWord() { return gram; }
        public List<Gram> getChildren() 
        {
            return new List<Gram>(children.Values.ToList().OrderBy(o => o.getWord()).ToList()); 
        }
        public void DisplayModel()
        {
            //Console.WriteLine(count);

        }
    }
}
