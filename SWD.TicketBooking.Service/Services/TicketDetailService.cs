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
        private readonly IRepository<Station_Service, int> _stationServiceRepo;
        private readonly IRepository<Station_Route, int> _stationRouteRepo;
        private readonly IRepository<Route_Company, int> _routeCompanyRepo;
        private readonly IRepository<TicketDetail_Service, int> _ticketDetailServiceRepo;

        public TicketDetailService(IRepository<TicketDetail, int> ticketDetailRepo, IRepository<Station_Route, int> stationRouteRepo, IRepository<Station_Service, int> stationServiceRepo, IRepository<Route_Company, int> routeCompanyRepo, IRepository<Booking, int> bookingRepo, IRepository<TicketDetail_Service, int> ticketDetailServiceRepo, IMapper mapper, IRepository<Trip, int> tripRepo)
        {
            _ticketDetailRepo = ticketDetailRepo;
            _bookingRepo = bookingRepo;
            _ticketDetailServiceRepo = ticketDetailServiceRepo;
            _stationServiceRepo = stationServiceRepo;
            _stationRouteRepo = stationRouteRepo;
            _routeCompanyRepo = routeCompanyRepo;
            _mapper = mapper;
            _tripRepo = tripRepo;
        }

        /*public async Task<GetDetailTicketDetailByTicketDetailModel> GetDetailTicketDetailByTicketDetail(int ticketDetailID)
        {
            try
            {
                var ticketDetail = await _ticketDetailRepo.FindByCondition(_ => _.TicketDetailID == ticketDetailID).FirstOrDefaultAsync();

                var booking = await _bookingRepo.FindByCondition(_ => _.BookingID == ticketDetail.BookingID)
                    .Include(_ => _.Trip.Route.FromCity).Include(_ => _.Trip.Route.ToCity).Include(_ => _.Trip.Route).FirstOrDefaultAsync();

                var company = await _routeCompanyRepo.FindByCondition(_ => _.RouteID == booking.Trip.RouteID).Include(_ => _.Company).FirstOrDefaultAsync();

                var stationsInRoute = await _stationRouteRepo.FindByCondition(_ => _.RouteID == booking.Trip.RouteID).Select(_ => _.Station).ToListAsync();

                var ticketDetailServices = await _ticketDetailServiceRepo.FindByCondition(_ => _.TicketDetailID == ticketDetail.TicketDetailID).ToListAsync();
                double servicePrice = 0;
                var serviceInStation = new Station_Service();

                foreach (TicketDetail_Service ticketDetail_Service in ticketDetailServices)
                {
                    servicePrice += ticketDetail_Service.Price * ticketDetail_Service.Quantity;
                    foreach (Station station in stationsInRoute)
                    {
                        serviceInStation = await _stationServiceRepo.FindByCondition(_ => _.StationID == station.StationID && _.ServiceID == ticketDetail_Service.ServiceID).FirstOrDefaultAsync();
                        var serviceDetailModel = new ServiceDetailModel
                        {
                            ServiceName
                        };
                    }
                }

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
        }*/


        public async Task<SearchTicketModel> SearchTicket(int QRCode)
        {
            try
            {
                var booking = await _bookingRepo.GetByIdAsync(QRCode);
                if (booking == null)
                {
                    throw new Exception("QR Code not found!");
                }
                else
                {
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

        public List<StationInSearchTicket> GetAllStationName(List<SWD.TicketBooking.Repo.Entities.Service> service)
        {
            var rs = new List<StationInSearchTicket>();
            foreach (var item in service)
            {
                var stationModel = new StationInSearchTicket
                {
                    price = item.Price,
                    staionName = item.Name
                };
                rs.Add(stationModel);
            }
            return rs;
        }

        public TripInSearchTicketModel GetTripBaseOnModel(Trip trip, Booking booking, Route route, TicketDetail ticket)
        {
            var company = _routeCompanyRepo.FindByCondition(x => x.RouteID == trip.RouteID).Select(c => c.Company).FirstOrDefault();
            var rs = new TripInSearchTicketModel
            {
                userName = booking.User.UserName,
                company = company.Name,
                route = $"{route.FromCity.Name} - {route.ToCity.Name}",
                position = ticket.SeatCode,
                date = trip.StartTime.ToString("yyyy-MM-dd"),
                time = trip.StartTime.ToString("HH:mm")
            };
            return rs;
        }





    }
}

