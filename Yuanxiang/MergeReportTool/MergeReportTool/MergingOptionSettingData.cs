using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeReportTool
{
    public class MergingOptionSettingData
    {
        public int CategoryId { get; set; }
        public bool FirstCharacterIsLetter { get; set; }
        public bool IncludeCharacterAndLetter { get; set; }
        public int LengthModel { get; set; }
        public bool isColourMatch { get; set; }
    }
}
