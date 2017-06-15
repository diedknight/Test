using System;
using System.Collections.Generic;
using System.Configuration;

namespace RelatedProductsTool
{
    /// <summary>
    /// 用户保存读取出来的countries节点的内容
    /// </summary>
    public class CountriesNodeInfo
    {
        public readonly Dictionary<int, CountryInfo> CountryInfoListDic;

        public CountriesNodeInfo(List<CountryInfo> countryInfoList)
        {
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
            string str = "";
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
        public List<int> CategoryIds { get; private set; }
        public List<int> ProductIds { get; private set; }
        public ConnectionStringSettings MyConnectionStringSettings { get; private set; }

        public CountryInfo(int countryId, string dbConnectionKey, List<int> categoryIds, List<int> productIds)
        {
            CountryId = countryId;
            MyConnectionStringSettings = ConfigurationManager.ConnectionStrings[dbConnectionKey];
            CategoryIds = categoryIds;
            ProductIds = productIds;
        }

        public override string ToString()
        {
            return "CountryId: " + CountryId + "\t ConnectionString: " + MyConnectionStringSettings.ConnectionString + "\t CategoryIds: " + string.Join(",", CategoryIds) + "\t ProductIds: " + string.Join(",", ProductIds);
        }
    }
}