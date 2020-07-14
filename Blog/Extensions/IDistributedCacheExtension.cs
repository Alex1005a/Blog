using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Extensions
{
    public static class IDistributedCacheExtension
    {
        public static async Task AddCache<T>(this IDistributedCache cache, string key, T data) where T : class
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(22),
            };
            await cache.SetStringAsync(key, JsonConvert.SerializeObject(data), options);
        }
    }
}
