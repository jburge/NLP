using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public static class Analysis
    {
        public static List<Tuple<double, string>> Evaluate(List<Tuple<double, string>> distribution, string currentWord)
        {
            List<Tuple<double, string>> valuation = new List<Tuple< double, string>>();
            foreach (Tuple<double, string> t in distribution)
            {
                int ed = EditDistance.ComputeEditDistanceDP(t.Item2, currentWord);
                if(ed != -1)//-1 cutoff occured
                {
                    valuation.Add(new Tuple<double, string>(ComputeValue(ed, t.Item1), t.Item2));
                }
            }
            valuation.Sort();
            valuation.Reverse();
            return valuation;
        }
        // word, ed, prob
        public static List<Tuple<double, string>> EvaluatePossibilities(List<Tuple<string, int, double>> potentialMatches)
        {
            List<Tuple<double, string>> values = new List<Tuple<double, string>>();
            foreach (Tuple<string, int, double> t in potentialMatches)
            {
                values.Add(new Tuple<double, string>(ComputeValue(t.Item2, t.Item3), t.Item1));
            }
            values.Sort();
            return values;
        }
        private static double ComputeValue(int ed, double prob)
        {
            return (1 / (double)ed * prob);
        }

    

    
    }
}
