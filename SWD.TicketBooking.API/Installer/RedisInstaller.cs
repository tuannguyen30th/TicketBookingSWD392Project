
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using static SWD.TicketBooking.Service.Configuration.ConfigurationModel;
using StackExchange.Redis;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Installer
{
    public class RedisInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisConfiguration = new RedisCacheConfiguration();
            configuration.GetSection("RedisCache").Bind(redisConfiguration);

            services.AddSingleton(redisConfiguration);

            if (!redisConfiguration.Enabled)
                return;
            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfiguration.RedisCacheConnection));
            services.AddStackExchangeRedisCache(option => option.Configuration = redisConfiguration.RedisCacheConnection);
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
   
}
