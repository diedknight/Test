using System;
using System.Collections.Generic;
using System.Configuration;

namespace PurgeCloudflareCacheService
{
    /// <summary>
    /// 用户保存读取出来的countries节点的内容
    /// </summary>
    public class CountriesNodeInfo
    {
        public int Interval { get; private set; }
        public readonly Dictionary<int, CountryInfo> CountryInfoListDic;

        public CountriesNodeInfo(int interval, List<CountryInfo> countryInfoList)
        {
            Interval = interval;
            CountryInfoListDic = new Dictionary<int, CountryInfo>();
            foreach (CountryInfo ci in countryInfoList)
            {
                CountryInfoListDic.Add(ci.CountryId, ci);
            }
        }

        public CountryInfo GetCountryInfo(int countryId)
        {
            if(CountryInfoListDic.ContainsKey(countryId))
            {
                return CountryInfoListDic[countryId];
            }
            return null;
        }

        public override string ToString()
        {
            string str = "Interval: " + Interval;
            foreach(CountryInfo ci in CountryInfoListDic.Values)
            {
                str += Environment.NewLine + "Country Info - " + ci.ToString();
            }
            return str;
        }
    }

    public class CountryInfo
    {
        public int CountryId { get; private set; }
        public string ZoneId { get; private set; }
        public string WebSite { get; private set; }
        public string AMPSite { get; private set; }
        public bool UrlSeo { get; set; }
        public ConnectionStringSettings MyConnectionStringSettings { get; private set; }

        public CountryInfo(int countryId, string dbConnectionKey, string zoneId, string webSite, string ampSite, bool urlSeo)
        {
            CountryId = countryId;
            MyConnectionStringSettings = ConfigurationManager.ConnectionStrings[dbConnectionKey];
            ZoneId = zoneId;
            WebSite = webSite;
            AMPSite = ampSite;
            UrlSeo = urlSeo;
        }

        public override string ToString()
        {
            return "CountryId: " + CountryId + "\t ConnectionString: " + MyConnectionStringSettings.ConnectionString + "\t ZoneId: " + ZoneId + "\t WebSite: " + WebSite + "\t AMPSite: " + AMPSite + "\t UrlSeo: " + UrlSeo;
        }
    }
}