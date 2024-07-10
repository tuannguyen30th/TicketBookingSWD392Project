using Google;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.IRepositories;
using SWD.TicketBooking.Repo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TicketBookingDbContext _context;
        public IBookingRepository BookingRepository { get; }
        public ICityRepository CityRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        public IFeedback_ImageRepository Feedback_ImageRepository { get; }
        public IFeedbackRepository FeedbackRepository { get; }
        public IRoute_CompanyRepository Route_CompanyRepository { get; }
        public IRouteRepository RouteRepository { get; }
        public IServiceRepository ServiceRepository { get; }
        public IServiceTypeRepository ServiceTypeRepository { get; }
        public IStation_CompanyRepository Station_CompanyRepository { get; }
        public IStationCompany_RouteRepository StationCompany_RouteRepository { get; }
        public IStation_ServiceRepository Station_ServiceRepository { get; }
        public IStationRepository StationRepository { get; }
        public ITicketDetail_ServiceRepository TicketDetail_ServiceRepository { get; }
        public ITicketDetailRepository TicketDetailRepository { get; }
        public ITicketType_TripRepository TicketType_TripRepository { get; }
        public ITicketTypeRepository TicketTypeRepository { get; }
        public ITransactionRepository TransactionRepository { get; }
        public ITrip_UtilityRepository Trip_UtilityRepository { get; }
        public ITripPictureRepository TripPictureRepository { get; }
        public ITripRepository TripRepository { get; }
        public IUserRepository UserRepository { get; }
        public IUserRoleRepository UserRoleRepository { get; }
        public IUtilityRepository UtilityRepository { get; }

        public UnitOfWork(TicketBookingDbContext context,
            IBookingRepository bookingRepository,
            ICityRepository cityRepository,
            ICompanyRepository companyRepository,
            IFeedback_ImageRepository feedback_ImageRepository,
            IFeedbackRepository feedbackRepository,
            IRoute_CompanyRepository route_CompanyRepository,
            IRouteRepository routeRepository,
            IServiceRepository serviceRepository,
            IServiceTypeRepository serviceTypeRepository,
            IStation_CompanyRepository station_CompanyRepository,
            IStationCompany_RouteRepository stationCompany_RouteRepository,
            IStation_ServiceRepository station_ServiceRepository,
            IStationRepository stationRepository,
            ITicketDetail_ServiceRepository ticketDetail_ServiceRepository,
            ITicketDetailRepository ticketDetailRepository,
            ITicketType_TripRepository ticketType_TripRepository,
            ITransactionRepository transactionRepository,
            ITicketTypeRepository ticketTypeRepository,
            ITrip_UtilityRepository trip_UtilityRepository,
            ITripPictureRepository tripPictureRepository,
            ITripRepository tripRepository,
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IUtilityRepository utilityRepository)
        {
            _context = context;
            BookingRepository = bookingRepository;
            CityRepository = cityRepository;
            CompanyRepository = companyRepository;
            Feedback_ImageRepository = feedback_ImageRepository;
            FeedbackRepository = feedbackRepository;
            Route_CompanyRepository = route_CompanyRepository;
            RouteRepository = routeRepository;
            ServiceRepository = serviceRepository;
            ServiceTypeRepository = serviceTypeRepository;
            Station_CompanyRepository = station_CompanyRepository;
            StationCompany_RouteRepository = stationCompany_RouteRepository;
            Station_ServiceRepository = station_ServiceRepository;
            StationRepository = stationRepository;
            TicketDetail_ServiceRepository = ticketDetail_ServiceRepository;
            TicketDetailRepository = ticketDetailRepository;
            TicketType_TripRepository = ticketType_TripRepository;
            TicketTypeRepository = ticketTypeRepository;
            Trip_UtilityRepository = trip_UtilityRepository;
            TransactionRepository = transactionRepository;
            TripPictureRepository = tripPictureRepository;
            TripRepository = tripRepository;
            UserRepository = userRepository;
            UserRoleRepository = userRoleRepository;
            UtilityRepository = utilityRepository;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }
     
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
