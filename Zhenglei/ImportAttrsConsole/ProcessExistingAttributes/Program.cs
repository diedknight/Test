using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using ImportAttrs.Data;

namespace ProcessExistingAttributes
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbStr = System.Configuration.ConfigurationManager.ConnectionStrings["PriceMe_DB"].ConnectionString;
            string sql = @"select * from AttributeUnmatchedReport where Status=1 and AttTitleID in
                            (select CompareAttributeID from Store_Compare_Attributes where AttributeTypeID in (4, 6)) and AttType = 3
                            union
                            select* from AttributeUnmatchedReport where Status = 1 and AttTitleID in
                            (select typeid from CSK_Store_ProductDescriptorTitle where AttributeTypeID in (4,6)) and AttType = 2 ";

            using (var con = new SqlConnection(dbStr))
            {
                var list = con.Query<UnmatchReportData>(sql).ToList();

                Console.WriteLine("read...list:" + list.Count);
                Console.WriteLine("dbStr:" + dbStr);

                list.ForEach(data =>
                {
                    var productInfo = new ImportProductInfo();
                    productInfo.CategoryId = data.CID;
                    productInfo.ProductId = data.PID;
                    productInfo.RetailerId = data.RID;
                    productInfo.AllAttributesDic = new Dictionary<string, string>();
                    productInfo.AllAttributesDic.Add(data.DR_AttName, data.DR_AttValue_Orignal);

                    ImportAttrs.ImportController1.ImportAttr(productInfo, data);

                });

            }

            Console.ReadLine();
        }
    }
}
