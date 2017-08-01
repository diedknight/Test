using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceMe
{
    /// <summary>
    /// Summary description for ResourcesInfoController
    /// </summary>
    public static class ResourcesInfoController
    {
        static Dictionary<string, PriceMeDBA.ResourcesInfo> ResourcesInfoDic_Static;

        public static PriceMeDBA.ResourcesInfo GetResourcesInfo(string title)
        {
            if(ResourcesInfoDic_Static != null && ResourcesInfoDic_Static.ContainsKey(title))
            {
                return ResourcesInfoDic_Static[title];
            }

            return null;
        }

        public static void Load(int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(PriceMeCommon.BusinessLogic.MultiCountryController.GetDBProvider(countryId)))
            {
                Dictionary<string, PriceMeDBA.ResourcesInfo> resourcesInfoDic = PriceMeDBA.ResourcesInfo.All().ToDictionary(r => r.ResourcesTitle, r => r);
                ResourcesInfoDic_Static = resourcesInfoDic;
            }
        }
    }
}