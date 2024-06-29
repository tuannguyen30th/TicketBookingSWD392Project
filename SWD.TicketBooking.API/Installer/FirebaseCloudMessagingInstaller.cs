
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Text.Json;
using static SWD.TicketBooking.Service.Configuration.ConfigurationModel;

namespace SWD.TicketBooking.API.Installer
{
    public class FirebaseCloudMessagingInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            var firebaseAdminSDK = configuration.GetSection("CloudMessaging").GetChildren().ToDictionary(x => x.Key, x => x.Value);


            var firebaseAdminSDKJson = JsonConvert.SerializeObject(firebaseAdminSDK);

            var googleCredential = GoogleCredential.FromJson(firebaseAdminSDKJson);

            FirebaseApp.Create(new AppOptions
            {
                Credential = googleCredential,
                ProjectId = configuration["CloudMessaging:project_id"]
            });
        }
    }
}