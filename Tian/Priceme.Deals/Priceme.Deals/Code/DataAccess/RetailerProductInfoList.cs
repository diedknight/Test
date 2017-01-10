using System;
using System.Collections.Generic;
using System.Web;


public class RetailerProductInfoList : List<RetailerProductInfo>
{
    RetailerProductInfo theBestPriceRetaielrProductInfo = null;
    RetailerProductInfo theMostExpensiveRetailerProductInfo = null;
    RetailerProductInfo theFeaturedRetailerProductInfo = null;
    List<RetailerProductInfo> results = new List<RetailerProductInfo>();
    List<RetailerProductInfo> restrictedRetailerProductInfo = new List<RetailerProductInfo>();
    List<RetailerProductInfo> allRetailerProductInfo = new List<RetailerProductInfo>();

    bool onlyOneRestrictedRetailerProduct = false;

    public bool OnlyOneRestrictedRetailerProduct
    {
        get { return onlyOneRestrictedRetailerProduct; }
    }

    public RetailerProductInfoList()
	{
		
	}

    public void InitForConsumer()
    {
        if (this.Count > 0)
        {
            theBestPriceRetaielrProductInfo = this[0];
            theMostExpensiveRetailerProductInfo = this[this.Count - 1];

            foreach (RetailerProductInfo rpi in results)
            {
                if (rpi.PPCMemberType == 2)
                {
                    theFeaturedRetailerProductInfo = rpi;
                    break;
                }
            }

            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].PPCMemberType == 2)
                {
                    if (results[i].PPCIndex > theFeaturedRetailerProductInfo.PPCIndex)
                    {
                        theFeaturedRetailerProductInfo = results[i];
                    }
                }
            }
        }
    }

    void Init()
    {
        if (this.Count == 0)
        {
            return;
        }
        int hour = DateTime.Now.Hour;
        foreach (RetailerProductInfo rpi in this)
        {
            if (rpi.IsDropOff || rpi.PPCMemberType == 2)
            {
                if (rpi.UpperHour <= hour && rpi.LowerHour > hour)
                {
                    results.Add(rpi);
                }
                else
                {
                    restrictedRetailerProductInfo.Add(rpi);
                }
            }
            else
            {
                restrictedRetailerProductInfo.Add(rpi);
            }
            allRetailerProductInfo.Add(rpi);
        }

        theBestPriceRetaielrProductInfo = allRetailerProductInfo[0];
        theMostExpensiveRetailerProductInfo = allRetailerProductInfo[allRetailerProductInfo.Count - 1];

        if (results.Count == 0 || restrictedRetailerProductInfo.Count == 1)
        {
            this.onlyOneRestrictedRetailerProduct = true;
            //if (restrictedRetailerProductInfo.Count > 0)
            //{
            //    theBestPriceRetaielrProductInfo = restrictedRetailerProductInfo[0];

            //    for (int i = 1; i < restrictedRetailerProductInfo.Count; i++)
            //    {
            //        if (restrictedRetailerProductInfo[i].Price < theBestPriceRetaielrProductInfo.Price)
            //        {
            //            theBestPriceRetaielrProductInfo = restrictedRetailerProductInfo[i];
            //        }
            //    }
            //    theMostExpensiveRetailerProductInfo = theBestPriceRetaielrProductInfo;
            //    results.Add(theBestPriceRetaielrProductInfo);
            //}
        }
    }

    public void InitForProductPage()
    {
        Init();
    }

    public void InitForCatalogPage()
    {
        Init();
        
        if (results.Count > 0)
        {
            foreach (RetailerProductInfo rpi in results)
            {
                if (rpi.PPCMemberType == 2)
                {
                    theFeaturedRetailerProductInfo = rpi;
                    break;
                }
            }

            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].PPCMemberType == 2)
                {
                    if (results[i].PPCIndex > theFeaturedRetailerProductInfo.PPCIndex)
                    {
                        theFeaturedRetailerProductInfo = results[i];
                    }
                }
            }
        }
    }

    public int GetPriceCount()
    {
        return this.Count;
    }

    public string GetPPCLogoPath()
    {
        if (theFeaturedRetailerProductInfo != null)
        {
            return theFeaturedRetailerProductInfo.PPCLogo;
        }

        return "";
    }

    public int GetPPCRetaielrID()
    {
        if (theFeaturedRetailerProductInfo != null)
        {
            return theFeaturedRetailerProductInfo.RetailerID;
        }

        return 0;
    }

    public string GetPPCLogoRPID()
    {
        if (theFeaturedRetailerProductInfo != null)
        {
            return theFeaturedRetailerProductInfo.RetaielrPrdocutID.ToString();
        }

        return "";
    }

    public string GetBestPriceString()
    {
        if (theBestPriceRetaielrProductInfo != null)
        {
            if (PriceMeCommon.ConfigAppString.CountryID == 56)
                return Resources.Resource.TextString_PriceSymbol + PriceMe.Utility.FormatPrice(theBestPriceRetaielrProductInfo.Price);
            else
                return PriceMe.Utility.FormatPrice(theBestPriceRetaielrProductInfo.Price);
        }
        return "0";
    }

    public string GetBestPriceRetaielrProductID()
    {
        if (theBestPriceRetaielrProductInfo != null)
        {
            return theBestPriceRetaielrProductInfo.RetaielrPrdocutID.ToString();
        }
        return "0";
    }

    public string GetBestPriceRetaielrID()
    {
        if (theBestPriceRetaielrProductInfo != null)
        {
            return theBestPriceRetaielrProductInfo.RetailerID.ToString();
        }
        return "0";
    }

    public string GetMaxPriceString()
    {
        if (theMostExpensiveRetailerProductInfo != null)
        {
            if (PriceMeCommon.ConfigAppString.CountryID == 56)
                return Resources.Resource.TextString_PriceSymbol + PriceMe.Utility.FormatPrice(theMostExpensiveRetailerProductInfo.Price);
            else
                return PriceMe.Utility.FormatPrice(theMostExpensiveRetailerProductInfo.Price);
        }
        return "0";
    }

    public List<RetailerProductInfo> GetResults()
    {
        return this.results;
    }
}