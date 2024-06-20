
namespace SWD.TicketBooking.API.Installer
{
    public class RedisInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisCacheConnection");
            });
        }
    }
}
