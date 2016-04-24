using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPRefactored
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
        private static int min(int x, int y, int z){        // custom min function for 3 inputs
            return Math.Min( Math.Min( x, y), z);
        }
        /// <summary>
        /// This function uses DP to compute the Levenshtein distance between w1 and w2
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        /// <returns>Integer edit distance value</returns>
        public static double ComputeEditDistanceDP(string w1, string w2)
        {
            //List<List<int>> dp = new List<List<int>>();
            int[,] dp = new int[w1.Length+1,w2.Length+1];
            for (int i = 0; i <= w1.Length; i++)
            {
                for (int j = 0; j <= w2.Length; j++)
                {
                    if (i == 0){
                        dp[i,j] = j;
                    }
                    else if (j == 0) {
                        dp[i,j] = i;
                    }
                    else if (w1[i - 1] == w2[j - 1]) {
                        dp[i,j] = dp[i - 1,j - 1];
                    }
                    else {                 // insert,    remove,      replace (+1)
                        dp[i,j] = 1 + min(dp[i,j - 1], dp[i - 1,j], dp[i - 1,j - 1]+1);
                        //prematurelly terminate if not promising
                        if(dp[i,j] > cutoff)
                        {
                            return -1;
                        }
                    }
                }
            }
            return dp[w1.Length, w2.Length];
        }
        public static List<Tuple<double, string>> ComputeEditDistances(List<string> words, string currentWord)
        {
            List<Tuple<double, string>> editDistances = new List<Tuple<double, string>>();
            foreach (string w_prime in words)
            {
                double ed = ComputeEditDistanceDP(w_prime, currentWord);
                if(ed != -1)
                    editDistances.Add(new Tuple<double, string>(ed + 1, w_prime));//adding a one right now to prevent divide by 0
            }
            return editDistances.OrderBy(x =>x).ToList();
        }
    }
    
}
