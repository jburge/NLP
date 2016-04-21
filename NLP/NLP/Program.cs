using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace NLP
{
    class Program
    {
        static void Main(string[] args)
        {
            Model2 myModel = new Model2(3);
            myModel.ReadInputCorpus("Input.txt");
            myModel.DisplayModel();
        }
    }
}
