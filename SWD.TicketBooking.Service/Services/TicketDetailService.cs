using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
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

        public async Task<GetDetailOfTicketByIDModel> GetDetailOfTicketByID(Guid ticketDetailID)
        {
            try
            {
                var ticketDetail = await _unitOfWork.TicketDetailRepository
                                                    .FindByCondition(_ => _.TicketDetailID == ticketDetailID)
                                                    .FirstOrDefaultAsync();
                if (ticketDetail == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("VÉ"));
                }
                var booking = await _unitOfWork.BookingRepository
                                               .FindByCondition(_ => _.BookingID == ticketDetail.BookingID)
                                               .Include(_ => _.Trip.Route_Company.Route.FromCity)
                                               .Include(_ => _.Trip.Route_Company.Route.ToCity)
                                               .Include(_ => _.Trip.Route_Company.Route)
                                               .Include(_ => _.User)
                                               .FirstOrDefaultAsync();
                if (booking == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("VÉ ĐÃ ĐẶT"));
                }
                var company = await _unitOfWork.Route_CompanyRepository
                                               .FindByCondition(_ => _.RouteID == booking.Trip.Route_Company.RouteID)
                                               .Include(_ => _.Company)
                                               .FirstOrDefaultAsync();
                if (company == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("CÔNG TY"));
                }
                var ticketDetailServices = await _unitOfWork.TicketDetail_ServiceRepository
                                                            .FindByCondition(_ => _.TicketDetailID == ticketDetail.TicketDetailID)
                                                            .Include(_ => _.Service)
                                                            .Include(_ => _.Station)
                                                            .ToListAsync();
                double servicePrice = 0;
                var serviceDetailList = new List<ServiceDetailModel>();

                foreach (var ticketDetail_Service in ticketDetailServices)
                {
                    var serviceDetailModel = new ServiceDetailModel
                    {
                        ServiceName = ticketDetail_Service.Service.Name,
                        ServicePrice = (double)ticketDetail_Service.Price,
                        Quantity = (int)ticketDetail_Service.Quantity,
                        ServiceInStation = ticketDetail_Service.Station.Name
                    };
                    serviceDetailList.Add(serviceDetailModel);
                    servicePrice += (double)(ticketDetail_Service.Price * ticketDetail_Service.Quantity);
                };

                var rs = new GetDetailOfTicketByIDModel
                {
                    BookingID = booking.BookingID,
                    CompanyName = company.Company.Name,
                    CustomerName = booking.User.FullName,
                    SeatCode = ticketDetail.SeatCode,
                    StartDate = booking.Trip.StartTime?.ToString("yyyy-MM-dd"),
                    StartTime = booking.Trip.StartTime?.ToString("HH:mm"),
                    EndDate = booking.Trip.EndTime?.ToString("yyyy-MM-dd"),
                    EndTime = booking.Trip.EndTime?.ToString("HH:mm"),
                    StartCity = booking.Trip.Route_Company.Route.FromCity.Name,
                    EndCity = booking.Trip.Route_Company.Route.ToCity.Name,
                    TicketPrice = (double)ticketDetail.Price,
                    TotalServicePrice = servicePrice,
                    SumOfPrice = (double)(servicePrice + ticketDetail.Price),
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
                            servicePrice += (double)(ticketDetail_Service.Price * ticketDetail_Service.Quantity);
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
                            StartDate = booking.Trip.StartTime?.ToString("yyyy-MM-dd"),
                            StartTime = booking.Trip.StartTime?.ToString("HH:mm"),
                            EndDate = booking.Trip.EndTime?.ToString("yyyy-MM-dd"),
                            EndTime = booking.Trip.EndTime?.ToString("HH:mm"),
                            TotalTime = (TimeSpan)(booking.Trip.EndTime - booking.Trip.StartTime),
                            StartCity = booking.Trip.Route_Company.Route.FromCity.Name,
                            EndCity = booking.Trip.Route_Company.Route.ToCity.Name,
                            SeatCode = ticketDetail.SeatCode,
                            TicketPrice = (double)ticketDetail.Price,
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
                    throw new NotFoundException(SD.Notification.NotFound("VÉ"));
                }
                var startTime = findTicket.Booking.Trip.StartTime;
                if (currentTime <= startTime?.AddHours(-12))
                {
                    findTicket.Status = SD.Booking_TicketStatus.CANCEL_TICKET;
                    totalBillCancel += (double)findTicket.Price;
                    _unitOfWork.TicketDetailRepository.Update(findTicket);
                    var findService = await _unitOfWork.TicketDetail_ServiceRepository
                                                       .GetAll()
                                                       .Where(_ => _.TicketDetailID == ticketDetailID)
                                                       .ToListAsync();
                    foreach (var ticket in findService)
                    {
                        ticket.Status = SD.Booking_ServiceStatus.CANCEL_TICKETSERVICE;
                        totalBillCancel += (double)(ticket.Quantity * ticket.Price);
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
                    throw new BadRequestException("THỜI GIAN HỦY VÉ ĐÃ QUÁ HẠN, XIN LỖI VÌ SỰ BẤT TIỆN NÀY!");
                }
                _unitOfWork.Complete();
                result.Message = "HỦY VÉ THÀNH CÔNG!";
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
        //        var checkUser = await _unitOfWork.UserRepository.GetAll().Where(u => u.Email.Equals(email)).Select(u => u.UserID).FirstOrDefaultAsync();
        //        var searchTicket = new SearchTicketModel();
        //        if (checkUser == Guid.Empty)
        //        {
        //            return searchTicket;
        //        }
        //        else
        //        {
        //            var check = await _unitOfWork.BookingRepository.GetAll().Where(b => b.QRCode.Equals(QRCode) && b.UserID.Equals(checkUser)).Select(b => b.BookingID).FirstOrDefaultAsync();
        //            if (check == Guid.Empty)
        //            {
        //                return searchTicket;
        //            }
        //            else
        //            {
        //                var booking = await _unitOfWork.BookingRepository.GetByIdAsync(check);
        //                var trip = await _unitOfWork.BookingRepository.GetAll().Where(t => t.QRCode.Equals(QRCode)).Select(t => t.Trip).FirstOrDefaultAsync();
        //                var ticketDetail = await _unitOfWork.TicketDetailRepository.FindByCondition(b => b.BookingID == booking.BookingID).FirstOrDefaultAsync();
        //                var services = await _unitOfWork.TicketDetail_ServiceRepository.GetAll().
        //                    Where(t => t.TicketDetailID == ticketDetail.TicketDetailID).
        //                                    Select(ts => ts.ServiceID).ToListAsync();

        //                var route = await _unitOfWork.TripRepository.GetAll().Where(x => x.TripID == trip.TripID).Select(r => r.Route_Company.Route).FirstOrDefaultAsync();
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

        public async Task<SearchTicketModel> SearchTicket(string QRCode, string email)
        {
            try
            {
                var searchTicket = new SearchTicketModel();
                var ticketDetail = await _unitOfWork.TicketDetailRepository
                                                    .GetAll()
                                                    .Include(t => t.Booking)
                                                    .Where(b => b.QRCode.Equals(QRCode) && b.Booking.Email.Equals(email))
                                                    .Include(t => t.Booking)
                                                    .ThenInclude(t => t.Trip)
                                                    .ThenInclude(t => t.Route_Company)
                                                    .ThenInclude(t => t.Route)
                                                    .FirstOrDefaultAsync();
                if (ticketDetail == null)
                {
                    return searchTicket;
                }
                else
                {
                    var booking = ticketDetail.Booking;
                    var trip = booking.Trip;
                    var services = await _unitOfWork.TicketDetail_ServiceRepository
                                                    .GetAll()
                                                    .Where(t => t.TicketDetailID == ticketDetail.TicketDetailID)
                                                    .Select(ts => ts.ServiceID)
                                                    .ToListAsync();

                    var route = trip.Route_Company.Route;
                    var priceRs = new PriceInSearchTicketModel
                    {
                        Price = (double)ticketDetail.Price,
                        Services = await GetAllServiceName(services)
                    };

                    var rs = new SearchTicketModel
                    {
                        Price = priceRs,
                        Trip = await GetTripBaseOnModel(trip, booking, route, ticketDetail),
                        TotalBill = (double)booking.TotalBill,
                        QrCodeImage = ticketDetail.QRCodeImage,
                        QrCode = ticketDetail.QRCode
                    };
                    return rs;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<ServiceInSearchTicket>> GetAllServiceName(List<Guid?> serviceID)
        {
            var rs = new List<ServiceInSearchTicket>();
            foreach (var item in serviceID)
            {
                var priceInService = await _unitOfWork.TicketDetail_ServiceRepository
                                                      .FindByCondition(t => t.ServiceID == item)
                                                      .Select(t => t.Price)
                                                      .FirstOrDefaultAsync();
                var name = await _unitOfWork.ServiceRepository
                                             .FindByCondition(t => t.ServiceID == item)
                                             .Select(s => s.Name)
                                             .FirstOrDefaultAsync();
                var stationModel = new ServiceInSearchTicket
                {
                    Price = (double)priceInService,
                    ServiceName = name
                };
                rs.Add(stationModel);
            }
            return rs;
        }

        public async Task<TripInSearchTicketModel> GetTripBaseOnModel(Trip trip, Booking booking, Route route, TicketDetail ticket)
        {

            var route_company = await _unitOfWork.TripRepository
                                                 .FindByCondition(t => t.TripID.Equals(trip.TripID))
                                                 .Select(c => c.Route_CompanyID)
                                                 .FirstOrDefaultAsync();
            var company = await _unitOfWork.Route_CompanyRepository
                                           .FindByCondition(r => r.Route_CompanyID.Equals(route_company))
                                           .Select(c => c.Company)
                                           .FirstOrDefaultAsync();

            var user = await _unitOfWork.UserRepository
                                        .FindByCondition(u => u.UserID == booking.UserID)
                                        .Select(u => u.UserName)
                                        .FirstOrDefaultAsync();

            var fromCity = await _unitOfWork.CityRepository
                                            .FindByCondition(c => c.CityID == route.FromCityID)
                                            .Select(c => c.Name)
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
                Date = trip.StartTime?.ToString("yyyy-MM-dd"),
                Time = trip.StartTime?.ToString("HH:mm")
            };
            return rs;
        }

        public async Task<GetTicketDetailInMobileModel> GetTicketDetailInMobile(string qrCode)
        {
            try
            {
                var ticketDetail = await _unitOfWork.TicketDetailRepository
                                                   .GetAll()
                                                   .Where(t => t.QRCode.Equals(qrCode) && t.Status.Equals(SD.Booking_TicketStatus.UNUSED_TICKET))
                                                   .FirstOrDefaultAsync();
                if (ticketDetail == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("QR code"));
                }

                var booking = await _unitOfWork.BookingRepository
                                               .GetAll()
                                               .Where(b => b.BookingID.Equals(ticketDetail.BookingID) && b.PaymentStatus.Equals(SD.BookingStatus.PAYING_BOOKING))
                                               .FirstOrDefaultAsync();
                var trip = await _unitOfWork.TripRepository
                                            .GetAll()
                                            .Where(t => t.TripID.Equals(booking.TripID) && t.Status.Equals(SD.GeneralStatus.ACTIVE))
                                            .FirstOrDefaultAsync();
                var routeCompany = await _unitOfWork.Route_CompanyRepository
                                                    .FindByCondition(rc => rc.Route_CompanyID.Equals(trip.Route_CompanyID) && rc.Status.Equals(SD.GeneralStatus.ACTIVE))
                                                    .FirstOrDefaultAsync();
                var route = await _unitOfWork.RouteRepository
                                             .FindByCondition(r => r.RouteID.Equals(routeCompany.RouteID) && r.Status.Equals(SD.GeneralStatus.ACTIVE))
                                             .FirstOrDefaultAsync();

                var routeResponse = GetTripBaseOnModel(trip, booking, route, ticketDetail);

                var result = new GetTicketDetailInMobileModel
                {
                    Name = booking.FullName,
                    PhoneNumber = booking.PhoneNumber,
                    SeatCode = ticketDetail.SeatCode,
                    QrCodeImage = ticketDetail.QRCodeImage,
                    Route = routeResponse.Result.Route,
                    StartDay = routeResponse.Result.Date,
                    StartTime = routeResponse.Result.Time,
                    Status = ticketDetail.Status,
                    Services = await GetServicesInTicket(ticketDetail.TicketDetailID)

                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<ServiceInTicketModel>> GetServicesInTicket(Guid ticketDetailId)
        {
            var listServices = await _unitOfWork.TicketDetail_ServiceRepository
                                                .GetAll()
                                                .Where(s => s.TicketDetailID.Equals(ticketDetailId) && s.Status.Equals(SD.Booking_ServiceStatus.PAYING_TICKETSERVICE))
                                                .Select(s => s.ServiceID)
                                                .ToListAsync();
            var rs = new List<ServiceInTicketModel>();
            foreach (var service in listServices)
            {
                var serviceResponse = await _unitOfWork.ServiceRepository.FindByCondition(s => s.ServiceID.Equals(service) && s.Status.Equals(SD.GeneralStatus.ACTIVE))
                                                                         .FirstOrDefaultAsync();
                var station_service = await _unitOfWork.Station_ServiceRepository.FindByCondition(st => st.ServiceID.Equals(service) && st.Status.Equals(SD.GeneralStatus.ACTIVE))
                                                                         .FirstOrDefaultAsync();
                var station = await _unitOfWork.StationRepository.FindByCondition(st => st.StationID.Equals(station_service.StationID) && st.Status.Equals(SD.GeneralStatus.ACTIVE))
                                                                         .FirstOrDefaultAsync();
                var service_ticket = await _unitOfWork.TicketDetail_ServiceRepository.FindByCondition(st => st.TicketDetailID.Equals(ticketDetailId) && st.ServiceID.Equals(service) && st.Status.Equals(SD.Booking_ServiceStatus.PAYING_TICKETSERVICE))
                                                                                    .FirstOrDefaultAsync();

                var serviceResult = new ServiceInTicketModel
                {
                    ServiceName = serviceResponse.Name,
                    Station = station.Name,
                    Quantity = service_ticket.Quantity ?? 0,
                    TotalPrice = (service_ticket.Price ?? 0) * (service_ticket.Quantity ?? 0)
                };

                rs.Add(serviceResult);
            }
            return rs;
        }

        public async Task<bool> VerifyTicketDetail(string qrCode)
        {
            try
            {
                var ticketDetail = await _unitOfWork.TicketDetailRepository
                                                   .GetAll()
                                                   .Where(t => t.QRCode.Equals(qrCode) && t.Status.Equals(SD.Booking_TicketStatus.UNUSED_TICKET))
                                                   .FirstOrDefaultAsync();
                if (ticketDetail == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("QR code"));
                }
                ticketDetail.Status = SD.Booking_TicketStatus.USED_TICKET;
                var ticketUpdate = _unitOfWork.TicketDetailRepository.Update(ticketDetail);
                if (ticketUpdate == null)
                {
                    throw new InternalServerErrorException(SD.Notification.Internal("VÉ XE", "KHI CẬP NHẬT VÉ XE"));
                }
                _unitOfWork.Complete();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
