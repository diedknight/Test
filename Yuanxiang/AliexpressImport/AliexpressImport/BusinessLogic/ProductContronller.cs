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

namespace AliexpressImport.BusinessLogic
{
    public static class ProductContronller
    {
        private static string removekey = System.Configuration.ConfigurationManager.AppSettings["RemoveKey"].ToString();

        public static int ProductMatching(ProductInfoEntity info)
        {
            int type = 0;

            Product product = Product.SingleOrDefault(p => p.AdminComment == info.AdminComment);
            if (product == null)
                product = Product.SingleOrDefault(p => p.Sku == info.Sku);

            if (product == null && product.Price > 0m)
            {
                ProductAdd(info);
                type = 1;
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
            product.Price = info.Price * 1.4m + info.Shipping;
            if (info.OldPrice == 0)
                product.OldPrice = info.OldPrice;
            else
                product.OldPrice = info.OldPrice * 1.4m + info.Shipping;

            product.ProductCost = info.Price + info.Shipping;
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
            string[] images = info.Picture.Split(';');
            foreach (string img in images)
            {
                if (!string.IsNullOrEmpty(img))
                    ProductImages(img, pseos, product.Id);
            }

            string[] atts = info.Atts.Split(';');
            foreach (string att in atts)
            {
                string[] temps = att.Split(':');
                if (temps.Length > 1 && !string.IsNullOrEmpty(temps[0]) && !string.IsNullOrEmpty(temps[1]) && temps[1] != "/")
                    ProductAtt(temps[0], temps[1], product.Id);
            }
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
            pic.SeoFilename = imgName.Substring(0, imgName.LastIndexOf('.'));
            pic.Save();
            
            fs.Flush();
            fs.Close();
            br.Close();

            string lastPart = GetFileExtensionFromMimeType(pic.MimeType);
            string thumbFileName = !String.IsNullOrEmpty(pic.SeoFilename)
                ? string.Format("{0}_{1}.{2}", pic.Id.ToString("0000000"), pic.SeoFilename, lastPart)
                : string.Format("{0}.{1}", pic.Id.ToString("0000000"), lastPart);
            
            AWSS3.UploadImageExecute(strimg, thumbFileName);
            
            string thumbFilePath = strimg;
            string sthumbFileSizePath = thumbFilePath.Insert(thumbFilePath.LastIndexOf("."), "_100");
            ResizeImage(thumbFilePath, sthumbFileSizePath, "S", thumbFileName);
            
            string msthumbFileSizePath = thumbFilePath.Insert(thumbFilePath.LastIndexOf("."), "_415");
            ResizeImage(thumbFilePath, msthumbFileSizePath, "MS", thumbFileName);
            
            string mthumbFileSizePath = thumbFilePath.Insert(thumbFilePath.LastIndexOf("."), "_550");
            ResizeImage(thumbFilePath, mthumbFileSizePath, "M", thumbFileName);

            string ssthumbFileSizePath = thumbFilePath.Insert(thumbFilePath.LastIndexOf("."), "_64");
            ResizeImage(thumbFilePath, ssthumbFileSizePath, "SS", thumbFileName);

            string mssthumbFileSizePath = thumbFilePath.Insert(thumbFilePath.LastIndexOf("."), "_75");
            ResizeImage(thumbFilePath, mssthumbFileSizePath, "MSS", thumbFileName);

            ProductImagesMap(pid, pic.Id);

            File.Delete(strimg);
        }

        private static void ProductAtt(string attname, string attvalue, int pid)
        {
            SpecificationAttribute sa = SpecificationAttribute.SingleOrDefault(s => s.Name.ToLower() == attname.ToLower());
            if (sa == null || sa.Id == 0)
            {
                sa = new SpecificationAttribute();
                sa.Name = attname;
                sa.DisplayOrder = 0;
                sa.Save();
            }

            SpecificationAttributeOption sao = SpecificationAttributeOption.SingleOrDefault(s => s.SpecificationAttributeId == sa.Id && s.Name == attvalue);
            if (sao == null || sao.Id == 0)
            {
                sao = new SpecificationAttributeOption();
                sao.Name = attvalue;
                sao.SpecificationAttributeId = sa.Id;
                sao.DisplayOrder = 0;
                sao.Save();
            }

            Product_SpecificationAttribute_Mapping samap = Product_SpecificationAttribute_Mapping.SingleOrDefault(s => s.ProductId == pid && s.SpecificationAttributeOptionId == sao.Id && s.AttributeTypeId == 0);
            if (samap == null || samap.Id == 0)
            {
                samap = new Product_SpecificationAttribute_Mapping();
                samap.SpecificationAttributeOptionId = sao.Id;
                samap.ProductId = pid;
                samap.AttributeTypeId = 0;
                samap.AllowFiltering = false;
                samap.ShowOnProductPage = true;
                samap.DisplayOrder = 0;
                samap.Save();
            }
        }

        private static string GetFileExtensionFromMimeType(string mimeType)
        {
            if (mimeType == null)
                return null;

            string[] parts = mimeType.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }
            return lastPart;
        }

        private static void ResizeImage(string thumbFilePath, string thumbFileSizePath, string size, string thumbFileName)
        {
            switch (size)
            {
                case "SS":
                    ImageOperator.ResizeImageToSS(thumbFilePath);
                    thumbFileName = thumbFileName.Insert(thumbFileName.LastIndexOf("."), "_64");
                    break;
                case "MSS":
                    ImageOperator.ResizeImageToMSS(thumbFilePath);
                    thumbFileName = thumbFileName.Insert(thumbFileName.LastIndexOf("."), "_75");
                    break;
                case "S":
                    ImageOperator.ResizeImageToS(thumbFilePath);
                    thumbFileName = thumbFileName.Insert(thumbFileName.LastIndexOf("."), "_100");
                    break;
                case "MS":
                    ImageOperator.ResizeImageToMS(thumbFilePath);
                    thumbFileName = thumbFileName.Insert(thumbFileName.LastIndexOf("."), "_415");
                    break;
                case "M":
                    ImageOperator.ResizeImageToM(thumbFilePath);
                    thumbFileName = thumbFileName.Insert(thumbFileName.LastIndexOf("."), "_550");
                    break;
            }
            
            AWSS3.UploadImageExecute(thumbFileSizePath, thumbFileName);

            File.Delete(thumbFileSizePath);
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

        public static string CleanSEOString(string str)
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
            strRet = strRet.Replace("\"", "");
            strRet = strRet.Replace(".", "");
            strRet = strRet.Replace("*", "");
            strRet = strRet.Replace("/", "");
            strRet = strRet.Replace("#", "");
            strRet = strRet.Replace("'", "");
            strRet = strRet.Replace("!", "");
            strRet = strRet.Replace("%", "");
            strRet = strRet.Replace("--", "-");
            strRet = strRet.Replace("---", "-");

            if (!string.IsNullOrEmpty(removekey))
            {
                string[] temps = removekey.Split(',');
                foreach (string key in temps)
                {
                    strRet = strRet.Replace(key, "");
                }
            }
            
            strRet = System.Web.HttpUtility.HtmlDecode(strRet);
            strRet = strRet.ToLower();
            strRet = strRet.Trim();
            return strRet;
        }
    }
}
