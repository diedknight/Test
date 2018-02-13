using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttrToExcel.Data
{
    public class ExcelLineData
    {
        public string DataSourceName { get; set; }
        public string RetailerID { get; set; }
        public string Cid { get; set; }
        public string Category { get; set; }
        public string AttributeGroup { get; set; }
        public string DSattributename { get; set; }
        public string DSAttributeUnit { get; set; }        
        public string PriceMeAttributename { get; set; }
        public string PriceMeAttributeUnit { get; set; }
        public string Examplevalue { get; set; }
        public string Definedvalues { get; set; }
        public string Valuetype { get; set; }
        public string Attributetype { get; set; }
    }
}
