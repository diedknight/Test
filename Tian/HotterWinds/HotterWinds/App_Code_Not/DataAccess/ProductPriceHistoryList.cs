using System;
using System.Collections.Generic;
using System.Web;

public class ProductPriceHistoryList : List<ProductPriceHistory>
{
    public int FPrice { get; set; }
    public int STEP { get; set; }
    public DateTime? StartDT { get; set; }
    public bool ShowEveryDay { get; set; }

    public ProductPriceHistoryList()
    {
        FPrice = 0;
        STEP = 0;
        StartDT = null;
        ShowEveryDay = false;
    }

    public int GetHighestPrice()
    {
        int max = 0;
        foreach (ProductPriceHistory pph in this)
        {
            if (max < pph.Price)
            {
                max = pph.Price;
            }
        }

        if (max < 10)
        {
            max = 10;
        }
        else if (max <= 100)
        {
            max = max / 10 * 10 + 10;
        }
        else
        {
            max = max / 100 * 100 + 100;
        }

        return max;
    }

    public int GetLowestPrice()
    {
        int min = int.MaxValue;
        foreach (ProductPriceHistory pph in this)
        {
            if (min > pph.Price)
            {
                min = pph.Price;
            }
        }

        if (min < 10)
        {
            min = 0;
        }
        else if (min <= 100)
        {
            min = min / 10 * 10;
        }
        else
        {
            min = min / 100 * 100;
        }

        return min;
    }

    public int GetPrice(DateTime dt)
    {
        foreach (ProductPriceHistory pph in this)
        {
            if (pph.PriceDate.Year == dt.Year && pph.PriceDate.Month == dt.Month && pph.PriceDate.Day == dt.Day)
            {
                return pph.Price;
            }
        }
        return -1;
    }

    public int GetPrice(DateTime startDT, DateTime endDT)
    {
        if (endDT < DateTime.Now)
        {
            int min = int.MaxValue;
            foreach (ProductPriceHistory pph in this)
            {
                if (pph.PriceDate >= startDT && pph.PriceDate < endDT)
                {
                    if (min > pph.Price)
                    {
                        min = pph.Price;
                    }
                }
            }
            if (min == int.MaxValue)
            {
                min = 0;
            }
            return min;
        }
        else
        {
            return this[this.Count - 1].Price;
        }
    }

    int GetStepDays()
    {
        int step = 0;

        DateTime highestDate = DateTime.Now;
        DateTime lowestDate = GetStartDate();
        TimeSpan timeSpan = highestDate - lowestDate;
        if (timeSpan.Days > 180)
        {
            step = 7;
        }
        else if (timeSpan.Days <= 180 && timeSpan.Days > 31)
        {
            step = 3;
        }
        else
        {
            step = 1;
        }

        return step;
    }

    public DateTime GetStartDate()
    {
        return new DateTime(this[0].PriceDate.Year, this[0].PriceDate.Month, this[0].PriceDate.Day);
    }

    string GetValueString()
    {
        string dataValue = "";

        DateTime startDT = GetStartDate();
        DateTime nowDate = DateTime.Now;
        TimeSpan timeSpan = nowDate - startDT;
        if (timeSpan.Days < STEP)
        {
            return dataValue;
        }
        DateTime endDT;
        int count = 0;
        int i = 1;
        while (startDT < nowDate)
        {
            if (startDT.Day == 1)
            {
                i = 1;
            }
            if (startDT.Day > 30 - STEP)
            {
                endDT = startDT.AddMonths(1);
                endDT = new DateTime(endDT.Year, endDT.Month, 1);
            }
            else
            {
                endDT = new DateTime(startDT.Year, startDT.Month, STEP * i);
            }
            int c = GetPrice(startDT, endDT);
            if (c == 0)
            {
                c = count;
            }
            count = c;
            dataValue += c + ",";
            startDT = endDT;
            i++;
        }

        return dataValue.TrimEnd(',');
    }

    string GetXaxisLabel()
    {
        string xLabel = string.Empty;
        DateTime startDT = GetStartDate();
        if (startDT.Day > 15)
        {
            startDT = startDT.AddMonths(1);
            startDT = new DateTime(startDT.Year, startDT.Month, 1);
        }
        else
        {
            startDT = new DateTime(startDT.Year, startDT.Month, 1);
        }
        xLabel = startDT.ToString("MMM") + "|";
        DateTime nowDate = DateTime.Now;
        while (startDT < nowDate)
        {
            if (startDT.Day < 16)
            {
                startDT = new DateTime(startDT.Year, startDT.Month, 16);
                xLabel += "||";
            }
            else
            {
                startDT = startDT.AddMonths(1);
                startDT = new DateTime(startDT.Year, startDT.Month, 1);
                if (startDT < nowDate)
                {
                    xLabel += "|" + startDT.ToString("MMM") + "|";
                }
                else
                {
                    xLabel += "|";
                }
            }
        }
        int days = (startDT - nowDate).Days;
        if (xLabel.Length > 2 && days >= 7)
        {
            xLabel = xLabel.Substring(0, xLabel.Length - 1);
        }
        return xLabel;
    }

    string GetYaxisLabel(out int fixStepPrice)
    {
        int lowestPrice = GetLowestPrice();
        int highestPrice = GetHighestPrice();
        int step = (highestPrice - lowestPrice) / 5;
        string yLabel = string.Empty;
        if (lowestPrice - step > 5)
        {
            fixStepPrice = step;
            yLabel = Resources.Resource.TextString_PriceSymbol + (lowestPrice - step) + "|";
        }
        else
        {
            fixStepPrice = 0;
        }
        
        while (lowestPrice <= highestPrice)
        {
            yLabel += Resources.Resource.TextString_PriceSymbol + lowestPrice + "|";
            lowestPrice += step;
        }
        return yLabel.TrimEnd('|');
    }

    public string GetGoogleChartString()
    {
        if (this.Count < 1 || GetStepDays() == 0)
        {
            return "http://chart.apis.google.com/chart?cht=lc&chxt=x,y&chg=100,10,4,2&chd=t:0&chs=600x250&chtt=No+price+history";
        }

        string url = "http://chart.apis.google.com/chart?cht=lc&chxt=x,y&chg=100,10,4,2&chs=600x250&chd=t:{0}&chds={1},{2}&chxl=0:|{3}1:|{4}";
        int fixStepPrice = 0;
        string yLabel = GetYaxisLabel(out fixStepPrice);
        string lowString = (GetLowestPrice() - fixStepPrice).ToString();
        string highString = GetHighestPrice().ToString();
        string xLabel = "";
        string dataValue = "";

        if (IsGetDayChart())
        {
            xLabel = GetDaysXaxisLabel();
            dataValue = GetDaysValueString();
        }
        else
        {
            xLabel = GetXaxisLabel();
            dataValue = GetValueString();
        }
        
        url = string.Format(url, dataValue, lowString, highString, xLabel, yLabel);
        return url;
    }

    private bool IsGetDayChart()
    {
        DateTime dt = GetStartDate();
        DateTime newDT = DateTime.Now;
        TimeSpan ts = newDT - dt;
        if (ts.Days < 92)
        {
            return true;
        }
        return false;
    }

    private string GetDaysXaxisLabel()
    {
        string xLabel = string.Empty;
        DateTime startDT = GetStartDate();
        DateTime nowDate = DateTime.Now;
        while (startDT < nowDate)
        {
            if (startDT.Day != 1 && startDT.Day != 15)
            {
                
                xLabel += "||";
            }
            else if (startDT.Day == 15)
            {
                xLabel += "|" + startDT.ToString("MMM") + "%2015|";
            }
            else
            {
                xLabel += "|" + startDT.ToString("MMM") + "%201||";
            }
            startDT = startDT.AddDays(1);
        }
        return xLabel;
    }

    private string GetDaysValueString()
    {
        string dataValue = "";

        DateTime startDT = GetStartDate();
        DateTime nowDate = DateTime.Now;
        int _savePrice = 0;
        int i = 1;
        while (startDT < nowDate)
        {
            int price = GetPrice(startDT);
            if (price < 0)
            {
                price = _savePrice;
            }
            _savePrice = price;
            dataValue += price + ",";
            startDT = startDT.AddDays(1);
            i++;
        }

        return dataValue.TrimEnd(',');
    }

    public string GetGoogleChartDataJsonString()
    {
        if (this.Count == 0) return "";

        int step = 0;
        if (STEP != 0)
        {
            step = STEP;
        }
        else
        {
            step = GetStepDays();
        }
        string dataJson = "[";

        DateTime startDT;
        if (StartDT != null)
        {
            startDT = StartDT.Value;
        }
        else
        {
            startDT = GetStartDate();
        }

        DateTime nowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        TimeSpan timeSpan = nowDate - startDT;
        if (timeSpan.Days < step)
        {
            int prc = GetPrice(startDT, nowDate);
            return dataJson + "{'AR1' : '" + startDT.ToString("MMM") + "', 'AR2' : " + prc + ", 'AR3' : '" + startDT.ToString("MMM dd") + " : " + Resources.Resource.TextString_PriceSymbol + prc + "'}," + "{'AR1' : '', 'AR2' : " + prc + ", 'AR3' : '" + startDT.ToString("MMM dd") + " : " + Resources.Resource.TextString_PriceSymbol + prc + "'}]";
        }
        DateTime endDT;
        int count = FPrice;
        if (step == 14)
        {
            if (startDT.Day > 14)
            {
                startDT = new DateTime(startDT.Year, startDT.Month, 14);
            }
            else
            {
                startDT = new DateTime(startDT.Year, startDT.Month, 1);
            }
        }

        StartDT = startDT;

        while (startDT < nowDate)
        {
            TimeSpan ts = nowDate - startDT;
            if (ts.Days < 0) break;
            if (ts.Days < step)
            {
                endDT = nowDate;
            }
            else
            {
                endDT = startDT.AddDays(step);
            }

            DateTime displayDT = startDT;
            int currentMonth = displayDT.Month;
            if (step != 1 && displayDT < nowDate)
            {
                displayDT = displayDT.AddDays(step);
                if (currentMonth != displayDT.Month && displayDT < nowDate && displayDT.Day != 1)
                {
                    displayDT = new DateTime(displayDT.Year, displayDT.Month, 1);
                    endDT = displayDT;
                }
            }

            int c = GetPrice(startDT, endDT);
            if (c == 0)
            {
                c = count;
            }
            count = c;

            string dateString = "";
            if (ShowEveryDay)
            {
                dateString = startDT.ToString("MMM dd");
            }
            else if (startDT.Day == 1)
            {
                dateString = startDT.ToString("MMM");
            }

            if (count == 0)
            {
                dataJson += "{'AR1' : '" + dateString + "', 'AR2' : 0, 'AR3' : 'No Record.'},";
            }
            else
            {
                dataJson += "{'AR1' : '" + dateString + "', 'AR2' : " + c + ", 'AR3' : '" + displayDT.ToString("MMM dd") + " : " + Resources.Resource.TextString_PriceSymbol + c + "'},";
            }
            startDT = endDT;
        }

        STEP = step;
        return dataJson.TrimEnd(',') + "]";
    }
}

public class ProductPriceHistory
{
    public string proDate { get; set; }
    public int month { get; set; }
    DateTime _dt;
    int _price;

    public ProductPriceHistory() { }

    public ProductPriceHistory(DateTime dt, int price)
    {
        this._dt = dt;
        this._price = price;
    }

    public DateTime PriceDate
    {
        get { return _dt; }
        set { _dt = value; }
    }

    public int Price
    {
        get { return _price; }
        set { _price = value; }
    }
}