


using SWD.TicketBooking.Repo.Helpers;

namespace SWD.TicketBooking.API.Installer
{
    public class MailInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
        }
    }
}
