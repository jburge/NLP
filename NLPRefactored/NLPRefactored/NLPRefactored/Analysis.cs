using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPRefactored
{
    public static class Analysis
    {
        private static double scale = 100000;
        private static double edWeight = 10000;
        private static double unigramWeight = .1;
        private static double bigramWeight = .5;
        private static double trigramWeight = .4;
        
        public static List<Tuple<double, string>> EvaluateLikelihood(Queue<string> predicate, string currentWord, Model m)
        {
            List<Tuple<double, string>> values = new List<Tuple<double, string>>();
            List<string> dictionary = m.GetDictionary();
            //if(!m.HasKey(currentWord))
              //  dictionary.Add(currentWord);
            List<Tuple<double, string>> ed = EditDistance.ComputeEditDistances(dictionary, currentWord);
            Dictionary<string, double> trigramDist = TrigramDistribution(m, dictionary, predicate);
            Dictionary<string, double> bigramDist = BigramDistribution(m, dictionary, predicate);
            Dictionary<string, double> unigramDist = UnigramDistribution(m, dictionary);
            List<double> nWeights = new List<double>();
            List<Dictionary<string, double>> distributions = new List<Dictionary<string, double>>();
            if(trigramDist != null)
            {
                nWeights.Add(trigramWeight);
                distributions.Add(trigramDist);
            }
            if (bigramDist != null)
            {
                nWeights.Add(bigramWeight);
                distributions.Add(bigramDist);
            }
            nWeights.Add(unigramWeight);
            distributions.Add(unigramDist);
            foreach(Tuple<double, string> t in ed)
            {
                double value = 0;
                for(int i = 0; i < nWeights.Count; i++)
                {
                    value += nWeights[i] * distributions[i][t.Item2];
                }
                value /= (t.Item1 * edWeight);
                value *= scale;
                values.Add(new Tuple<double, string>(value, t.Item2));
            }
            values.Sort();
            values.Reverse();
            Writer.PrintPostWriteEvaluation(values);
            Writer.PrintProbabilityDistribution(values, distributions[0]);
            Writer.PrintEditDistance(values, ed, currentWord);
            return values;
        }
        private static Dictionary<string, double> UnigramDistribution(Model m, List<string> dictionary)
        {
            Dictionary<string, double> probabilityDistribution = new Dictionary<string, double>();
            int events = m.GetEventCount();

            foreach (string word in dictionary)
            {
                int count = m.GetWordCount(word);
                probabilityDistribution.Add(word, count / (double)events);
            }
            return probabilityDistribution;
        }
        private static Dictionary<string, double> BigramDistribution(Model m, List<string> dictionary, Queue<string> predicate) 
        {
            if (predicate.Count >= 1)
            {
                while (predicate.Count > 1)
                {
                    predicate.Dequeue();
                }
                return LaplacianSmoothing(m.getGramFromChain(new Queue<string>(predicate.ToArray())), dictionary);
            }
            return null;
        }

        private static Dictionary<string, double> TrigramDistribution(Model m, List<string> dictionary, Queue<string> predicate) 
        {
            if(predicate.Count >= 2)
            {
                while(predicate.Count > 2)
                {
                    predicate.Dequeue();
                }
                return LaplacianSmoothing(m.getGramFromChain(new Queue<string>(predicate.ToArray())), dictionary);
            }
            return null;
        }
        private static Dictionary<string, double> LaplacianSmoothing(Gram predicateState, List<string> dictionary) //this list dictionary may contain words not in the official dictionary such as a misspelling of currentword
        {
            Dictionary<string, double> probabilityDistribution = new Dictionary<string, double>();
            int predicateFrequency = predicateState.getCount();
            predicateFrequency += dictionary.Count;     // the plus one smoothing
            foreach(string word in dictionary)
            {
                int count = 1;                          // the plus one smoothing
                count += predicateState.NextWordCount(word);
                probabilityDistribution.Add(word, count / (double)predicateFrequency);
            }
            return probabilityDistribution;
        }   
        public static List<Tuple<double, string>> ExpectedWords(Gram pState, List<string> dictionary)
        {
            List<Tuple<double, string>> probabilityDistribution = new List<Tuple<double, string>>();
            int predicateFrequency = pState.getCount();
            predicateFrequency += dictionary.Count;     // the plus one smoothing
            foreach (string word in dictionary)
            {
                int count = 1;                          // the plus one smoothing
                count += pState.NextWordCount(word);
                probabilityDistribution.Add(new Tuple<double, string>(count * 100 / (double)predicateFrequency, word));
            }
            return probabilityDistribution;
        } 
    }
    
}
