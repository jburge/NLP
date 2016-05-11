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
        private static Queue<string> evidence;

        public ModelTestManager(Model _model, string filePath)
        {
            model = _model;
            testFilePath = filePath;
            events = 0;
            correctPredictions = 0;
            fake = 0;
            evidence = new Queue<string>();
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
        private void UpdateTestState(string word, string phrase)
        {
            events++;
            evidence.Enqueue(word);
            if (evidence.Count >= model.getModelDepth())
                evidence.Dequeue();
            bool terminator = (phrase != word);
            if (terminator)
            {
                evidence.Clear();
            }
        }
        private string GetWordFromPhrase(string phrase)
        {
            string word = phrase.ToLower();
            if (!Model.exceptionList.Contains(word))
                word = Regex.Replace(word, "[\\.\\?\\!;~]", "").ToLower();
            return word;
        }
        public Tuple<int, int> TestModelPrediction()
        {
            Debugger.StartTest(model, testFilePath.Split('\\').Last());
            //string[] lines = System.IO.File.ReadAllLines("../../" + fileName);
            string[] phrases = RegexLogic.GetPhrasesFromFile(testFilePath);
            for (int i = 0; i < phrases.Count(); i++)
            {
                string phrase = phrases[i];
                string word = GetWordFromPhrase(phrase);
                if (word == "")
                {
                    fake++;
                    continue;
                }
                string prediction = PredictWord(new Queue<string>(evidence.ToArray()), word);
                if (prediction == word)
                {
                    correctPredictions++;
                    Debugger.LogMatch(model, testFilePath.Split('\\').Last(), word);
                }
                UpdateTestState(word, phrase);
            }
            Debugger.Log(String.Format("{0}:\n\tevents: {1}\n\tcorrect: {2}\n\tfake: {3}", testFilePath, events, correctPredictions, fake));
            Console.WriteLine();
            Debugger.FinishTest(model, testFilePath.Split('\\').Last());
            return new Tuple<int, int>(correctPredictions, events);
        }
        public double EvaluateWord(Queue<string> evidence, string word)
        {
            double likelihood = Analysis.GetWordLikelihood(model, evidence, word);
            UpdateModel(evidence, word);
            return likelihood;
        }
	public double TestModelValuation()
	{
		Debugger.StartTest(model, testFilePath.Split('\\').Last());
		string[] phrases = RegexLogic.GetPhrasesFromFile(testFilePath);
		double scoreSum = 0;
		for (int i = 0; i < phrases.Count(); i++)
		{
			string phrase = phrases[i];
            string word = GetWordFromPhrase(phrase);
			if(word =="")
			{
				fake++;
				continue;
			}
			double modelEvaluation = EvaluateWord(new Queue<string>(evidence.ToArray()), word);
			scoreSum += modelEvaluation;
            UpdateTestState(word, phrase);
		}
		double modelScore = scoreSum / (double)events;
		Debugger.Log(String.Format("{0}: {1}", testFilePath, modelScore));
        Console.WriteLine();
        Debugger.FinishTest(model, testFilePath.Split('\\').Last());
		return modelScore;
	}

    }
}
