using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherPaykelTool.Model
{
    public class DateRange
    {
        private RangeType _rangeType = RangeType.Monthly;
        private DateTime _start = DateTime.MinValue;
        private DateTime _end = DateTime.MinValue;

        public DateTime Start { get { return this._start; } }
        public DateTime End { get { return this._end; } }

        public DateRange()
        {
            string[] dateRange = System.Configuration.ConfigurationManager.AppSettings["daterange"].ToString().Split('-');
            string pricetype = System.Configuration.ConfigurationManager.AppSettings["pricetype"].ToString();

            this._start = Convert.ToDateTime(dateRange[0].Trim());
            this._end = Convert.ToDateTime(dateRange[1].Trim());

            if ((this._start - this._end).TotalDays > 0)
            {
                this._start = Convert.ToDateTime(dateRange[1].Trim());
                this._end = Convert.ToDateTime(dateRange[0].Trim());
            }

            if (pricetype.ToLower().Trim() == "weekly") this._rangeType = RangeType.Weekly;
        }

        public List<Range> GetRange()
        {
            if (this._rangeType == RangeType.Monthly) return this.GetRangeByMonthly();
            else return GetRangeByWeekly();
        }

        private List<Range> GetRangeByWeekly()
        {
            List<Range> list = new List<Range>();

            int daySpan = (this._end - this._start).Days;
            DateTime tempStart = this._start;
            DateTime tempEnd = this._start.AddDays(7);
            
            for (int i = 0; i < daySpan / 7; i++)
            {
                Range range = new Range();
                range.Start = tempStart;
                range.End = tempEnd;

                list.Add(range);

                tempStart = tempEnd;
                tempEnd = tempEnd.AddDays(7);
            }

            if (daySpan % 7 > 0)
            {
                Range range = new Range();
                range.Start = tempStart;
                range.End = tempStart.AddDays(daySpan % 7);

                list.Add(range);
            }

            return list;
        }

        private List<Range> GetRangeByMonthly()
        {
            List<Range> list = new List<Range>();

            //同年月
            //if (this._end.Year == this._start.Year && this._end.Month == this._start.Month)
            //{
            //    if (this._end.Day <= 15) return list;

            //    Range range = new Range();
            //    range.Start = new DateTime(this._start.Year, this._start.Month, 15);
            //    range.End = this._end;
            //    list.Add(range);

            //    return list;
            //}

            int monthSpan = (this._end.Year - this._start.Year) * 12 + (this._end.Month - this._start.Month);

            if (monthSpan == 0)
            {
                Range range = new Range();
                range.Start = new DateTime(this._start.Year, this._start.Month, 1);
                range.End = this._end;
                list.Add(range);

                return list;
            }

            DateTime tempDate = this._start;

            while (monthSpan >= 0)
            {
                Range range = new Range();
                range.Start = new DateTime(tempDate.Year, tempDate.Month, 15);
                range.End = new DateTime(tempDate.Year, tempDate.Month, 1).AddMonths(1).AddDays(-1);

                list.Add(range);

                tempDate = tempDate.AddMonths(1);
                monthSpan--;
            }

            return list;
        }

        //class
        public class Range
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }

            public override string ToString()
            {
                return this.Start.ToString("yyyy/MM/dd") + " - " + this.End.ToString("yyyy/MM/dd");
            }
        }

        private enum RangeType
        {
            Weekly,
            Monthly
        }

    }
}
