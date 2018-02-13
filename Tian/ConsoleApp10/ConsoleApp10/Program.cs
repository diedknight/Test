using F23.StringSimilarity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test cases similarity merging.xlsx");
            Priceme.Infrastructure.Excel.ExcelSimpleHelper exhelper = new Priceme.Infrastructure.Excel.ExcelSimpleHelper();
            exhelper.Read(path);

            var list = GetData(exhelper);

            list.ForEach(data => {

                GetResults(data).ForEach(res => {
                    exhelper.CurIndex = res.ExcelLineIndex;

                    exhelper.UpdateCell(2, res.SimilarityScore.ToString());
                    exhelper.UpdateCell(7, res.SimilarityPrice.ToString());
                    exhelper.UpdateCell(8, res.TotSimilarity.ToString());
                });
            });

            exhelper.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.xlsx"));
        }


        public static List<ResultData> GetResults(Data data)
        {
            var cos = new Cosine();

            List<ResultData> list = new List<ResultData>();

            data.CompareList.ForEach(compare => {
                double priceDiff = 1;
                if (data.Price != 0)
                {
                    priceDiff = (double)(Math.Abs(data.Price - compare.Price) / data.Price);
                }

                //w1 * (1-a*(price diff %) )/100
                double similarityPrice = data.W1 * (1 - data.A * priceDiff) / 100;

                //w2 * Cosine similarity
                double similarityCosine = data.W2 * cos.Similarity(data.ProductName, compare.ProductName);

                double totalSimilarity = similarityPrice + similarityCosine;

                ResultData result = new ResultData();
                result.ExcelLineIndex = compare.ExcelLineIndex;
                result.PriceDiff = priceDiff;
                result.SimilarityPrice = similarityPrice;
                result.SimilarityScore = similarityCosine;
                result.TotSimilarity = totalSimilarity;

                list.Add(result);
            });

            return list;
        }


        public static List<Data> GetData(Priceme.Infrastructure.Excel.ExcelSimpleHelper exhelper)
        {
            List<Data> list = new List<Data>();
            Data data = null;

            var line = exhelper.ReadLine();
            while (line != null)
            {
                if (!IsEmptyLine(line) && !IsHeaderLine(line) && line.Count > 1)
                {
                    if (line[1].ToLower() == "y") //base y
                    {
                        data = new Data();
                        data.CompareList = new List<CompareData>();
                        data.ProductName = line[0].Trim().ToLower();
                        data.Price = Convert.ToDecimal(line[3]);
                        data.W1 = string.IsNullOrEmpty(line[4]) ? 1d : Convert.ToDouble(line[4]);
                        data.W2 = string.IsNullOrEmpty(line[5]) ? 1d : Convert.ToDouble(line[5]);
                        data.A = string.IsNullOrEmpty(line[6]) ? 1d : Convert.ToDouble(line[6]);
                        data.ExcelLineIndex = exhelper.CurIndex - 1;

                        list.Add(data);
                    }

                    if (line[1].ToLower() == "n")    //base n
                    {
                        CompareData cData = new CompareData();
                        cData.ProductName = line[0].Trim().ToLower();
                        cData.Price = string.IsNullOrEmpty(line[3]) ? 0m : Convert.ToDecimal(line[3]);
                        cData.ExcelLineIndex = exhelper.CurIndex - 1;

                        data.CompareList.Add(cData);
                    }
                }

                line = exhelper.ReadLine();
            }

            return list;
        }

        public static bool IsHeaderLine(List<string> line)
        {
            return line[0].ToLower() == "product" && line[1].ToLower() == "base";
        }

        public static bool IsEmptyLine(List<string> line)
        {
            if (line.Count == 0) return true;

            var isEmptyLine = true;

            line.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(item))
                {
                    isEmptyLine = false;
                }
            });

            return isEmptyLine;
        }

    }
}
