using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using UTIL.interfaces;

namespace UTIL
{
    public class CacheHelper
    {
        public const int DEFAULT_SECONDS_TO_EXPIRE = 180; //=3minutes

        #region private static members
        private static MemoryCache getCache()
        {
            return MemoryCache.Default;
        }


        private static string getKey<T>(params object[] uniqueIDs)
        {
            var concatIds = String.Join("-", uniqueIDs);
            return typeof(T).FullName + "_" + concatIds;
        }


        private static string getKey<T>(string cacheKey) where T : ICacheable<T>
        {
            return typeof(T).FullName + "_" + cacheKey;
        }
        #endregion


        #region public static members
        public static void addToCache<T>(T itemToCache, int? secondsToExpire, params object[] uniqueIdentifier)
        {
            if (!secondsToExpire.HasValue || secondsToExpire.Value < 0)
                secondsToExpire = DEFAULT_SECONDS_TO_EXPIRE;

            var key = getKey<T>(uniqueIdentifier);
            var policy = new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(secondsToExpire.Value) };

            getCache().Set(new CacheItem(key, itemToCache), policy);
        }


        public static void addToCache<T>(T itemToCache, int? secondsToExpire) where T : ICacheable<T>
        {
            if (!secondsToExpire.HasValue || secondsToExpire.Value < 0)
                secondsToExpire = DEFAULT_SECONDS_TO_EXPIRE;

            var key = getKey<T>(itemToCache.UniqueCacheKey());
            var policy = new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(secondsToExpire.Value) };

            getCache().Set(new CacheItem(key, itemToCache), policy);
        }


        public static T readFromCache<T>(params object[] uniqueIdentifier)
        {
            return (T)getCache().Get(getKey<T>(uniqueIdentifier));
        }


        public static T readFromCache<T>(string cacheKey) where T : ICacheable<T>
        {
            return (T)getCache().Get(getKey<T>(cacheKey));
        }


        public static bool existsInCache<T>(string cacheKey) where T : ICacheable<T>
        {
            return getCache().Contains(getKey<T>(cacheKey));
        }


        public static bool existsInCache<T>(T cachedItem) where T : ICacheable<T>
        {
            return existsInCache<T>(cachedItem.UniqueCacheKey());
        }


        public static void evictFromCache<T>(params object[] uniqueIdentifier)
        {
            getCache().Remove(getKey<T>(uniqueIdentifier));
        }


        public static void evictFromCache<T>(T cachedItem) where T : ICacheable<T>
        {
            getCache().Remove(getKey<T>(cachedItem.UniqueCacheKey()));
        }


        public static void evictFromCache<T>(string cachedKey) where T : ICacheable<T>
        {
            getCache().Remove(getKey<T>(cachedKey));
        }
        #endregion
    }

}
