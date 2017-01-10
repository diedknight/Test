using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;

namespace PriceMeCommon
{
    public static class ResourcesInfoController
    {
        private static Hashtable _resourcesInfoHT = null;
        private static PriceMeDBDB db = PriceMeStatic.PriceMeDB;

        public static Hashtable ResourcesInfoHT
        { get { return ResourcesInfoController._resourcesInfoHT; } }

        public static void Load()
        {
            _resourcesInfoHT = new Hashtable();

            List<ResourcesInfo> resourcesInfoList = (from resour in db.ResourcesInfos select resour).ToList();

            foreach (ResourcesInfo each in resourcesInfoList)
                _resourcesInfoHT.Add(each.ResourcesTitle, each);
        }
    }
}