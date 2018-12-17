using Microsoft.Extensions.Configuration;
using PricemeResource.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PricemeResource
{
    public class WebSiteConfig
    {
        public static Dictionary<int, ResourcesData> dicResources;
        public static string CssJsPath { get; set; }
        public static string WEB_cssVersion { get; set; }
        public static string GMapZoom { get; set; }
        public static string ImageWebsite { get; set; }
        public static DbInfo PamUserDbInfo { get; set; }
        public static string IframeResource { get; set; }

        public static string ScriptFormat_Static = "dataLayer.push({{'transactionId': '{0}','transactionTotal': {6},'transactionProducts': [{{ 'name': '{1}', 'sku': '{2}','category': {3}, 'price': {4}, 'quantity': 1, 'dimension1' : '{5}'}}],'event': 'pmco_trans'}});";
        public static void Init(IConfigurationRoot configuration)
        {
            CssJsPath = configuration["CssJsPath"];
            WEB_cssVersion = configuration["WEB_cssVersion"];
            GMapZoom = configuration["GMapZoom"];
            ImageWebsite = configuration["ImageWebsite"];
            IframeResource = configuration["IframeResource"];

            BindResources(configuration);

            PamUserDbInfo = configuration.GetSection("PamUserDbInfo").Get<DbInfo>();
        }

        private static void BindResources(IConfigurationRoot configuration)
        {
            dicResources = new Dictionary<int, ResourcesData>();
            ResourcesData res = new ResourcesData();
            res.CurrentCulture = CultureInfo.CreateSpecificCulture("en-NZ");
            res.PriceSymbol = "$";
            res.String_Price = "Price";
            res.PriceTrend = "Price Trend";
            res.PriceHistory = "Price History";
            res.NoHistory = "Sorry, No history.";
            res.RetailerMapDes = "The map below shows you where to find this retailer. PriceMe displays purchase locations for both physical stores and online stores with pickup.";
            res.RetailerMapNo = "There is no retailer map.";
            res.HomeUrl = "https://www.priceme.co.nz";
            res.GoogleMapUrl = "https://map.google.co.nz";
            res.TrackRootUrl = "https://track.priceme.co.nz";
            res.GetDirections = "Get directions";
            res.DbInfo = configuration.GetSection("PricemeDbInfo_NZ").Get<DbInfo>();
            dicResources.Add(3, res);

            res = new ResourcesData();
            res.CurrentCulture = CultureInfo.CreateSpecificCulture("en-AU");
            res.PriceSymbol = "$";
            res.String_Price = "Price";
            res.PriceTrend = "Price Trend";
            res.PriceHistory = "Price History";
            res.NoHistory = "Sorry, No history.";
            res.RetailerMapDes = "The map below shows you where to find this retailer. PriceMe displays purchase locations for both physical stores and online stores with pickup.";
            res.RetailerMapNo = "There is no retailer map.";
            res.HomeUrl = "https://www.priceme.com.au";
            res.GoogleMapUrl = "https://maps.google.com.au";
            res.TrackRootUrl = "https://track.priceme.com.au";
            res.GetDirections = "Get directions";
            res.DbInfo = configuration.GetSection("PricemeDbInfo_AU").Get<DbInfo>();
            dicResources.Add(1, res);

            res = new ResourcesData();
            res.CurrentCulture = CultureInfo.CreateSpecificCulture("en-MY");
            res.PriceSymbol = "RM";
            res.String_Price = "Price";
            res.PriceTrend = "Price Trend";
            res.PriceHistory = "Price History";
            res.NoHistory = "Sorry, No history.";
            res.RetailerMapDes = "The map below shows you where to find this retailer. PriceMe displays purchase locations for both physical stores and online stores with pickup.";
            res.RetailerMapNo = "There is no retailer map.";
            res.HomeUrl = "https://www.priceme.com.my";
            res.GoogleMapUrl = "https://maps.google.com.my";
            res.TrackRootUrl = "https://track.priceme.com.my";
            res.GetDirections = "Get directions";
            res.DbInfo = configuration.GetSection("PricemeDbInfo_MY").Get<DbInfo>();
            dicResources.Add(45, res);

            res = new ResourcesData();
            res.CurrentCulture = CultureInfo.CreateSpecificCulture("en-PH");
            res.PriceSymbol = "P";
            res.String_Price = "Price";
            res.PriceTrend = "Price Trend";
            res.PriceHistory = "Price History";
            res.NoHistory = "Sorry, No history.";
            res.RetailerMapDes = "The map below shows you where to find this retailer. PriceMe displays purchase locations for both physical stores and online stores with pickup.";
            res.RetailerMapNo = "There is no retailer map.";
            res.HomeUrl = "https://www.priceme.com.ph";
            res.GoogleMapUrl = "https://maps.google.com.ph";
            res.TrackRootUrl = "https://track.priceme.com.ph";
            res.GetDirections = "Get directions";
            res.DbInfo = configuration.GetSection("PricemeDbInfo_PH").Get<DbInfo>();
            dicResources.Add(28, res);

            res = new ResourcesData();
            res.CurrentCulture = CultureInfo.CreateSpecificCulture("en-SG");
            res.PriceSymbol = "S$";
            res.String_Price = "Price";
            res.PriceTrend = "Price Trend";
            res.PriceHistory = "Price History";
            res.NoHistory = "Sorry, No history.";
            res.RetailerMapDes = "The map below shows you where to find this retailer. PriceMe displays purchase locations for both physical stores and online stores with pickup.";
            res.RetailerMapNo = "There is no retailer map.";
            res.HomeUrl = "https://www.priceme.com.sg";
            res.GoogleMapUrl = "https://maps.google.com.sg";
            res.TrackRootUrl = "https://track.priceme.com.sg";
            res.GetDirections = "Get directions";
            res.DbInfo = configuration.GetSection("PricemeDbInfo_SG").Get<DbInfo>();
            dicResources.Add(36, res);

            res = new ResourcesData();
            res.CurrentCulture = CultureInfo.CreateSpecificCulture("id-ID");
            res.PriceSymbol = "Rp";
            res.String_Price = "Harga";
            res.PriceTrend = "Tren Harga";
            res.PriceHistory = "Riwayat Harga";
            res.NoHistory = "Maaf, Tidak ada riwayat.";
            res.RetailerMapDes = "Peta dibawah ini menunjukkan pada anda dimana menemukan ritel ini. PriceMe menunjukkan lokasi pembelian baik toko-toko fisik dan toko-toko online dengan pengambilan.";
            res.RetailerMapNo = "Tidak ada peta pengecer.";
            res.HomeUrl = "https://www.priceme.co.id";
            res.GoogleMapUrl = "https://maps.google.co.id";
            res.TrackRootUrl = "https://track.priceme.co.id";
            res.GetDirections = "Cari petunjuk";
            res.DbInfo = configuration.GetSection("PricemeDbInfo_ID").Get<DbInfo>();
            dicResources.Add(51, res);

            res = new ResourcesData();
            res.CurrentCulture = CultureInfo.CreateSpecificCulture("id-TH");
            res.PriceSymbol = "฿";
            res.String_Price = "ราคา";
            res.PriceTrend = "เทรนด์ราคา";
            res.PriceHistory = "ประวัติราคา";
            res.NoHistory = "ขออภัย,ไม่มีประวัติ";
            res.RetailerMapDes = "ด้านล่างจะแสดงให้คุณที่จะหาร้านค้านี้ แสดง PriceMe ซื้อสถานที่สำหรับทั้งร้านค้าทางกายภาพและร้านค้าออนไลน์ที่มีการรับของ";
            res.RetailerMapNo = "ไม่มีแผนที่ร้านค้าปลีก";
            res.HomeUrl = "https://www.pricemethailand.com";
            res.GoogleMapUrl = "https://www.google.co.th/";
            res.TrackRootUrl = "https://track.pricemethailand.com";
            res.GetDirections = "Cari petunjuk";
            res.DbInfo = configuration.GetSection("PricemeDbInfo_TH").Get<DbInfo>();
            dicResources.Add(55, res);

            res = new ResourcesData();
            res.CurrentCulture = CultureInfo.CreateSpecificCulture("vi-VN");
            res.PriceSymbol = "Đ";
            res.String_Price = "Giá";
            res.PriceTrend = "Xu hướng Giá";
            res.PriceHistory = "Lịch Sử Giá";
            res.NoHistory = "Xin lỗi, Không có lịch sử.";
            res.RetailerMapDes = "Bản đồ dưới đây cho bạn thấy nơi để tìm nhà bán lẻ này. PriceMe hiển thị các địa điểm mua hàng của cả các cửa hàng ngoài đường và cửa hàng trực tuyến để lựa chọn.";
            res.RetailerMapNo = "There is no retailer map.";
            res.HomeUrl = "https://www.priceme.co.vn";
            res.GoogleMapUrl = "https://www.google.co.vn/";
            res.TrackRootUrl = "https://track.priceme.vn";
            res.GetDirections = "Xin lỗi, Không có lịch sử.";
            res.DbInfo = configuration.GetSection("PricemeDbInfo_VN").Get<DbInfo>();
            dicResources.Add(56, res);

            res = new ResourcesData();
            res.CurrentCulture = CultureInfo.CreateSpecificCulture("zh-HK");
            res.PriceSymbol = "HK$";
            res.String_Price = "價格";
            res.PriceTrend = "價格走勢";
            res.PriceHistory = "價格歷史";
            res.NoHistory = "抱歉，沒有歷史。";
            res.RetailerMapDes = "下面的地圖顯示你可以在哪找到零售商。PriceMe 提供實體店和整理出的網店的具體位置。";
            res.RetailerMapNo = "There is no retailer map.";
            res.HomeUrl = "https://www.priceme.com.hk";
            res.GoogleMapUrl = "https://maps.google.com.hk";
            res.TrackRootUrl = "https://track.priceme.com.hk";
            res.GetDirections = "Get directions";
            res.DbInfo = configuration.GetSection("PricemeDbInfo_HK").Get<DbInfo>();
            dicResources.Add(41, res);
        }
    }
}
