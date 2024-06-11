using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public class TicketDetailService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TicketDetail, int> _ticketDetailRepo;
        private readonly IRepository<Booking, int> _bookingRepo;
        private readonly IRepository<Trip, int> _tripRepo;
        private readonly IRepository<User, int> _userRepo;
        private readonly IRepository<City, int> _cityRepo;


        private readonly IRepository<SWD.TicketBooking.Repo.Entities.Service, int> _serviceRepo;

        private readonly IRepository<Station_Service, int> _stationServiceRepo;
        private readonly IRepository<Station_Route, int> _stationRouteRepo;
        private readonly IRepository<Route_Company, int> _routeCompanyRepo;
        private readonly IRepository<TicketDetail_Service, int> _ticketDetailServiceRepo;

        public TicketDetailService(IRepository<TicketDetail, int> ticketDetailRepo, IRepository<Station_Route, int> stationRouteRepo, IRepository<Station_Service, int> stationServiceRepo, IRepository<Route_Company, int> routeCompanyRepo, IRepository<Booking, int> bookingRepo, IRepository<TicketDetail_Service, int> ticketDetailServiceRepo, IMapper mapper, IRepository<Trip, int> tripRepo, IRepository<SWD.TicketBooking.Repo.Entities.Service, int> serviceRepo, IRepository<User, int> userRepo, IRepository<City, int> cityRepo)
        {
            _ticketDetailRepo = ticketDetailRepo;
            _bookingRepo = bookingRepo;
            _ticketDetailServiceRepo = ticketDetailServiceRepo;
            _stationServiceRepo = stationServiceRepo;
            _stationRouteRepo = stationRouteRepo;
            _routeCompanyRepo = routeCompanyRepo;
            _mapper = mapper;
            _tripRepo = tripRepo;
            _serviceRepo = serviceRepo;
            _userRepo = userRepo;
            _cityRepo = cityRepo;
        }

        public async Task<GetDetailTicketDetailByTicketDetailModel> GetDetailTicketDetailByTicketDetail(int ticketDetailID)
        {
            try
            {
                var ticketDetail = await _ticketDetailRepo.FindByCondition(_ => _.TicketDetailID == ticketDetailID).FirstOrDefaultAsync();

                var booking = await _bookingRepo.FindByCondition(_ => _.BookingID == ticketDetail.BookingID)
                    .Include(_ => _.Trip.Route.FromCity).Include(_ => _.Trip.Route.ToCity).Include(_ => _.Trip.Route).Include(_ => _.User).FirstOrDefaultAsync();

                var company = await _routeCompanyRepo.FindByCondition(_ => _.RouteID == booking.Trip.RouteID).Include(_ => _.Company).FirstOrDefaultAsync();

                var ticketDetailServices = await _ticketDetailServiceRepo.FindByCondition(_ => _.TicketDetailID == ticketDetail.TicketDetailID).Include(_ => _.Service).Include(_ => _.Station).ToListAsync();
                double servicePrice = 0;

                var serviceDetailList = new List<ServiceDetailModel>();

                foreach (TicketDetail_Service ticketDetail_Service in ticketDetailServices)
                {
                    var serviceDetailModel = new ServiceDetailModel
                    {
                        ServiceName = ticketDetail_Service.Service.Name,
                        ServicePrice = ticketDetail_Service.Price,
                        Quantity = ticketDetail_Service.Quantity,
                        ServiceInStation = ticketDetail_Service.Station.Name
                    };
                    serviceDetailList.Add(serviceDetailModel);
                    servicePrice += ticketDetail_Service.Price * ticketDetail_Service.Quantity;
                }

                var rs = new GetDetailTicketDetailByTicketDetailModel
                {
                    BookingID = booking.BookingID,
                    CompanyName = company.Company.Name,
                    CustomerName = booking.User.FullName,
                    SeatCode = ticketDetail.SeatCode,
                    StartDate = booking.Trip.StartTime.ToString("yyyy-MM-dd"),
                    StartTime = booking.Trip.StartTime.ToString("HH:mm"),
                    EndDate = booking.Trip.EndTime.ToString("yyyy-MM-dd"),
                    EndTime = booking.Trip.EndTime.ToString("HH:mm"),
                    StartCity = booking.Trip.Route.FromCity.Name,
                    EndCity = booking.Trip.Route.ToCity.Name,
                    TicketPrice = ticketDetail.Price,
                    TotalServicePrice = servicePrice,
                    SumOfPrice = servicePrice + ticketDetail.Price,
                    QrCodeImage = booking.QRCodeImage,
                    QrCode = booking.QRCode,
                    Status = ticketDetail.Status,
                    ServiceDetailList = serviceDetailList,
                };

                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<GetTicketDetailByUserModel>> GetTicketDetailByUser(int userID)
        {
            try
            {
                var bookings = await _bookingRepo.FindByCondition(_ => _.UserID == userID && _.Status.Equals(SD.ACTIVE))
                    .Include(_ => _.Trip.Route.FromCity).Include(_ => _.Trip.Route.ToCity).Include(_ => _.Trip.Route).ToListAsync();

                var rsList = new List<GetTicketDetailByUserModel>();

                foreach (Booking booking in bookings)
                {
                    var ticketDetails = await _ticketDetailRepo.GetAll().Where(_ => _.BookingID == booking.BookingID).ToListAsync();

                    foreach (TicketDetail ticketDetail in ticketDetails)
                    {
                        var ticketDetailServices = await _ticketDetailServiceRepo.FindByCondition(_ => _.TicketDetailID == ticketDetail.TicketDetailID).ToListAsync();
                        double servicePrice = 0;

                        foreach (TicketDetail_Service ticketDetail_Service in ticketDetailServices)
                        {
                            servicePrice += ticketDetail_Service.Price * ticketDetail_Service.Quantity;
                        }

                        var company = await _routeCompanyRepo.FindByCondition(_ => _.RouteID == booking.Trip.RouteID).Include(_ => _.Company).FirstOrDefaultAsync();

                        var rs = new GetTicketDetailByUserModel
                        {
                            BookingID = booking.BookingID,
                            TicketDetailID = ticketDetail.TicketDetailID,
                            CompanyName = company.Company.Name,
                            StartDate = booking.Trip.StartTime.ToString("yyyy-MM-dd"),
                            StartTime = booking.Trip.StartTime.ToString("HH:mm"),
                            EndDate = booking.Trip.EndTime.ToString("yyyy-MM-dd"),
                            EndTime = booking.Trip.EndTime.ToString("HH:mm"),
                            TotalTime = booking.Trip.EndTime - booking.Trip.StartTime,
                            StartCity = booking.Trip.Route.FromCity.Name,
                            EndCity = booking.Trip.Route.ToCity.Name,
                            SeatCode = ticketDetail.SeatCode,
                            TicketPrice = ticketDetail.Price,
                            TotalServicePrice = servicePrice,
                            Status = ticketDetail.Status,
                        };
                        rsList.Add(rs);
                    }
                }

                return rsList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<SearchTicketModel> SearchTicket(string QRCode)
        {
            try
            {
                var check = await _bookingRepo.GetAll().Where(b=> b.QRCode.Equals(QRCode)).Select(b=>b.BookingID).FirstAsync();

                if (check == null)
                {
                    throw new Exception("QR Code not found!");
                }
                else
                {
                    var booking = await _bookingRepo.GetByIdAsync(check);
                    var trip = await _bookingRepo.GetAll().Where(t => t.QRCode.Equals(QRCode)).Select(t => t.Trip).FirstOrDefaultAsync();
                    var ticketDetail = await _ticketDetailRepo.FindByCondition(b => b.BookingID == booking.BookingID).FirstOrDefaultAsync();
                    var services = await _ticketDetailServiceRepo.GetAll().
                        Where(t => t.TicketDetailID == ticketDetail.TicketDetailID).
                                        Select(ts => ts.ServiceID).ToListAsync();

                    var route = await _tripRepo.GetAll().Where(x => x.TripID == trip.TripID).Select(r => r.Route).FirstOrDefaultAsync();
                    var priceRs = new PriceInSearchTicketModel
                    {
                        price = ticketDetail.Price,
                        stations = GetAllStationName(services)
                    };

                    var rs = new SearchTicketModel
                    {
                        price = priceRs,
                        trip = GetTripBaseOnModel(trip, booking, route, ticketDetail),
                        TotalBill = booking.TotalBill,
                        QrCodeImage = booking.QRCodeImage
                    };
                    return rs;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<StationInSearchTicket> GetAllStationName(List<int> serviceID)
        {
            var rs = new List<StationInSearchTicket>();
            foreach (var item in serviceID)
            {
                var priceInService = _ticketDetailServiceRepo.FindByCondition(t => t.ServiceID == item).Select(t=>t.Price).FirstOrDefault();
                var name  = _serviceRepo.FindByCondition(t => t.ServiceID == item).Select(s=>s.Name).FirstOrDefault();
                var stationModel = new StationInSearchTicket
                {
                    price = priceInService,
                    staionName = name
                };
                rs.Add(stationModel);
            }
            return rs;
        }

        public TripInSearchTicketModel GetTripBaseOnModel(Trip trip, Booking booking, Route route, TicketDetail ticket)
        {
            var company = _routeCompanyRepo.FindByCondition(x => x.RouteID == trip.RouteID).Select(c => c.Company).FirstOrDefault();
            var user = _userRepo.FindByCondition(u=>u.UserID == booking.UserID).Select(u=>u.UserName).FirstOrDefault();

            var fromCity = _cityRepo.FindByCondition(c=>c.CityID == route.FromCityID).Select(c=>c.Name).FirstOrDefault();
            var toCity = _cityRepo.FindByCondition(c => c.CityID == route.ToCityID).Select(c => c.Name).FirstOrDefault();

            var rs = new TripInSearchTicketModel
            {
                userName = user,
                company = company.Name,
                route = $"{fromCity} - {toCity}",
                position = ticket.SeatCode,
                date = trip.StartTime.ToString("yyyy-MM-dd"),
                time = trip.StartTime.ToString("HH:mm")
            };
            return rs;
        }





    }
}

