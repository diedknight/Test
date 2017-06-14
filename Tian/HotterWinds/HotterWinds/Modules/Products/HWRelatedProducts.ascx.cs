using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds.Modules.Products
{
    public partial class HWRelatedProducts : System.Web.UI.UserControl
    {
        public List<PriceMeCommon.Data.ProductCatalog> ProductCatalogs;
        public int ProductID;
        public string ProductName;
        public string CategoryName;

        protected List<ProductCatalog> filtedRelatedProducts = new List<ProductCatalog>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ProductCatalogs != null)
            {
                ProductCatalogs.ForEach(pc =>
                {
                    if (int.Parse(pc.ProductID) != ProductID && pc.ProductName.Trim() != "")
                    {
                        if (filtedRelatedProducts.Count < 6)
                        {
                            filtedRelatedProducts.Add(pc);
                        }
                    }
                });
            }

        }

        protected string GetPriceRangeString(string min, string max)
        {
            double minPrice = double.Parse(min, System.Globalization.NumberStyles.Any, PriceMeCommon.PriceMeStatic.Provider);
            string minString = PriceMe.Utility.FormatPrice(minPrice);

            return minString;
        }

        protected string GetTitle(string cate, string product)
        {
            if (PriceMe.WebConfig.CountryId == 56)
            {
                return string.Format("{0} {1} {2}", cate, product, Resources.Resource.Product_SimilarLinks_Related);
            }
            else
            {
                return string.Format("{0} {1} {2}", Resources.Resource.Product_SimilarLinks_Related, product, cate);
            }
        }

        protected string GetImage(string defaultImage)
        {
            return PriceMe.Utility.GetImage(defaultImage, "_ms");
        }

        protected string GetLinkUrl(int pId, int rId, int rpId, int cid)
        {             
            string retailerProductURL = PriceMe.Utility.GetRootUrl("/ResponseRedirect.aspx?pid=" + pId + "&rid=" + rId + "&rpid=" + rpId + "&countryID=" + PriceMe.WebConfig.CountryId + "&cid=" + cid + "&aid=40&t=" + "HW", PriceMe.WebConfig.CountryId);
            string uuid = Guid.NewGuid().ToString();
            retailerProductURL += "&uuid=" + uuid;

            return retailerProductURL;
        }

    }
}