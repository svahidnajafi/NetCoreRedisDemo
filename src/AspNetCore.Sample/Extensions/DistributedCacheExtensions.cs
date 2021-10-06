using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Utf8Json;

namespace AspNetCore.Sample.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetCacheAsync<T>(this IDistributedCache cache,
            string key, T payload,
            TimeSpan? absoluteExpirationTime = null,
            TimeSpan? unusedExpirationTime = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpirationTime ?? TimeSpan.FromSeconds(30),
                SlidingExpiration = unusedExpirationTime
            };
            var jsonString = JsonSerializer.ToJsonString(payload);
            await cache.SetStringAsync(key, jsonString, options);
        }

        public static async Task<T> GetCacheAsync<T>(this IDistributedCache cache, string key)
        {
            var jsonString = await cache.GetStringAsync(key);
            
            if (jsonString == null) return default(T);

            T result = JsonSerializer.Deserialize<T>(jsonString);
            return result;
        }
    }
}