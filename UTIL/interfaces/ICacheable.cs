using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTIL.interfaces
{
    public interface ICacheable<T>
    {
        string UniqueCacheKey();
        bool ExistsInCache();
        void SaveToCache(string newKey = null);
        void RemoveFromCache();
    }
}
