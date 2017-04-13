using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace PurgeCloudflareCacheService
{
    /// <summary>
    /// 
    /// 用于读取自定义配置节点
    /// 节点名字：countries
    /// 节点格式示例如下：
    /// 
    ///     <countries interval="10000">
    ///         interval为检测路径变化的时间间隔，单位ms
    ///         <country id = "3" dbConnectionKey="PriceMeDB_NZ" /> 
    ///         id为国家id，dbConnectionKey为对应的connectionStrings中的name
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

                var dbConnectionKeyAttr = node.Attributes["dbConnectionKey"];
                if (dbConnectionKeyAttr == null || string.IsNullOrEmpty(dbConnectionKeyAttr.Value))
                    throw new Exception("no dbConnectionKey.");

                var zoneIdAttr = node.Attributes["zoneId"];
                if (zoneIdAttr == null || string.IsNullOrEmpty(zoneIdAttr.Value))
                    throw new Exception("no zoneId.");

                var webSiteAttr = node.Attributes["webSite"];
                if (webSiteAttr == null || string.IsNullOrEmpty(webSiteAttr.Value))
                    throw new Exception("no webSite.");

                var urlSeoAttr = node.Attributes["urlSeo"];
                if (urlSeoAttr == null || string.IsNullOrEmpty(urlSeoAttr.Value))
                    throw new Exception("no finance.");
                bool urlSeo;
                if (!bool.TryParse(urlSeoAttr.Value, out urlSeo))
                    urlSeo = false;

                CountryInfo ci = new CountryInfo(id, dbConnectionKeyAttr.Value, zoneIdAttr.Value, webSiteAttr.Value, urlSeo);
                countryInfoList.Add(ci);
            }

            CountriesNodeInfo cni = new CountriesNodeInfo(interval, countryInfoList);
            return cni;
        }
    }
}
