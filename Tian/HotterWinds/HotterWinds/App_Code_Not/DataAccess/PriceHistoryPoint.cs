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
/// HistoryPoint 的摘要说明
/// </summary>
public class PriceHistoryPoint
{
    int _value;
    DateTime _date;
    int _price;

    public int Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
        }
    }

    public int Price
    {
        get
        {
            return _price;
        }
        set
        {
            _price = value;
        }
    }

    public DateTime Date
    {
        get
        {
            return _date;
        }
        set
        {
            _date = value;
        }
    }
}
