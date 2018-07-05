using System;

namespace BaseProductTool
{
    public class IntraLinkingGenerationAndRelated
    {
        public int ProductId { get; set; } = 0;
        public string BaseProductValue { get; set; } = "";
        public string LinkType { get; set; } = "Variant";
        public string ShowType { get; set; } = "1";
        public int LinedPID { get; set; } = 0;
        public string VariantProductValue { get; set; } = "";
        public string VariableNameOfPID { get; set; } = "";
        public string LinedPname { get; set; } = "";
        public string VariableNameOfPN { get; set; } = "";
        public string AttributeName { get; set; } = "";
        public string VariableNameOfAN { get; set; } = "";
        public string AttributeURL { get; set; } = "";
        public string VariableNameOfAURL { get; set; } = "";
        public string Text { get; set; } = "";
        public int VariantTypeID { get; set; } = 0;
        public string CreatedBy { get; set; } = "BaseProductTool";
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "BaseProductTool";
        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public string ToSqlString(DateTime dt)
        {
            string format = "select {0}, '{1}', '{2}', '{3}', {4}, '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', {14}, '{15}', '{16}', '{17}', '{18}'";
            return string.Format(format, ProductId, BaseProductValue, LinkType, ShowType, LinedPID, VariantProductValue, VariableNameOfPID, LinedPname, VariableNameOfPN, AttributeName, VariableNameOfAN, AttributeURL, VariableNameOfAURL, Text, VariantTypeID, CreatedBy, dt.ToString("yyyy-MM-dd HH:mm:ss"), ModifiedBy, dt.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
