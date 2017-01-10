using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Data.Caching;

namespace PriceMeCommon {
    /// <summary>
    ///VelocityControler2 的摘要说明
    /// </summary>
    public class VelocityControler2 {

        private static Dictionary<string, VelocityControler2> usedEndPoint = new Dictionary<string, VelocityControler2>();

        private DataCache dataCache;

        public DataCache DataCache {
            get { return dataCache; }
            private set { dataCache = value; }
        }

        public static VelocityControler2 GetInstance(string hostName, int cachePort, string cacheHostName, string region) {
            string name = string.Format("{0}:{1}:{2}:{3}", hostName, cachePort, cacheHostName, region);
            if (!usedEndPoint.ContainsKey(name)) {
                try {
                    VelocityControler2 vc2 = new VelocityControler2(hostName, cachePort, cacheHostName, region);
                    if (!usedEndPoint.ContainsKey(name))
                        usedEndPoint.Add(name, vc2);
                } catch {
                    return null;
                }
            }
            return usedEndPoint[name];
        }

        private VelocityControler2(string hostName, int cachePort, string cacheHostName, string region) {
            DataCacheServerEndpoint ep = new DataCacheServerEndpoint(hostName, cachePort, cacheHostName);
            DataCacheFactory factory = new DataCacheFactory(new DataCacheServerEndpoint[] { ep }, false, false);
            dataCache = factory.GetCache("default");
            try {
                dataCache.CreateRegion(region, false);
            } catch { }
        }

        public void GetRegion(string region) {
        }
    }
}