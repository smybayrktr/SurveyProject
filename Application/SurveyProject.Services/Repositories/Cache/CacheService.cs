using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace SurveyProject.Services.Repositories.Cache;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T> GetDataAsync<T>(string key)
    {
        var value = await _distributedCache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(value))
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        return default;
    }

    public async Task SetDataAsync<T>(string key, T value)
    {
        await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value));
    }

    public async Task RemoveDataAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }
}