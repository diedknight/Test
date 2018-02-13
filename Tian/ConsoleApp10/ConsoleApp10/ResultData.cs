using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    public class ResultData
    {
        public double SimilarityScore { get; set; }
        public double PriceDiff { get; set; }
        public double SimilarityPrice { get; set; }
        public double TotSimilarity { get; set; }

        public int ExcelLineIndex { get; set; }
    }
}
