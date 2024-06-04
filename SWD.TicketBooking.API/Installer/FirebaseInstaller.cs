

using SWD.TicketBooking.API.Installer;
using SWD.TicketBooking.Service.Configuration;

namespace TicketBooking.API.Installer
{
    public class FirebaseInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var firebase = configuration.GetSection(nameof(ConfigurationModel.FirebaseConfiguration)).Get<ConfigurationModel.FirebaseConfiguration>();
            services.Configure<ConfigurationModel.FirebaseConfiguration>(val =>
            {
                val.ApiKey = firebase.ApiKey;
                val.Bucket = firebase.Bucket;
                val.AuthEmail = firebase.AuthEmail;
                val.AuthPassword = firebase.AuthPassword;
            });
        }
    }
}
