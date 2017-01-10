using System;
using System.Collections.Generic;
using System.Web;

public class ManufacturerCache : System.IComparable<ManufacturerCache> , System.ICloneable
{
    int manufacturerID;
    int productCount;
    string manufacturerName;
    string url;

    public int ManufacturerID
    {
        get { return manufacturerID; }
        set { manufacturerID = value; }
    }

    public int ProductCount
    {
        get { return productCount; }
        set { productCount = value; }
    }

    public string ManufacturerName
    {
        get { return manufacturerName; }
        set { manufacturerName = value; }
    }

    public string Url
    {
        get { return url; }
        set { url = value; }
    }

    #region IComparable<ManufacturerCache> 成员

    public int CompareTo(ManufacturerCache other)
    {
        return this.manufacturerName.CompareTo(other.manufacturerName);
    }

    #endregion

    #region ICloneable 成员

    public object Clone()
    {
        ManufacturerCache manufacturerCache = new ManufacturerCache();
        manufacturerCache.ManufacturerID = this.manufacturerID;
        manufacturerCache.ProductCount = this.productCount;
        manufacturerCache.ManufacturerName = this.manufacturerName;
        manufacturerCache.Url = this.url;
        return manufacturerCache;
    }

    #endregion
}