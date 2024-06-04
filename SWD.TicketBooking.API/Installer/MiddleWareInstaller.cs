

using SWD.TicketBooking.API.Middlewares;

namespace SWD.TicketBooking.API.Installer
{
    public class MiddleWareInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
                 services.AddScoped<ExceptionMiddleware>();

        }
    }
}
