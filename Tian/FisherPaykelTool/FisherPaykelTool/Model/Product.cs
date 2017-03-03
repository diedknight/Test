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

            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["Priceme"].ConnectionString;

            string sql = " select TT.RetailerProductId,TT.RetailerId,R.RetailerName,R.StoreType,ProductId,TT.ManufacturerId,TT.CategoryId,ProductName,RetailerProductName,RetailerPrice,PurchaseURL"
                        + " ,RetailerType = (select top 1 StoreTypeName from CSK_Store_RetailerStoreType where RetailerStoreTypeID = R.StoreType)"
                        + " ,Brand = (select top 1 ManufacturerName from CSK_Store_Manufacturer where ManufacturerID=TT.ManufacturerId)"
                        + " ,ProductCategory = (select top 1 CategoryName from CSK_Store_Category where CategoryID=TT.CategoryID)"

                        + " from csk_store_retailer R"
                        + " inner join"
                        + " ("
                        + "	select RetailerProductId,RetailerId,P.ProductID,P.ManufacturerID,categoryid,ProductName,RetailerProductName,RetailerPrice,PurchaseURL from CSK_Store_RetailerProduct RP"
                        + "	inner join CSK_Store_Product P on RP.ProductId = P.ProductID"
                        + "	where Rp.RetailerProductStatus=1"
                        + "	and RP.IsDeleted=0"
                        + "	and IsMerge=1"
                        + "	and retailerid<>1979"
                        + "	and CategoryID in @CIds"
                        + "	and ManufacturerID in @MIds"
                        + " ) TT"
                        + " on R.RetailerId=TT.retailerid where RetailerStatus=1 and RetailerCountry=3 and R.RetailerId in @RIds"
                        //+ " on R.RetailerId=TT.retailerid where RetailerStatus=1 and RetailerCountry=3"
                        + " order by CategoryId,ProductName";

            var cateCollection = CateAttrCollection.Load();

            List<int> mIds = System.Configuration.ConfigurationManager.AppSettings["ManufacturerIds"].Split(',').Select(item => Convert.ToInt32(item.Trim())).ToList();
            List<int> rIds = System.Configuration.ConfigurationManager.AppSettings["RID"].Split(',').Select(item => Convert.ToInt32(item.Trim())).ToList();
            List<int> cIds = new List<int>();
            
            switch (type)
            {
                case 0: cIds = cateCollection.GetCateIds(); break;
                case 1: cIds = cateCollection.GetOtherCateIds(); break;
            }

            using (SqlConnection con = new SqlConnection(conStr))
            {
                list = con.Query<Product>(sql, new { CIds = cIds, MIds = mIds, RIds = rIds }, null, true, 3000).ToList();
            }

            //group
            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();

            list.ForEach(item =>
            {
                //number
                string numKey = "num_" + item.ProductId;
                if (dic.ContainsKey(numKey)) dic[numKey] = dic[numKey] + 1;
                else dic.Add(numKey, 1);

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

            return list;
        }
    }
}
