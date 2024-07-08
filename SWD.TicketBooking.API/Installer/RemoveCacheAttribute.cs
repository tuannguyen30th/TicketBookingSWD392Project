using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SWD.TicketBooking.API.Installer
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RemoveCacheAttribute : ActionFilterAttribute
    {
        private readonly string _urlContains;

        public RemoveCacheAttribute(string urlContains)
        {
            _urlContains = urlContains;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cache = context.HttpContext.RequestServices.GetService<IDistributedCache>();
            var logger = context.HttpContext.RequestServices.GetService<ILogger<RemoveCacheAttribute>>();

            if (cache == null)
            {
                logger?.LogError("IDistributedCache is not available in the service collection.");
                await next();
                return;
            }

            var resultContext = await next();

            var requestPath = context.HttpContext.Request.Path.ToString();
            if (requestPath.StartsWith(_urlContains))
            {
                logger?.LogInformation($"Removing cache for URL containing: {_urlContains}");
                await cache.RemoveAsync(requestPath);
            }
        }
    }
}
