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
/// Summary description for NewProductCollection
/// </summary>
public class NewProductCollection : System.Collections.Generic.List<NewProduct>
{

}

public class NewProduct
{
    string productName;
    string guid;
    int productid;
    DateTime createdOn;

    public int Productid
    {
        get { return productid; }
        set { productid = value; }
    }

    public string ProductName
    {
        get { return productName; }
        set { productName = value; }
    }

    public string Guid
    {
        get { return guid; }
        set { guid = value; }
    }

    public DateTime CreatedOn
    {
        get { return createdOn; }
        set { createdOn = value; }
    }
}
