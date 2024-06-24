using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGatewayService _paymentGatewayService;

        public readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IFirebaseService firebaseService, IPaymentGatewayService paymentGatewayService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;       
            _paymentGatewayService = paymentGatewayService;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }

        public async Task<ActionOutcome> AddOrUpdateBookingVNPayPayment(BookingModel bookingModel, HttpContext context)
        {
            var result = new ActionOutcome();
            bool isValid = true;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (bookingModel.AddOrUpdateBookingModel == null || bookingModel.AddOrUpdateTicketModels == null)
                    {
                        throw new BadRequestException("BookingModel hoặc TicketModels không được bỏ trống!");
                    }
                    if (bookingModel.AddOrUpdateBookingModel.UserID == null
                        || bookingModel.AddOrUpdateBookingModel.TripID == null
                        || bookingModel.AddOrUpdateBookingModel.Quantity <= 0
                        || bookingModel.AddOrUpdateBookingModel.TotalBill <= 0
                        || bookingModel.AddOrUpdateBookingModel.FullName == null
                        || bookingModel.AddOrUpdateBookingModel.PhoneNumber == null
                        || bookingModel.AddOrUpdateBookingModel.Email == null)
                    {
                        throw new BadRequestException("Những trường trong BookingModel không được bỏ trống!");
                    }
                    if (!IsValidEmail(bookingModel.AddOrUpdateBookingModel.Email) || !IsValidPhoneNumber(bookingModel.AddOrUpdateBookingModel.PhoneNumber))
                    {
                        throw new BadRequestException("Email hoặc số điện thoại không đúng với quy định!");
                    }

                    var totalQuantity = bookingModel.AddOrUpdateTicketModels
                                                    .Select(_ => _.SeatCode)
                                                    .Count();
                    double totalPrice = 0;
                    double totalBalance = 0;
                    var newBooking = new Booking();
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
                        throw new BadRequestException("Sai khác về số lượng hoặc tổng hóa đơn.");
                    }
                    if (bookingModel.AddOrUpdateBookingModel.IsBalance == true)
                    {                   
                        totalBalance = await _unitOfWork.UserRepository
                                                        .FindByCondition(_ => _.UserID == bookingModel.AddOrUpdateBookingModel.UserID)
                                                        .Select(_ => _.Balance)
                                                        .FirstOrDefaultAsync();
                        if (totalBalance > 0)
                        {
                            newBooking = new Booking
                            {
                                BookingID = Guid.NewGuid(),
                                UserID = bookingModel.AddOrUpdateBookingModel.UserID,
                                TripID = bookingModel.AddOrUpdateBookingModel.TripID,
                                FullName = bookingModel.AddOrUpdateBookingModel.FullName,
                                PhoneNumber = bookingModel.AddOrUpdateBookingModel.PhoneNumber,
                                Email = bookingModel.AddOrUpdateBookingModel.Email,
                                Quantity = bookingModel.AddOrUpdateBookingModel.Quantity,
                                TotalBill = bookingModel.AddOrUpdateBookingModel.TotalBill,
                                TotalVnpayPayment = bookingModel.AddOrUpdateBookingModel.TotalBill - totalBalance,
                                TotalBalancePayment = totalBalance,
                                PaymentStatus = SD.BookingStatus.NOTPAYING_BOOKING,
                            };
                        }
                        else throw new BadRequestException("Số dư không đủ để thực hiện dịch vụ này!");
                    }
                    if (bookingModel.AddOrUpdateBookingModel.IsBalance == false)
                    {
                        newBooking = new Booking
                        {
                            BookingID = Guid.NewGuid(),
                            UserID = bookingModel.AddOrUpdateBookingModel.UserID,
                            TripID = bookingModel.AddOrUpdateBookingModel.TripID,
                            FullName = bookingModel.AddOrUpdateBookingModel.FullName,
                            PhoneNumber = bookingModel.AddOrUpdateBookingModel.PhoneNumber,
                            Email = bookingModel.AddOrUpdateBookingModel.Email,
                            Quantity = bookingModel.AddOrUpdateBookingModel.Quantity,
                            TotalBill = bookingModel.AddOrUpdateBookingModel.TotalBill,
                            TotalVnpayPayment = bookingModel.AddOrUpdateBookingModel.TotalBill,
                            TotalBalancePayment = 0,
                            PaymentStatus = SD.BookingStatus.NOTPAYING_BOOKING,
                        };
                    }
                    await _unitOfWork.BookingRepository.AddAsync(newBooking);
                   foreach(var ticketDetailItem in bookingModel.AddOrUpdateTicketModels) 
                    {
                        if (ticketDetailItem.TicketType_TripID == null
                            || ticketDetailItem.Price <= 0
                            || ticketDetailItem.SeatCode == null)
                        {
                            throw new BadRequestException("Những trường trong TicketDetail không được bỏ trống!");
                        }
                        var newTicketDetail = new TicketDetail
                        {
                            TicketDetailID = Guid.NewGuid(),
                            TicketType_TripID = ticketDetailItem.TicketType_TripID,
                            BookingID = newBooking.BookingID,
                            Price = ticketDetailItem.Price,
                            SeatCode = ticketDetailItem.SeatCode,
                            Status = SD.Booking_TicketStatus.NOTPAYING_TICKET
                        };
                        var ticketRs = await _unitOfWork.TicketDetailRepository.AddAsync(newTicketDetail);

                        if (ticketDetailItem.AddOrUpdateServiceModels != null && ticketDetailItem.AddOrUpdateServiceModels.Any())
                        {
                            foreach(var ticketService in ticketDetailItem.AddOrUpdateServiceModels)
                            {
                                if (ticketService.StationID == null
                                    || ticketService.ServiceID == null
                                    || ticketService.Quantity <= 0
                                    || ticketService.Price <= 0)
                                {
                                    throw new BadRequestException("Những trường trong Service không được bỏ trống!");
                                }
                                var newTicketService = new TicketDetail_Service
                                {
                                    TicketDetail_ServiceID = Guid.NewGuid(),
                                    TicketDetailID = ticketRs.TicketDetailID,
                                    StationID = ticketService.StationID,
                                    ServiceID = ticketService.ServiceID,
                                    Quantity = ticketService.Quantity,
                                    Price = ticketService.Price,
                                    Status = SD.Booking_ServiceStatus.NOTPAYING_TICKETSERVICE
                                };
                                await _unitOfWork.TicketDetail_ServiceRepository.AddAsync(newTicketService);
                            };
                        }
                        else
                        {
                            isValid = false;
                        }
                    };
                    _unitOfWork.Complete();
                    scope.Complete();
                    var payment = new PaymentInformationModel
                    {
                        AccountID = newBooking.UserID.ToString(),
                        Amount = (double)newBooking.TotalVnpayPayment,
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
        public async Task<ActionOutcome> AddOrUpdateBookingBalancePayment(BookingModel bookingModel)
        {
            var result = new ActionOutcome();
            bool isValid = true;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (bookingModel.AddOrUpdateBookingModel == null || bookingModel.AddOrUpdateTicketModels == null)
                    {
                        throw new BadRequestException("BookingModel hoặc TicketModels không được bỏ trống!");
                    }
                    if (bookingModel.AddOrUpdateBookingModel.UserID == null
                        || bookingModel.AddOrUpdateBookingModel.TripID == null
                        || bookingModel.AddOrUpdateBookingModel.Quantity <= 0
                        || bookingModel.AddOrUpdateBookingModel.TotalBill <= 0
                        || bookingModel.AddOrUpdateBookingModel.FullName == null
                        || bookingModel.AddOrUpdateBookingModel.PhoneNumber == null
                        || bookingModel.AddOrUpdateBookingModel.Email == null)
                    {
                        throw new BadRequestException("Những trường trong BookingModel không được bỏ trống!");
                    }
                    if (!IsValidEmail(bookingModel.AddOrUpdateBookingModel.Email) || !IsValidPhoneNumber(bookingModel.AddOrUpdateBookingModel.PhoneNumber))
                    {
                        throw new BadRequestException("Email hoặc số điện thoại không đúng với quy định!");
                    }

                    var totalQuantity = bookingModel.AddOrUpdateTicketModels
                                                    .Select(_ => _.SeatCode)
                                                    .Count();
                    double totalPrice = 0;
                    double totalBalance = 0;
                    var newBooking = new Booking();
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
                        throw new BadRequestException("Sai khác về số lượng hoặc tổng hóa đơn.");
                    }
                    if (bookingModel.AddOrUpdateBookingModel.IsBalance == true)
                    { 
                        totalBalance = await _unitOfWork.UserRepository
                                                        .FindByCondition(_ => _.UserID == bookingModel.AddOrUpdateBookingModel.UserID)
                                                        .Select(_ => _.Balance)
                                                        .FirstOrDefaultAsync();
                        if (totalBalance >= bookingModel.AddOrUpdateBookingModel.TotalBill)
                        {
                            newBooking = new Booking
                            {
                                BookingID = Guid.NewGuid(),
                                UserID = bookingModel.AddOrUpdateBookingModel.UserID,
                                TripID = bookingModel.AddOrUpdateBookingModel.TripID,
                                FullName = bookingModel.AddOrUpdateBookingModel.FullName,
                                PhoneNumber = bookingModel.AddOrUpdateBookingModel.PhoneNumber,
                                Email = bookingModel.AddOrUpdateBookingModel.Email,
                                Quantity = bookingModel.AddOrUpdateBookingModel.Quantity,
                                TotalBill = bookingModel.AddOrUpdateBookingModel.TotalBill,
                                TotalVnpayPayment = 0,
                                TotalBalancePayment = bookingModel.AddOrUpdateBookingModel.TotalBill,
                                PaymentStatus = SD.BookingStatus.NOTPAYING_BOOKING,
                            };
                        }
                        else throw new BadRequestException("Số dư không đủ để thực hiện dịch vụ này!");
                    }
                    await _unitOfWork.BookingRepository.AddAsync(newBooking);
                    foreach (var ticketDetailItem in bookingModel.AddOrUpdateTicketModels)
                    {
                        {
                            if (ticketDetailItem.TicketType_TripID == null
                                || ticketDetailItem.Price <= 0
                                || ticketDetailItem.SeatCode == null)
                            {
                                throw new BadRequestException("Những trường trong TicketDetail không được bỏ trống!");
                            }
                            var newTicketDetail = new TicketDetail
                            {
                                TicketDetailID = Guid.NewGuid(),
                                TicketType_TripID = ticketDetailItem.TicketType_TripID,
                                BookingID = newBooking.BookingID,
                                Price = ticketDetailItem.Price,
                                SeatCode = ticketDetailItem.SeatCode,
                                Status = SD.Booking_TicketStatus.NOTPAYING_TICKET
                            };
                            var ticketResult = await _unitOfWork.TicketDetailRepository.AddAsync(newTicketDetail);

                            if (ticketDetailItem.AddOrUpdateServiceModels != null && ticketDetailItem.AddOrUpdateServiceModels.Any())
                            {
                                foreach (var ticketService in ticketDetailItem.AddOrUpdateServiceModels)
                                {
                                    if (ticketService.StationID == null
                                        || ticketService.ServiceID == null
                                        || ticketService.Quantity <= 0
                                        || ticketService.Price <= 0)
                                    {
                                        throw new BadRequestException("Những trường trong Service không được bỏ trống!");
                                    }
                                    var newTicketService = new TicketDetail_Service
                                    {
                                        TicketDetail_ServiceID = Guid.NewGuid(),
                                        TicketDetailID = ticketResult.TicketDetailID,
                                        StationID = ticketService.StationID,
                                        ServiceID = ticketService.ServiceID,
                                        Quantity = ticketService.Quantity,
                                        Price = ticketService.Price,
                                        Status = SD.Booking_ServiceStatus.NOTPAYING_TICKETSERVICE
                                    };
                                    await _unitOfWork.TicketDetail_ServiceRepository.AddAsync(newTicketService);
                                };
                            }
                            else
                            {
                                isValid = false;
                            }
                        };
                    }
                    _unitOfWork.Complete();
                    await UpdateStatusBooking(newBooking.BookingID);
                    scope.Complete();
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

                var findBooking = await _unitOfWork.BookingRepository.FindByCondition(_ => _.BookingID == bookingID)
                                                                     .Include(_ => _.Trip.Route_Company.Route)
                                                                     .Include(_ => _.User)
                                                                     .FirstOrDefaultAsync();
                if (findBooking == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("Booking"));
                }
                findBooking.BookingTime = DateTime.Now;
                findBooking.PaymentStatus = SD.BookingStatus.PAYING_BOOKING;
                _unitOfWork.BookingRepository.Update(findBooking);
                var findTicket = await _unitOfWork.TicketDetailRepository.GetAll()
                                                                         .Include(_ => _.Booking)
                                                                         .Where(_ => _.BookingID == bookingID)
                                                                         .ToListAsync();
                foreach(var ticket in findTicket)
                {
                    string qr = GenerateCode();
                    ticket.Status = SD.Booking_TicketStatus.UNUSED_TICKET;
                    var checkQRCodeExisted = await _unitOfWork.TicketDetailRepository
                                                              .GetAll()
                                                              .FirstOrDefaultAsync(_ => _.QRCode == qr);

                    while (checkQRCodeExisted != null)
                    {
                        qr = GenerateCode();
                        checkQRCodeExisted = await _unitOfWork.TicketDetailRepository
                                                              .GetAll()
                                                              .FirstOrDefaultAsync(_ => _.QRCode == qr);
                    }
                    ticket.QRCode = qr;
                    var imagePathQr = FirebasePathName.TICKET_BOOKINGQR + $"{ticket.TicketDetailID}";
                    var imageUploadQrResult = await _firebaseService.UploadFileToFirebase(GenerateQRCode(qr), imagePathQr);
                    if (imageUploadQrResult.IsSuccess)
                    {
                        ticket.QRCodeImage = (string)imageUploadQrResult.Result;
                    }
                    _unitOfWork.TicketDetailRepository.Update(ticket);

                    var findService = await _unitOfWork.TicketDetail_ServiceRepository
                                                       .GetAll()
                                                       .Where(_ => _.TicketDetailID == ticket.TicketDetailID)
                                                       .ToListAsync();

                   foreach(var service in findService)
                    {
                        service.Status = SD.Booking_ServiceStatus.PAYING_TICKETSERVICE;
                        _unitOfWork.TicketDetail_ServiceRepository.Update(service);
                    };                
                };              
                if (findBooking.TotalBalancePayment > 0)
                {
                    findBooking.User.Balance -= findBooking.TotalBalancePayment;
                    if(findBooking.User.Balance < 0)
                    {
                        findBooking.User.Balance = 0;
                    }
                    _unitOfWork.UserRepository.Update(findBooking.User);
                }
                _unitOfWork.Complete();
                foreach(var ticket in findTicket)
                {
                    var mailBookingServices = await _unitOfWork.TicketDetail_ServiceRepository
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
                        QrCodeImage = ticket.QRCodeImage,
                        MailBookingServices = mailBookingServices
                    };
                    mailBooking.Add(mailBookingModel);
                };
                return mailBooking;
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
                var findTicket = await _unitOfWork.TicketDetailRepository.GetByIdAsync(ticketDetailID);
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
                    var findUser = await _unitOfWork.TicketDetailRepository
                                                    .FindByCondition(_ => _.TicketDetailID == ticketDetailID)
                                                    .Select(_ => _.Booking.User)
                                                    .FirstOrDefaultAsync();
                    findUser.Balance = totalBillCancel * 0.7;
                    _unitOfWork.UserRepository.Update(findUser);
                    var otherTickets = await _unitOfWork.TicketDetailRepository
                                                        .FindByCondition(_ => _.BookingID == findTicket.BookingID && _.TicketDetailID != ticketDetailID)
                                                        .ToListAsync();

                    if (otherTickets == null || !otherTickets.Any())
                    {
                        var findBooking = await _unitOfWork.BookingRepository.GetByIdAsync(findTicket.BookingID);
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
        public async Task<ActionOutcome> GetBooking(Guid bookingID)
        {
            try
            {
                var result = new ActionOutcome();
                var getEmail = await _unitOfWork.BookingRepository.FindByCondition(_ => _.BookingID == bookingID).FirstOrDefaultAsync();
                if (getEmail == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("Booking"));
                }
                result.Result = getEmail;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ActionOutcome> GetEmailBooking(Guid bookingID)
        {
            try
            {
                var result = new ActionOutcome();
                var getEmail = await _unitOfWork.BookingRepository
                                                .FindByCondition(_ => _.BookingID == bookingID)
                                                .Select(_ => _.Email)
                                                .FirstOrDefaultAsync();
                if (getEmail == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("Booking"));
                }
                result.Value = getEmail;
                return result;
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