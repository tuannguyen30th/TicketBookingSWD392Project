using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace SWD.TicketBooking.API.Installer
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RemoveCacheAttribute : ActionFilterAttribute
    {
        private readonly string _cacheKey;

        public RemoveCacheAttribute(string cacheKey)
        {
            _cacheKey = cacheKey;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cache = (IDistributedCache)context.HttpContext.RequestServices.GetService(typeof(IDistributedCache));
            await next();
            await cache.RemoveAsync(_cacheKey);
        }
    }
}
