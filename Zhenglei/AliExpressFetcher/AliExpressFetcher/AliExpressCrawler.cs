﻿using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AliExpressFetcher
{
    public class AliExpressCrawler : IDisposable
    {
        static Regex SkuRegex_Static;
        static Regex PriceRegex_Static;
        static Regex UnitRegex_Static;
        static List<List<string>> RelatedProductFormatList;
        static int RetryCount_Static;
        static int TimeInterval_Static;

        string mAccount;
        string mPassword;
        string mCountry;
        string mCurrency;
        string mChromeWebDriverDir;
        List<string> mAddedSkuList;

        string mFeedPath;
        string mResultPath;
        StreamWriter mCsvStreamWriter;

        public string FeedPath
        {
            get { return mFeedPath; }
        }

        int mWorkerCount;
        public bool AllFinished
        {
            get { return mWorkerCount == 0; }
        }

        static AliExpressCrawler()
        {
            SkuRegex_Static = new Regex("/(?<sku>[\\w]+)\\.html", RegexOptions.IgnoreCase | RegexOptions.Singleline| RegexOptions.Compiled);
            PriceRegex_Static = new Regex("(?<price>[\\d]+(\\.[\\d]{0,2})?)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            UnitRegex_Static = new Regex("(?<l>[\\d]+(\\.[\\d]{0,2})?)(?<unit>\\w+)\\s?x\\s?(?<w>[\\d]+(\\.[\\d]{0,2})?)\\w+\\s?x\\s?(?<h>[\\d]+(\\.[\\d]{0,2})?)\\w+", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

            string relatedProductFormatFilePath = System.Configuration.ConfigurationManager.AppSettings["RelatedProductFormatFile"];
            RelatedProductFormatList = GetRelatedProductFormatList(relatedProductFormatFilePath);

            RetryCount_Static = int.Parse(System.Configuration.ConfigurationManager.AppSettings["RetryCount"]);
            TimeInterval_Static = int.Parse(System.Configuration.ConfigurationManager.AppSettings["TimeInterval"]);
        }

        private static List<List<string>> GetRelatedProductFormatList(string relatedProductFormatFilePath)
        {
            //内容格式 标签名，属性名，属性值 之间用Tab符号隔开
            List<List<string>> formatList = new List<List<string>>();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(relatedProductFormatFilePath))
            {
                string line = sr.ReadLine();
                while(!string.IsNullOrEmpty(line))
                {
                    string[] infos = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> formats = new List<string>();
                    formats.AddRange(infos);
                    formatList.Add(formats);

                    line = sr.ReadLine();
                }
            }

            return formatList;
        }

        public AliExpressCrawler(string account, string password, string country, string currency, string chromeWebDriverDir)
        {
            mAccount = account;
            mPassword = password;
            mCountry = country;
            mCurrency = currency;
            mChromeWebDriverDir = chromeWebDriverDir;
        }

        public AliExpressCrawler(string account, string password, string country, string currency, string chromeWebDriverDir, string feedPath, string resultPath)
        {
            mAccount = account;
            mPassword = password;
            mCountry = country;
            mCurrency = currency;
            mChromeWebDriverDir = chromeWebDriverDir;
            mFeedPath = feedPath;
            mResultPath = resultPath;
        }

        public void CrawlProductsRealTimeAndMultiThread(List<CategoryInfo> categoryInfoList, List<string> crawledUrls, int threadCount)
        {
            if(crawledUrls.Count == 0 )
            {
                WriteCsvFileHeader();
            }

            mAddedSkuList = new List<string>();
            mCsvStreamWriter = new StreamWriter(mFeedPath, true);
            var s1 = ThreadPool.SetMinThreads(1, 1);
            var s = ThreadPool.SetMaxThreads(threadCount, threadCount);
            mWorkerCount = 0;
            foreach (var categoryInfo in categoryInfoList)
            {
                if (!crawledUrls.Contains(categoryInfo.CategoryUrl))
                {
                    Thread.Sleep(10000);
                    mWorkerCount++;
                    ThreadPool.QueueUserWorkItem(CrawlCatalogProducts, categoryInfo);
                }
            }
        }

        private void CrawlCatalogProducts(object categoryInfo)
        {
            try
            {
                CategoryInfo ci = categoryInfo as CategoryInfo;

                ChromeOptions chromeOptions = new ChromeOptions();
                if ("true".Equals(System.Configuration.ConfigurationManager.AppSettings["DisableImage"], StringComparison.InvariantCultureIgnoreCase))
                {
                    chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
                }

                using (ChromeDriver driver = new ChromeDriver(mChromeWebDriverDir, chromeOptions))
                {
                    InitDriver(driver);

                    driver.Navigate().GoToUrl("https://www.aliexpress.com/");

                    ClosePopup(driver, true);

                    Login(driver);

                    ClosePopup(driver, true);

                    if (!string.IsNullOrEmpty(mCountry))
                    {
                        bool selected = SelectCountry(driver);

                        if (!selected)
                        {
                            Log("CrawlCatalogProducts log : " + mCountry + " not exist.");
                            return;
                        }
                    }

                    string nextPageUrl = ci.CategoryUrl;

                    do
                    {
                        int retry = 0;
                        while (retry < RetryCount_Static)
                        {
                            try
                            {
                                driver.Navigate().GoToUrl(nextPageUrl);

                                ClosePopup(driver, true);

                                string currentCurrency = driver.FindElementByCssSelector(".currency").GetAttribute("innerText").Trim();
                                if (!currentCurrency.Equals(mCurrency, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    SelectCurrency(driver);
                                }

                                var nextATag = driver.FindElementsByCssSelector("a.page-next.ui-pagination-next");
                                if (nextATag.Count > 0)
                                {
                                    nextPageUrl = nextATag[0].GetAttribute("href").Trim();
                                }
                                else
                                {
                                    nextPageUrl = "";
                                }

                                var productsTag = driver.FindElementsByCssSelector("#list-items .list-item a.product");
                                List<string> productUrls = new List<string>();
                                foreach (var pATag in productsTag)
                                {
                                    productUrls.Add(pATag.GetAttribute("href").Trim());
                                }

                                foreach (string pUrl in productUrls)
                                {
                                    ProductInfo pi = GetProductInfo(pUrl, ci.CategoryName, driver);
                                    Thread.Sleep(TimeInterval_Static);
                                    if (pi != null)
                                    {
                                        WriteToCsv(pi);
                                    }
                                }
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message + "\t" + ex.StackTrace);
                                retry++;
                            }
                        }
                    }
                    while (!string.IsNullOrEmpty(nextPageUrl));
                }

                AddCrawledUrl(ci.CategoryUrl);
            }
            catch(Exception ex)
            {
                Log("CrawlCatalogProducts ex : " + ex.StackTrace + "\t" + ex.Message);
            }
            mWorkerCount--;
        }

        private void WriteCsvFileHeader()
        {
            using (StreamWriter sw = new StreamWriter(mFeedPath, false))
            {
                sw.WriteLine(ProductInfo.ToCsvHeaderStringNew());
                sw.Flush();
            }
        }

        private void WriteToCsv(ProductInfo pi)
        {
            mCsvStreamWriter.WriteLine(pi.ToCsvStringNew());
            mCsvStreamWriter.Flush();
        }

        private void AddCrawledUrl(string url)
        {
            using (StreamWriter sw = new StreamWriter(mResultPath, true))
            {
                sw.WriteLine(url);
            }
        }

        public List<ProductInfo> CrawlProducts(List<CategoryInfo> categoryInfoList)
        {
            mAddedSkuList = new List<string>();

            List<ProductInfo> list = new List<ProductInfo>();

            ChromeOptions chromeOptions = new ChromeOptions();
            if ("true".Equals(System.Configuration.ConfigurationManager.AppSettings["DisableImage"], StringComparison.InvariantCultureIgnoreCase))
            {
                chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            }
            using (ChromeDriver driver = new ChromeDriver(mChromeWebDriverDir, chromeOptions))
            {
                InitDriver(driver);

                driver.Navigate().GoToUrl("https://www.aliexpress.com/");

                ClosePopup(driver, true);

                Login(driver);

                ClosePopup(driver, true);

                if (!string.IsNullOrEmpty(mCountry))
                {
                    bool selected = SelectCountry(driver);

                    if (!selected)
                    {
                        Log(mCountry + " not exist.");
                        return list;
                    }
                }

                
                foreach (var ci in categoryInfoList)
                {
                    Log("Url : " + ci.CategoryUrl);
                    List<ProductInfo> products = CrawlCategoryProducts(ci, driver);
                    list.AddRange(products);
                }
            }

            return list;
        }

        private void Log(string msg)
        {
            Console.WriteLine(msg);
        }

        private List<ProductInfo> CrawlCategoryProducts(CategoryInfo ci, ChromeDriver driver)
        {
            List<ProductInfo> list = new List<ProductInfo>();

            string nextPageUrl = ci.CategoryUrl;

            do
            {
                int retry = 0;
                while (retry < RetryCount_Static)
                {
                    try
                    {
                        driver.Navigate().GoToUrl(nextPageUrl);

                        ClosePopup(driver, true);

                        string currentCurrency = driver.FindElementByCssSelector(".currency").GetAttribute("innerText").Trim();
                        if (!currentCurrency.Equals(mCurrency, StringComparison.InvariantCultureIgnoreCase))
                        {
                            SelectCurrency(driver);
                        }

                        var nextATag = driver.FindElementsByCssSelector("a.page-next.ui-pagination-next");
                        if (nextATag.Count > 0)
                        {
                            nextPageUrl = nextATag[0].GetAttribute("href").Trim();
                        }
                        else
                        {
                            nextPageUrl = "";
                        }

                        var productsTag = driver.FindElementsByCssSelector("#list-items .list-item a.product");
                        List<string> productUrls = new List<string>();
                        foreach (var pATag in productsTag)
                        {
                            productUrls.Add(pATag.GetAttribute("href").Trim());
                        }

                        foreach (string pUrl in productUrls)
                        {
                            ProductInfo pi = GetProductInfo(pUrl, ci.CategoryName, driver);
                            if (pi != null)
                            {
                                list.Add(pi);
                            }
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "\t" + ex.StackTrace);
                        retry++;
                    }
                }
            }
            while (!string.IsNullOrEmpty(nextPageUrl));

            return list;
        }

        private ProductInfo GetProductInfo(string productLink, string categoryName, ChromeDriver driver)
        {
            ProductInfo pi = null;
            int retry = 0;

            while (retry < RetryCount_Static)
            {
                try
                {
                    string sku = SkuRegex_Static.Match(productLink).Groups["sku"].Value;
                    if (mAddedSkuList.Contains(sku))
                    {
                        return null;
                    }

                    driver.Navigate().GoToUrl(productLink);

                    System.Threading.Thread.Sleep(300);

                    var footerEle = driver.FindElementByCssSelector(".site-footer");
                    driver.ExecuteScript("arguments[0].scrollIntoView(true);", footerEle);
                    System.Threading.Thread.Sleep(1000);
                    var headerEle = driver.FindElementByCssSelector(".top-lighthouse");
                    driver.ExecuteScript("arguments[0].scrollIntoView(true);", headerEle);

                    string productName = driver.FindElementByCssSelector("h1.product-name").Text.Trim();
                    string productPriceCurrency = "";
                    string productPriceStr = "";
                    int stockNum = 0;

                    if (driver.FindElementsByCssSelector("#j-sell-stock-num").Count > 0)
                    {
                        string stockInfo = driver.FindElementByCssSelector("#j-sell-stock-num").Text.Trim();
                        stockNum = int.Parse(stockInfo.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0]);
                    }

                    if (driver.FindElementsByCssSelector(".product-multi-price-main.notranslate span[itemprop=priceCurrency]").Count > 0)
                    {
                        productPriceCurrency = driver.FindElementByCssSelector(".product-multi-price-main.notranslate span[itemprop=priceCurrency]").Text.Trim();
                        productPriceStr = driver.FindElementByCssSelector("#j-multi-currency-price span[itemprop=price]").Text.Trim();
                    }
                    else
                    {
                        productPriceCurrency = driver.FindElementByCssSelector(".p-price-content .p-symbol").Text.Trim();
                        productPriceStr = "";
                        var productPriceEles = driver.FindElementsByCssSelector("#j-sku-discount-price");


                        if (productPriceEles.Count > 0)
                        {
                            productPriceStr = productPriceEles[0].Text.Trim();
                        }
                        else
                        {
                            productPriceStr = driver.FindElementByCssSelector("#j-sku-price").Text.Trim();
                        }
                    }

                    if (productPriceStr.Contains(" - "))
                        productPriceStr = productPriceStr.Split(new string[] { " - " }, StringSplitOptions.None)[0].Trim();
                    decimal productPrice = decimal.Parse(productPriceStr);
                    string productPriceUnit = driver.FindElementByCssSelector(".p-price-content .p-unit").Text.Trim();

                    string oldProductPriceCurrency = "";
                    decimal oldProductPrice = 0;
                    if (driver.FindElementsByCssSelector(".p-del-price-detail").Count > 0)
                    {
                        oldProductPriceCurrency = driver.FindElementByCssSelector(".p-del-price-detail .p-symbol").Text.Trim();
                        string oldProductPriceStr = driver.FindElementByCssSelector(".p-del-price-detail .p-price").Text.Trim();
                        if (oldProductPriceStr.Contains(" - "))
                            oldProductPriceStr = oldProductPriceStr.Split(new string[] { " - " }, StringSplitOptions.None)[0].Trim();
                        oldProductPrice = decimal.Parse(oldProductPriceStr);
                    }

                    string bulkPriceStr = "";

                    var bulkPriceNodes = driver.FindElementsByCssSelector(".bulk-price-tips.ui-balloon.ui-balloon-tl");
                    if (bulkPriceNodes.Count > 0)
                    {
                        bulkPriceStr = bulkPriceNodes[0].GetAttribute("innerText").Trim();
                    }

                    string vender = driver.FindElementByCssSelector(".shop-name a").Text.Trim();

                    string unitType = "";
                    float weight = 0;
                    float length = 0;
                    float width = 0;
                    float height = 0;
                    string unit = "";

                    var packagingListEles = driver.FindElementsByCssSelector(".product-packaging-list.util-clearfix .packaging-item");
                    driver.ExecuteScript("arguments[0].scrollIntoView(true);", packagingListEles[0]);
                    foreach (var pge in packagingListEles)
                    {
                        string pgTitle = pge.FindElement(By.ClassName("packaging-title")).GetAttribute("innerText").Trim().TrimEnd(':');
                        string pgValue = pge.FindElement(By.ClassName("packaging-des")).GetAttribute("innerText").Trim();
                        if (pgTitle.Equals("Unit Type", StringComparison.InvariantCultureIgnoreCase))
                        {
                            unitType = pgValue;
                        }
                        else if (pgTitle.Equals("Package Size", StringComparison.InvariantCultureIgnoreCase))
                        {
                            string unitString = pgValue.Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                            Match match = UnitRegex_Static.Match(unitString);
                            if (match.Success)
                            {
                                length = float.Parse(match.Groups["l"].Value);
                                width = float.Parse(match.Groups["w"].Value);
                                height = float.Parse(match.Groups["h"].Value);
                                unit = match.Groups["unit"].Value;
                            }
                        }
                        else if (pgTitle.Equals("Package Weight", StringComparison.InvariantCultureIgnoreCase))
                        {
                            string pgw = pgValue.Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries)[0].Replace("kg", "").Trim();
                            weight = float.Parse(pgw);
                        }
                    }

                    var descEle = driver.FindElementByCssSelector(".description-content");
                    driver.ExecuteScript("arguments[0].scrollIntoView(true);", descEle);
                    System.Threading.Thread.Sleep(800);
                    string fullDescription = descEle.GetAttribute("innerHTML").Trim();
                    int reTryCount = 3;
                    while (fullDescription.Length < 80 && reTryCount > 0)
                    {
                        System.Threading.Thread.Sleep(500);
                        fullDescription = descEle.GetAttribute("innerHTML").Trim();
                        reTryCount--;
                    }
                    fullDescription = FixDesc(fullDescription);

                    List<string> images = new List<string>();
                    var imageTags = driver.FindElementsByCssSelector("#j-image-thumb-list img");
                    foreach (var img in imageTags)
                    {
                        string imgUrl = img.GetAttribute("src").Replace("_50x50.jpg", "");
                        images.Add(imgUrl);
                    }

                    Dictionary<string, string> attrs = new Dictionary<string, string>();
                    var attrTags = driver.FindElementsByCssSelector(".product-property-list.util-clearfix .property-item");
                    foreach (var attrTag in attrTags)
                    {
                        string key = attrTag.FindElement(By.ClassName("propery-title")).GetAttribute("innerText").Trim().TrimEnd(':').TrimEnd(':').Trim();
                        string value = attrTag.FindElement(By.ClassName("propery-des")).GetAttribute("innerText").Trim();
                        if (!attrs.ContainsKey(key))
                        {
                            attrs.Add(key, value);
                        }
                    }

                    List<ShippingInfo> shippingInfos = GetShippingInfos(driver);

                    pi = new ProductInfo();
                    pi.Name = productName;
                    pi.Url = productLink;
                    if (pi.Url.Contains(".html?"))
                    {
                        pi.Url = pi.Url.Split(new string[] { ".html?" }, StringSplitOptions.RemoveEmptyEntries)[0] + ".html";
                    }
                    pi.Category = categoryName;
                    pi.Price = productPrice;
                    pi.PriceCurrency = productPriceCurrency;
                    pi.OldPrice = oldProductPrice;
                    pi.OldPriceCurrency = oldProductPriceCurrency;
                    pi.ProductPriceUnit = productPriceUnit;
                    pi.SKU = sku;
                    pi.Vender = vender;
                    pi.FullDescription = fullDescription;
                    pi.ShippingInfos = shippingInfos;
                    pi.Images = images;
                    pi.Attributes = attrs;
                    pi.Length = length;
                    pi.Width = width;
                    pi.Height = height;
                    pi.UnitType = unitType;
                    pi.Unit = unit;
                    pi.Weight = weight;
                    pi.StockNum = stockNum;

                    mAddedSkuList.Add(sku);

                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    retry++;
                }
            }
            return pi;
        }

        private string FixDesc(string fullDescription)
        {
            string html = fullDescription;
            CsQuery.CQ cq = CsQuery.CQ.Create(html);

            bool removed = false;
            foreach (var formats in RelatedProductFormatList)
            {
                if (cq[formats[0]] != null)
                {
                    var divList = cq[formats[0]].ToList();
                    foreach (var div in divList)
                    {
                        string style = div.Attributes[formats[1]];
                        if (style.Equals(formats[2], StringComparison.InvariantCultureIgnoreCase))
                        {
                            div.Remove();
                            removed = true;
                            break;
                        }
                    }
                }
                if (removed) break;
            }

            string newHtml = cq.Render();
            return newHtml;
        }

        private List<ShippingInfo> GetShippingInfos(ChromeDriver driver)
        {
            List<ShippingInfo> list = new List<ShippingInfo>();

            var shippingTag = driver.FindElementByCssSelector(".shipping-link");
            if(!shippingTag.Displayed)
            {
                driver.FindElementByCssSelector("#j-product-info-sku .p-item-main > ul > li:not(.disabled) a").Click();
            }
            System.Threading.Thread.Sleep(500);
            driver.ExecuteScript("arguments[0].scrollIntoView(true);", shippingTag);
            System.Threading.Thread.Sleep(200);
            shippingTag.Click();
            System.Threading.Thread.Sleep(800);

            var shippingTr = driver.FindElementsByCssSelector("#j-shipping-company-list .s-company-table tr");

            for(int i = 1; i < shippingTr.Count; i++)
            {
                var tr = shippingTr[i];

                ShippingInfo shippingInfo = new ShippingInfo();

                var imgs = tr.FindElements(By.CssSelector(".col-cam img"));
                if (imgs.Count > 0)
                {
                    shippingInfo.Name = imgs[0].GetAttribute("alt");
                }
                else
                {
                    shippingInfo.Name = tr.FindElement(By.CssSelector(".col-cam")).Text.Trim();
                }
                shippingInfo.EstimatedDeliveryTime = tr.FindElements(By.TagName("td"))[2].Text.Trim();
                string shippingPriceInfo = tr.FindElements(By.TagName("td"))[3].Text.Trim();
                Match priceMatch = PriceRegex_Static.Match(shippingPriceInfo);
                if (priceMatch.Success)
                {
                    string priceStr = priceMatch.Groups["price"].Value;
                    shippingInfo.PriceCurrency = shippingPriceInfo.Replace(priceStr, "").Trim();
                    shippingInfo.Price = decimal.Parse(priceStr);
                }
                else if(shippingPriceInfo.Equals("Free Shipping", StringComparison.InvariantCultureIgnoreCase))
                {
                    shippingInfo.Price = 0;
                }
                else
                {
                    shippingInfo.Price = -999;
                }

                list.Add(shippingInfo);
            }

            return list;
        }

        private bool SelectCountry(ChromeDriver driver)
        {
            bool selected = false;

            var switcher = driver.FindElementById("switcher-info");
            driver.ExecuteScript("arguments[0].click();", switcher);
            //switcher.Click();
            

            var switcherCountrySelector = driver.FindElementByClassName("switcher-shipto-c");
            while(switcherCountrySelector == null)
            {
                System.Threading.Thread.Sleep(100);
                switcherCountrySelector = driver.FindElementByClassName("switcher-shipto-c");
            }
            driver.ExecuteScript("arguments[0].click();", switcherCountrySelector);
            //switcherCountrySelector.Click();

            var countryNameSpans = driver.FindElementsByCssSelector(".list-container .country-text");
            while(countryNameSpans == null || countryNameSpans.Count == 0)
            {
                System.Threading.Thread.Sleep(100);
                countryNameSpans = driver.FindElementsByCssSelector(".list-container .country-text");
            }
            foreach (var cn in countryNameSpans)
            {
                if (cn.GetAttribute("innerText").Equals(mCountry, StringComparison.InvariantCultureIgnoreCase))
                {
                    //driver.ExecuteScript("arguments[0].scrollIntoView(true);", cn);
                    //System.Threading.Thread.Sleep(500);
                    driver.ExecuteScript("arguments[0].click();", cn);
                    //cn.Click();
                    selected = true;
                    break;
                }
            }

            if (selected)
            {
                var countryNameSaveButton = driver.FindElementByCssSelector(".switcher-common .go-contiune-btn");
                driver.ExecuteScript("arguments[0].click();", countryNameSaveButton);
                //countryNameSaveButton.Click();
            }

            return selected;
        }

        private bool SelectCurrency(ChromeDriver driver)
        {
            bool selected = false;

            var switcher = driver.FindElementById("switcher-info");
            driver.ExecuteScript("arguments[0].click();", switcher);
            //switcher.Click();
            //System.Threading.Thread.Sleep(200);

            var switcherCurrencySelector = driver.FindElementByClassName("switcher-currency-c");
            while (switcherCurrencySelector == null)
            {
                System.Threading.Thread.Sleep(100);
                switcherCurrencySelector = driver.FindElementByClassName("switcher-currency-c");
            }
            driver.ExecuteScript("arguments[0].click();", switcherCurrencySelector);
            //switcherCurrencySelector.Click();
            //System.Threading.Thread.Sleep(400);

            var currencyNameSpans = driver.FindElementsByCssSelector(".switcher-currency .notranslate a");
            while (currencyNameSpans == null || currencyNameSpans.Count == 0)
            {
                System.Threading.Thread.Sleep(100);
                currencyNameSpans = driver.FindElementsByCssSelector(".switcher-currency .notranslate a");
            }
            foreach (var cn in currencyNameSpans)
            {
                if (cn.GetAttribute("data-currency").Equals(mCurrency, StringComparison.InvariantCultureIgnoreCase))
                {
                    driver.ExecuteScript("arguments[0].click();", cn);
                    //driver.ExecuteScript("arguments[0].scrollIntoView(true);", cn);
                    //System.Threading.Thread.Sleep(500);
                    //cn.Click();
                    selected = true;
                    break;
                }
            }

            if (selected)
            {
                var countryNameSaveButton = driver.FindElementByCssSelector(".switcher-common .go-contiune-btn");
                driver.ExecuteScript("arguments[0].click();", countryNameSaveButton);
                //countryNameSaveButton.Click();
            }

            return selected;
        }

        private void Login(ChromeDriver driver)
        {
            var loginATag = driver.FindElementByCssSelector(".account-unsigned a[data-role=sign-link]");
            loginATag.Click();

            driver.SwitchTo().Frame(driver.FindElementById("alibaba-login-box"));

            var loginIdEle = driver.FindElementById("fm-login-id");
            var loginPasswordEle = driver.FindElementById("fm-login-password");
            var loginSubmitEle = driver.FindElementById("fm-login-submit");

            loginIdEle.SendKeys(mAccount);
            loginPasswordEle.SendKeys(mPassword);
            System.Threading.Thread.Sleep(1200);
            loginSubmitEle.Click();

            driver.SwitchTo().DefaultContent();
        }

        private void ClosePopup(ChromeDriver driver, bool retry)
        {
            //活动1
            //try
            //{
            //    var popup = driver.FindElementByCssSelector(".ui-newuser-layer-dialog > .ui-window-bd > .ui-window-content > a.close-layer");

            //    popup.Click();
            //    System.Threading.Thread.Sleep(2000);
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.Message + "\t" + ex.StackTrace);
            //    if(retry)
            //    {
            //        System.Threading.Thread.Sleep(20000);
            //        ClosePopup(driver, false);
            //    }
            //}

            //活动2
            try
            {
                var popup = driver.FindElementByCssSelector(".ui-window-transition > .ui-window-bd > .ui-window-content > a.close-layer");

                popup.Click();
                System.Threading.Thread.Sleep(1500);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\t" + ex.StackTrace);
                if (retry)
                {
                    System.Threading.Thread.Sleep(5000);
                    ClosePopup(driver, false);
                }
            }
        }

        private static void InitDriver(ChromeDriver driver)
        {
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 960);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(6);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(2);
        }

        public void CloseFile()
        {
            if (mCsvStreamWriter != null)
            {
                mCsvStreamWriter.Close();
            }
        }

        public void Dispose()
        {
            if(mCsvStreamWriter != null)
            {
                mCsvStreamWriter.Close();
            }
        }
    }
}
