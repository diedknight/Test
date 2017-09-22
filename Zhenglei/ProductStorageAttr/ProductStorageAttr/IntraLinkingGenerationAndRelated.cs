using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStorageAttr
{
    public class IntraLinkingGenerationAndRelated
    {
        public int ProductId { get; set; } = 0;
        public string LinkType { get; set; } = "Related product";
        public string ShowType { get; set; } = "2";
        public int LinedPID { get; set; } = 0;
        public string VariableNameOfPID { get; set; } = "";
        public string LinedPname { get; set; } = "";
        public string VariableNameOfPN { get; set; } = "";
        public string AttributeName { get; set; } = "";
        public string VariableNameOfAN { get; set; } = "";
        public string AttributeURL { get; set; } = "";
        public string VariableNameOfAURL { get; set; } = "";
        public string Text { get; set; } = "";
        public string CreatedBy { get; set; } = "RelatedStorage";
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "RelatedStorage";
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public string ToSqlString(DateTime dt)
        {
            string format = "select {0}, '{1}', '{2}', {3}, '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}'";
            return string.Format(format, ProductId, LinkType, ShowType, LinedPID, VariableNameOfPID, LinedPname, VariableNameOfPN, AttributeName, VariableNameOfAN, AttributeURL, VariableNameOfAURL, Text, CreatedBy, dt.ToString("yyyy-MM-dd HH:mm:ss"), ModifiedBy, dt.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}