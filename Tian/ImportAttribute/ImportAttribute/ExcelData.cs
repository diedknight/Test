using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttribute
{
    public class ExcelData
    {
        public string DataSourceName { get; set; }
        public string RetailerID { get; set; }
        public string Cid { get; set; }
        public string Category { get; set; }
        public string AttributeGroup { get; set; }
        public string DSAttributeName { get; set; }
        public string DSAttributeUnit { get; set; }
        public string PriceMeAttributeName { get; set; }
        public string PriceMeAttributeUnit { get; set; }
        public string ExampleValue { get; set; }
        public string DefinedValues { get; set; }
        public string ValueType { get; set; }
        public string AttributeType { get; set; }
        public string Remark { get; set; }
        public string Description { get; set; }

        public static List<ExcelData> Load()
        {
            List<ExcelData> list = new List<ExcelData>();

            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dragon Matching.xlsx");

            var ex = new Priceme.Infrastructure.Excel.ExcelSimpleHelper();
            ex.Read(fileName);

            for (int i = 0; i < 500; i++)
            {
                var line = ex.ReadLine(0, 15);

                if (i < 1) continue;

                ExcelData info = new ExcelData();
                info.DataSourceName = line[0];
                info.RetailerID = line[1];
                info.Cid = line[2];
                info.Category = line[3];
                info.AttributeGroup = line[4];
                info.DSAttributeName = line[5];
                info.DSAttributeUnit = line[6];
                info.PriceMeAttributeName = line[7];
                info.PriceMeAttributeUnit = line[8];
                info.ExampleValue = line[9];
                info.DefinedValues = line[10];
                info.ValueType = line[11];
                info.AttributeType = line[12];
                info.Remark = line[13];
                info.Description = line[14];

                if (info.DataSourceName == "" &&
                    info.RetailerID == "" &&
                    info.Cid == "" &&
                    info.Category == "" &&
                    info.AttributeGroup == "" &&
                    info.DSAttributeName == "" &&
                    info.DSAttributeUnit == "" &&
                    info.PriceMeAttributeName == "" &&
                    info.PriceMeAttributeUnit == "" &&
                    info.ExampleValue == "" &&
                    info.DefinedValues == "" &&
                    info.ValueType == "" &&
                    info.AttributeType == "" &&
                    info.Remark == "" &&
                    info.Description == ""
                    )
                {
                    continue;
                }

                list.Add(info);
            }

            return list;
        }

    }
}
