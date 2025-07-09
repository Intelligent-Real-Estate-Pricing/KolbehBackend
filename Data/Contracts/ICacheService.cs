using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contracts
{
    public interface ICacheService
    {
        Task SetAsync(string key, string value, TimeSpan expiration);
        Task<string?> GetAsync(string key);
        Task<int?> GetIntAsync(string key);
        Task<bool> ExistsAsync(string key);
        Task RemoveAsync(string key);
        Task IncrementAsync(string key, int amount, TimeSpan? expire = null);
        Task<TimeSpan?> GetTimeToLiveAsync(string key);
    }

}
