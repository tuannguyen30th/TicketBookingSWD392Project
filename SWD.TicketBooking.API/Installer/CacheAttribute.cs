using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace SWD.TicketBooking.API.Installer
{
    public class CacheAttribute : ActionFilterAttribute
    {
        private readonly int _duration;
        private readonly string _cacheKey;

        public CacheAttribute(int duration, string cacheKey = null)
        {
            _duration = duration;
            _cacheKey = cacheKey;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cache = (IDistributedCache)context.HttpContext.RequestServices.GetService(typeof(IDistributedCache));
            var cacheKey = _cacheKey ?? GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cachedResponse = await cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            var executedContext = await next();
            if (executedContext.Result is ObjectResult objectResult)
            {
                var response = JsonConvert.SerializeObject(objectResult.Value);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_duration)
                };
                await cache.SetStringAsync(cacheKey, response, options);
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new System.Text.StringBuilder();
            keyBuilder.Append(request.Path.ToString());
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
