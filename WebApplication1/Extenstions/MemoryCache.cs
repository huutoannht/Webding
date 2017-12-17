using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Extenstions
{
    public static class MemoryCache
    {
        private static IDictionary<string, object> _cache;

        private static IDictionary<string, object> Cache
        {
            get
            {
                if (_cache == null)
                    _cache = new Dictionary<string, object>();
                return _cache;
            }
        }

        public static T TryGetFromCache<T>(string key, Func<object> getValue)
            where T : class
        {
            if (!Cache.ContainsKey(key))
            {
                Cache[key] = getValue();
            }

            return Cache[key] as T;
        }
    }
}
