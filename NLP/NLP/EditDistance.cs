using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    /// <summary>
    /// This is a static class used to compute edit distance
    /// Edit distance a linear weight used to consider which words may have been intended given predicate chain q and currentword w
    /// All words w' in the dictionary are are compared to w and scored on Levenshtein distance
    /// Various techniques may be used to try and provide more useful feedback (including keyboard proximity among other strats)
    /// </summary>
    public static class EditDistance
    {
        private static int cutoff = 3;                      // used to prevent wasteful computation
        private static int insCost = 1;
        private static int remCost = 1;
        private static int subCost = 2;
        private static int min(int x, int y, int z)
        {        // custom min function for 3 inputs
            return Math.Min(Math.Min(x, y), z);
        }
        /// <summary>
        /// This function uses DP to compute the Levenshtein distance between w1 and w2
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        /// <returns>Integer edit distance value</returns>
        public static int ComputeEditDistanceDP(string w1, string w2)
        {
            int lengthDif = Math.Abs(w1.Length - w2.Length);
            if (lengthDif > cutoff)
                return -1;
            int[,] dp = new int[w1.Length + 1, w2.Length + 1];

            for (int i = 0; i <= w1.Length; i++)
            {
                bool cut = true;
                for (int j = 0; j <= w2.Length; j++)
                {
                    if (i == 0)
                    {
                        dp[i, j] = j;
                    }
                    else if (j == 0)
                    {
                        dp[i, j] = i;
                    }
                    else if (w1[i - 1] == w2[j - 1])
                    {
                        dp[i, j] = dp[i - 1, j - 1];
                    }
                    else
                    {                 // insert,    remove,      replace
                        dp[i, j] = min(dp[i, j - 1] + insCost, dp[i - 1, j] + remCost, dp[i - 1, j - 1] + subCost);
                    }
                    if (w2.Length - j <= w1.Length - i)
                    {
                        if (dp[i, j] < cutoff)
                        {
                            cut = false;
                        }
                    }
                    //Console.Write(dp[i, j] + " ");
                }
                if (cut)
                {
                    return -1;
                }
                //Console.WriteLine();
            }
            return dp[w1.Length, w2.Length];
        }
        public static List<Tuple<double, string>> ComputeEditDistances(List<string> words, string currentWord)
        {
            List<Tuple<double, string>> editDistances = new List<Tuple<double, string>>();
            foreach (string w_prime in words)
            {
                double ed = ComputeEditDistanceDP(w_prime, currentWord);
                if (ed != -1)
                    editDistances.Add(new Tuple<double, string>(ed + .0001, w_prime));//adding a one right now to prevent divide by 0
            }
            return editDistances.OrderBy(x => x).ToList();
        }
    }

}
