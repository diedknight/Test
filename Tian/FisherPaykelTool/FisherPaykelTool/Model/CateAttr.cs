using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherPaykelTool.Model
{
    public class CateAttr
    {
        public int CategoryId { get; set; }

        public List<AttrInfo> Size_Capacity { get; set; }
        public List<AttrInfo> Type_Functions { get; set; }
        public List<AttrInfo> Finish { get; set; }
        public List<AttrInfo> Energy_Water_Rating { get; set; }        
    }
}
