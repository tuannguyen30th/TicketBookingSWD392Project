using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.SeedData;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;
using SWD.TicketBooking.Service.Services.PaymentService;

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
            services.AddScoped<IEmailService,EmailService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IUtilityService, UtilityService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IServiceTypeService, ServiceTypeService>();
            services.AddScoped<IStationService, StationService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<ITicketDetailService, TicketDetailService>();
            services.AddScoped<IStation_ServiceService, Station_ServiceService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
        }
    }
}