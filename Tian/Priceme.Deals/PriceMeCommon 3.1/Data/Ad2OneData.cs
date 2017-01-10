using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    /// <summary>
    /// Ad2One的广告信息
    /// 所有参数的值不能包含空格
    /// </summary>
    public class Ad2OneData
    {
        private Ad2OnePageName pageName = Ad2OnePageName.Other;

        private int bannerType; // 1 top , 2 right, 4 right-down
        private string kw = "";
        private string brand = "";
        private string retail = "";
        private string cat = "";
        private string subcat = "";
        private int height;
        private int width;
        private string sizeString;
        private string location = "";
        private string pricerange = "";
        private string tile;

        public Ad2OnePageName PageName
        {
            get { return pageName; }
            set { pageName = value; }
        }

        public string Kw
        {
            get { return kw; }
            set { kw = (value == null ? null : value.ToLower()); }
        }

        public string Brand
        {
            get { return brand; }
            set { brand = (value == null ? null : value.ToLower()); }
        }

        public string Retail
        {
            get { return retail; }
            set { retail = (value == null ? null : value.ToLower()); }
        }

        public string Cat
        {
            get { return cat; }
            set { cat = (value == null ? null : value.ToLower()); }
        }

        public string Subcat
        {
            get { return subcat; }
            set { subcat = (value == null ? null : value.ToLower()); }
        }

        /// <summary>
        /// 1 top , 2 right
        /// </summary>
        public int BannerType
        {
            get { return bannerType; }
        }

        public int Height
        {
            get { return height; }
        }

        public int Width
        {
            get { return width; }
        }

        public string SizeString
        {
            get { return sizeString; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        public void SetPriceRange(decimal minPrice, decimal maxPrice)
        {
            if(minPrice == maxPrice)
            {
                this.pricerange = GetPriceRangeString(minPrice);
            }
            else
            {
                string min = GetPriceRangeString(minPrice);
                string max = GetPriceRangeString(maxPrice);
                if(min == max)
                {
                    this.pricerange = min;
                }
                else
                {
                    this.pricerange = min + "-" + max;
                }
            }
        }

        string GetPriceRangeString(decimal price)
        {
            string priceRangeString = "";
            if (price > 500)
            {
                priceRangeString = "500plus";
            }
            else if(price > 400)
            {
                priceRangeString = "400";
            }
            else if (price > 300)
            {
                priceRangeString = "300";
            }
            else if (price > 200)
            {
                priceRangeString = "200";
            }
            else if (price > 100)
            {
                priceRangeString = "100";
            }
            else if (price > 50)
            {
                priceRangeString = "50";
            }
            else if (price > 0)
            {
                priceRangeString = "0";
            }
            return priceRangeString;
        }

        public bool SetBannerType(int type)
        {
            bool valid = true;
            if(type == 1)
            {
                this.height = 90;
                this.width = 728;
            }
            else if(type == 2)
            {
                this.height = 250;
                this.width = 300;
            }
            else if (type == 3)
            {
                this.height = 20;
                this.width = 600;
            }
            else if (type == 4)
            {
                this.height = 160;
                this.width = 600;
                valid = true;
            }
            else
            {
                this.height = 0;
                this.width = 0;
                valid = false;
            }
            this.tile = type.ToString();
            this.sizeString = this.Width + "x" + this.Height;
            return valid;
        }

        static string ad2oneScript = "'<scr' + 'ipt language=\"javascript\" src=\"http://ad-apac.doubleclick.net/adj/{0}\" type=\"text/javascript\"></scr' + 'ipt><noscript><a href=\"http://ad-apac.doubleclick.net/jump/{0}\" target=\"_blank\"><img src=\"http://ad-apac.doubleclick.net/ad/{0}\" width=\"{1}\" height=\"{2}\" border=\"0\" alt=\"\"></a></noscript>'";
        //static string ad2oneScript = "'<scr' + 'ipt language=\"javascript\" src=\"{0}\" type=\"text/javascript\"></scr' + 'ipt><noscript><a href=\"http://ad-apac.doubleclick.net/jump/{0}\" target=\"_blank\"><img src=\"http://ad-apac.doubleclick.net/ad/{0}\" width=\"{1}\" height=\"{2}\" border=\"0\" alt=\"\"></a></noscript>'";

        public string GetAd2OneScript()
        {
            string adTagSyntax = GetAdTagSyntax(this);
            return string.Format(ad2oneScript, adTagSyntax, this.Width, this.Height);
        }

        public string GetAdTagSyntax(Ad2OneData ad2OneData)
        {
            string adTagSyntax = "";
            if (ad2OneData.PageName == Ad2OnePageName.Homepage)
            {
                if (ad2OneData.BannerType == 1)
                {
                    adTagSyntax = "ad2onenz.priceme/homepage;sec=homepage;sz={0};tile=1;dcopt=ist;ord=[timestamp]?";
                }
                else
                {
                    adTagSyntax = "ad2onenz.priceme/homepage;sec=homepage;sz={0};tile=" + this.tile + ";ord=[timestamp]?";
                }

                adTagSyntax = string.Format(adTagSyntax, ad2OneData.SizeString);
            }
            else if (ad2OneData.PageName == Ad2OnePageName.Other)
            {
                if (ad2OneData.BannerType == 1)
                {
                    adTagSyntax = "ad2onenz.priceme/others;sec=others;sz={0};tile=1;dcopt=ist;ord=[timestamp]?";
                }
                else
                {
                    adTagSyntax = "ad2onenz.priceme/others;sec=others;sz={0};tile=" + this.tile + ";ord=[timestamp]?";
                }
                adTagSyntax = string.Format(adTagSyntax, ad2OneData.SizeString);
            }
            else if (ad2OneData.PageName == Ad2OnePageName.Search)
            {
                if (ad2OneData.BannerType == 1)
                {
                    adTagSyntax = "ad2onenz.priceme/search;sec=search;kw={0};retail={1};sz={2};tile=1;dcopt=ist;ord=[timestamp]?";
                }
                else
                {
                    adTagSyntax = "ad2onenz.priceme/search;sec=search;kw={0};retail={1};sz={2};tile=" + this.tile + ";ord=[timestamp]?";
                }
                adTagSyntax = string.Format(adTagSyntax, ad2OneData.Kw, ad2OneData.Retail, ad2OneData.SizeString);
            }
            else if (ad2OneData.PageName == Ad2OnePageName.Categories)
            {
                if (ad2OneData.BannerType == 1)
                {
                    adTagSyntax = "ad2onenz.priceme/categories;sec=categories;cat={0};subcat={1};brand={2};retail={3};loc={4};sz={5};tile=1;dcopt=ist;ord=[timestamp]?";
                }
                else
                {
                    adTagSyntax = "ad2onenz.priceme/categories;sec=categories;cat={0};subcat={1};brand={2};retail={3};loc={4};sz={5};tile=" + this.tile + ";ord=[timestamp]?";
                }
                adTagSyntax = string.Format(adTagSyntax, ad2OneData.Cat, ad2OneData.Subcat, ad2OneData.Brand, ad2OneData.Retail, ad2OneData.Location, ad2OneData.SizeString);
            }
            else if (ad2OneData.PageName == Ad2OnePageName.Products || ad2OneData.PageName == Ad2OnePageName.RetailerProduct)
            {
                string pageInfo = "";
                if (ad2OneData.PageName == Ad2OnePageName.Products)
                {
                    pageInfo = "ad2onenz.priceme/products;sec=products;";
                }
                else
                {
                    pageInfo = "ad2onenz.priceme/retailerproducts;sec=retailerproducts;";
                }
                if (ad2OneData.BannerType == 1)
                {
                    adTagSyntax = pageInfo + "cat={0};subcat={1};brand={2};retail={3};loc={4};prod={5};sz={6};pricerange={7};tile=1;dcopt=ist;ord=[timestamp]?";
                }
                else
                {
                    adTagSyntax = pageInfo + "cat={0};subcat={1};brand={2};retail={3};loc={4};prod={5};sz={6};pricerange={7};tile=" + this.tile + ";ord=[timestamp]?";
                }
                adTagSyntax = string.Format(adTagSyntax, ad2OneData.Cat, ad2OneData.Subcat, ad2OneData.Brand, ad2OneData.Retail, ad2OneData.Location, ad2OneData.Kw, ad2OneData.SizeString, ad2OneData.pricerange);
            }
            else if (ad2OneData.PageName == Ad2OnePageName.Deals)
            {
               // "ad2onenz.priceme/deals;sec=deals;cat=[value];subcat=[value];brand=[value];retail=[value];prod=[value];loc=[value];pricerange=[value];sz=600x20;tile=3;ord=[timestamp]?"

                if (ad2OneData.BannerType == 1)
                {
                    adTagSyntax = "ad2onenz.priceme/deals;sec=deals;cat={0};subcat={1};brand={2};retail={3};loc={4};sz={5};tile=1;dcopt=ist;ord=[timestamp]?";
                }
                else
                {
                    adTagSyntax = "ad2onenz.priceme/deals;sec=deals;cat={0};subcat={1};brand={2};retail={3};loc={4};sz={5};tile=" + this.tile + ";ord=[timestamp]?";
                }
                adTagSyntax = string.Format(adTagSyntax, ad2OneData.Cat, ad2OneData.Subcat, ad2OneData.Brand, ad2OneData.Retail, ad2OneData.Location, ad2OneData.SizeString);
            }
            
            //adTagSyntax = "http://localhost:8072/SearchManufacturer.aspx?sm=canon";
            return adTagSyntax;
        }
    }

    public enum Ad2OnePageName
    {
        Homepage,
        Search,
        Other,
        Categories,
        Products,
        RetailerProduct,
        Deals
    }
}