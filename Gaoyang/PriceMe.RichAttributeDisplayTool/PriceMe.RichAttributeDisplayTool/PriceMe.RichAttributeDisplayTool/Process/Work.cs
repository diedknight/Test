using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prt = PriceMe.RichAttributeDisplayTool;

namespace PriceMe.RichAttributeDisplayTool.Process
{
    class Work
    {
        public Work() { 
            
        }

        static List<Prt.RichClass.AttributeDisplayType> Adt=new List<RichClass.AttributeDisplayType>();

        static List<Prt.RichClass.ProductValue> AttList = new List<RichClass.ProductValue>();

        public static void StartWork()
        {

            getAttributeDisplayType();

            var accList = new List<Prt.RichClass.AttributeCategoryComparisons>();
            var single = new Prt.DataProcessTool.LinqBllBase<Prt.DataProcessTool.AttributeCategoryComparison>();

            var singleAll = single.Query(q => q.ID > 0);

            var updateRecord = new Dictionary<int, int>();
            var updateSucc = new Dictionary<int, int>();
            foreach (var item in Adt)
            {

                getProductTotalByAid(item.AttributeID);

                //Top10
                var top10 = getRank(0.1);

                //Top10
                var top20 = getRank(0.2);

                //Top30
                var top30 = getRank(0.3);

                //Average
                var average = getRank(0.5);

                //bottom10
                var bottom10 = getRank(0.9);

                //bottom20
                var bottom20 = getRank(0.8);

                //bottom30
                var bottom30 = getRank(0.7);

                var getSingle = singleAll.SingleOrDefault(s => s.Aid == item.AttributeID);

                if (getSingle != null)
                {
                    try
                    {
                        getSingle.Aid = item.AttributeID;
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
                    catch (Exception ex) {
                        if (!updateRecord.ContainsKey(getSingle.Aid))
                            updateRecord.Add(getSingle.Aid, getSingle.Aid);
                        continue;
                    }
                    
                }
                else
                {
                    var acc = new Prt.RichClass.AttributeCategoryComparisons();

                    acc.Aid = item.AttributeID;
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

            //批量添加新数据
            string result = Prt.DataProcessTool.SqlHelper.InsertToTables(accList);

            if (string.IsNullOrEmpty(result) || updateRecord.Count <= 0)
                LogSucc(accList, updateRecord, updateSucc);
            else 
                LogFailed(accList, updateRecord);
            
        }

        private static void LogFailed(List<RichClass.AttributeCategoryComparisons> accList, Dictionary<int, int> updateRecord)
        {
            var info = new StringBuilder();

            info.AppendLine("Add Failed 0 record.");
            accList.ForEach(f =>
            {
                info.AppendLine("     Aid:" + f.Aid);
            });
            info.AppendLine("Update Failed 0 record.");

            updateRecord.ToList().ForEach(f =>
            {
                info.AppendLine("     Aid:" + f.Value);
            });

            info.AppendLine("Total Succ 0 record.");
            info.AppendLine("Exe Datetime " + DateTime.Now);

            if (Task.isSendEmal)
                Utility.sendEmail("PriceMe.RichAttributeDisplayTool", info.ToString());

            Utility.writeFailLog(info.ToString());
        }

        private static void LogSucc(List<RichClass.AttributeCategoryComparisons> accList, Dictionary<int, int> updateRecord, Dictionary<int, int> updateSucc)
        {
            var info = new StringBuilder();

            info.AppendLine("Add Succ " + accList.Count() + " record.");
            accList.ForEach(f =>
            {
                info.AppendLine("     Aid:" + f.Aid);
            });
            info.AppendLine("Update Succ " + updateSucc.Count + " record.");

            updateSucc.ToList().ForEach(f =>
            {
                info.AppendLine("     Aid:" + f.Value);
            });
            info.AppendLine("Total Succ " + (accList.Count() + updateRecord.Count) + " record.");
            info.AppendLine("Exe Datetime " + DateTime.Now);

            if (Task.isSendEmal)
                Utility.sendEmail("PriceMe.RichAttributeDisplayTool", info.ToString());


            Utility.writeSuccLog(info.ToString());
        }

        /// <summary>
        /// 获取排名
        /// </summary>
        /// <param name="proCount"></param>
        /// <returns></returns>
        private static string getRank(double per)
        {
            int proCount = AttList.Count();

            var disCount = proCount * per;

            var roundCount = disCount.Round();

            var result = proCount - roundCount;

            var top10 = AttList[result - 1].Value;

            return top10.ToString();
        }

        /// <summary>
        /// 获取AttributeDisplayType表数据
        /// </summary>
        private static void getAttributeDisplayType()
        {
            #region sql
            var sql = new StringBuilder();
            sql.Append("select");

            sql.Append(" ID,");
            sql.Append(" AttributeID,");
            sql.Append(" TypeID,");
            sql.Append(" [IsComparison],");
            sql.Append(" [DisplayAdjectiveBetter],");
            sql.Append(" [DisplayAdjectiveWorse]");

            sql.Append(" FROM");
            sql.Append(" [AttributeDisplayType]");

            Adt= Prt.DataProcessTool.SqlHelper.sqlReader<Prt.RichClass.AttributeDisplayType>(sql.ToString());

            #endregion
        }
        /// <summary>
        /// 获取分类下有Attribute的产品总数
        /// </summary>
        /// <param name="aid"></param>
        private static void getProductTotalByAid(int aid)
        {

            #region sql

            var sql = new StringBuilder();
            sql.Append("select");

            sql.Append(" ProductID,");
            sql.Append(" PD.AttributeValueID,");
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
            //sql.Append(" order by PD.AttributeValueID");

            string sqlFormat = string.Format(sql.ToString(), aid);

            #endregion

            AttList = Prt.DataProcessTool.SqlHelper.sqlReader<Prt.RichClass.ProductValue>(sqlFormat.ToString()).OrderBy(o=>o.Value).ToList();

        }


    }
}
