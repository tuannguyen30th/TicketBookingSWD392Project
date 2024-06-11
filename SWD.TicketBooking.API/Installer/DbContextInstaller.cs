using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;

namespace SWD.TicketBooking.API.Installer
{
    public class DbContextInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TicketBookingDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("TienConnection"));
            });
        }
    }
}