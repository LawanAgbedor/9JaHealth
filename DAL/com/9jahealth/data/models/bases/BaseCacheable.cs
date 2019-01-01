using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTIL;
using UTIL.interfaces;

namespace DAL.com._9jahealth.data.models.bases
{
    public abstract class BaseCacheable<T, K> : BaseRecord<K>, ICacheable<T> where T : BaseCacheable<T, K>//, IRecord<K>//, ICacheable<T>
    {
        //private K _ID;
        //[DataMember]
        //protected virtual K ID { get { return _ID; } set { _ID = value; SetUniqueCacheKey(value); } } 

        //private K _RecordId;
        //[DataMember]
        //public virtual K RecordId { get { return _RecordId; } protected set { _RecordId = value; } }

        #region Interface ICacheable Implementations
        private string _UniqueCacheKey;
        public virtual string UniqueCacheKey()
        {
            if (string.IsNullOrWhiteSpace(_UniqueCacheKey))
            {
                SetUniqueCacheKey(this.GetType().Name, "-", RecordId);
            }
            return _UniqueCacheKey;
        }

        public virtual bool ExistsInCache() { return BaseCacheable<T, K>.ExistsInCache(_UniqueCacheKey); }

        public virtual void SaveToCache(string newKey = null) { BaseCacheable<T, K>.SaveToCache((T)this, newKey); }

        //public virtual T ReadFromCache() { return BaseCacheable<T, K>.ReadFromCache(_UniqueCacheKey); }

        public virtual void RemoveFromCache() { BaseCacheable<T, K>.RemoveFromCache((T)this); }
        #endregion


        #region protected members
        protected virtual void SetUniqueCacheKey(params object[] cacheKey)
        {
            if (cacheKey != null)
            {
                _UniqueCacheKey = string.Concat(cacheKey);
            }
        }

        //protected void toCache(T item, string newKey = null)
        //{
        //    if (newKey != null)
        //    {
        //        CacheHelper.addToCache<T>(item, null, newKey);
        //    }
        //    else if (!string.IsNullOrWhiteSpace(item.UniqueCacheKey))
        //    {
        //        CacheHelper.addToCache<T>(item, null);
        //    }
        //}

        //protected T fromCache(string cacheKey) 
        //{
        //    if (!string.IsNullOrWhiteSpace(cacheKey))
        //    {
        //        return CacheHelper.readFromCache<T>(cacheKey);
        //    }
        //    return default(T);
        //}

        //protected void evictCache(T item)
        //{
        //    if (!string.IsNullOrWhiteSpace(item.UniqueCacheKey))
        //    {
        //        CacheHelper.evictFromCache<T>(item);
        //    }
        //}

        //protected bool existsInCache(string cacheKey)
        //{
        //    if (!string.IsNullOrWhiteSpace(cacheKey))
        //    {
        //        return CacheHelper.existsInCache<T>(cacheKey);
        //    }
        //    return false;
        //}

        //protected bool existsInCache(T item)
        //{
        //    if (!string.IsNullOrWhiteSpace(item.UniqueCacheKey))
        //    {
        //        return CacheHelper.existsInCache<T>(item);
        //    }
        //    return false;
        //}
        #endregion


        #region public static members
        public static void SaveToCache(T item, string newKey = null)
        {
            if (newKey != null)
            {
                CacheHelper.addToCache<T>(item, null, newKey);
            }
            else if (!string.IsNullOrWhiteSpace(item.UniqueCacheKey()))
            {
                CacheHelper.addToCache<T>(item, null);
            }
        }

        public static bool ExistsInCache(T item)
        {
            if (!string.IsNullOrWhiteSpace(item.UniqueCacheKey()))
            {
                return CacheHelper.existsInCache<T>(item);
            }
            return false;
        }

        public static bool ExistsInCache(string cacheKey)
        {
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                return CacheHelper.existsInCache<T>(cacheKey);
            }
            return false;
        }

        public static T ReadFromCache(string cacheKey)
        {
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                return CacheHelper.readFromCache<T>(cacheKey);
            }
            return default(T);
        }

        public static void RemoveFromCache(T item)
        {
            if (!string.IsNullOrWhiteSpace(item.UniqueCacheKey()))
            {
                CacheHelper.evictFromCache<T>(item);
            }
        }
        #endregion
    }
}
