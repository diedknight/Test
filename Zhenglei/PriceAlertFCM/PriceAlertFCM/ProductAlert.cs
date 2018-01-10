using Parse;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PriceAlertFCM
{
    public class ProductAlert
    {
        static readonly string isDebug = ConfigurationManager.AppSettings["IsDebug"].ToString();
        static readonly string debugToken = ConfigurationManager.AppSettings["DebugToken"].ToString();
        static readonly string debugDeviceType = ConfigurationManager.AppSettings["DebugDeviceType"].ToString();
        static readonly string PriceSymbol = ConfigurationManager.AppSettings["PriceSymbol"].ToString();
        static readonly Regex PriceRegex = new Regex("(?<p>[\\d|,]+)");
        static int CountryId = int.Parse(ConfigurationManager.AppSettings["CountryId"].ToString());

        StreamWriter _sw;
        public StreamWriter SW
        {
            get { return _sw; }
            set { _sw = value; }
        }

        public void Alert()
        {
            string culture = ConfigurationManager.AppSettings["Culture"].ToString();
            var currentCulture = new System.Globalization.CultureInfo(culture);

            int countryId = int.Parse(ConfigurationManager.AppSettings["CountryId"].ToString());

            int _count = 0;
            decimal priceLimit = decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["PriceLimit"].ToString().Replace("%", "")) / 100;

            Write("Begin process......   " + DateTime.Now);

            List<ProductAlertData> pas = GetProductAlert();

            Write("Get " + pas.Count + " product alert.");

            foreach (ProductAlertData pa in pas)
            {
                try
                {
                    if (pa.ProductPrice == 0m) continue;

                    decimal price = GetProductAlertPrice(pa, countryId);

                    if (price > 0 && pa.ProductPrice > (price + 0.5m))
                    {
                        decimal avgPrice = GetAvgPrice(pa.ProductId);
                        decimal maxPrice = avgPrice * (1 + priceLimit);
                        decimal bestPrice = avgPrice * priceLimit;
                        if (price > maxPrice || price < bestPrice)
                        {
                            Write("alertid:" + pa.AlertId + " not send for big change price, average price: " + avgPrice + ", sent price:" + price);
                            continue;
                        }

                        string token = debugToken;
                        string deviceSystem = debugDeviceType;
                        if (!isDebug.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                        {
                            int deviceSystemType;
                            GetUserPushInfo(pa.UserId, out deviceSystemType, out token);
                            deviceSystem = deviceSystemType.ToString();
                        }

                        if (string.IsNullOrEmpty(token))
                        {
                            Write("alertid:" + pa.AlertId + " user token is null.");
                        }
                        else
                        {
                            PriceAlertInfo pai = GetPriceAlertInfo(pa, price, countryId, currentCulture);
                            string rs = "";
                            if (deviceSystem == "0")
                            {
                                rs = GlobalOperator.SendPushNotification(pai, token);
                                Write("Android PushNotification :" + rs + " token : " + token);
                            }
                            else if(deviceSystem == "1")
                            {
                                IOSPush.AppleApns.ProductInfo productInfo = new IOSPush.AppleApns.ProductInfo();
                                productInfo.AlertMsg = pai.AlertMsg;
                                productInfo.CountryId = CountryId;
                                productInfo.ProductId = pai.ProductId;
                                productInfo.ProductImage = pai.ProductImage;
                                productInfo.ProductName = pai.ProductName;
                                productInfo.ProductBestPrice = pai.ProductBestPrice;
                                IOSPush.AppleApns.Push(productInfo, token);
                                Write("Ios Push Notification : Ok. token : " + token);
                            }
                            else
                            {
                                Write("Unknow system.");
                            }

                            _count++;

                            if (!isDebug.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                            {
                                string sql = string.Empty;
                                if (pa.PriceType == 0 || pa.AlertType == 0)
                                {
                                    sql = "Update CSK_Store_ProductAlert Set APPStatus = 1 Where AlertId = " + pa.AlertId;
                                    SavePriceAlert(sql);
                                }
                                else if (pa.PriceType == 1 || pa.PriceType == 2)
                                {
                                    decimal newProductPrice = price - pa.PriceEach;
                                    if (pa.PriceType == 1)
                                        newProductPrice = price;

                                    sql = "Update CSK_Store_ProductAlert Set ProductPrice = " + newProductPrice + " Where AlertId = " + pa.AlertId;
                                    Write("Update CSK_Store_ProductAlert sql:" + sql);
                                    SavePriceAlert(sql);
                                }

                                SavePriceAlertHistory(pa.AlertId, price, DateTime.Now);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Write("AlertId: " + pa.AlertId + " " + ex.Message + ex.StackTrace);
                }
            }
            Write(_count + " retailer product alert Send success");
            Write("End process......   " + DateTime.Now);
        }

        private PriceAlertInfo GetPriceAlertInfo(ProductAlertData pa, decimal price, int countryId, System.Globalization.CultureInfo currentCulture)
        {
            string pName = string.Empty;
            string pImage = string.Empty;
            string sql = "Select ProductName, DefaultImage From CSK_Store_ProductNew Where ProductID = " + pa.ProductId;

            using (SqlConnection sqlConn = SqlOP.CreateSqlConnection())
            {
                using (SqlCommand sqlCmd = SqlOP.CreateTextSqlCommand(sql))
                {
                    sqlCmd.Connection = sqlConn;
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCmd.ExecuteReader())
                    {
                        if (sqlDR.Read())
                        {
                            pName = sqlDR.GetString(0);
                            pImage = sqlDR.GetString(1);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }

            string imageSRC = !pImage.Contains(".") ? "" : (GlobalOperator.FixImagePath(pImage.Insert(pImage.LastIndexOf('.'), "_ms")));
            if (!imageSRC.StartsWith("http:") && !imageSRC.StartsWith("https:"))
            {
                imageSRC = "https://images.pricemestatic.com" + imageSRC;
            }
            else
            {
                imageSRC = imageSRC.Replace("http://", "https://");
            }

            string priceInfo = price.ToString("C0", currentCulture);
            var match1 = PriceRegex.Match(priceInfo);
            if (match1.Success)
            {
                priceInfo = PriceSymbol + match1.Groups["p"].Value;
            }

            PriceAlertInfo pai = new PriceAlertInfo();
            pai.ProductId = pa.ProductId;
            pai.ProductName = pName;
            pai.ProductBestPrice = price;
            pai.AlertMsg = pName + " dropped to " + priceInfo;
            pai.ProductImage = imageSRC;
            pai.CountryId = countryId;

            return pai;
        }

        public static string NewPriceCultureString_Int(double price, IFormatProvider culture, string replayStr)
        {
            string priceInfo = price.ToString("C0", culture).Replace(replayStr, "").Replace("$", "").Replace("₱", "");

            return priceInfo;
        }

        private string GetUserFirebaseToken(string userId)
        {
            var objectList = ParseObject.GetQuery("MemberShip").WhereEqualTo("ParseID", userId).FirstOrDefaultAsync();
            objectList.Wait();

            if (objectList != null && objectList.Result != null)
            {
                if (objectList.Result.ContainsKey("FirebaseToken") && objectList.Result["FirebaseToken"] != null)
                {
                    return objectList.Result["FirebaseToken"].ToString();
                }
            }

            return "";
        }

        private void GetUserPushInfo(string userId, out int deviceSystemType, out string pushChannelId)
        {
            deviceSystemType = -1;
            pushChannelId = "";

            var objectList = ParseObject.GetQuery("MemberShip").WhereEqualTo("ParseID", userId).FirstOrDefaultAsync();
            objectList.Wait();

            if (objectList != null && objectList.Result != null)
            {
                if (objectList.Result.ContainsKey("userloginsystem") && objectList.Result["userloginsystem"] != null)
                {
                    string deviceSystem = objectList.Result["userloginsystem"].ToString();
                    if(deviceSystem.Equals("Android", StringComparison.InvariantCultureIgnoreCase))
                    {
                        deviceSystemType = 0;
                        pushChannelId = objectList.Result["FirebaseToken"].ToString();
                    }
                    else if (deviceSystem.Equals("IOS", StringComparison.InvariantCultureIgnoreCase))
                    {
                        deviceSystemType = 1;
                        pushChannelId = objectList.Result["IOSToken"].ToString();
                    }
                }
            }
        }

        private void SavePriceAlertHistory(int alertId, decimal price, DateTime sendTime)
        {
            string sql = "INSERT [dbo].[CSK_Store_ProductAlertHistory] VALUES ( " + alertId + "," + price + ",GetDate())";

            using (SqlConnection sqlConn = SqlOP.CreateSqlConnection())
            {
                using (SqlCommand sqlCmd = SqlOP.CreateTextSqlCommand(sql))
                {
                    sqlCmd.Connection = sqlConn;
                    sqlConn.Open();
                    sqlCmd.ExecuteNonQuery();
                }
            }
            Write("insert tableName sql:" + sql);
        }

        private decimal GetProductAlertPrice(ProductAlertData pa, int countryId)
        {
            decimal price = 0m;
            decimal BestPrice = 0m;

            BestPrice = GetBestPrice(pa.ProductId, countryId);

            //当天是否有新的最低价格
            if (BestPrice < pa.ProductPrice)
            {
                if (pa.PriceType == 0 || pa.AlertType == 0)    //以前的alert
                    price = GetOldPriceAlert(pa, countryId);
                else
                {
                    //判断是否要除去一些RetailerId
                    bool isER = false;
                    if (!string.IsNullOrEmpty(pa.ExcludedRetailers))
                        isER = true;

                    //判断PriceType 
                    if (pa.PriceType == 1)  //只要价格有变化就发邮件
                    {
                        //判断是否需要考虑库存
                        if (pa.AlertType == 1)  //不用考虑库存
                        {
                            if (!isER)  //不用考虑Retailer
                            {
                                //price = phs[0].Price;
                                //一定要读数据库
                                decimal rpPrice = IsCheckRetailerProduct(pa.ProductId, pa.ProductPrice, isER, pa.ExcludedRetailers, false, countryId);
                                if (rpPrice < pa.ProductPrice)
                                    price = rpPrice;
                            }
                            else
                            {
                                decimal rpPrice = IsCheckRetailerProduct(pa.ProductId, pa.ProductPrice, isER, pa.ExcludedRetailers, false, countryId);
                                if (rpPrice < pa.ProductPrice)
                                    price = rpPrice;
                            }
                        }
                        else
                        {
                            decimal rpPrice = IsCheckRetailerProduct(pa.ProductId, pa.ProductPrice, isER, pa.ExcludedRetailers, true, countryId);
                            if (rpPrice < pa.ProductPrice)
                                price = rpPrice;
                        }
                    }
                    else
                    {
                        bool isStock = false;
                        if (pa.AlertType != 1)
                            isStock = true;

                        decimal rpPrice = IsCheckRetailerProduct(pa.ProductId, pa.ProductPrice, isER, pa.ExcludedRetailers, isStock, countryId);
                        if (rpPrice < pa.ProductPrice)
                            price = rpPrice;
                    }
                }
            }

            return price;
        }

        private decimal GetBestPrice(int productId, int countryId)
        {
            string sql = "select min([RetailerPrice]) as minPrice from [CSK_Store_RetailerProductNew] where [ProductId] = " + productId + " and RetailerID in (SELECT [RetailerId] FROM [dbo].[CSK_Store_PPCMember] where [RetailerCountry] = " + countryId + ") group by [ProductId]";
            decimal bp = 0;

            using (SqlConnection sqlConn = SqlOP.CreateSqlConnection())
            {
                using (SqlCommand sqlCmd = SqlOP.CreateTextSqlCommand(sql))
                {
                    sqlCmd.Connection = sqlConn;
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCmd.ExecuteReader())
                    {
                        if (sqlDR.Read())
                        {
                            string _price = sqlDR["minPrice"].ToString();
                            decimal.TryParse(_price, out bp);
                        }
                    }
                }
            }

            return bp;
        }

        private decimal GetOldPriceAlert(ProductAlertData pa, int countryId)
        {
            decimal price = 0m;

            decimal rpPrice = IsCheckRetailerProduct(pa.ProductId, pa.ProductPrice, false, pa.ExcludedRetailers, false, countryId);
            if (rpPrice < pa.ProductPrice)
                price = rpPrice;

            return price;
        }

        private string GetProductName(int productId)
        {
            string pname = string.Empty;
            string sql = "Select ProductName From CSK_Store_Product "
                        + "Where ProductID = " + productId;

            using (SqlConnection sqlConn = SqlOP.CreateSqlConnection())
            {
                using (SqlCommand sqlCmd = SqlOP.CreateTextSqlCommand(sql))
                {
                    sqlCmd.Connection = sqlConn;
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCmd.ExecuteReader())
                    {
                        if (sqlDR.Read())
                        {
                            return sqlDR.GetString(0);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        private decimal IsCheckRetailerProduct(int productId, decimal bestPrice, bool isER, string er, bool isStock, int countryId)
        {
            decimal price = 0m;

            string where = string.Empty;
            if (isER)
                where = " And RetailerId not in (" + er.Substring(0, er.Length - 1) + ")";
            if (isStock)
                where += " And StockStatus = -2";

            string sql = "Select Top 1 RetailerPrice From CSK_Store_RetailerProductNew "
                        + "Where ProductID = " + productId + " And RetailerProductStatus = 1 And IsDeleted = 0 and RetailerID in (SELECT [RetailerId] FROM [dbo].[CSK_Store_PPCMember] where [RetailerCountry] = " + countryId + ")"
                        + " And RetailerPrice < " + bestPrice + where + " Order By RetailerPrice";

            using (SqlConnection sqlConn = SqlOP.CreateSqlConnection())
            {
                using (SqlCommand sqlCmd = SqlOP.CreateTextSqlCommand(sql))
                {
                    sqlCmd.Connection = sqlConn;
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCmd.ExecuteReader())
                    {
                        if (sqlDR.Read())
                        {
                            string _price = sqlDR["RetailerPrice"].ToString();
                            decimal.TryParse(_price, out price);
                        }
                    }
                }
            }

            if (price > 0)
                Write("ProductId: " + productId + " price: " + price + "   sql: " + sql);

            return price;
        }

        private void SavePriceAlert(string sql)
        {
            using (SqlConnection sqlConn = SqlOP.CreateSqlConnection())
            {
                using (SqlCommand sqlCmd = SqlOP.CreateTextSqlCommand(sql))
                {
                    sqlCmd.Connection = sqlConn;
                    sqlConn.Open();
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

        private List<ProductAlertData> GetProductAlert()
        {
            List<ProductAlertData> paList = new List<ProductAlertData>();
            string sql = "Select AlertId, ProductId, Email, ProductPrice, APPStatus, APPIsActive, ParseId, "
                        + "AlertType, ExcludedRetailers, PriceType, PriceEach, AlertGUID From CSK_Store_ProductAlert "
                        + "Where APPStatus = 0 And APPIsactive = 1 and AlertType > 0 and PriceType > 0";

            using (SqlConnection sqlConn = SqlOP.CreateSqlConnection())
            {
                using (SqlCommand sqlCmd = SqlOP.CreateTextSqlCommand(sql))
                {
                    sqlCmd.Connection = sqlConn;
                    sqlConn.Open();
                    using (SqlDataReader dr = sqlCmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int alertId, productId, status, alertType, priceType;
                            decimal productPrice, priceEach;
                            bool isActive;

                            string _alertId = dr["AlertId"].ToString();
                            int.TryParse(_alertId, out alertId);
                            string _productId = dr["ProductId"].ToString();
                            int.TryParse(_productId, out productId);
                            string _email = dr["Email"].ToString();
                            string _productPrice = dr["ProductPrice"].ToString();
                            decimal.TryParse(_productPrice, out productPrice);
                            string _status = dr["APPStatus"].ToString();
                            int.TryParse(_status, out status);
                            string _isActive = dr["APPIsActive"].ToString();
                            bool.TryParse(_isActive, out isActive);
                            string _alertType = dr["AlertType"].ToString();
                            int.TryParse(_alertType, out alertType);
                            string _excludedRetailers = dr["ExcludedRetailers"].ToString();
                            string _priceType = dr["PriceType"].ToString();
                            int.TryParse(_priceType, out priceType);
                            string _priceEach = dr["PriceEach"].ToString();
                            decimal.TryParse(_priceEach, out priceEach);
                            string _alertGUID = dr["AlertGUID"].ToString();
                            string _userId = dr["ParseId"].ToString();

                            ProductAlertData pa = new ProductAlertData();
                            pa.AlertId = alertId;
                            pa.ProductId = productId;
                            pa.Email = _email;
                            pa.ProductPrice = productPrice;
                            pa.Status = status;
                            pa.IsActive = isActive;
                            pa.AlertType = alertType;
                            pa.ExcludedRetailers = _excludedRetailers;
                            pa.PriceType = priceType;
                            pa.PriceEach = priceEach;
                            pa.AlertGUID = _alertGUID;
                            pa.UserId = _userId;

                            paList.Add(pa);
                        }
                    }
                }
            }

            return paList;
        }

        private decimal GetAvgPrice(int pid)
        {
            decimal avgPrice = 0;
            string sql = "select sum(RetailerPrice) [sum], COUNT(RetailerProductId) [count] from csk_store_retailerproductnew "
                        + "where productid = " + pid + " and RetailerProductStatus = 1";

            using (SqlConnection sqlConn = SqlOP.CreateSqlConnection())
            {
                using (SqlCommand sqlCmd = SqlOP.CreateTextSqlCommand(sql))
                {
                    sqlCmd.Connection = sqlConn;
                    sqlConn.Open();
                    using (SqlDataReader sqlDR = sqlCmd.ExecuteReader())
                    {
                        if (sqlDR.Read())
                        {
                            decimal sum = 0;
                            decimal.TryParse(sqlDR["sum"].ToString(), out sum);
                            int count = 0;
                            int.TryParse(sqlDR["count"].ToString(), out count);

                            if (count != 0)
                                avgPrice = decimal.Round((sum / count), 2);
                        }
                    }
                }
            }

            return avgPrice;
        }

        private void Write(string info)
        {
            _sw.WriteLine(info);
            _sw.Flush();
        }
    }
}
