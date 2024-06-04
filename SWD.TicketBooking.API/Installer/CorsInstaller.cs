
using SWD.TicketBooking.API.Installer;

namespace SWD.TicketBooking.API.Installer
{
    public class CorsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(option =>
              option.AddPolicy("CORS", builder =>
                  builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((host) => true)));
        }
    }
}
