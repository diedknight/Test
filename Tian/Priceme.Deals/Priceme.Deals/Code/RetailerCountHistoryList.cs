using System;
using System.Collections.Generic;
using System.Web;

namespace PriceMe.Data
{
    /// <summary>
    ///RetailerCountHistoryList 的摘要说明
    /// </summary>
    public class RetailerCountHistoryList : List<RetailerCountHistory>
    {
        public int FCount { get; set; }
        public int STEP { get; set; }
        public DateTime? StartDT { get; set; }
        public bool ShowEveryDay { get; set; }

        public RetailerCountHistoryList()
        {
            FCount = 0;
            STEP = 0;
            StartDT = null;
            ShowEveryDay = false;
        }

        public DateTime GetStartDate()
        {
            return this[0].Date;
        }

        public int GetCount(DateTime _sd, DateTime _ed)
        {
            if (_ed < DateTime.Now)
            {
                int count = 0;
                foreach (RetailerCountHistory pch in this)
                {
                    if (pch.Date >= _sd && pch.Date < _ed)
                    {
                        if (pch.Count > count)
                        {
                            count = pch.Count;
                        }
                    }
                }
                return count;
            }
            else
            {
                return this[this.Count - 1].Count;
            }
        }

        public int GetHighestCount()
        {
            int count = 0;
            foreach (RetailerCountHistory pch in this)
            {
                if (pch.Count > count)
                {
                    count = pch.Count;
                }
            }
            if (count % 5 != 0)
            {
                count = count / 5 * 5 + 5;
            }
            return count;
        }

        public int GetLowestCount()
        {
            return 0;
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
            int count = -1;
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
                int c = GetCount(startDT, endDT);
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

        string GetYaxisLabel()
        {
            int lowestPrice = GetLowestCount();
            int highestPrice = GetHighestCount();
            int step = (highestPrice - lowestPrice) / 5;
            string yLabel = string.Empty;
            while (lowestPrice <= highestPrice)
            {
                yLabel += lowestPrice + "|";
                lowestPrice += step;
            }
            return yLabel.TrimEnd('|');
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

        public string GetGoogleChartString()
        {
            if (this.Count == 0) return "";

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

            if (this.Count < 1 || string.IsNullOrEmpty(dataValue))
            {
                return "http://chart.apis.google.com/chart?cht=lc&chxt=x,y&chg=20,25,1,0&chd=t:0&chs=590x250&chtt=No+retailer+count+history";
            }

            string url = "http://chart.apis.google.com/chart?cht=lc&chxt=x,y&chg=100,10,4,2&chs=590x250&chco=00ff00&chd=t:{0}&chds={1},{2}&chxl=0:|{3}1:|{4}";
            
            string yLabel = GetYaxisLabel();
            string lowString = GetLowestCount().ToString();
            string highString = GetHighestCount().ToString();
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
                    xLabel += "|" + startDT.ToString("MMM") + "%201|";
                }
                startDT = startDT.AddDays(1);
            }
            return xLabel;
        }

        public int GetCount(DateTime dt)
        {
            foreach (RetailerCountHistory pch in this)
            {
                if (pch.Date.Year == dt.Year && pch.Date.Month == dt.Month && pch.Date.Day == dt.Day)
                {
                    return pch.Count;
                }
            }
            return -1;
        }

        private string GetDaysValueString()
        {
            string dataValue = "";

            DateTime startDT = GetStartDate();
            DateTime nowDate = DateTime.Now;
            int _saveCount = 0;
            int i = 1;
            while (startDT < nowDate)
            {
                int count = GetCount(startDT);
                if (count < 0)
                {
                    count = _saveCount;
                }
                _saveCount = count;
                dataValue += count + ",";
                startDT = startDT.AddDays(1);
                i++;
            }

            return dataValue.TrimEnd(',');
        }

        int GetStepDays()
        {
            int step = 0;

            DateTime highestDate = DateTime.Now;
            DateTime lowestDate = GetStartDate();
            TimeSpan timeSpan = highestDate - lowestDate;
            if (timeSpan.Days > 180)
            {
                step = 14;
            }
            else if (timeSpan.Days <= 180 && timeSpan.Days > 45)
            {
                step = 7;
            }
            else
            {
                step = 1;
            }

            return step;
        }

        public string GetGoogleChartDataJson()
        {
            if (this.Count == 0) return "";
            int step = GetStepDays();
            string dataJson = "[";

            DateTime startDT = GetStartDate();
            DateTime nowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            TimeSpan timeSpan = nowDate - startDT;
            if (timeSpan.Days < step)
            {
                int rc = GetCount(startDT, nowDate);
                string retailerString = Resources.Resource.TextString_Retailer;
                if (PriceMeCommon.ConfigAppString.CountryID != 51)
                {
                    if (rc > 1)
                    {
                        retailerString += "s";
                    }
                }
                return dataJson + "['" + startDT.ToString("MMM") + "', '" + rc + ", '" + startDT.ToString("MMM dd") + " : " + rc + "']," + "['', '" + rc + ", '" + startDT.ToString("MMM dd") + " : " + rc + " " + retailerString + "']]";
            }
            DateTime endDT;
            int count = 0;
            if (step != 1)
            {
                if (startDT.Day > 14 )
                {
                    startDT = new DateTime(startDT.Year, startDT.Month, 14);
                }
                else
                {
                    startDT = new DateTime(startDT.Year, startDT.Month, 1);
                }
            }
            
            while (startDT < nowDate)
            {
                TimeSpan ts = nowDate - startDT;
                if (ts.Days < 1) break;
                if (ts.Days < step)
                {
                    endDT = nowDate;
                }
                else
                {
                    endDT = startDT.AddDays(step);
                }

                if (step != 1 && endDT.Day > 27)
                {
                    DateTime dt = new DateTime(endDT.Year, endDT.Month, 1);
                    endDT = dt.AddMonths(1);
                }

                int c = GetCount(startDT, endDT);
                if (c == 0)
                {
                    c = count;
                }
                count = c;

                string dateString = "";
                if(ShowEveryDay)
                {
                    dateString = startDT.ToString("MMM dd");
                }
                else if (startDT.Day == 1)
                {
                    dateString = startDT.ToString("MMM");  
                }

                if (count == 0)
                {
                    dataJson += "['', , ''],";
                }
                else
                {
                    string retailerString = Resources.Resource.TextString_Retailer;
                    if (PriceMeCommon.ConfigAppString.CountryID != 51)
                    {
                        if (count > 1)
                        {
                            retailerString += "s";
                        }
                    }

                    dataJson += "['" + dateString + "', " + c + ", '" + endDT.ToString("MMM dd") + " : " + c + " " + retailerString + "'],";
                }
                startDT = endDT;

            }

            return dataJson.TrimEnd(',') + "]";
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
                int rc = GetCount(startDT, nowDate);

                string retailerString = Resources.Resource.TextString_Retailer;
                if (PriceMeCommon.ConfigAppString.CountryID != 51)
                {
                    if (rc > 1)
                    {
                        retailerString += "s";
                    }
                }

                return dataJson + "{'AR1' : '" + startDT.ToString("MMM") + "', 'AR2' : " + rc + ", 'AR3' : '" + startDT.ToString("MMM dd") + " : " + rc + " " + retailerString + "'}," + "{'AR1' : '', 'AR2' : " + rc + ", 'AR3' : '" + startDT.ToString("MMM dd") + " : " + rc + " " + retailerString + "'}]";
            }
            DateTime endDT;
            int count = FCount;
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

            while (startDT < nowDate)
            {
                TimeSpan ts = nowDate - startDT;
                if (ts.Days < 1) break;
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

                int c = GetCount(new DateTime(startDT.Year, startDT.Month, startDT.Day, 23, 59, 59), new DateTime(endDT.Year, endDT.Month, endDT.Day, 23, 59, 59));
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
                    string retailerString = Resources.Resource.TextString_Retailer;
                    if (PriceMeCommon.ConfigAppString.CountryID != 51)
                    {
                        if (count > 1)
                        {
                            retailerString += "s";
                        }
                    }

                    dataJson += "{'AR1' : '" + dateString + "', 'AR2' : " + c + ", 'AR3' : '" + endDT.ToString("MMM dd") + " : " + c + " " + retailerString + "'},";
                }
                startDT = endDT;

            }

            return dataJson.TrimEnd(',') + "]";
        }
    }

    public class RetailerCountHistory
    {
        int _count;
        DateTime _date;

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }
    }
}