using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace SimilarityMatchTool
{
    class Program
    {
        static void Main(string[] args)
        {            
            var cache1 = ProductCtrl.GetCache1();
            var cache2 = ProductCtrl.GetCache2();

            cache1.ForEach(product => {

                var compareList = cache2.Where(compare => compare.ManufacturerID == product.ManufacturerID).ToList();
                compareList.ForEach(compareProduct => {
                    var score = Score.Similarity(product.ProductName.Trim(), product.Price, compareProduct.ProductName.Trim(), compareProduct.Price);
                    if (score > AppConfig.Score)
                    {
                        using (var con = DB.Connection)
                        {
                            con.Execute(SqlConfig.InsertSimilarityMatchReport, new
                            {
                                CID = product.CategoryID,
                                PID = product.ProductID,
                                PName = product.ProductName,
                                Price = product.Price,
                                ToPID = compareProduct.ProductID,
                                ToPName = compareProduct.ProductName,
                                ToPrice = compareProduct.Price,
                                Score = score,
                                CreatedBy = ""
                            });
                        }
                    }

                });

            });

        }
    }
}
