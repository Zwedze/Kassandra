using System;
using System.Runtime.Caching;

namespace Kassandra.Core.Components
{
    public class CachedRepository : ICacheRepository
    {
        public TOutput GetEntry<TOutput>(string cacheKey)
        {
            if (string.IsNullOrWhiteSpace(cacheKey))
            {
                throw new ArgumentException("Cachekey is not defined", "cacheKey");
            }
            CacheItem cacheItem;
            if ((cacheItem = MemoryCache.Default.GetCacheItem(cacheKey)) != null)
            {
                return (TOutput) cacheItem.Value;
            }

            return default(TOutput);
        }

        public void Insert(string cacheKey, object item, TimeSpan duration,
            CacheItemPriority priority = CacheItemPriority.Default)
        {
            if (item == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(cacheKey))
            {
                throw new ArgumentException("Cachekey is not defined", "cacheKey");
            }
            CacheItem cacheItem = new CacheItem(cacheKey, item);

            MemoryCache.Default.Add(cacheItem, new CacheItemPolicy
            {
                Priority = priority,
                SlidingExpiration = duration
            });
        }
    }
}