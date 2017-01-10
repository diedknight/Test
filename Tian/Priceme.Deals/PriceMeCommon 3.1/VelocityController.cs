using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon
{
    public static class VelocityController
    {
        static string _hostName;
        static int _port;
        static string _cacheHostName;
        static string _region;
        static Microsoft.Data.Caching.DataCache _dataCache;
        static bool _isCacheServerOK = false;

        public static bool IsCacheServerOK
        {
            get { return _isCacheServerOK; }
        }

        static VelocityController()
        {
            Init();
        }

        static void Init()
        {
            _hostName = ConfigAppString.VelocityHostName;
            _port = ConfigAppString.VelocityPort;
            _cacheHostName = ConfigAppString.VelocityCacheHostName;
            _region = ConfigAppString.VelocityRegion;

            try
            {
                Microsoft.Data.Caching.DataCacheServerEndpoint dataCacheServerEndpoint = new Microsoft.Data.Caching.DataCacheServerEndpoint(_hostName, _port, _cacheHostName);
                Microsoft.Data.Caching.DataCacheFactory dataCacheFactory = new Microsoft.Data.Caching.DataCacheFactory(new Microsoft.Data.Caching.DataCacheServerEndpoint[] { dataCacheServerEndpoint }, false, false);
                _dataCache = dataCacheFactory.GetCache("default");
                _isCacheServerOK = true;
            }
            catch (Exception ex)
            {
                LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.Message);
                return;
            }
        }

        public static T GetCache<T>(PriceMeCommon.Data.VelocityCacheKey cacheKey) where T : class
        {
            object obj = GetCache(cacheKey);
            if (obj != null)
            {
                return obj as T;
            }
            return null;
        }

        public static object GetCache(PriceMeCommon.Data.VelocityCacheKey cacheKey)
        {
            try
            {
                if (_isCacheServerOK && ConfigAppString.UseVelocity)
                {
                    return _dataCache.Get(cacheKey.ToString(), _region);
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteExceptionToDB(ex.Message + "\t" + ex.StackTrace, "GetCache", "VelocityController", "PriceMe", 0);
            }
            return null;
        }

        public static object GetCache(string key)
        {
            try
            {
                if (_isCacheServerOK && ConfigAppString.UseVelocity)
                {
                    return _dataCache.Get(key, _region);
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteExceptionToDB(ex.Message + "\t" + ex.StackTrace, "GetCache", "VelocityController", "PriceMe", 0);
            }
            return null;
        }

        public static object GetCache(string key, string region)
        {
            if (_isCacheServerOK && ConfigAppString.UseVelocity)
            {
                return _dataCache.Get(key, region);
            }
            return null;
        }
    }
}