using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    /// <summary>
    /// 用来联系Velocity cache
    /// </summary>
    public class VelocityController
    {
        Microsoft.Data.Caching.DataCache _dataCache;
        public VelocityInfo MyVelocityInfo { get; private set; }

        public bool IsCacheServerOK { get; private set; }

        public VelocityController(VelocityInfo velocityInfo)
        {
            MyVelocityInfo = velocityInfo;
            IsCacheServerOK = false;
            try
            {
                Microsoft.Data.Caching.DataCacheServerEndpoint dataCacheServerEndpoint = new Microsoft.Data.Caching.DataCacheServerEndpoint(MyVelocityInfo.VelocityHostName, MyVelocityInfo.VelocityPort, MyVelocityInfo.VelocityCacheHostName);
                Microsoft.Data.Caching.DataCacheFactory dataCacheFactory = new Microsoft.Data.Caching.DataCacheFactory(new Microsoft.Data.Caching.DataCacheServerEndpoint[] { dataCacheServerEndpoint }, false, false);
                _dataCache = dataCacheFactory.GetCache("default");
                IsCacheServerOK = true;
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + "\t" + ex.StackTrace);
            }
        }

        public void TryCreateRegion()
        {
            try
            {
                _dataCache.CreateRegion(MyVelocityInfo.VelocityRegion, false);
            }
            catch
            { }
        }


        public T GetCache<T>(PriceMeCommon.Data.VelocityCacheKey cacheKey) where T : class
        {
            object obj = GetCache(cacheKey);
            if (obj != null)
            {
                return obj as T;
            }
            return null;
        }

        public object GetCache(Data.VelocityCacheKey cacheKey)
        {
            try
            {
                if (IsCacheServerOK)
                {
                    return _dataCache.Get(cacheKey.ToString(), MyVelocityInfo.VelocityRegion);
                }
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + "\t" + ex.StackTrace);
            }
            return null;
        }

        public object GetCache(string key)
        {
            try
            {
                if (IsCacheServerOK)
                {
                    return _dataCache.Get(key, MyVelocityInfo.VelocityRegion);
                }
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + "\t" + ex.StackTrace);
            }
            return null;
        }

        public object PutCache(Data.VelocityCacheKey cacheKey, object cache)
        {
            try
            {
                if (IsCacheServerOK)
                {
                    return _dataCache.Put(cacheKey.ToString(), cache, new TimeSpan(48, 0, 0), MyVelocityInfo.VelocityRegion);
                }
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + "\t" + ex.StackTrace);
            }
            return null;
        }
    }
}
