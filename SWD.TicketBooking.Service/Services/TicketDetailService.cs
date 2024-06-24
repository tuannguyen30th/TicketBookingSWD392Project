using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;

namespace SWD.TicketBooking.Service.Services
{
    public class TicketDetailService 
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TicketDetail, Guid> _ticketDetailRepo;
        private readonly IRepository<Booking, Guid> _bookingRepo;
        private readonly IRepository<Trip, Guid> _tripRepo;
        private readonly IRepository<User, Guid> _userRepo;
        private readonly IRepository<City, Guid> _cityRepo;
        private readonly IRepository<SWD.TicketBooking.Repo.Entities.Service, Guid> _serviceRepo;
        private readonly IRepository<Station_Service, Guid> _stationServiceRepo;
        private readonly IRepository<Station_Route, Guid> _stationRouteRepo;
        private readonly IRepository<Route_Company, Guid> _routeCompanyRepo;
        private readonly IRepository<TicketDetail_Service, Guid> _ticketDetailServiceRepo;

        public TicketDetailService(IRepository<TicketDetail, Guid> ticketDetailRepo, IRepository<Station_Route, Guid> stationRouteRepo, IRepository<Station_Service, Guid> stationServiceRepo, IRepository<Route_Company, Guid> routeCompanyRepo, IRepository<Booking, Guid> bookingRepo, IRepository<TicketDetail_Service, Guid> ticketDetailServiceRepo, IMapper mapper, IRepository<Trip, Guid> tripRepo, IRepository<SWD.TicketBooking.Repo.Entities.Service, Guid> serviceRepo, IRepository<User, Guid> userRepo, IRepository<City, Guid> cityRepo)
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

        public async Task<GetDetailTicketDetailByTicketDetailModel> GetDetailTicketDetailByTicketDetail(Guid ticketDetailID)
        {
            try
            {
                var ticketDetail = await _ticketDetailRepo.FindByCondition(_ => _.TicketDetailID == ticketDetailID).FirstOrDefaultAsync();

                var booking = await _bookingRepo.FindByCondition(_ => _.BookingID == ticketDetail.BookingID)
                    .Include(_ => _.Trip.Route_Company.Route.FromCity).Include(_ => _.Trip.Route_Company.Route.ToCity).Include(_ => _.Trip.Route_Company.Route).Include(_ => _.User).FirstOrDefaultAsync();

                var company = await _routeCompanyRepo.FindByCondition(_ => _.RouteID == booking.Trip.Route_Company.RouteID).Include(_ => _.Company).FirstOrDefaultAsync();

                var ticketDetailServices = await _ticketDetailServiceRepo.FindByCondition(_ => _.TicketDetailID == ticketDetail.TicketDetailID).Include(_ => _.Service).Include(_ => _.Station).ToListAsync();
                double servicePrice = 0;

                var serviceDetailList = new List<ServiceDetailModel>();

                foreach (var ticketDetail_Service in ticketDetailServices)
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
                    StartCity = booking.Trip.Route_Company.Route.FromCity.Name,
                    EndCity = booking.Trip.Route_Company.Route.ToCity.Name,
                    TicketPrice = ticketDetail.Price,
                    TotalServicePrice = servicePrice,
                    SumOfPrice = servicePrice + ticketDetail.Price,
                    QrCodeImage = ticketDetail.QRCodeImage,
                    QrCode = ticketDetail.QRCode,
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

        public async Task<List<GetTicketDetailByUserModel>> GetTicketDetailByUser(Guid customerID)
        {
            try
            {
                var bookings = await _bookingRepo.FindByCondition(_ => _.UserID == customerID)
                    .Include(_ => _.Trip.Route_Company.Route.FromCity).Include(_ => _.Trip.Route_Company.Route.ToCity).Include(_ => _.Trip.Route_Company.Route).ToListAsync();

                var rsList = new List<GetTicketDetailByUserModel>();

                foreach (var booking in bookings)
                {
                    var ticketDetails = await _ticketDetailRepo.GetAll().Where(_ => _.BookingID == booking.BookingID).ToListAsync();

                    foreach (var ticketDetail in ticketDetails)
                    {
                        var ticketDetailServices = await _ticketDetailServiceRepo.FindByCondition(_ => _.TicketDetailID == ticketDetail.TicketDetailID).ToListAsync();
                        double servicePrice = 0;

                        foreach (var ticketDetail_Service in ticketDetailServices)
                        {
                            servicePrice += ticketDetail_Service.Price * ticketDetail_Service.Quantity;
                        }

                        var company = await _routeCompanyRepo.FindByCondition(_ => _.RouteID == booking.Trip.Route_Company.RouteID).Include(_ => _.Company).FirstOrDefaultAsync();

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
                            StartCity = booking.Trip.Route_Company.Route.FromCity.Name,
                            EndCity = booking.Trip.Route_Company.Route.ToCity.Name,
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


        //public async Task<SearchTicketModel> SearchTicket(string QRCode, string email)
        //{
        //    try
        //    {
        //        var checkUser = await _userRepo.GetAll().Where(u => u.Email.Equals(email)).Select(u => u.UserID).FirstOrDefaultAsync();
        //        var searchTicket = new SearchTicketModel();
        //        if (checkUser == Guid.Empty)
        //        {
        //            return searchTicket;
        //        }
        //        else
        //        {
        //            var check = await _bookingRepo.GetAll().Where(b => b.QRCode.Equals(QRCode) && b.UserID.Equals(checkUser)).Select(b => b.BookingID).FirstOrDefaultAsync();
        //            if (check == Guid.Empty)
        //            {
        //                return searchTicket;
        //            }
        //            else
        //            {
        //                var booking = await _bookingRepo.GetByIdAsync(check);
        //                var trip = await _bookingRepo.GetAll().Where(t => t.QRCode.Equals(QRCode)).Select(t => t.Trip).FirstOrDefaultAsync();
        //                var ticketDetail = await _ticketDetailRepo.FindByCondition(b => b.BookingID == booking.BookingID).FirstOrDefaultAsync();
        //                var services = await _ticketDetailServiceRepo.GetAll().
        //                    Where(t => t.TicketDetailID == ticketDetail.TicketDetailID).
        //                                    Select(ts => ts.ServiceID).ToListAsync();

        //                var route = await _tripRepo.GetAll().Where(x => x.TripID == trip.TripID).Select(r => r.Route_Company.Route).FirstOrDefaultAsync();
        //                var priceRs = new PriceInSearchTicketModel
        //                {
        //                    Price = ticketDetail.Price,
        //                    Stations = GetAllStationName(services)
        //                };

        //                var rs = new SearchTicketModel
        //                {
        //                    Price = priceRs,
        //                    Trip = GetTripBaseOnModel(trip, booking, route, ticketDetail),
        //                    TotalBill = booking.TotalBill,
        //                    QrCodeImage = booking.QRCodeImage,
        //                    QrCode = booking.QRCode
        //                };
        //                return rs;
        //            }
                
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //}

        public List<StationInSearchTicket> GetAllStationName(List<Guid> serviceID)
        {
            var rs = new List<StationInSearchTicket>();
            foreach (var item in serviceID)
            {
                var priceInService = _ticketDetailServiceRepo.FindByCondition(t => t.ServiceID == item).Select(t=>t.Price).FirstOrDefault();
                var name  = _serviceRepo.FindByCondition(t => t.ServiceID == item).Select(s=>s.Name).FirstOrDefault();
                var stationModel = new StationInSearchTicket
                {
                    Price = priceInService,
                    StaionName = name
                };
                rs.Add(stationModel);
            }
            return rs;
        }

        public TripInSearchTicketModel GetTripBaseOnModel(Trip trip, Booking booking, Route route, TicketDetail ticket)
        {
            //var company = _routeCompanyRepo.FindByCondition(x => x.RouteID == trip.Route_Company.RouteID).Select(c => c.Company).FirstOrDefault();
            
            var route_company = _tripRepo.FindByCondition(t=> t.TripID.Equals(trip.TripID)).Select(c=>c.Route_CompanyID).FirstOrDefault();
            var company = _routeCompanyRepo.FindByCondition(r=>r.Route_CompanyID.Equals(route_company)).Select(c=>c.Company).FirstOrDefault();
            
            var user = _userRepo.FindByCondition(u=>u.UserID == booking.UserID).Select(u=>u.UserName).FirstOrDefault();

            var fromCity = _cityRepo.FindByCondition(c=>c.CityID == route.FromCityID).Select(c=>c.Name).FirstOrDefault();
            var toCity = _cityRepo.FindByCondition(c => c.CityID == route.ToCityID).Select(c => c.Name).FirstOrDefault();

            var rs = new TripInSearchTicketModel
            {
                UserName = user,
                Company = company.Name,
                Route = $"{fromCity} - {toCity}",
                Position = ticket.SeatCode,
                Date = trip.StartTime.ToString("yyyy-MM-dd"),
                Time = trip.StartTime.ToString("HH:mm")
            };
            return rs;
        }





    }
}

