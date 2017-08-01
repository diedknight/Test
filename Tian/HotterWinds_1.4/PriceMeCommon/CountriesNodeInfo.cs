using System;
using System.Collections.Generic;
using System.Configuration;

namespace PriceMeCommon
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
        public string ConfigPath { get; private set; }
        public int CountryId { get; private set; }
        public string IndexPathKey { get; private set; }
        public bool Finance { get; private set; }
        public bool RealTimeUpdateIndex { get; private set; }
        public VelocityInfo VelocityInfo { get; private set; }
        public ConnectionStringSettings MyConnectionStringSettings { get; private set; }

        public CountryInfo(string configPath, int countryId, string indexPathKey, string dbConnectionKey, bool finance, bool realTimeUpdateIndex, VelocityInfo velocityInfo)
        {
            ConfigPath = configPath;
            CountryId = countryId;
            IndexPathKey = indexPathKey;
            MyConnectionStringSettings = ConfigurationManager.ConnectionStrings[dbConnectionKey];
            Finance = finance;
            RealTimeUpdateIndex = realTimeUpdateIndex;
            VelocityInfo = velocityInfo;
        }

        public override string ToString()
        {
            return "CountryId: " + CountryId + "\t ConfigPath: " + ConfigPath + "\t IndexPathKey: " + IndexPathKey + "\t ConnectionString: " + MyConnectionStringSettings.ConnectionString + "\t Finance:" + Finance + "\t RealTimeUpdateIndex:" + RealTimeUpdateIndex + Environment.NewLine + "VelocityInfo - " + VelocityInfo.ToString();
        }
    }

    public class VelocityInfo
    {
        public string VelocityHostName { get; private set; }
        public int VelocityPort { get; private set; }
        public string VelocityCacheHostName { get; private set; }
        public string VelocityRegion { get; private set; }

        public VelocityInfo(string velocityHostName, int velocityPort, string velocityCacheHostName, string velocityRegion)
        {
            VelocityHostName = velocityHostName;
            VelocityPort = velocityPort;
            VelocityCacheHostName = velocityCacheHostName;
            VelocityRegion = velocityRegion;
        }

        public override string ToString()
        {
            return "VelocityHostName: " + VelocityHostName + "\t VelocityPort: " + VelocityPort + "\t VelocityCacheHostName: " + VelocityCacheHostName + "\t VelocityRegion: " + VelocityRegion;
        }
    }
}