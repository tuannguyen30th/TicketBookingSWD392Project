using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using SWD.TicketBooking.Service.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ILogger<ResponseCacheService> _logger;

        public ResponseCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, ILogger<ResponseCacheService> logger)
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
            _logger = logger;
        }

        public async Task<string> GetCacheResponseAsync(string cacheKey)
        {
            var cacheResponse = await _distributedCache.GetStringAsync(cacheKey);
            return string.IsNullOrEmpty(cacheResponse) ? null : cacheResponse;
        }

        public async Task RemoveCacheResponseAsync(string pattern)
        {
            try
            {
                _logger.LogInformation("RemoveCacheResponseAsync called with pattern: {Pattern}", pattern);
                if (string.IsNullOrWhiteSpace(pattern))
                {
                    throw new ArgumentException("Pattern cannot be null or whitespace", nameof(pattern));
                }

                await foreach (var key in GetKeysAsync(pattern + "*"))
                {
                    _logger.LogInformation($"Removing cache key: {key}");
                    await _distributedCache.RemoveAsync(key);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing cache keys with pattern {Pattern}", pattern);
                throw;
            }
        }

        private async IAsyncEnumerable<string> GetKeysAsync(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentException("Value cannot be null or whitespace", nameof(pattern));
            }

            _logger.LogInformation("GetKeysAsync called with pattern: {Pattern}", pattern);

            foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endPoint);
                if (server == null)
                {
                    _logger.LogWarning("Server not found for endpoint: {EndPoint}", endPoint);
                    continue;
                }

                await foreach (var key in server.KeysAsync(pattern: pattern))
                {
                    _logger.LogInformation("Key found: {Key}", key);
                    yield return key.ToString();
                }
            }
        }

        public async Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut)
        {
            if (response == null)
                return;

            var serializerResponse = response.ToString(); 

            await _distributedCache.SetStringAsync(cacheKey, serializerResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeOut
            });
        }
    }
}
