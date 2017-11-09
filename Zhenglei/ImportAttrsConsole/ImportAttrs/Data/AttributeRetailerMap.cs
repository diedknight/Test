using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttrs.Data
{
    /// <summary>
    /// 对应[CSK_Store_AttributeRetailerMap]表
    /// </summary>
    public class AttributeRetailerMap
    {
        public int Id { get; set; }
        public int RetailerId { get; set; }
        public int CategoryId { get; set; }
        public string RetailerAttributeName { get; set; }
        public string Unit { get; set; }
        public string PM_AttributeID { get; set; }
        public int AttributeType { get; set; }
        public bool IsRemove { get; set; }
        public string RemoveKeword { get; set; }
        public bool IsSplit { get; set; }
        public string SplitKeyword { get; set; }
        public bool KeepBefore { get; set; }
        public bool IsCombine { get; set; }
        public string CombineAttribute { get; set; }
        public bool KeepCombineAttributeBefore { get; set; }
    }
}