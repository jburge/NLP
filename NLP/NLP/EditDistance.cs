using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public static class EditDistance
    {
        private static int min(int x, int y, int z){
            return Math.Min( Math.Min( x, y), z);
        }
        //Non-weighted edit distance
        //should add terminiation cutoff value
        public static int ComputeEditDistanceDP(string w1, string w2, int c1, int c2)
        {
            //List<List<int>> dp = new List<List<int>>();
            int[,] dp = new int[c1+1,c2+1];
            for (int i = 0; i <= c1; i++)
            {
                for (int j = 0; j <= c2; j++)
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
                    else {                 // insert,    remove,      replace
                        dp[i,j] = 1 + min(dp[i,j - 1], dp[i - 1,j], dp[i - 1,j - 1]);
                    }
                }
            }
            return dp[c1,c2];
        }
    }
}
