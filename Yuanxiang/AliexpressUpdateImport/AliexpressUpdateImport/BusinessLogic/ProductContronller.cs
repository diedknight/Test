using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliexpressImport.Data;
using AliexpressDBA;
using System.IO;
using SubSonic.Schema;
using System.Data;
using System.Diagnostics;

namespace AliexpressImport.BusinessLogic
{
    public static class ProductContronller
    {
        public static int ProductMatching(ProductInfoEntity info)
        {
            int type = 0;

            Product product = Product.SingleOrDefault(p => p.AdminComment == info.AdminComment);
            if (product == null)
                product = Product.SingleOrDefault(p => p.Sku == info.Sku);

            if (product != null)
            {
                ProductUpdate(product, info);
                type = 2;
            }

            return type;
        }

        private static void ProductAdd(ProductInfoEntity info)
        {
            Product product = new Product();
            product.Name = info.ProductName;
            product.AdminComment = info.AdminComment;
            product.VendorId = GetVendor(info.Vendor);
            product.Sku = info.Sku;
            product.DeliveryDateId = GetDeliveryDate(info.DeliveryDate);
            product.StockQuantity = info.Stock;
            product.DisplayStockAvailability = info.Stock < 1 ? true : false;
            product.ManageInventoryMethodId = info.Stock < 1 ? 1 : 0;
            product.Price = info.Price * 1.4m;
            product.OldPrice = info.OldPrice * 1.4m;
            product.ProductCost = info.Price;
            product.Width = info.Width;
            product.Length = info.Length;
            product.Height = info.Height;
            product.Weight = info.Weight;
            product.FullDescription = info.FullDescription;
            product.CreatedOnUtc = DateTime.Now;
            product.UpdatedOnUtc = DateTime.Now;

            #region 默认值
            product.ProductTypeId = 5;
            product.ParentGroupedProductId = 0;
            product.VisibleIndividually = true;
            product.ProductTemplateId = 1;
            product.ShowOnHomePage = false;
            product.AllowCustomerReviews = true;
            product.ApprovedRatingSum = 0;
            product.NotApprovedRatingSum = 0;
            product.ApprovedTotalReviews = 0;
            product.NotApprovedTotalReviews = 0;
            product.SubjectToAcl = false;
            product.LimitedToStores = false;
            product.IsGiftCard = false;
            product.GiftCardTypeId = 0;
            product.RequireOtherProducts = false;
            product.AutomaticallyAddRequiredProducts = false;
            product.IsDownload = false;
            product.DownloadId = 0;
            product.UnlimitedDownloads = false;
            product.MaxNumberOfDownloads = 0;
            product.DownloadActivationTypeId = 0;
            product.HasSampleDownload = false;
            product.SampleDownloadId = 0;
            product.HasUserAgreement = false;
            product.IsRecurring = false;
            product.RecurringCycleLength = 0;
            product.RecurringCyclePeriodId = 0;
            product.RecurringTotalCycles = 0;
            product.IsRental = false;
            product.RentalPriceLength = 0;
            product.RentalPricePeriodId = 0;
            product.IsShipEnabled = true;
            product.IsFreeShipping = true;
            product.ShipSeparately = false;
            product.AdditionalShippingCharge = 0;
            product.IsTaxExempt = false;
            product.TaxCategoryId = 2;
            product.IsTelecommunicationsOrBroadcastingOrElectronicServices = false;
            product.ProductAvailabilityRangeId = 0;
            product.UseMultipleWarehouses = false;
            product.WarehouseId = 0;
            product.DisplayStockQuantity = false;
            product.MinStockQuantity = 0;
            product.LowStockActivityId = 0;
            product.NotifyAdminForQuantityBelow = 0;
            product.BackorderModeId = 0;
            product.AllowBackInStockSubscriptions = false;
            product.OrderMinimumQuantity = 1;
            product.OrderMaximumQuantity = 100;
            product.AllowAddingOnlyExistingAttributeCombinations = false;
            product.NotReturnable = false;
            product.DisableBuyButton = false;
            product.DisableWishlistButton = false;
            product.AvailableForPreOrder = false;
            product.CallForPrice = false;
            product.CustomerEntersPrice = false;
            product.MinimumCustomerEnteredPrice = 0;
            product.MaximumCustomerEnteredPrice = 0;
            product.BasepriceEnabled = false;
            product.BasepriceAmount = 0;
            product.BasepriceUnitId = 1;
            product.BasepriceBaseAmount = 0;
            product.BasepriceBaseUnitId = 1;
            product.MarkAsNew = false;
            product.HasTierPrices = false;
            product.HasDiscountsApplied = false;
            product.DisplayOrder = 0;
            product.Published = true;
            product.Deleted = false;
            #endregion

            product.Save();

            string pseos = CleanSEOString(info.ProductName);

            string purstring = pseos;
            int urcount = SelectUrlRecord(pseos);
            if (urcount > 0)
                purstring = pseos + "_" + (urcount + 1);
            InsertUrlRecord(product.Id, "Product", purstring);

            //Category
            ProductCategoryMap(info, product.Id);

            //images
            List<string> listImages = new List<string>();
            if (!string.IsNullOrEmpty(info.Picture1))
                listImages.Add(info.Picture1);
            if (!string.IsNullOrEmpty(info.Picture2))
                listImages.Add(info.Picture2);
            if (!string.IsNullOrEmpty(info.Picture3))
                listImages.Add(info.Picture3);
            if (!string.IsNullOrEmpty(info.Picture4))
                listImages.Add(info.Picture4);
            if (!string.IsNullOrEmpty(info.Picture5))
                listImages.Add(info.Picture5);
            if (!string.IsNullOrEmpty(info.Picture6))
                listImages.Add(info.Picture6);
            foreach (string img in listImages)
                ProductImages(img, pseos, product.Id);
        }

        private static void ProductCategoryMap(ProductInfoEntity info, int pid)
        {
            CategoryData cate = CategoryContronller.listCategory.SingleOrDefault(c => c.CategoryName == info.Category);
            if (cate != null)
            {
                Product_Category_Mapping map = new Product_Category_Mapping();
                map.ProductId = pid;
                map.CategoryId = cate.CategoryId;
                map.IsFeaturedProduct = false;
                map.DisplayOrder = 1;
                map.Save();
            }
        }

        private static void ProductUpdate(Product product, ProductInfoEntity info)
        {
            product.AdminComment = info.AdminComment;
            product.Sku = info.Sku;
            product.Price = info.Price * 1.4m;
            product.ProductCost = info.Price;
            product.Deleted = false;
            product.UpdatedOnUtc = DateTime.Now;
            product.Save();
        }

        private static void ProductImages(string imageUrl, string pseos, int pid)
        {
            string dirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageCache");
            string imageSrc = imageUrl;

            string imagetype = string.Empty;
            string imgName = ImageOperator.GetRandomString(8);
            imgName = ImageOperator.DownImage(imageUrl, dirPath, imgName, out imagetype);
            imagetype = "image/" + imagetype.Replace(".", "");

            string strimg = dirPath + "\\" + imgName; 
            FileStream fs = new FileStream(strimg, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br = new BinaryReader(fs);
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length); //将流读入到字节数组中

            Picture pic = new Picture();
            pic.PictureBinary = imgBytesIn;
            pic.MimeType = imagetype;
            pic.SeoFilename = pseos;
            pic.Save();
            
            fs.Flush();
            fs.Close();
            br.Close();

            AWSS3.UploadImageExecute(strimg);

            ProductImagesMap(pid, pic.Id);

            File.Delete(strimg);
        }

        private static void ProductImagesMap(int pid, int imgid)
        {
            Product_Picture_Mapping map = new Product_Picture_Mapping();
            map.ProductId = pid;
            map.PictureId = imgid;
            map.Save();
        }

        private static int GetVendor(string vendor)
        {
            Vendor ven = Vendor.SingleOrDefault(v => v.Name == vendor);
            if (ven == null)
            {
                ven = new Vendor();
                ven.Name = vendor;
                ven.Email = "support@shop.co.nz";
                ven.Save();

                InsertUrlRecord(ven.Id, "Vendor", CleanSEOString(vendor));
            }

            return ven.Id;
        }

        private static int GetDeliveryDate(string delivery)
        {
            DeliveryDate del = DeliveryDate.SingleOrDefault(d => d.Name == delivery);
            if (del == null)
            {
                del = new DeliveryDate();
                del.Name = delivery;
                del.Save();
            }

            return del.Id;
        }

        private static int SelectUrlRecord(string slug)
        {
            List<UrlRecord> urs = AliexpressStatic.AliexpressDB.UrlRecords.Where(r => r.Slug.Contains(slug)).ToList();
            if (urs == null)
                return 0;

            return urs.Count;
        }

        private static void InsertUrlRecord(int entityId, string entityName, string slug)
        {
            UrlRecord ur = new UrlRecord();
            ur.EntityId = entityId;
            ur.EntityName = entityName;
            ur.Slug = slug;
            ur.IsActive = true;
            ur.Save();
        }

        private static string CleanSEOString(string str)
        {
            String strRet = str.Replace(" ", "-");
            strRet = strRet.Replace(",", "-");
            strRet = strRet.Replace("（", "-");
            strRet = strRet.Replace("）", "-");
            strRet = strRet.Replace("+", "-");
            strRet = strRet.Replace("&", "-");
            strRet = strRet.Replace(":", "-");
            strRet = strRet.Replace("[", "-");
            strRet = strRet.Replace("]", "-");
            strRet = strRet.Replace(".", "");
            strRet = strRet.Replace("*", "");
            strRet = strRet.Replace("/", "");
            strRet = strRet.Replace("#", "");
            strRet = strRet.Replace("'", "");
            strRet = strRet.Replace("!", "");
            strRet = strRet.Replace("%", "");
            strRet = strRet.Replace("--", "-");
            strRet = strRet.Replace("---", "-");
            
            strRet = System.Web.HttpUtility.HtmlDecode(strRet);
            strRet = strRet.ToLower();
            strRet = strRet.Trim();
            return strRet;
        }

        public static int GetAllProductCountByCategory(int cid)
        {
            int count = 0;
            string sql = "Select COUNT(Id) as cun From Product Where Deleted = 0 And Id in (Select ProductId From Product_Category_Mapping Where CategoryId = " + cid + ")";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int.TryParse(dr["cun"].ToString(), out count);
            }
            dr.Close();
            OutManagerContronller.WriterInfo(TraceEventType.Verbose, "all count: " + sql);

            return count;
        }

        public static int GetUpdateProductCountByCategory(int cid, DateTime startTime)
        {
            int count = 0;
            string sql = "Select COUNT(Id) as cun From Product Where Deleted = 0 And UpdatedOnUtc > '" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "' And Id in (Select ProductId From Product_Category_Mapping Where CategoryId = " + cid + ")";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int.TryParse(dr["cun"].ToString(), out count);
            }
            dr.Close();
            OutManagerContronller.WriterInfo(TraceEventType.Verbose, "Update count: " + sql);

            return count;
        }

        public static void UpdateProductBySql(string sql)
        {
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            sp.Execute();
        }
    }
}
