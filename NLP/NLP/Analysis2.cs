using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public static class Analysis2
    {
        private static List<Dictionary<string, double>> distributions;

        private static Model model;
        public static List<Tuple<double, string>> DetermineLikelyReplacements(Queue<string> predicate, string word, Model _model)
        {
            model = _model;
            Dictionary<string, double> distribution = ComputeDistribution(predicate);
            Dictionary<string, double> editDistances = getEditDistances(word);

            List<Tuple<double, string>> candidates = new List<Tuple<double, string>>();
            foreach(string w in model.GetDictionary())
            {
                candidates.Add(new Tuple<double,string>( distribution[w] / editDistances[w], w));
            }
            candidates.Sort();
            candidates.Reverse();

            Writer.PrintPostWriteEvaluation(candidates);
            Writer.PrintProbabilityDistribution(candidates, distribution);
            Writer.PrintEditDistance(candidates, editDistances);
            return candidates;
        }
        private static Dictionary<string, double> ComputeDistribution(Queue<string> predicate)
        {
            distributions = new List<Dictionary<string, double>>();
            Dictionary<string, double> values = new Dictionary<string, double>();
            while (predicate.Count >= model.getWeights().Count())
            {
                predicate.Dequeue();
            }
            for(int i = 0; i < model.getWeights().Count(); i++)
            {
                distributions.Add(getLaplacianDistribution(predicate));
                predicate.Dequeue();
            }
            distributions.Reverse(); //now in uni -> larger gram order
            foreach (string word in model.GetDictionary())
            {
                double value = getInterpolatedValue(word);
                values.Add(word, value);
            }
            return values;
        }
        private static Dictionary<string, double> getEditDistances(string word)
        {
            Dictionary<string, double> editDistances = new Dictionary<string, double>();
            foreach(string w in model.GetDictionary())
            {
                editDistances.Add(w, EditDistance.ComputeEditDistanceDP(word, w));
            }
            return editDistances;
        }
        private static double getInterpolatedValue(string word)
        {
            double value = 0;
            for (int i = 0; i < distributions.Count(); i++)
            {
                value += model.getWeights()[i] * distributions[i][word];
            }
            return value;
        }
        private static Dictionary<string, double> getLaplacianDistribution(Queue<string> predicate)
        {
            Gram predicateState = model.getGramFromChain(new Queue<string>(predicate.ToArray()));
            Dictionary<string, double> probDistribution = new Dictionary<string, double>();
            List<string> vocabulary = model.GetDictionary();
            int predicateOccurances = predicateState.getCount();
            predicateOccurances += vocabulary.Count();
            foreach(string word in vocabulary)
            {
                int count = 1;
                count += predicateState.NextWordCount(word);
                probDistribution.Add(word, count / (double)predicateOccurances);
            }
            return probDistribution;
        }

    }
}
