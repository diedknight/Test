using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace PriceMeCommon
{
    /// <summary>
    /// 
    /// 用于读取自定义配置节点
    /// 节点名字：countries
    /// 节点格式示例如下：
    /// 
    ///     <countries interval="10000">
    ///         interval为检测路径变化的时间间隔，单位ms
    ///         <country id = "3" indexPathKey="IndexNZ" dbConnectionKey="PriceMeDB_NZ" configPath="E:\configFile\newlucene_help.config" finance="false">
    ///             <velocity velocityHostName="12RMB-DBCenter" velocityPort="22233" velocityCacheHostName="DistributedCacheService" velocityRegion="NZ_PriceMe_4" />
    ///         </country> 
    ///         id为国家id，indexPathKey为LuceneConfig中对应的key name，dbConnectionKey为对应的connectionStrings中的name，configPath为lucene config文件的路径, finance为是否包含finance的站点
    ///     </countries>
    ///     
    /// </summary>
    class CountryNodeInfoHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            if(section.ChildNodes.Count == 0)
                throw new Exception("no country info.");

            var intervalAttr = section.Attributes["interval"];
            if (intervalAttr == null || string.IsNullOrEmpty(intervalAttr.Value))
                throw new Exception("no interval.");
            int interval = 0;
            if (!int.TryParse(intervalAttr.Value, out interval))
                throw new Exception("interval invalid.");

            List<CountryInfo> countryInfoList = new List<CountryInfo>();
            foreach(XmlNode node in section.ChildNodes)
            {
                var idAttr = node.Attributes["id"];
                if(idAttr == null || string.IsNullOrEmpty(idAttr.Value))
                    throw new Exception("no id.");

                int id = 0;
                if(!int.TryParse(idAttr.Value, out id))
                    throw new Exception("id invalid.");

                var pathAttr = node.Attributes["configPath"];
                if (pathAttr == null || string.IsNullOrEmpty(pathAttr.Value))
                    throw new Exception("no configPath.");

                var indexPathKeyAttr = node.Attributes["indexPathKey"];
                if (indexPathKeyAttr == null || string.IsNullOrEmpty(indexPathKeyAttr.Value))
                    throw new Exception("no indexPathKey.");

                var dbConnectionKeyAttr = node.Attributes["dbConnectionKey"];
                if (dbConnectionKeyAttr == null || string.IsNullOrEmpty(dbConnectionKeyAttr.Value))
                    throw new Exception("no dbConnectionKey.");

                var financeAttr = node.Attributes["finance"];
                if (financeAttr == null || string.IsNullOrEmpty(financeAttr.Value))
                    throw new Exception("no finance.");

                bool finance;
                if (!bool.TryParse(financeAttr.Value, out finance))
                    throw new Exception("id finance.");

                if (node.ChildNodes.Count == 0)
                    throw new Exception("no velocity info.");

                XmlNode velocityNode = node.ChildNodes[0];
                if(velocityNode.Name != "velocity")
                    throw new Exception("no velocity info.");

                var velocityHostNameAttr = velocityNode.Attributes["velocityHostName"];
                if (velocityHostNameAttr == null || string.IsNullOrEmpty(velocityHostNameAttr.Value))
                    throw new Exception("no velocityHostName.");

                var velocityPortAttr = velocityNode.Attributes["velocityPort"];
                if (velocityPortAttr == null || string.IsNullOrEmpty(velocityPortAttr.Value))
                    throw new Exception("no velocityPort.");

                int velocityPort = 0;
                if (!int.TryParse(velocityPortAttr.Value, out velocityPort))
                    throw new Exception("id velocityPort.");

                var velocityCacheHostNameAttr = velocityNode.Attributes["velocityCacheHostName"];
                if (velocityCacheHostNameAttr == null || string.IsNullOrEmpty(velocityCacheHostNameAttr.Value))
                    throw new Exception("no velocityCacheHostName.");

                var velocityRegionAttr = velocityNode.Attributes["velocityRegion"];
                if (velocityRegionAttr == null || string.IsNullOrEmpty(velocityRegionAttr.Value))
                    throw new Exception("no velocityRegion.");

                VelocityInfo velocityInfo = new VelocityInfo(velocityHostNameAttr.Value, velocityPort, velocityCacheHostNameAttr.Value, velocityRegionAttr.Value);
                CountryInfo ci = new CountryInfo(pathAttr.Value, id, indexPathKeyAttr.Value, dbConnectionKeyAttr.Value, finance, velocityInfo);
                countryInfoList.Add(ci);
            }

            CountriesNodeInfo cni = new CountriesNodeInfo(interval, countryInfoList);
            return cni;
        }
    }
}
