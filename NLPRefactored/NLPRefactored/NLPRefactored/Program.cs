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
        public static int windowW = 120;
        public static int windowH = 50;
        static void Main(string[] args)
        {
            SetUpWindow();
            Model model = InitializeModel();
            DynamicReader.InputLoop(model);
        }

        static void SetUpWindow()
        {
            Console.WindowWidth = windowW;
            Console.WindowHeight = windowH;
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
            return m;
        }
    }
}
