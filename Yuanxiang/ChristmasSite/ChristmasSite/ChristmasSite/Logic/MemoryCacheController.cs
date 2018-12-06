using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChristmasSite.Logic
{
    public static class MemoryCacheController
    {
        public static void Init(IServiceCollection services)
        {
            MyMemoryCache = services.BuildServiceProvider().GetService<IMemoryCache>();
        }

        public static IMemoryCache MyMemoryCache
        {
            get; private set;
        }

        public static void Set<T>(string key, T cacheObj, TimeSpan absoluteExpirationRelativeToNow)
        {
            MyMemoryCache.Set<T>(key, cacheObj, absoluteExpirationRelativeToNow);
        }

        public static T Get<T>(string key)
        {
            return MyMemoryCache.Get<T>(key);
        }
    }
}
