using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace FisherPaykelTool.Model
{
    public class Product
    {
        public DateTime RetailerProductModifiedOn { get; set; }
        public bool RetailerProductStatus { get; set; }
        public int RetailerProductId { get; set; }

        public int RetailerId { get; set; }
        public string RetailerName { get; set; }
        public int StoreType { get; set; }
        public int ProductId { get; set; }
        public int ManufacturerId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string RetailerProductName { get; set; }
        public decimal RetailerPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public string PurchaseURL { get; set; }

        public string RetailerType { get; set; }
        public string Brand { get; set; }
        public string ProductCategory { get; set; }

        public int NumberRetailers { get; set; }
        public decimal LowestPrice { get; set; }
        public string ModelNo { get; set; }


        public string Size_CapacityValue { get; set; }
        public string Size_CapacityName { get; set; }

        public string Type_FunctionsValue { get; set; }
        public string Type_FunctionsName { get; set; }

        public string FinishValue { get; set; }
        public string FinishName { get; set; }

        public string Energy_Water_RatingValue { get; set; }
        public string Energy_Water_RatingName { get; set; }

        public static List<Product> Get(int type = 0)
        {
            List<Product> list = new List<Product>();

            List<int> exceptRIds = GetConfigArr("ExceptRetaierIDs");
            List<int> mIds = GetConfigArr("ManufacturerIds");
            List<int> rIds = GetConfigArr("RID");
            List<int> cIds = new List<int>();
            var cateCollection = CateAttrCollection.Load();

            //GetConfigArr("RRP-rid").ForEach(id => {
            //    if (rIds.Contains(id)) return;

            //    rIds.Add(id);
            //});

            switch (type)
            {
                case 0: cIds = cateCollection.GetCateIds(); break;
                case 1: cIds = cateCollection.GetOtherCateIds(); break;
            }

            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["Priceme"].ConnectionString;

            StringBuilder sql = new StringBuilder();

            sql.AppendLine(" select TT.Modifiedon as RetailerProductModifiedOn,TT.RetailerProductStatus,TT.RetailerProductId,TT.RetailerId,R.RetailerName,R.StoreType,ProductId,TT.ManufacturerId,TT.CategoryId,ProductName,RetailerProductName,RetailerPrice,PurchaseURL");
            sql.AppendLine(" ,RetailerType = (select top 1 StoreTypeName from CSK_Store_RetailerStoreType where RetailerStoreTypeID = R.StoreType)");
            sql.AppendLine(" ,Brand = (select top 1 ManufacturerName from CSK_Store_Manufacturer where ManufacturerID=TT.ManufacturerId)");
            sql.AppendLine(" ,ProductCategory = (select top 1 CategoryName from CSK_Store_Category where CategoryID=TT.CategoryID)");

            sql.AppendLine(" from csk_store_retailer R");
            sql.AppendLine(" inner join");
            sql.AppendLine(" (");
            sql.AppendLine("	select RetailerProductId,RetailerId,P.ProductID,P.ManufacturerID,categoryid,ProductName,RetailerProductName,RetailerPrice,PurchaseURL,RetailerProductStatus,RP.Modifiedon from CSK_Store_RetailerProduct RP");
            sql.AppendLine("	inner join CSK_Store_Product P on RP.ProductId = P.ProductID");
            //+ "	where Rp.RetailerProductStatus=1";
            sql.AppendLine("	where 1=1");
            sql.AppendLine("	and RP.IsDeleted=0");
            sql.AppendLine("	and IsMerge=1");
            sql.AppendLine("	and retailerid<>1979");
            sql.AppendLine("	and CategoryID in @CIds");
            sql.AppendLine("	and ManufacturerID in @MIds");
            sql.AppendLine(" ) TT");
            sql.AppendLine(" on R.RetailerId=TT.retailerid where RetailerStatus=1 and RetailerCountry=3");
            if (rIds.Count != 0)
            {
                sql.AppendLine(" and R.RetailerId in @RIds");
            }
            else
            {
                sql.AppendLine(" and R.RetailerId not in @exceptRIds");
            }
            //+ " on R.RetailerId=TT.retailerid where RetailerStatus=1 and RetailerCountry=3";
            sql.AppendLine(" order by CategoryId,ProductName");

            string ab = sql.ToString();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                List<Product> tempList = new List<Product>();
                DateTime lastYear = DateTime.Now.AddYears(-1);

                list = con.Query<Product>(sql.ToString(), new { CIds = cIds, MIds = mIds, RIds = rIds, exceptRIds = exceptRIds }, null, true, 3000).ToList();
                list.ForEach(item =>
                {
                    if (item.RetailerProductStatus == false && item.RetailerProductModifiedOn < lastYear) return;

                    tempList.Add(item);
                });

                list = tempList;
            }


            sql.Clear();

            sql.AppendLine(" select TT.Modifiedon as RetailerProductModifiedOn,TT.RetailerProductStatus,TT.RetailerProductId,TT.RetailerId,R.RetailerName,R.StoreType,ProductId,TT.ManufacturerId,TT.CategoryId,ProductName,RetailerProductName,RetailerPrice,PurchaseURL");
            sql.AppendLine(" ,RetailerType = (select top 1 StoreTypeName from CSK_Store_RetailerStoreType where RetailerStoreTypeID = R.StoreType)");
            sql.AppendLine(" ,Brand = (select top 1 ManufacturerName from CSK_Store_Manufacturer where ManufacturerID=TT.ManufacturerId)");
            sql.AppendLine(" ,ProductCategory = (select top 1 CategoryName from CSK_Store_Category where CategoryID=TT.CategoryID)");

            sql.AppendLine(" from csk_store_retailer R");
            sql.AppendLine(" inner join");
            sql.AppendLine(" (");
            sql.AppendLine("	select RetailerProductId,RetailerId,P.ProductID,P.ManufacturerID,categoryid,ProductName,RetailerProductName,RetailerPrice,PurchaseURL,RetailerProductStatus,RP.Modifiedon from PriceMe_D.dbo.Priceme_CSK_Store_RetailerProduct RP");
            sql.AppendLine("	inner join CSK_Store_Product P on RP.ProductId = P.ProductID");
            //+ "	where Rp.RetailerProductStatus=1";
            sql.AppendLine("	where 1=1");
            sql.AppendLine("	and RP.IsDeleted=0");
            sql.AppendLine("	and IsMerge=1");
            sql.AppendLine("	and retailerid<>1979");
            sql.AppendLine("	and CategoryID in @CIds");
            sql.AppendLine("	and ManufacturerID in @MIds");
            sql.AppendLine(" ) TT");
            sql.AppendLine(" on R.RetailerId=TT.retailerid where RetailerStatus=1 and RetailerCountry=3");
            if (rIds.Count != 0)
            {
                sql.AppendLine(" and R.RetailerId in @RIds");
            }
            else
            {
                sql.AppendLine(" and R.RetailerId not in @exceptRIds");
            }
            //+ " on R.RetailerId=TT.retailerid where RetailerStatus=1 and RetailerCountry=3";
            sql.AppendLine(" order by CategoryId,ProductName");

            //string ab = sql.ToString();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                List<Product> tempList = new List<Product>();
                List<Product> tempList1 = new List<Product>();
                DateTime lastYear = DateTime.Now.AddYears(-1);

                tempList = con.Query<Product>(sql.ToString(), new { CIds = cIds, MIds = mIds, RIds = rIds, exceptRIds = exceptRIds }, null, true, 3000).ToList();
                tempList.ForEach(item =>
                {
                    if (item.RetailerProductStatus == false && item.RetailerProductModifiedOn < lastYear) return;

                    tempList1.Add(item);
                });

                list.AddRange(tempList1);
            }






            //group
            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();
            HashSet<string> PidRIds = new HashSet<string>();

            list.ForEach(item =>
            {
                if (!PidRIds.Contains(item.ProductId + "_" + item.RetailerId))
                {
                    //number
                    string numKey = "num_" + item.ProductId;
                    if (dic.ContainsKey(numKey)) dic[numKey] = dic[numKey] + 1;
                    else dic.Add(numKey, 1);

                    PidRIds.Add(item.ProductId + "_" + item.RetailerId);
                }

                //price
                string priceKey = "price_" + item.ProductId;
                if (dic.ContainsKey(priceKey))
                    dic[priceKey] = dic[priceKey] > item.RetailerPrice ? item.RetailerPrice : dic[priceKey];    //get min price
                else
                    dic.Add(priceKey, item.RetailerPrice);
            });

            list.ForEach(item =>
            {
                if (!string.IsNullOrEmpty(item.Brand)) item.ModelNo = item.ProductName.Replace(item.Brand, "").Trim();

                string numKey = "num_" + item.ProductId;
                string priceKey = "price_" + item.ProductId;

                item.NumberRetailers = Convert.ToInt32(dic[numKey]);
                item.LowestPrice = dic[priceKey];

                var cateAttr = cateCollection.GetCateAttr(item.CategoryId);
                if (cateAttr == null) return;

                var scAttrList = cateAttr.Size_Capacity.Where(a => a.ProductId == item.ProductId);
                item.Size_CapacityValue = string.Join(" | ", scAttrList.Select(a => a.ToString()));
                item.Size_CapacityName = string.Join(" | ", scAttrList.Select(a => a.Name));

                var tfAttrList = cateAttr.Type_Functions.Where(a => a.ProductId == item.ProductId);
                item.Type_FunctionsValue = string.Join(" | ", tfAttrList.Select(a => a.ToString()));
                item.Type_FunctionsName = string.Join(" | ", tfAttrList.Select(a => a.Name));

                var fiAttrList = cateAttr.Finish.Where(a => a.ProductId == item.ProductId);
                item.FinishValue = string.Join(" | ", fiAttrList.Select(a => a.ToString()));
                item.FinishName = string.Join(" | ", fiAttrList.Select(a => a.Name));

                var ewrAttrList = cateAttr.Energy_Water_Rating.Where(a => a.ProductId == item.ProductId);
                item.Energy_Water_RatingValue = string.Join(" | ", ewrAttrList.Select(a => a.ToString()));
                item.Energy_Water_RatingName = string.Join(" | ", ewrAttrList.Select(a => a.Name));

            });

            //original price
            var rrpList = RRPProduct.Get();
            list.ForEach(item =>
            {
                if (item.OriginalPrice != 0) return;

                var rrp = rrpList.FirstOrDefault(rrpItem => rrpItem.ProductId == item.ProductId && rrpItem.OriginalPrice != 0);
                if (rrp == null) return;

                item.OriginalPrice = rrp.OriginalPrice;
            });

            return list;
        }

        private static List<int> GetConfigArr(string key)
        {
            List<int> list = new List<int>();

            string str = System.Configuration.ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(str)) return list;

            list = str.Split(',').Select(item => Convert.ToInt32(item.Trim())).ToList();

            return list;
        }

        //private class
        public class RRPProduct
        {
            public int OrderIndex { get; set; }
            public int RetailerId { get; set; }
            public int ProductId { get; set; }
            public int RetailerProductId { get; set; }
            public decimal OriginalPrice { get; set; }

            public static List<RRPProduct> Get()
            {
                List<RRPProduct> list = new List<RRPProduct>();
                List<int> rIds = GetConfigArr("RRP-rid");

                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["Priceme"].ConnectionString;
                string sql = "select RetailerId,RetailerProductId,ProductId,OriginalPrice from CSK_Store_RetailerProduct where RetailerProductStatus=1 and OriginalPrice<>0 and RetailerId in @RIds";

                using (SqlConnection con = new SqlConnection(conStr))
                {                                     
                    list = con.Query<RRPProduct>(sql.ToString(), new { RIds = rIds}, null, true, 3000).ToList();
                }

                list.ForEach(item => { item.OrderIndex = rIds.IndexOf(item.RetailerId); });
                list = list.OrderBy(item => item.OrderIndex).ToList();

                return list;
            }
        }

    }
}
