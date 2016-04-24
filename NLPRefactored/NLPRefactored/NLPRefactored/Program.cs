using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;

namespace NLPRefactored
{
    class Program
    {
       
        static void Main(string[] args)
        {
            SetUpWindow();
            Model model = InitializeModel();
            //model.DisplayModel();
            DynamicReader.InputLoop(model);
        }
        static void GetInfo(Model model){
            string temp = Console.ReadLine();
            Console.WriteLine(model.GetWordCount(temp)); 
        }
        static void SetUpWindow()
        {
            Console.WindowWidth = Writer.windowW;
            Console.WindowHeight = Writer.windowH;
        }
        static Model InitializeModel()
        {
            Model m = new Model(3);
            string txtFolderPath = "..\\..\\TextFiles\\";
            string[] files = Directory.GetFiles(txtFolderPath, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (string fileName in files)
            {
                m.TrainModel(fileName);
            }
            Console.Clear();
            return m;
        }
    }
}
