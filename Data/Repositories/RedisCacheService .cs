using Data.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer connection)
        {
            _db = connection.GetDatabase();
        }

        public async Task SetAsync(string key, string value, TimeSpan expiration)
        {
            await _db.StringSetAsync(key, value, expiration);
        }

        public async Task<string?> GetAsync(string key)
        {
            var result = await _db.StringGetAsync(key);
            return result.HasValue ? result.ToString() : null;
        }

        public async Task<int?> GetIntAsync(string key)
        {
            var value = await GetAsync(key);
            return int.TryParse(value, out var num) ? num : null;
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task IncrementAsync(string key, int amount, TimeSpan? expire = null)
        {
            var value = await _db.StringIncrementAsync(key, amount);
            if (expire.HasValue)
            {
                await _db.KeyExpireAsync(key, expire);
            }
        }

        public async Task<TimeSpan?> GetTimeToLiveAsync(string key)
        {
            return await _db.KeyTimeToLiveAsync(key);
        }
    }

}
