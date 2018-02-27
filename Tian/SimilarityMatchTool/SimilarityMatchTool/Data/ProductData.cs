using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarityMatchTool.Data
{
    public class ProductData
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public int ManufacturerID { get; set; }
        public decimal Price { get; set; }
    }
}
