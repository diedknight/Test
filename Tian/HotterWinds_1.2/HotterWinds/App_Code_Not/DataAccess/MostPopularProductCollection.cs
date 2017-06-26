using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for MostPopularProductCollection
/// </summary>
public class MostPopularProductCollection : System.Collections.Generic.List<MPProduct>
{

}

public class MPProduct
{
    string guid;
    decimal bestPrice;
    string productName;
    string imageFile;
    int productID;

    public int ProductID
    {
        get { return productID; }
        set { productID = value; }
    }

    public string Guid
    {
        get { return guid; }
        set { guid = value; }
    }
    
    public decimal BestPrice
    {
        get { return bestPrice; }
        set { bestPrice = value; }
    }
    
    public string ProductName
    {
        get { return productName; }
        set { productName = value; }
    }
    
    public string ImageFile
    {
        get { return imageFile; }
        set { imageFile = value; }
    }
}
