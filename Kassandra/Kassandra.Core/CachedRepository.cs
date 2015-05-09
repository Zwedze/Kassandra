using System;
using System.Runtime.Caching;
using Kassandra.Core.Interfaces;

namespace Kassandra.Core
{
    public class CachedRepository : ICacheRepository
    {
        public TOutput GetEntry<TOutput>(string cacheKey)
        {
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
            var cacheItem = new CacheItem(cacheKey, item);

            MemoryCache.Default.Add(cacheItem, new CacheItemPolicy
            {
                Priority = priority,
                SlidingExpiration = duration
            });
        }
    }
}