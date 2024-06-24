using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.IRepositories;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;

namespace SWD.TicketBooking.API.Installer
{
    public class UnitOfWorkInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IBookingRepository, BookingRepository>();
            services.AddTransient<ICityRepository, CityRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IFeedbackRepository, FeedbackRepository>();
            services.AddTransient<IFeedback_ImageRepository, Feedback_ImageRepository>();
            services.AddTransient<IRoute_CompanyRepository, Route_CompanyRepository>();
            services.AddTransient<IRouteRepository, RouteRepository>();
            services.AddTransient<IServiceRepository, ServiceRepository>();
            services.AddTransient<IServiceTypeRepository, ServiceTypeRepository>();
            services.AddTransient<IStation_RouteRepository, Station_RouteRepository>();
            services.AddTransient<IStation_ServiceRepository, Station_ServiceRepository>();
            services.AddTransient<IStationRepository, StationRepository>();
            services.AddTransient<ITicketDetailRepository, TicketDetailRepository>();
            services.AddTransient<ITicketDetail_ServiceRepository, TicketDetail_ServiceRepository>();
            services.AddTransient<ITicketTypeRepository, TicketTypeRepository>();
            services.AddTransient<ITicketType_TripRepository, TicketType_TripRepository>();
            services.AddTransient<ITripPictureRepository, TripPictureRepository>();
            services.AddTransient<ITripRepository, TripRepository>();
            services.AddTransient<ITrip_UtilityRepository, Trip_UtilityRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserRoleRepository, UserRoleRepository>();
            services.AddTransient<IUtilityRepository, UtilityRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
