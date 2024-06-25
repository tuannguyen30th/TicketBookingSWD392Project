using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Utilities;

namespace SWD.TicketBooking.Service.Services
{
    public class TicketDetailService : ITicketDetailService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TicketDetailService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetDetailTicketDetailByTicketDetailModel> GetDetailTicketDetailByTicketDetail(Guid ticketDetailID)
        {
            try
            {
                var ticketDetail = await _unitOfWork.TicketDetailRepository
                                                    .FindByCondition(_ => _.TicketDetailID == ticketDetailID)
                                                    .FirstOrDefaultAsync();

                var booking = await _unitOfWork.BookingRepository
                                               .FindByCondition(_ => _.BookingID == ticketDetail.BookingID)
                                               .Include(_ => _.Trip.Route_Company.Route.FromCity)
                                               .Include(_ => _.Trip.Route_Company.Route.ToCity)
                                               .Include(_ => _.Trip.Route_Company.Route)
                                               .Include(_ => _.User)
                                               .FirstOrDefaultAsync();

                var company = await _unitOfWork.Route_CompanyRepository
                                               .FindByCondition(_ => _.RouteID == booking.Trip.Route_Company.RouteID)
                                               .Include(_ => _.Company)
                                               .FirstOrDefaultAsync();

                var ticketDetailServices = await _unitOfWork.TicketDetail_ServiceRepository
                                                            .FindByCondition(_ => _.TicketDetailID == ticketDetail.TicketDetailID)
                                                            .Include(_ => _.Service)
                                                            .Include(_ => _.Station)
                                                            .ToListAsync();
                double servicePrice = 0;

                var serviceDetailList = new List<ServiceDetailModel>();

                foreach(var ticketDetail_Service in ticketDetailServices)
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
                };

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
                var bookings = await _unitOfWork.BookingRepository
                                                .FindByCondition(_ => _.UserID == customerID)
                                                .Include(_ => _.Trip.Route_Company.Route.FromCity)
                                                .Include(_ => _.Trip.Route_Company.Route.ToCity)
                                                .Include(_ => _.Trip.Route_Company.Route)
                                                .ToListAsync();

                var rsList = new List<GetTicketDetailByUserModel>();

                foreach (var booking in bookings)
                {
                    var ticketDetails = await _unitOfWork.TicketDetailRepository
                                                         .GetAll()
                                                         .Where(_ => _.BookingID == booking.BookingID)
                                                         .ToListAsync();

                    foreach (var ticketDetail in ticketDetails)
                    {
                        var ticketDetailServices = await _unitOfWork.TicketDetail_ServiceRepository
                                                                    .FindByCondition(_ => _.TicketDetailID == ticketDetail.TicketDetailID)
                                                                    .ToListAsync();
                        double servicePrice = 0;

                        foreach (var ticketDetail_Service in ticketDetailServices)
                        {
                            servicePrice += ticketDetail_Service.Price * ticketDetail_Service.Quantity;
                        }

                        var company = await _unitOfWork.Route_CompanyRepository
                                                       .FindByCondition(_ => _.RouteID == booking.Trip.Route_Company.RouteID)
                                                       .Include(_ => _.Company)
                                                       .FirstOrDefaultAsync();

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
        public async Task<ActionOutcome> CancelTicket(Guid ticketDetailID)
        {
            try
            {
                var result = new ActionOutcome();
                double totalBillCancel = 0;
                DateTime currentTime = DateTime.Now;
                var findTicket = await _unitOfWork.TicketDetailRepository
                                                  .GetAll()
                                                  .Where(_ => _.TicketDetailID == ticketDetailID)
                                                  .Include(_ => _.Booking)
                                                  .ThenInclude(b => b.Trip)
                                                  .Include(_ => _.Booking)
                                                  .ThenInclude(b => b.User)
                                                  .FirstOrDefaultAsync();
                if (findTicket == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("Ticket"));
                }            
                var startTime = findTicket.Booking.Trip.StartTime;
                if (currentTime <= startTime.AddHours(-6))
                {
                    findTicket.Status = SD.Booking_TicketStatus.CANCEL_TICKET;
                    totalBillCancel += findTicket.Price;
                    _unitOfWork.TicketDetailRepository.Update(findTicket);
                    var findService = await _unitOfWork.TicketDetail_ServiceRepository
                                                       .GetAll()
                                                       .Where(_ => _.TicketDetailID == ticketDetailID)
                                                       .ToListAsync();
                    foreach (var ticket in findService)
                    {
                        ticket.Status = SD.Booking_ServiceStatus.CANCEL_TICKETSERVICE;
                        totalBillCancel += ticket.Quantity * ticket.Price;
                        _unitOfWork.TicketDetail_ServiceRepository.Update(ticket);
                    }
                    var findUser = findTicket.Booking.User;
                    findUser.Balance += totalBillCancel * 0.7;
                    _unitOfWork.UserRepository.Update(findUser);
                    var otherTickets = await _unitOfWork.TicketDetailRepository
                                                        .FindByCondition(_ => _.BookingID == findTicket.BookingID && _.TicketDetailID != ticketDetailID && _.Status.Trim() != SD.Booking_TicketStatus.CANCEL_TICKET)
                                                        .ToListAsync();

                    if (otherTickets == null || !otherTickets.Any())
                    {
                        var findBooking = findTicket.Booking;
                        if (findBooking != null)
                        {
                            findBooking.PaymentStatus = SD.BookingStatus.CANCEL_BOOKING;
                            _unitOfWork.BookingRepository.Update(findBooking);
                        }
                    }
                }
                else
                {
                    throw new BadRequestException("Thời gian hủy vé đã quá hạn, xin lỗi vì sự bất tiện này!");
                }
                _unitOfWork.Complete();
                result.Message = "Hủy vé thành công!";
                return result;
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

        public async Task<List<StationInSearchTicket>> GetAllStationName(List<Guid> serviceID)
        {
            var rs = new List<StationInSearchTicket>();
            foreach (var item in serviceID)
            {
                var priceInService = await _unitOfWork.TicketDetail_ServiceRepository
                                                      .FindByCondition(t => t.ServiceID == item)
                                                      .Select(t=>t.Price)
                                                      .FirstOrDefaultAsync();
                var name  = await _unitOfWork.ServiceRepository
                                             .FindByCondition(t => t.ServiceID == item)
                                             .Select(s=>s.Name)
                                             .FirstOrDefaultAsync();
                var stationModel = new StationInSearchTicket
                {
                    Price = priceInService,
                    StaionName = name
                };
                rs.Add(stationModel);
            }
            return rs;
        }

        public async Task<TripInSearchTicketModel> GetTripBaseOnModel(Trip trip, Booking booking, Route route, TicketDetail ticket)
        {
            
            var route_company = await _unitOfWork.TripRepository
                                                 .FindByCondition(t=> t.TripID.Equals(trip.TripID))
                                                 .Select(c=>c.Route_CompanyID)
                                                 .FirstOrDefaultAsync();
            var company = await _unitOfWork.Route_CompanyRepository
                                           .FindByCondition(r=>r.Route_CompanyID.Equals(route_company))
                                           .Select(c=>c.Company)
                                           .FirstOrDefaultAsync();
            
            var user = await _unitOfWork.UserRepository
                                        .FindByCondition(u=>u.UserID == booking.UserID)
                                        .Select(u=>u.UserName)
                                        .FirstOrDefaultAsync();

            var fromCity = await _unitOfWork.CityRepository
                                            .FindByCondition(c=>c.CityID == route.FromCityID)
                                            .Select(c=>c.Name)
                                            .FirstOrDefaultAsync();
            var toCity = await _unitOfWork.CityRepository
                                          .FindByCondition(c => c.CityID == route.ToCityID)
                                          .Select(c => c.Name)
                                          .FirstOrDefaultAsync();

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

