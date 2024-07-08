
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SWD.TicketBooking.API.Installer
{
    public class FormatResponseInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                     options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                     options.JsonSerializerOptions.IgnoreNullValues = false;
                 });

        }
    }
}
