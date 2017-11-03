using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyze
{
    public class WordScore
    {
        private int _times = 0;

        public int PId { get; set; }

        private double _score = 0d;
        public double Score
        {
            get { return _score; }
            set
            {
                _score = value;
                _times++;
            }
        }

        public double AverageScore { get { return _score / _times; } }
        
    }
}
