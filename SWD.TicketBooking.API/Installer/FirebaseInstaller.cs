

using SWD.TicketBooking.API.Installer;
using SWD.TicketBooking.Service.Configuration;
using static SWD.TicketBooking.Service.Configuration.ConfigurationModel;

namespace TicketBooking.API.Installer
{
    public class FirebaseInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var firebaseConfigSection = configuration.GetSection("Firebase");
            var firebaseConfig = firebaseConfigSection.Get<FirebaseConfiguration>();
            services.Configure<FirebaseConfiguration>(firebaseConfigSection);
            services.AddSingleton(firebaseConfig);
        }
    }
}
