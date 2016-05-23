using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public partial class Model
    {
        public static string terminators = ".!?;";
        public static string wordBreak = " -_";
        public static string RegexCharRemoval = "[^a-zA-Z0-9\\.\\?\\!;\' ]";
        public static string RegexTerminators = "[\\.\\!\\?;]";
        public static List<string> exceptionList = new List<string> { "mr.", "mrs.", "dr." };

        private List<double> weights;
        private int modelDepth;
        /// EDIT DISTANCE COST FUNCTIONS ///
        private int editDistanceCutoff = 3;
        private int insCost = 1;
        private int remCost = 1;
        private int subCost = 2;
      
    }
}
