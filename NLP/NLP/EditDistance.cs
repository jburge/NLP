using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public static class EditDistance
    {
        private static int min(int x, int y, int z)
        {
            return Math.Min(Math.Min(x, y), z);
        }

        /// <summary>
        /// This function uses DP to compute the Levenshtein distance between w1 and w2
        /// </summary>
        /// <param name="w1"></param>
        /// <param name="w2"></param>
        /// <returns>Integer edit distance value</returns>
        public static int ComputeEditDistance(Model model, string w1, string w2)
        {
            int cutoff = model.getCutoff();
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
                        dp[i, j] = min(dp[i, j - 1] + model.getInsertCost(), 
                            dp[i - 1, j] + model.getRemoveCost(), 
                            dp[i - 1, j - 1] + model.getSubstitutionCost());
                    }
                    if (w2.Length - j <= w1.Length - i)
                    {
                        if (dp[i, j] < cutoff)
                        {
                            cut = false;
                        }
                    }
                }
                if (cut)
                {
                    return -1;
                }
            }
            return dp[w1.Length, w2.Length];
        }
    }
}
