using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    public class Data
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public double W1 { get; set; }
        public double W2 { get; set; }
        public double A { get; set; }

        public int ExcelLineIndex { get; set; }


        public List<CompareData> CompareList { get; set; }
    }
}
