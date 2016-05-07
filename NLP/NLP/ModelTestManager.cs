using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NLP
{
    /// <summary>
    /// This class will score a models ability to predict the next for of a test corpus
    /// </summary>
    public static class ModelTestManager
    {
        private static Model model;
        private static string testFilePath;
        private static int events;
        private static int correctPredictions;

        public static void InitializeManager(Model _model, string filePath)
        {
            model = _model;
            testFilePath = filePath;
            events = 0;
            correctPredictions = 0;
        }
        private static string PredictWord(Queue<string> evidence, string word)
        {
            string predicted = Analysis.PredictWord(model, new Queue<string>(evidence.ToArray()));
            UpdateModel(evidence, word);
            return predicted;
        }
        private static void UpdateModel(Queue<string> evidence, string word)
        {
            while (evidence.Count > 0)
            {
                Gram temp = model.getGramFromChain(new Queue<string>(evidence.ToArray()));
                temp[word].Increment();
                evidence.Dequeue();
            }
            model[word].Increment();
        }
        public static Tuple<int, int> TestModel()
        {
            Console.WriteLine(String.Format("Beginning test using {0}", testFilePath));
            Queue<string> evidence = new Queue<string>();
            //string[] lines = System.IO.File.ReadAllLines("../../" + fileName);
            int count = 0;
            string[] lines = System.IO.File.ReadAllLines(testFilePath);
            for (int i = 0; i < lines.Count() && count < 1000; i++)
            {
                count++;
                Writer.SetCursor(0,2);
                Writer.ClearLine();
                Console.WriteLine(count);
                //Console.WriteLine(lines[i]);
                string stripped = Regex.Replace(lines[i], Model.RegexCharRemoval, "");
                //Console.WriteLine(stripped);
                string[] phrases = stripped.Split(' ', '-', '_');
                for (int j = 0; j < phrases.Count(); j++)
                {
                    //Console.WriteLine(phrases[j]);
                    string word = phrases[j].ToLower();
                    //Console.WriteLine(word);
                    string check = Regex.Replace(word, Model.RegexTerminators, "");
                    if (check == "")
                        break;
                    //Console.WriteLine(check);
                    string prediction = PredictWord(new Queue<string>(evidence.ToArray()), check);
                    events++;
                    if (prediction == check)
                        correctPredictions++;
                    evidence.Enqueue(check);
                    if (evidence.Count >= model.getModelDepth())
                        evidence.Dequeue();
                    bool terminator = (check != word);
                    if (terminator)
                    {
                        evidence.Clear();
                    }
                }
            }
            Console.WriteLine("Trained on file " + testFilePath);
            Console.WriteLine("Scored {0} of {1}", correctPredictions, events);

            return new Tuple<int, int>(correctPredictions, events);
        }


    }
}
