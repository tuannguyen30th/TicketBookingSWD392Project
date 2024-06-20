
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using static SWD.TicketBooking.Service.Configuration.ConfigurationModel;

namespace SWD.TicketBooking.API.Installer
{
    public class RedisInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisCacheConfiguration>(configuration.GetSection("RedisCache"));
        }
    }
   
}
