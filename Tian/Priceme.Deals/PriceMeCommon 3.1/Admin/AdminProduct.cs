using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PriceMeDBA;
using PriceMeCommon.BusinessLogic;
using System.IO;

namespace PriceMeCommon.Admin
{
    public static class AdminProduct
    {
        static string driverPath = System.Configuration.ConfigurationManager.AppSettings["Driver"];
        static string newDriverPath = System.Configuration.ConfigurationManager.AppSettings["newDriverPath"];
        static string imgFilePath = System.Configuration.ConfigurationManager.AppSettings["ImgFilePath"];

        public static bool ChangeCategory(int productID, int categoryID, string userName)
        {
            SubSonic.Schema.StoredProcedure sp = PriceMeStatic.PriceMeDB.ChangeProductCategory();

            sp.Command.AddParameter("@productID", productID, DbType.Int32);
            sp.Command.AddParameter("@categoryID", categoryID, DbType.Int32);
            sp.Command.AddParameter("@userName", userName, DbType.String);

            sp.Execute();

            return true;
        }

        public static bool ChangeManufacturer(int productID, int manufacturerID, string userName)
        {
            var product = ProductController.GetProduct(productID);
            product.ManufacturerID = manufacturerID;
            product.ModifiedBy = userName;
            product.Save();

            return true;
        }

        //public static void SaveNotMerge(List<int> productIDs, int tpid, int adminId)
        //{
        //    foreach (int pid in productIDs)
        //    {
        //        if (mainID != pid)
        //        {
        //            Csk_store_auto product = ProductController.GetProduct(pid);

        //            if (product != null)
        //            {
        //                CSK_Store_ProductIsMerged_Temp csp = new CSK_Store_ProductIsMerged_Temp();
        //                csp.ProductID = pid;
        //                csp.ToProductID = mainID;
        //                csp.CreatedOn = DateTime.Now;
        //                csp.CreatedBy = userName;

        //                csp.Save();
        //            }

        //            if (!ConfigAppString.LuceneIndexReadOnly)
        //            {
        //                try
        //                {
        //                    deleted += LuceneController.DeleteProductFromIndex("ProductID", pid.ToString());
        //                }
        //                catch (Exception ex)
        //                {
        //                    LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
        //                }
        //            }
        //        }
        //    }

        //    return mainID;
        //}

        public static int MergeProduct(int mainID, List<int> productIDs, string userName)
        {
            int deleted = 0;

            //double mPrice = SearchController.GetBestPriceByProductID(mainID);
            //foreach (string pid in mergeProductID)
            //{
            //    if (!mainID.ToString().Equals(pid, StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        double pPrice = SearchController.GetBestPriceByProductID(int.Parse(pid));

            //        if (pPrice <= mPrice / 2d || pPrice >= mPrice * 2d)
            //        {
            //            string pps = GetPPString(mainID.ToString(), mergeProductID);
            //            Response.Redirect("Merge.aspx?mpid=" + mainID + "&ppids=" + pps + "&q=" + Utility.GetParameter("q"));
            //            return;
            //        }
            //    }
            //}

            foreach (int pid in productIDs)
            {
                if (mainID != pid)
                {
                    CSK_Store_Product product = ProductController.GetProduct(pid);

                    if (product != null)
                    {
                        MergeProductsSite.IsMergedTempData tempData = new MergeProductsSite.IsMergedTempData();
                        tempData.ProductID = pid;
                        tempData.ToProductID = mainID;
                        tempData.CreatedOn = DateTime.Now;
                        tempData.CreatedBy = userName;

                        SaveMergedData(tempData);
                        //CSK_Store_ProductIsMerged_Temp csp = new CSK_Store_ProductIsMerged_Temp();
                        //csp.ProductID = pid;
                        //csp.ToProductID = mainID;
                        //csp.CreatedOn = DateTime.Now;
                        //csp.CreatedBy = userName;

                        //csp.Save();
                    }

                    if (!ConfigAppString.LuceneIndexReadOnly)
                    {
                        try
                        {
                            deleted += LuceneController.DeleteProductFromIndex("ProductID", pid.ToString());
                        }
                        catch (Exception ex)
                        {
                            LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
                        }
                    }
                }
            }

            return mainID;
        }

        static object lockObj = new object();
        static List<MergeProductsSite.IsMergedTempData> mergedTempDataList = new List<MergeProductsSite.IsMergedTempData>();
        static int MergedTempCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MergedTempCount"]);
        private static void SaveMergedData(MergeProductsSite.IsMergedTempData tempData)
        {
            lock (lockObj)
            {
                mergedTempDataList.Add(tempData);
                if(mergedTempDataList.Count >= MergedTempCount)
                {
                    List<MergeProductsSite.IsMergedTempData> list = mergedTempDataList;
                    mergedTempDataList = new List<MergeProductsSite.IsMergedTempData>();
                    SaveMergedDataToDB(list);
                }
            }
        }

        private static void SaveMergedDataToDB(List<MergeProductsSite.IsMergedTempData> mergedTempDataList)
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                string sql = @"INSERT INTO [CSK_Store_ProductIsMerged_Temp]
                           ([ProductID],[ToProductID],[CreatedOn],[CreatedBy]) ";

                sqlConn.Open();

                DateTime dtNow = DateTime.Now;

                foreach (MergeProductsSite.IsMergedTempData mtd in mergedTempDataList)
                {
                    sql += mtd.ToSqlString() + " union all ";
                }
                if (sql.EndsWith(" union all "))
                {
                    sql = sql.Substring(0, sql.Length - " union all ".Length);
                }
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand())
                {
                    sqlCMD.CommandText = sql.Trim();
                    sqlCMD.CommandTimeout = 0;
                    sqlCMD.CommandType = System.Data.CommandType.Text;
                    sqlCMD.Connection = sqlConn;

                    try
                    {
                        sqlCMD.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, sql);

                        LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.Message);
                        LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.StackTrace);
                    }
                }
            }
        }

        public static int GetMainMergeProductID(List<int> productIDs)
        {
            int iOut = 0;
            List<CSK_Store_Product> pcList = new List<CSK_Store_Product>();
            foreach (int pid in productIDs)
            {
                CSK_Store_Product product = ProductController.GetProduct(pid);

                if (product == null)
                {
                    continue;
                }

                if (product.IsMerge == true)
                {
                    pcList.Add(product);
                }
            }

            int retailerAmount = 0;
            if (pcList.Count == 0)
            {
                foreach (int pid in productIDs)
                {
                    int numberStores = ProductController.GetNumberStores(pid);
                    if (numberStores > retailerAmount)
                    {
                        retailerAmount = numberStores;
                        iOut = pid;
                    }
                }
            }
            else if (pcList.Count == 1)
            {
                return pcList[0].ProductID;
            }
            else
            {
                foreach (CSK_Store_Product product in pcList)
                {
                    int numberStores = ProductController.GetNumberStores(product.ProductID);
                    if (numberStores > retailerAmount)
                    {
                        retailerAmount = numberStores;
                        iOut = product.ProductID;
                    }
                }
            }

            return iOut;
        }

        public static int GetNumberStores(int productID)
        {
            int retailerAmount = 0;

            SubSonic.Schema.StoredProcedure sp = PriceMeStatic.PriceMeDB.CSK_Store_12RMB_Product_GetRetailerProductCount();
            sp.Command.AddParameter("@productId", productID, DbType.Int32);

            using (IDataReader idr = sp.ExecuteReader())
            {
                if (idr.Read())
                {
                    int.TryParse(idr[0].ToString(), out retailerAmount);
                }
                return retailerAmount;
            }
        }

        public static int SetProductImage(List<int> productIDs, string userName)
        {
            List<CSK_Store_Product> productList = GetProductList(productIDs);

            CSK_Store_Product product = GetImagePathProduct(productList);

            if (product != null)
            {
                foreach (CSK_Store_Product _product in productList)
                {
                    if (string.IsNullOrEmpty(_product.DefaultImage))
                    {
                        _product.DefaultImage = product.DefaultImage;
                        _product.Update(userName);
                    }
                }
                return product.ProductID;
            }

            return 0;
        }

        static CSK_Store_Product GetImagePathProduct(List<CSK_Store_Product> productList)
        {
            foreach (CSK_Store_Product product in productList)
            {
                if (!string.IsNullOrEmpty(product.DefaultImage))
                {
                    return product;
                }
            }
            return null;
        }

        static List<CSK_Store_Product> GetProductList(List<int> productIDs)
        {
            List<CSK_Store_Product> productList = new List<CSK_Store_Product>();
            foreach (int productID in productIDs)
            {
                CSK_Store_Product product = ProductController.GetProduct(productID);
                if (product != null)
                {
                    productList.Add(product);
                }
            }
            return productList;
        }

        public static bool SetIsMerged(int productID)
        {
            CSK_Store_Product product = ProductController.GetProduct(productID);
            if (product != null)
            {
                product.IsMerge = true;
                product.Save();
                return true;
            }
            return false;
        }

        public static bool SetProductDefaultImage(int productID, int retailerProductID)
        {
            CSK_Store_Product product = ProductController.GetProduct(productID);
            CSK_Store_RetailerProduct retailerProduct = RetailerProductController.
                GetRetailerProduct(retailerProductID);
            if (product == null || retailerProduct == null)
            {
                return false;
            }
            else                
            {
                product.DefaultImage = retailerProduct.DefaultImage;
                product.Save();
                return true;
            }
        }

        public static bool DownAndSetImage(int productID, string imageURL)
        {
            CSK_Store_Product product = ProductController.GetProduct(productID);
            if (product != null)
            {
                string imageName = GetImageName(imageURL);
                return DownloadEachImage(imageURL, imageName, product);
            }
            return false;
        }

        private static string GetImageName(string imageURL)
        {
            string imageName = "";
            if (imageURL.Contains("/"))
            {
                imageName = imageURL.Substring(imageURL.LastIndexOf("/") + 1);
            }
            return imageName.Replace("%20", " ").Trim();
        }

        public static bool ResizeImageToL_NewDir(string dbImagePath)
        {
            string imagePath = ConverToLocalPath(dbImagePath);
            string localImagePath = driverPath + imagePath;
            localImagePath = localImagePath.Replace("\\\\", "\\");

            string newLargeImagePath = newDriverPath + imagePath;
            newLargeImagePath = newLargeImagePath.Replace("\\\\", "\\");
            newLargeImagePath = newLargeImagePath.Insert(newLargeImagePath.LastIndexOf("."), "_l");
            ImageOperator.ResizeImageToL(localImagePath, newLargeImagePath);
            return true;
        }

        public static bool ResizeImageToL(string dbImagePath)
        {
            string localImagePath = driverPath + ConverToLocalPath(dbImagePath);
            localImagePath = localImagePath.Replace("\\\\", "\\");
            ImageOperator.ResizeImageToL(localImagePath);
            return true;
        }

        private static string ConverToLocalPath(string dbImagePath)
        {
            dbImagePath = dbImagePath.Replace("/", "\\");
            if (!dbImagePath.StartsWith("\\"))
            {
                dbImagePath = "\\" + dbImagePath;
            }
            return dbImagePath;
        }

        public static bool DownloadEachImage(string imgUrl, string imgName, CSK_Store_Product product)
        {
            string imagePath = imgFilePath + DateTime.Now.ToString("yyyyMM");
            string imageSrc = imgUrl;
            string imageDirPath = imagePath;
            string imageFilePath = imageDirPath + "\\" + imgName;

            if (!File.Exists(driverPath + imageFilePath))
            {
                try
                {
                    ImageOperator.DownAndResizeImage(imgUrl, driverPath + imageFilePath);

                    product.DefaultImage = imageFilePath;
                    product.Save();

                    return true;
                }
                catch (Exception ex)
                {
                    LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
                }
            }

            return false;
        }

        public static List<CSK_Store_Image> GetProductImages(int productID)
        {
            return CSK_Store_Image.Find(csi => csi.ProductID == productID).ToList();
        }

        public static void DeleteProductImages(int imageID)
        {
            CSK_Store_Image.Delete(csi => csi.ImageID == imageID);
        }

        public static List<PriceMeCommon.Data.RetailerProductImage> GetAllRetailerProductImages(int productID)
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            string selectSqlString = "SELECT [RetailerProductId],[RetailerId],[PurchaseURL],[RetailerProductStatus],[IsDeleted],[IsImageCheck],[DefaultImage] FROM [CSK_Store_RetailerProduct] where ProductId = " + productID;
            List<PriceMeCommon.Data.RetailerProductImage> retailerProductImageList = new List<Data.RetailerProductImage>();
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(selectSqlString, sqlConn))
                {
                    sqlConn.Open();

                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            string imagePath = sqlDR["DefaultImage"].ToString();
                            if (string.IsNullOrEmpty(imagePath)) continue;

                            PriceMeCommon.Data.RetailerProductImage rpImage = new Data.RetailerProductImage();
                            rpImage.ProductID = productID;
                            rpImage.RetailerProductID = int.Parse(sqlDR["RetailerProductId"].ToString());
                            rpImage.ImagePath = imagePath;

                            retailerProductImageList.Add(rpImage);
                        }
                    }
                }
            }

            return retailerProductImageList;
        }

        public static string GetExtName(string fileName)
        {
            string extName = "";
            if (fileName.Contains("."))
            {
                extName = fileName.Substring(fileName.LastIndexOf("."));
            }
            return extName;
        }

        public static bool SetProductDefaultImageFromImageTable(int productID, int imageID)
        {
            CSK_Store_Product product = ProductController.GetProduct(productID);
            CSK_Store_Image image = CSK_Store_Image.SingleOrDefault(img => img.ImageID == imageID);
            if (product == null || image == null)
            {
                return false;
            }
            else
            {
                product.DefaultImage = image.ImageFile;
                product.Save();
                return true;
            }
        }

        public static void DeleteRetailerProductImages(List<int> retailerProductIDs)
        {
            foreach (int rpid in retailerProductIDs)
            {
                CSK_Store_RetailerProduct retailerProduct = CSK_Store_RetailerProduct.SingleOrDefault(rp => rp.RetailerProductId == rpid);
                if (retailerProduct != null)
                {
                    DeteleImage(retailerProduct.DefaultImage);
                    retailerProduct.DefaultImage = "";
                    retailerProduct.Save();
                }
            }
        }

        private static void DeteleImage(string dbImagePath)
        {
            string localImagePath = driverPath + imgFilePath + ConverToLocalPath(dbImagePath);
            localImagePath = localImagePath.Replace("\\\\", "\\");
            if (File.Exists(localImagePath))
            {
                try
                {
                    File.Delete(localImagePath);
                }
                catch { }
            }
        }

        public static bool DownAndSetImageTable(int productID, string imageURL)
        {
            string imageName = GetImageName(imageURL);

            string imagePath = imgFilePath + DateTime.Now.ToString("yyyyMM");
            string imageSrc = imageURL;
            string imageDirPath = imagePath;
            string imageFilePath = imageDirPath + "\\" + imageName;

            if (!File.Exists(driverPath + imageFilePath))
            {
                try
                {
                    ImageOperator.DownAndResizeImage(imageURL, driverPath + imageFilePath, true);

                    CSK_Store_Image image = new CSK_Store_Image();
                    image.ProductID = productID;
                    image.SourceUrl = imageURL;
                    image.ImageFile = imageFilePath;
                    image.Save();

                    return true;
                }
                catch (Exception ex)
                {
                    LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
                }
            }

            return false;
        }

        public static bool DownAndSetImage(int productID, string imageURL, string imageName, bool includeL)
        {
            CSK_Store_Product product = ProductController.GetProduct(productID);
            if (product != null)
            {
                return DownloadImageUseImageName(imageURL, imageName, product, includeL);
            }
            return false;
        }

        public static bool DownAndSetImageTable(int productID, string imageURL, string imageName)
        {
            string imagePath = imgFilePath + DateTime.Now.ToString("yyyyMM");
            string imageDirPath = imagePath;

            try
            {
                string imageFileName = ImageOperator.DownAndResizeImage(imageURL, driverPath + imagePath, imageName, true);

                if (!string.IsNullOrEmpty(imageFileName))
                {
                    string dbImagePath = imagePath + "\\" + imageFileName;

                    CSK_Store_Image image = new CSK_Store_Image();
                    image.ProductID = productID;
                    image.SourceUrl = imageURL;
                    image.ImageFile = dbImagePath;
                    image.Save();

                    return true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
            }

            return false;
        }

        private static bool DownloadImageUseImageName(string imageURL, string imageName, CSK_Store_Product product, bool includeL)
        {
            string imagePath = imgFilePath + DateTime.Now.ToString("yyyyMM");
            string imageDirPath = imagePath;
            
            try
            {
                string imageFileName = ImageOperator.DownAndResizeImage(imageURL, driverPath + imagePath, imageName, false);

                if (!string.IsNullOrEmpty(imageFileName))
                {
                    string dbImagePath = imagePath + "\\" + imageFileName;

                    product.DefaultImage = dbImagePath;
                    product.Save();

                    return true;
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
            }

            return false;
        }
    }
}