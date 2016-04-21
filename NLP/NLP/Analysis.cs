using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public static class Analysis
    {
        // word, ed, prob
        public static List<Tuple<double, string>> EvaluatePossibilities(List<Tuple<string, int, double>> potentialMatches)
        {
            List<Tuple<double, string>> values = new List<Tuple<double, string>>();
            foreach (Tuple<string, int, double> t in potentialMatches)
            {
                values.Add(new Tuple<double, string>(ComputeValue(t.Item2, t.Item3), t.Item1));
            }
            return values;
        }
        private static double ComputeValue(int ed, double prob)
        {
            return (1 / (double)ed * prob);
        }

    

    
    }
}
