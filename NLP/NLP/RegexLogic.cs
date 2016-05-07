using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NLP
{
    public static class RegexLogic
    {
        public static string[] GetPhrasesFromFile(string filePath)
        {
            string fileContents = System.IO.File.ReadAllText(filePath);
            fileContents = Regex.Replace(fileContents, "[^a-zA-Z0-9\\.\\?\\!;\'~\\-_ ]", "");

            fileContents = Regex.Replace(fileContents, "[\\-_]", " ");
            // put one space where there is any whitespace
            fileContents = Regex.Replace(fileContents, @"\s+", " ");
            // next two lines remove single quotes but seek to allow apostrophes
            // fails on (boys') case
            fileContents = Regex.Replace(fileContents, "(?<![a-z])'", "");
            fileContents = Regex.Replace(fileContents, "'(?![a-z])", "");
            return fileContents.Split(' ');
        }
    }
}
