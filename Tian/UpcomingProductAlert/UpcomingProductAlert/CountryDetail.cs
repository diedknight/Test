using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpcomingProductAlert
{
    public class CountryDetail
    {
        public int CountryId { get; private set; }
        public string CountryName { get; private set; }
        public string PriceMeHost { get; private set; }

        private CountryDetail()
        { }

        private static List<CountryDetail> _list = new List<CountryDetail>();

        static CountryDetail()
        {
            string countryDetailStr = System.Configuration.ConfigurationManager.AppSettings["CountryDetail"];

            countryDetailStr.Split(';').ToList().ForEach(item => {
                var arr = item.Split(',');

                var detail = new CountryDetail();
                detail.CountryId = Convert.ToInt32(arr[0].Trim());
                detail.CountryName = arr[1].Trim();
                detail.PriceMeHost = arr[2].Trim();

                _list.Add(detail);
            });
        }

        public static CountryDetail GetCountryDetail(int countryId)
        {
            return _list.SingleOrDefault(item => item.CountryId == countryId);
        }

        public static CountryDetail GetCountryDetail(string countryName)
        {
            return _list.SingleOrDefault(item => item.CountryName == countryName);
        }

    }
}
