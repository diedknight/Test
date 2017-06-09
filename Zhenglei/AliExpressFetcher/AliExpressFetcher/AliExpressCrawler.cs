using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AliExpressFetcher
{
    public class AliExpressCrawler
    {
        static Regex SkuRegex_Static;
        static Regex PriceRegex_Static;
        static Regex UnitRegex_Static;

        string mAccount;
        string mPassword;
        string mCountry;
        string mCurrency;
        string mChromeWebDriverDir;
        List<string> mAddedSkuList;

        static AliExpressCrawler()
        {
            SkuRegex_Static = new Regex("/(?<sku>[\\w]+)\\.html", RegexOptions.IgnoreCase | RegexOptions.Singleline| RegexOptions.Compiled);
            PriceRegex_Static = new Regex("(?<price>[\\d]+(\\.[\\d]{0,2})?)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            UnitRegex_Static = new Regex("(?<l>[\\d]+(\\.[\\d]{0,2})?)(?<unit>\\w+)\\s?x\\s?(?<w>[\\d]+(\\.[\\d]{0,2})?)\\w+\\s?x\\s?(?<h>[\\d]+(\\.[\\d]{0,2})?)\\w+", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        }

        public AliExpressCrawler(string account, string password, string country, string currency, string chromeWebDriverDir)
        {
            mAccount = account;
            mPassword = password;
            mCountry = country;
            mCurrency = currency;
            mChromeWebDriverDir = chromeWebDriverDir;
        }

        public List<ProductInfo> CrawlProducts(List<CategoryInfo> categoryInfoList)
        {
            mAddedSkuList = new List<string>();

            List<ProductInfo> list = new List<ProductInfo>();

            ChromeOptions chromeOptions = new ChromeOptions();

            using (ChromeDriver driver = new ChromeDriver(mChromeWebDriverDir, new ChromeOptions()))
            {
                InitDriver(driver);

                driver.Navigate().GoToUrl("https://www.aliexpress.com/");

                Login(driver);

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
                driver.Navigate().GoToUrl(nextPageUrl);

                string currentCurrency = driver.FindElementByCssSelector(".currency").GetAttribute("innerText").Trim();
                if(!currentCurrency.Equals(mCurrency, StringComparison.InvariantCultureIgnoreCase))
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
                foreach(var pATag in productsTag)
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
            }
            while (!string.IsNullOrEmpty(nextPageUrl));

            return list;
        }

        private ProductInfo GetProductInfo(string productLink, string categoryName, ChromeDriver driver)
        {
            ProductInfo pi = null;

            try
            {
                string sku = SkuRegex_Static.Match(productLink).Groups["sku"].Value;

                if(mAddedSkuList.Contains(sku))
                {
                    return null;
                }
                else
                {
                    mAddedSkuList.Add(sku);
                }

                driver.Navigate().GoToUrl(productLink);
                
                var footerEle = driver.FindElementByCssSelector(".site-footer");
                driver.ExecuteScript("arguments[0].scrollIntoView(true);", footerEle);
                System.Threading.Thread.Sleep(1000);
                var headerEle = driver.FindElementByCssSelector(".top-lighthouse");
                driver.ExecuteScript("arguments[0].scrollIntoView(true);", headerEle);
                
                string productName = driver.FindElementByCssSelector("h1.product-name").Text.Trim();
                string productPriceCurrency = "";
                string productPriceStr = "";

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

                decimal productPrice = decimal.Parse(productPriceStr);
                string productPriceUnit = driver.FindElementByCssSelector(".p-price-content .p-unit").Text.Trim();

                string oldProductPriceCurrency = "";
                decimal oldProductPrice = 0;
                if (driver.FindElementsByCssSelector(".p-del-price-detail").Count > 0)
                {
                    oldProductPriceCurrency = driver.FindElementByCssSelector(".p-del-price-detail .p-symbol").Text.Trim();
                    string oldProductPriceStr = driver.FindElementByCssSelector(".p-del-price-detail .p-price").Text.Trim();
                    oldProductPrice = decimal.Parse(oldProductPriceStr);
                }

                string bulkPriceStr = "";

                var bulkPriceNodes = driver.FindElementsByCssSelector(".bulk-price-tips.ui-balloon.ui-balloon-tl");
                if(bulkPriceNodes.Count > 0)
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
                    if(pgTitle.Equals("Unit Type", StringComparison.InvariantCultureIgnoreCase))
                    {
                        unitType = pgValue;
                    }
                    else if (pgTitle.Equals("Package Size", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string unitString = pgValue.Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                        Match match = UnitRegex_Static.Match(unitString);
                        if(match.Success)
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
                while(fullDescription.Length < 80 && reTryCount > 0)
                {
                    System.Threading.Thread.Sleep(300);
                    fullDescription = descEle.GetAttribute("innerHTML").Trim();
                    reTryCount--;
                }

                List<string> images = new List<string>();
                var imageTags = driver.FindElementsByCssSelector("#j-image-thumb-list img");
                foreach(var img in imageTags)
                {
                    string imgUrl = img.GetAttribute("src").Replace("_50x50.jpg", "");
                    images.Add(imgUrl);
                }

                Dictionary<string, string> attrs = new Dictionary<string, string>();
                var attrTags = driver.FindElementsByCssSelector(".product-property-list.util-clearfix .property-item");
                foreach (var attrTag in attrTags)
                {
                    string key = attrTag.FindElement(By.ClassName("propery-title")).GetAttribute("innerText").Trim().TrimEnd(':');
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return pi;
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
            switcher.Click();
            System.Threading.Thread.Sleep(500);

            var switcherCountrySelector = driver.FindElementByClassName("switcher-shipto-c");
            switcherCountrySelector.Click();
            System.Threading.Thread.Sleep(1000);

            var countryNameSpans = driver.FindElementsByCssSelector(".list-container .country-text");
            foreach (var cn in countryNameSpans)
            {
                if (cn.GetAttribute("innerText").Equals(mCountry, StringComparison.InvariantCultureIgnoreCase))
                {
                    driver.ExecuteScript("arguments[0].scrollIntoView(true);", cn);
                    System.Threading.Thread.Sleep(100);
                    cn.Click();
                    selected = true;
                    break;
                }
            }

            if (selected)
            {
                var countryNameSaveButton = driver.FindElementByCssSelector(".switcher-common .go-contiune-btn");
                countryNameSaveButton.Click();
            }

            return selected;
        }

        private bool SelectCurrency(ChromeDriver driver)
        {
            bool selected = false;

            var switcher = driver.FindElementById("switcher-info");
            switcher.Click();
            System.Threading.Thread.Sleep(500);

            var switcherCurrencySelector = driver.FindElementByClassName("switcher-currency-c");
            switcherCurrencySelector.Click();
            System.Threading.Thread.Sleep(800);

            var currencyNameSpans = driver.FindElementsByCssSelector(".switcher-currency .notranslate a");
            foreach (var cn in currencyNameSpans)
            {
                if (cn.GetAttribute("data-currency").Equals(mCurrency, StringComparison.InvariantCultureIgnoreCase))
                {
                    driver.ExecuteScript("arguments[0].scrollIntoView(true);", cn);
                    System.Threading.Thread.Sleep(100);
                    cn.Click();
                    selected = true;
                    break;
                }
            }

            if (selected)
            {
                var countryNameSaveButton = driver.FindElementByCssSelector(".switcher-common .go-contiune-btn");
                countryNameSaveButton.Click();
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
            loginSubmitEle.Click();

            driver.SwitchTo().DefaultContent();
        }

        private static void InitDriver(ChromeDriver driver)
        {
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 960);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(6);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(2);
        }
    }
}
