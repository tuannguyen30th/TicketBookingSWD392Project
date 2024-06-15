using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.SeedData;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;
using SWD.TicketBooking.Service.Services.EmailService;
using SWD.TicketBooking.Service.Services.FirebaseService;
using SWD.TicketBooking.Service.Services.IdentityService;
using SWD.TicketBooking.Service.Services.PaymentService;
using SWD.TicketBooking.Service.Services.UserService;

namespace SWD.TicketBooking.API.Installer
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFirebaseService, FirebaseService>();
            services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<DatabaseInitialiser>();
            services.AddScoped<IdentityService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<EmailService>();
            services.AddScoped<RouteService>();
            services.AddScoped<TripService>();
            services.AddScoped<UtilityService>();
            services.AddScoped<CityService>();
            services.AddScoped<FeedbackService>();
            services.AddScoped<ServiceTypeService>();
            services.AddScoped<StationService>();
            services.AddScoped<CompanyService>();
            services.AddScoped<ServiceService>();
            services.AddScoped<TicketDetailService>();
            services.AddScoped<Station_ServiceService>();
            services.AddScoped<BookingService>();
            services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
        }
    }
}