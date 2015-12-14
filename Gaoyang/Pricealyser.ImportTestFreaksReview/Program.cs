using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Pricealyser.ImportTestFreaksReview
{
    class Program
    {
        static void Main(string[] args)
        {

            #region 注释代码

            //test
          //  List<string> needUpdateModifyOn = new List<string>();
          //  for (int j = 0; j < 15; j++)
          //  {
          //      for (int k = 0; k < 6; k++)
          //      {
          //          needUpdateModifyOn.Add(j+"|"+k);
          //      }
          //  }
          //string  sql = "Update CSK_Store_ExpertReviewAU "
          //                                + "Set ModifiedOn= '" + DateTime.Now+"' "
          //                                + " Where ";//ProductID = " + productId + " And SourceID = " + "sid";
          //  for (int i = 0; i < needUpdateModifyOn.Count; i ++)
          //  {
          //      string[] temp = needUpdateModifyOn[i].Split('|');
          //      string productId = temp[0];
          //      string sid = temp[1];

          //      if ((i % 50 == 0 || i == needUpdateModifyOn.Count - 1) && i!=0)
          //      {
          //          sql += "(ProductID = " + productId + " And SourceID = " + sid + ")";

          //          string a = sql;
          //          sql =  "Update CSK_Store_ExpertReviewAU "
          //                                + "Set ModifiedOn= '" + DateTime.Now+"' "
          //                                + " Where ";
          //      }
          //      else
          //      {
          //          sql += "(ProductID = " + productId + " And SourceID = " + sid +") or ";
          //      }
            //  }
            #endregion

            string logPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"].ToString();
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.BelowNormal;

            string date = DateTime.Now.ToString("yyyyMMdd");

            CreateDir(logPath);
            StreamWriter sw = new StreamWriter(logPath + "\\" + date + ".txt");

            ImportReview ir = new ImportReview();
            ir.SW = sw;
            ir.Import();
        }

        private static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
