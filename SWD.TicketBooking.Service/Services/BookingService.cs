using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.BackendService;
using SWD.TicketBooking.Service.Dtos.Booking;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services.PaymentService;
using SWD.TicketBooking.Service.Utilities;
using System.Text.RegularExpressions;
using System.Transactions;

namespace SWD.TicketBooking.Service.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Booking, Guid> _bookingRepository;
        private readonly IRepository<TicketDetail, Guid> _ticketDetailRepository;
        private readonly IRepository<TicketDetail_Service, Guid> _ticketDetailServiceRepository;
        private readonly IPaymentGatewayService _paymentGatewayService;
        public readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;

        public BookingService(IFirebaseService firebaseService, IPaymentGatewayService paymentGatewayService, IRepository<Booking, Guid> bookingRepository, IRepository<TicketDetail, Guid> ticketDetailRepository, IRepository<TicketDetail_Service, Guid> ticketDetailServiceRepository, IMapper mapper)
        {
            _ticketDetailRepository = ticketDetailRepository;
            _ticketDetailServiceRepository = ticketDetailServiceRepository;
            _bookingRepository = bookingRepository;
            _paymentGatewayService = paymentGatewayService;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }

        public async Task<ActionOutcome> AddOrUpdateBooking(BookingModel bookingModel, HttpContext context)
        {
            var result = new ActionOutcome();
            bool isValid = true;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (bookingModel.AddOrUpdateBookingModel == null || bookingModel.AddOrUpdateTicketModels == null)
                    {
                        throw new BadRequestException("BookingModel or TicketModels cannot be null!");
                    }
                    if (bookingModel.AddOrUpdateBookingModel.UserID == null
                        || bookingModel.AddOrUpdateBookingModel.TripID == null
                        || bookingModel.AddOrUpdateBookingModel.Quantity <= 0
                        || bookingModel.AddOrUpdateBookingModel.TotalBill <= 0
                        || bookingModel.AddOrUpdateBookingModel.FullName == null
                        || bookingModel.AddOrUpdateBookingModel.PhoneNumber == null
                        || bookingModel.AddOrUpdateBookingModel.Email == null)
                    {
                        throw new BadRequestException("Field in BookingModel cannot be null!");
                    }
                    if (!IsValidEmail(bookingModel.AddOrUpdateBookingModel.Email) || !IsValidPhoneNumber(bookingModel.AddOrUpdateBookingModel.PhoneNumber))
                    {
                        throw new BadRequestException("Email or PhoneNumber does not correct format!");
                    }

                    var totalQuantity = bookingModel.AddOrUpdateTicketModels.Select(_ => _.SeatCode).Count();
                    double totalPrice = 0;

                    foreach (var ticket in bookingModel.AddOrUpdateTicketModels)
                    {
                        totalPrice += ticket.Price;

                        if (ticket.AddOrUpdateServiceModels != null && ticket.AddOrUpdateServiceModels.Any())
                        {
                            foreach (var service in ticket.AddOrUpdateServiceModels)
                            {
                                totalPrice += service.Price * service.Quantity;
                            }
                        }
                    }

                    if (bookingModel.AddOrUpdateBookingModel.Quantity != totalQuantity ||
                        bookingModel.AddOrUpdateBookingModel.TotalBill != totalPrice)
                    {
                        throw new BadRequestException("Mismatch in quantity or total bill.");
                    }

                    var newBooking = new Booking
                    {
                        BookingID = Guid.NewGuid(),
                        UserID = bookingModel.AddOrUpdateBookingModel.UserID,
                        TripID = bookingModel.AddOrUpdateBookingModel.TripID,
                        FullName = bookingModel.AddOrUpdateBookingModel.FullName,
                        PhoneNumber = bookingModel.AddOrUpdateBookingModel.PhoneNumber,
                        Email = bookingModel.AddOrUpdateBookingModel.Email,
                        Quantity = bookingModel.AddOrUpdateBookingModel.Quantity,
                        TotalBill = bookingModel.AddOrUpdateBookingModel.TotalBill,
                        Status = SD.BOOKING_NOTCOMPLETED,
                    };

                    await _bookingRepository.AddAsync(newBooking);
                    await _bookingRepository.Commit();
                    foreach (var ticketDetailItem in bookingModel.AddOrUpdateTicketModels)
                    {
                        if (ticketDetailItem.TicketType_TripID == null
                            || ticketDetailItem.Price <= 0
                            || ticketDetailItem.SeatCode == null)
                        {
                            throw new BadRequestException("Field in TicketDetail cannot be null!");
                        }
                        var newTicketDetail = new TicketDetail
                        {
                            TicketDetailID = Guid.NewGuid(),
                            TicketType_TripID = ticketDetailItem.TicketType_TripID,
                            BookingID = newBooking.BookingID,
                            Price = ticketDetailItem.Price,
                            SeatCode = ticketDetailItem.SeatCode,
                            Status = SD.NOTPAYING_TICKET
                        };
                        var ticketRs = await _ticketDetailRepository.AddAsync(newTicketDetail);

                        if (ticketDetailItem.AddOrUpdateServiceModels != null && ticketDetailItem.AddOrUpdateServiceModels.Any())
                        {
                            foreach (var ticketService in ticketDetailItem.AddOrUpdateServiceModels)
                            {
                                if (ticketService.StationID == null
                                    || ticketService.ServiceID == null
                                    || ticketService.Quantity <= 0
                                    || ticketService.Price <= 0)
                                {
                                    throw new BadRequestException("Field in Service cannot be null!");
                                }
                                var newTicketService = new TicketDetail_Service
                                {
                                    TicketDetail_ServiceID = Guid.NewGuid(),
                                    TicketDetailID = ticketRs.TicketDetailID,
                                    StationID = ticketService.StationID,
                                    ServiceID = ticketService.ServiceID,
                                    Quantity = ticketService.Quantity,
                                    Price = ticketService.Price,
                                    Status = SD.NOTPAYING_TICKETSERVICE
                                };
                                await _ticketDetailServiceRepository.AddAsync(newTicketService);
                            }
                            await _ticketDetailServiceRepository.Commit();
                        }
                        else
                        {
                            isValid = false;
                        }
                    }
                    await _bookingRepository.Commit();
                    scope.Complete();
                    var payment = new PaymentInformationRequest
                    {
                        AccountID = newBooking.UserID.ToString(),
                        Amount = (double)newBooking.TotalBill,
                        CustomerName = newBooking.FullName,
                        BookingID = newBooking.BookingID.ToString(),
                    };
                    result.Result = await _paymentGatewayService.CreatePaymentUrlVnpay(payment, context);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
            return result;
        }

        public async Task<List<SendMailBookingModel.MailBookingModel>> UpdateStatusBooking(Guid bookingID)
        {
            try
            {
                var result = new ActionOutcome();
                var mailBooking = new List<SendMailBookingModel.MailBookingModel>();
                string qr = GenerateCode();
                var findBooking = await _bookingRepository.FindByCondition(_ => _.BookingID == bookingID).Include(_ => _.Trip.Route_Company.Route).FirstOrDefaultAsync();
                if (findBooking == null)
                {
                    throw new NotFoundException("Not Found!");
                }
                findBooking.BookingTime = DateTime.Now;
                findBooking.Status = SD.ACTIVE;
                findBooking.PaymentMethod = SD.PM_CASH;
                findBooking.Status = SD.BOOKING_COMPLETED;
                findBooking.QRCode = qr;
                var imagePathQr = FirebasePathName.BOOKINGQR + $"{findBooking.BookingID}";
                var imageUploadQrResult = await _firebaseService.UploadFileToFirebase(GenerateQRCode(qr), imagePathQr);
                if (imageUploadQrResult.IsSuccess)
                {
                    findBooking.QRCodeImage = (string)imageUploadQrResult.Result;
                }
                _bookingRepository.Update(findBooking);
                var findTicket = await _ticketDetailRepository.GetAll().Include(_ => _.Booking).Where(_ => _.BookingID == bookingID).ToListAsync();
                foreach (var ticket in findTicket)
                {
                    ticket.Status = SD.UNUSED_TICKET;
                    _ticketDetailRepository.Update(ticket);
                    var findService = await _ticketDetailServiceRepository.GetAll().Where(_ => _.TicketDetailID == ticket.TicketDetailID).ToListAsync();
                    foreach (var service in findService)
                    {
                        service.Status = SD.PAYING_TICKETSERVICE;
                        _ticketDetailServiceRepository.Update(service);
                    }
                }
                await _bookingRepository.Commit();
                await _ticketDetailRepository.Commit();
                await _ticketDetailServiceRepository.Commit();
                foreach (var ticket in findTicket)
                {
                    var mailBookingServices = await _ticketDetailServiceRepository
                                                    .GetAll()
                                                    .Where(_ => _.TicketDetailID == ticket.TicketDetailID)
                                                    .Select(_ => new SendMailBookingModel.MailBookingServiceModel
                                                    {
                                                        ServicePrice = _.Price,
                                                        AtStation = _.Station.Name
                                                    }).ToListAsync();
                    var mailBookingModel = new SendMailBookingModel.MailBookingModel
                    {
                        Email = findBooking.Email,
                        Price = ticket.Price,
                        FullName = findBooking.FullName,
                        FromTo = $"{findBooking.Trip.Route_Company.Route.StartLocation} - {findBooking.Trip.Route_Company.Route.EndLocation}",
                        StartTime = findBooking.Trip.StartTime.ToString("HH:mm"),
                        StartDate = findBooking.Trip.StartTime.ToString("yyyy-MM-dd"),
                        SeatCode = ticket.SeatCode,
                        TotalBill = findBooking.TotalBill.ToString("C"),
                        QrCodeImage = findBooking.QRCodeImage,
                        MailBookingServices = mailBookingServices
                    };
                   mailBooking.Add(mailBookingModel);
                }
                return mailBooking;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ActionOutcome> GetBooking(Guid bookingID)
        {
            try
            {
                var result = new ActionOutcome();
                var getEmail = await _bookingRepository.FindByCondition(_ => _.BookingID == bookingID).FirstOrDefaultAsync();
                if (getEmail == null)
                {
                    throw new NotFoundException("Not Found!");
                }
                result.Result = getEmail;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<string> GetEmailBooking(Guid bookingID)
        {
            try
            {
                var result = new ActionOutcome();
                var getEmail = await _bookingRepository.FindByCondition(_ => _.BookingID == bookingID).Select(_ => _.Email).FirstOrDefaultAsync();
                if (getEmail == null)
                {
                    throw new NotFoundException("Not Found!");
                }
                return getEmail;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, "^0\\d{9,11}$");
        }

        private IFormFile GenerateQRCode(string QRCodeText)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(QRCodeText, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);
            var ms = new MemoryStream(qrCodeImage);
            var formFile = new FormFile(ms, 0, ms.Length, "qrCode", "qrcode.png")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };
            return formFile;
        }

        private static string GenerateCode()
        {
            Random random = new Random();
            int code = random.Next(10000000, 100000000);
            return code.ToString();
        }
    }
}