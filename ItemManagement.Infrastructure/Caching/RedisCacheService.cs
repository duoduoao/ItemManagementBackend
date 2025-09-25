using ItemManagement.Application.Contract;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ItemManagement.Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        public RedisCacheService(IDistributedCache cache) => _cache = cache;

        public async Task<T?> GetAsync<T>(string key)
        {
            var bytes = await _cache.GetAsync(key);
            if (bytes == null) return default;
            var value = JsonSerializer.Deserialize<T>(bytes);
            return value;
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl };
            await _cache.SetAsync(key, bytes, options);
        }
        public async Task RemoveAsync(string key) => await _cache.RemoveAsync(key);
    }

}
