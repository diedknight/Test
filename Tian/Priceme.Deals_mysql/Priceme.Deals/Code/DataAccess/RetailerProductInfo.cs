using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
///RetailerProductInfo 的摘要说明
/// </summary>
public class RetailerProductInfo : IComparable<RetailerProductInfo>
{
    int _retailerID;
    int _retaielrPrdocutID;
    double _price;
    int _upperHour;
    int _lowerHour;
    int _ppcmembertype;
    string _ppcLogo;
    decimal _ppcIndex;
    bool _isDropOff;
    bool _isRestricted;
    bool _isNoLink;

    public bool IsNoLink
    {
        get { return _isNoLink; }
        set { _isNoLink = value; }
    }

    public int RetailerID
    {
        get { return _retailerID; }
        set { _retailerID = value; }
    }

    public int RetaielrPrdocutID
    {
        get { return _retaielrPrdocutID; }
        set { _retaielrPrdocutID = value; }
    }

    public int UpperHour
    {
        get { return _upperHour; }
        set { _upperHour = value; }
    }

    public int LowerHour
    {
        get { return _lowerHour; }
        set { _lowerHour = value; }
    }

    public int PPCMemberType
    {
        get { return _ppcmembertype; }
        set { _ppcmembertype = value; }
    }

    public double Price
    {
        get { return _price; }
        set { _price = value; }
    }

    public string PPCLogo
    {
        get { return _ppcLogo; }
        set { _ppcLogo = value; }
    }

    public decimal PPCIndex
    {
        get { return _ppcIndex; }
        set { _ppcIndex = value; }
    }

    public bool IsDropOff
    {
        get { return _isDropOff; }
        set { _isDropOff = value; }
    }

    public bool IsRestricted
    {
        get { return _isRestricted; }
        set { _isRestricted = value; }
    }
	public RetailerProductInfo()
	{
        

        
	}

    #region IComparable<RetailerProductInfo> 成员

    public int CompareTo(RetailerProductInfo other)
    {
        return this.Price.CompareTo(other.Price);
    }

    #endregion
}
