using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prt = PriceMe.RichAttributeDisplayTool;

namespace PriceMe.RichAttributeDisplayTool.Process
{
    class ProcessData
    {

       public  List<Prt.RichClass.AttributeDisplayType> Adt = new List<RichClass.AttributeDisplayType>();

       public  List<Prt.RichClass.ProductValue> AttList = new List<RichClass.ProductValue>();


       public delegate void LogEventHandler(Object sender, LogEventArgs e);

       public event LogEventHandler Log;

       protected virtual void OnLog(LogEventArgs e)
       {
           if (Log != null)
               Log(this, e);
       }


       public delegate List<string> RankEventHandler(Object sender, RankEventArgs e);

       public event RankEventHandler Rank;

       protected virtual List<string> OnRank(RankEventArgs e)
       {

           if (Rank != null)
              return Rank(this,e);

           return null;
       }



       private List<double> getPer()
       {
           List<double> per = new List<double>();

           per.Add(0.1);
           per.Add(0.2);
           per.Add(0.3);
           per.Add(0.5);
           per.Add(0.7);
           per.Add(0.8);
           per.Add(0.9);

           return per;
       }


       private Dictionary<int, int> updateSucc = new Dictionary<int, int>();


       public void Process()
       {

           getAttributeDisplayType();

           var accList = new List<Prt.RichClass.AttributeCategoryComparisons>();
           var single = new Prt.DataProcessTool.LinqBllBase<Prt.DataProcessTool.AttributeCategoryComparison>();

           var singleAll = single.Query(q => q.ID > 0);

           #region CrawlData
           foreach (var item in Adt)
           {

               if (!item.IsComparison) continue;

               getProductTotalByAid(item.AttributeID, item.IsCompareAttribute);

               //当AccList列表里有数据大于10条时，触发Rank事件
               if (AttList.Count <= 10) continue;

               RankEventArgs rank = new RankEventArgs(getPer());

               var getRank = OnRank(rank);

               //Top10
               var top10 = getRank.First();

               //Top10
               var top20 = getRank[1];

               //Top30
               var top30 = getRank[2];

               //Average
               var average = getRank[3];

               //bottom10
               var bottom10 = getRank.Last();

               //bottom20
               var bottom20 = getRank[5];

               //bottom30
               var bottom30 = getRank[4];

               var getSingle = singleAll.SingleOrDefault(s => s.Aid == item.AttributeID && s.IsCompareAttribute == item.IsCompareAttribute);

               if (getSingle != null)
               {
                   getSingle.Aid = item.AttributeID;
                   getSingle.IsCompareAttribute = item.IsCompareAttribute;
                   getSingle.Average = average;
                   getSingle.Top10 = top10;
                   getSingle.Top20 = top20;
                   getSingle.Top30 = top30;
                   getSingle.Bottom10 = bottom10;
                   getSingle.Bottom20 = bottom20;
                   getSingle.Bottom30 = bottom30;
                   //getSingle.IsHigherBetter = true;
                   getSingle.Modifiedon = DateTime.Now;

                   single.Update(getSingle);

                   if (!updateSucc.ContainsKey(getSingle.Aid))
                       updateSucc.Add(getSingle.Aid, getSingle.Aid);
               }
               else
               {
                   var acc = new Prt.RichClass.AttributeCategoryComparisons();

                   acc.Aid = item.AttributeID;
                   acc.IsCompareAttribute = item.IsCompareAttribute;
                   acc.Average = average;
                   acc.Top10 = top10;
                   acc.Top20 = top20;
                   acc.Top30 = top30;
                   acc.Bottom10 = bottom10;
                   acc.Bottom20 = bottom20;
                   acc.Bottom30 = bottom30;
                   acc.IsHigherBetter = true;
                   acc.Modifiedon = DateTime.Now;
                   acc.Createon = DateTime.Now;

                   accList.Add(acc);
               }

           }
           #endregion

           //批量添加新数据
           string result = Prt.DataProcessTool.SqlHelper.InsertToTables(accList);

           LogEventArgs log = new LogEventArgs(accList, updateSucc);

           OnLog(log);

           Console.WriteLine("Exe Success.");

       }

       /// <summary>
       /// 获取分类下有Attribute的产品总数(一般Attribute或ComAttribute)
       /// </summary>
       /// <param name="aid"></param>
       /// <param name="isCom">True为ComAttribute，False为一般Attribute</param>
       private void getProductTotalByAid(int aid,bool isCom)
       {

           #region sql

           var sql = new StringBuilder();
           sql.Append("select");

           sql.Append(" ProductID,");
           sql.Append(" Value");

           sql.Append(" from");
           sql.Append(" CSK_Store_ProductDescriptor PD");
           sql.Append(" inner join");
           sql.Append(" CSK_Store_AttributeValue AV");
           sql.Append(" on");
           sql.Append(" PD.AttributeValueID=AV.AttributeValueID");
           sql.Append(" where");
           sql.Append(" PD.TypeID={0}");
           sql.Append(" and PD.ProductID in");
           sql.Append(" (select");
           sql.Append(" ProductID");
           sql.Append(" from");
           sql.Append(" CSK_Store_Product");
           sql.Append(" where");
           sql.Append(" ProductStatus=1 and ProductID in");
           sql.Append(" (select");
           sql.Append(" ProductID");
           sql.Append(" from");
           sql.Append(" CSK_Store_RetailerProduct");
           sql.Append(" where");
           sql.Append(" RetailerProductStatus=1 and IsDeleted=0");
           sql.Append(" and RetailerId in");
           sql.Append(" (select");
           sql.Append(" RetailerId");
           sql.Append(" from");
           sql.Append(" CSK_Store_Retailer");
           sql.Append(" where RetailerStatus=1");
           sql.Append(")");
           sql.Append(")");
           sql.Append(")");

           #region sqlCom

           var sqlCom = new StringBuilder();
           sqlCom.Append("select CAM.ProductID,CompareValue as Value");
           sqlCom.Append(" from dbo.Store_Compare_Attribute_Map CAM inner join Store_Compare_Attributes CA");
           sqlCom.Append(" on CAM.CompareAttributeID=CA.CompareAttributeID");
           sqlCom.Append(" where CAM.CompareAttributeID={0}");
           sqlCom.Append(" and CAM.ProductID in");
           sqlCom.Append(" (select ProductID");
           sqlCom.Append(" from CSK_Store_Product");
           sqlCom.Append(" where ProductStatus=1 and ProductID in");
           sqlCom.Append(" (select ProductID");
           sqlCom.Append(" from CSK_Store_RetailerProduct");
           sqlCom.Append(" where RetailerProductStatus=1 and IsDeleted=0 and");
           sqlCom.Append(" RetailerId in");
           sqlCom.Append(" (select RetailerId");
           sqlCom.Append(" from CSK_Store_Retailer");
           sqlCom.Append(" where RetailerStatus=1");
           sqlCom.Append(" )))");

           #endregion

           string sqlFormat = string.Format(isCom?sqlCom.ToString():sql.ToString(), aid);

           #endregion

           AttList = Prt.DataProcessTool.SqlHelper.sqlReader<Prt.RichClass.ProductValue>(sqlFormat.ToString()).OrderBy(o => o.Value).ToList();
       }

        
        /// <summary>
        /// 获取AttributeDisplayType表数据
        /// </summary>
        private  void getAttributeDisplayType()
        {
            #region sql
            var sql = new StringBuilder();
            sql.Append("select");

            sql.Append(" ID,");
            sql.Append(" AttributeID,");
            sql.Append(" IsCompareAttribute,");
            sql.Append(" TypeID,");
            sql.Append(" [IsComparison],");
            sql.Append(" [DisplayAdjectiveBetter],");
            sql.Append(" [DisplayAdjectiveWorse]");

            sql.Append(" FROM");
            sql.Append(" [AttributeDisplayType]");

            Adt = Prt.DataProcessTool.SqlHelper.sqlReader<Prt.RichClass.AttributeDisplayType>(sql.ToString());

            #endregion
        }

    }
}
