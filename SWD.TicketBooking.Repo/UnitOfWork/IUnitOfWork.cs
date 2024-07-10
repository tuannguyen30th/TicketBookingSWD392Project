using SWD.TicketBooking.Repo.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBookingRepository BookingRepository { get; }
        ICityRepository CityRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IFeedback_ImageRepository Feedback_ImageRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }
        IRoute_CompanyRepository Route_CompanyRepository { get; }
        IRouteRepository RouteRepository { get; }
        IServiceRepository ServiceRepository { get; }
        IServiceTypeRepository ServiceTypeRepository { get; }
        IStation_CompanyRepository Station_CompanyRepository { get; }
        IStationCompany_RouteRepository StationCompany_RouteRepository { get; }
        IStation_ServiceRepository Station_ServiceRepository { get; }
        IStationRepository StationRepository { get; }
        ITicketDetail_ServiceRepository TicketDetail_ServiceRepository { get; }
        ITicketDetailRepository TicketDetailRepository { get; }
        ITicketType_TripRepository TicketType_TripRepository { get; }
        ITicketTypeRepository TicketTypeRepository { get; }
        ITrip_UtilityRepository Trip_UtilityRepository { get; }
        ITripPictureRepository TripPictureRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        ITripRepository TripRepository { get; }
        IUserRepository UserRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IUtilityRepository UtilityRepository { get; }
        int Complete();
   
    }
}
