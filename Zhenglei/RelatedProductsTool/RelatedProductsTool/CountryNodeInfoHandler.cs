using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace RelatedProductsTool
{
    /// <summary>
    /// 
    /// 用于读取自定义配置节点
    /// 节点名字：countries
    /// </summary>
    class CountryNodeInfoHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            if(section.ChildNodes.Count == 0)
                throw new Exception("no country info.");

            List<CountryInfo> countryInfoList = new List<CountryInfo>();
            foreach(XmlNode node in section.ChildNodes)
            {
                if(node.LocalName.Equals("#comment"))
                {
                    continue;
                }

                var idAttr = node.Attributes["id"];
                if(idAttr == null || string.IsNullOrEmpty(idAttr.Value))
                    throw new Exception("no id.");

                int id = 0;
                if(!int.TryParse(idAttr.Value, out id))
                    throw new Exception("id invalid.");

                var dbConnectionKeyAttr = node.Attributes["dbConnectionKey"];
                if (dbConnectionKeyAttr == null || string.IsNullOrEmpty(dbConnectionKeyAttr.Value))
                    throw new Exception("no dbConnectionKey.");

                var categoryIdsAttr = node.Attributes["categoryIds"];
                if (categoryIdsAttr == null)
                    throw new Exception("no categoryIds.");
                List<int> cids = Utility.GetIntList(categoryIdsAttr.Value, ",");

                var productIdsAttr = node.Attributes["productIds"];
                if (productIdsAttr == null)
                    throw new Exception("no productIds.");
                List<int> pids = Utility.GetIntList(productIdsAttr.Value, ",");

                var conditionAttr = node.Attributes["condition"];
                if (conditionAttr == null || string.IsNullOrEmpty(conditionAttr.Value))
                    throw new Exception("no condition.");

                if (cids.Count == 0 && pids.Count == 0)
                {
                    throw new Exception("no productIds and no categoryIds.");
                }

                CountryInfo ci = new CountryInfo(id, dbConnectionKeyAttr.Value, cids, pids, conditionAttr.Value);
                countryInfoList.Add(ci);
            }

            CountriesNodeInfo cni = new CountriesNodeInfo(countryInfoList);
            return cni;
        }
    }
}
