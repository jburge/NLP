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
    public class ModelTestManager
    {
        private static Model model;
        private static string testFilePath;
        private static int events;
        private static int correctPredictions;
        private static int fake;

        public ModelTestManager(Model _model, string filePath)
        {
            model = _model;
            testFilePath = filePath;
            events = 0;
            correctPredictions = 0;
            fake = 0;
        }
        private string PredictWord(Queue<string> evidence, string word)
        {
            string predicted = Analysis.PredictWord(model, new Queue<string>(evidence.ToArray()));
            UpdateModel(evidence, word);
            return predicted;
        }
        private void UpdateModel(Queue<string> evidence, string word)
        {
            while (evidence.Count > 0)
            {
                Gram temp = model.getGramFromChain(new Queue<string>(evidence.ToArray()));
                temp[word].Increment();
                evidence.Dequeue();
            }
            model[word].Increment();
        }
        public Tuple<int, int> TestModel()
        {
            Console.WriteLine(String.Format("Beginning test using {0}", testFilePath));
            Queue<string> evidence = new Queue<string>();
            //string[] lines = System.IO.File.ReadAllLines("../../" + fileName);
            string[] phrases = RegexLogic.GetPhrasesFromFile(testFilePath);
            Console.WriteLine(String.Format("{0} words", phrases.Count()));
            for (int i = 0; i < phrases.Count(); i++)
            {
                //Writer.SetCursor(0, Console.CursorTop);
                //Writer.ClearLine();
                //Console.Write(events + "\t" + fake);
                string phrase = phrases[i];
                string word = phrase.ToLower();
                if (!Model.exceptionList.Contains(word))
                    word = Regex.Replace(word, "[\\.\\?\\!;~]", "").ToLower();
                if (word == "")
                {
                    fake++;
                    continue;
                }
                //Console.WriteLine(check);
                string prediction = PredictWord(new Queue<string>(evidence.ToArray()), word);
                events++;
                //Debugger.Log(word);
                if (prediction == word)
                    correctPredictions++;
                evidence.Enqueue(word);
                if (evidence.Count >= model.getModelDepth())
                    evidence.Dequeue();
                bool terminator = (phrase != word);
                if (terminator)
                {
                    evidence.Clear();
                }
            }
            Console.WriteLine();
            Debugger.Log(String.Format("{0}:\n\tevents: {1}\n\tcorrect: {2}", testFilePath, events, correctPredictions));
            Debugger.Log(String.Format("Fake:{0}", fake));
            return new Tuple<int, int>(correctPredictions, events);
        }


    }
}
