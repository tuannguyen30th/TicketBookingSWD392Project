
namespace SWD.TicketBooking.API.Installer
{
    public class SystemInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddMvc();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }
}
