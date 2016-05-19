using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public static class Analysis
    {
        private static double scale = 100;
        private static double edWeight = 100;
        private static double unigramWeight = .01;
        private static double bigramWeight = .345;
        private static double trigramWeight = .645;
        private static Dictionary<string, double> trigramDist;
        private static Dictionary<string, double> bigramDist;
        private static Dictionary<string, double> unigramDist;
        private static List<Tuple<double, string>> ed;
        private static List<double> nWeights;
        private static List<Dictionary<string, double>> distributions;
        
        private static List<string> dictionary;
        private static Model model;
        public static List<double> getWeights() { return new List<double> { unigramWeight, bigramWeight, trigramWeight }; }
        private static void ComputeDistributions(Queue<string> predicate)
        {
            trigramDist = TrigramDistribution(model, dictionary, predicate);
            bigramDist = BigramDistribution(model, dictionary, predicate);
            unigramDist = UnigramDistribution(model, dictionary);
        }
        private static void SetUpDistributionVector(Queue<string> predicate)
        {
            ComputeDistributions(predicate);
            nWeights = new List<double>();
            distributions = new List<Dictionary<string, double>>();
            if (trigramDist != null)
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
        }
        private static double ComputeWeightedProbability(string word)
        {
            double value = 0;
            for (int i = 0; i < nWeights.Count; i++)
            {
                value += nWeights[i] * distributions[i][word];
            }
            return value;
        }
        public static List<Tuple<double, string>> EvaluateLikelihood(Queue<string> predicate, string currentWord, Model m)
        {
            model = m; 
            List<Tuple<double, string>> values = new List<Tuple<double, string>>();
            dictionary = model.GetDictionary();
            
            ed = EditDistance.ComputeEditDistances(dictionary, currentWord);
            SetUpDistributionVector(predicate);
            foreach (Tuple<double, string> t in ed)
            {
                double value = ComputeWeightedProbability(t.Item2);
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
        private static Dictionary<string, double> UnigramDistribution(Model model, List<string> dictionary)
        {
            Dictionary<string, double> probabilityDistribution = new Dictionary<string, double>();
            int events = model.GetEventCount();

            foreach (string word in dictionary)
            {
                int count = model.GetWordCount(word);
                probabilityDistribution.Add(word, count / (double)events);
            }
            return probabilityDistribution;
        }
        private static Dictionary<string, double> BigramDistribution(Model model, List<string> dictionary, Queue<string> predicate)
        {
            if (predicate.Count >= 1)
            {
                while (predicate.Count > 1)
                {
                    predicate.Dequeue();
                }
                return LaplacianSmoothing(model.getGramFromChain(new Queue<string>(predicate.ToArray())), dictionary);
            }
            return null;
        }

        private static Dictionary<string, double> TrigramDistribution(Model model, List<string> dictionary, Queue<string> predicate)
        {
            if (predicate.Count >= 2)
            {
                while (predicate.Count > 2)
                {
                    predicate.Dequeue();
                }
                return LaplacianSmoothing(model.getGramFromChain(new Queue<string>(predicate.ToArray())), dictionary);
            }
            return null;
        }
        private static Dictionary<string, double> LaplacianSmoothing(Gram predicateState, List<string> dictionary) //this list dictionary may contain words not in the official dictionary such as a misspelling of currentword
        {
            Dictionary<string, double> probabilityDistribution = new Dictionary<string, double>();
            int predicateFrequency = predicateState.getCount();
            predicateFrequency += dictionary.Count;     // the plus one smoothing
            foreach (string word in dictionary)
            {
                int count = 1;                          // the plus one smoothing
                count += predicateState.NextWordCount(word);
                probabilityDistribution.Add(word, count / (double)predicateFrequency);
            }
            return probabilityDistribution;
        }
        public static string PredictWord(Model m, Queue<string> evidence)
        {
            model = m;
            dictionary = model.GetDictionary();
            SetUpDistributionVector(evidence);
            double bestScore = 0;
            string bestWord = "";
            foreach(string s in dictionary)
            {
                double value = ComputeWeightedProbability(s);
                if(value > bestScore)
                {
                    bestScore = value;
                    bestWord = s;
                }
            }
            return bestWord;
        }
        public static double GetWordLikelihood(Model model, Queue<string> evidence, string word)
        {   
            List<double> weights = getWeights();
            List<double> likelihoods = new List<double>();
            nWeights = new List<double>();
            while (evidence.Count >= 0)
            {
                Gram g = model.getGramFromChain(evidence);
                double temp =  0;
                int total = g.getCount();
                if (total > 0)
                {
                    int occurences = g[word].getCount();
                    temp = occurences / (double)total;
                }
                likelihoods.Add(temp);
                nWeights.Add(weights[evidence.Count]);
                if (evidence.Count == 0)
                    break;
                evidence.Dequeue();
            }
            double likelihood = 0;
            for(int i = 0; i < likelihoods.Count; i++)
            {
                likelihood += likelihoods[i] * nWeights[i];
            }
            return likelihood * 100;
        }
    }

}
