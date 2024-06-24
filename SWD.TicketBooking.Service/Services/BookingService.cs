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
                        throw new BadRequestException("Mismatch in quantity or total bill.");
                    }
                    if (bookingModel.AddOrUpdateBookingModel.IsBalance == true)
                    {
                       
                        totalBalance = await _unitOfWork.UserRepository.FindByCondition(_ => _.UserID == bookingModel.AddOrUpdateBookingModel.UserID).Select(_ => _.Balance).FirstOrDefaultAsync();
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
                        else throw new BadRequestException("Blance does not enough to serve this service!");
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
                    Parallel.ForEach(bookingModel.AddOrUpdateTicketModels, async (ticketDetailItem) =>
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
                            Status = SD.Booking_TicketStatus.NOTPAYING_TICKET
                        };
                        var ticketRs = await _unitOfWork.TicketDetailRepository.AddAsync(newTicketDetail);

                        if (ticketDetailItem.AddOrUpdateServiceModels != null && ticketDetailItem.AddOrUpdateServiceModels.Any())
                        {
                            Parallel.ForEach(ticketDetailItem.AddOrUpdateServiceModels, async (ticketService) =>
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
                                    Status = SD.Booking_ServiceStatus.NOTPAYING_TICKETSERVICE
                                };
                                await _unitOfWork.TicketDetail_ServiceRepository.AddAsync(newTicketService);
                            });
                        }
                        else
                        {
                            isValid = false;
                        }
                    });
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
                    double totalBalance = 0;
                    var newBooking = new Booking();
                    Parallel.ForEach(bookingModel.AddOrUpdateTicketModels, async (ticket) =>
                    {
                        totalPrice += ticket.Price;

                        if (ticket.AddOrUpdateServiceModels != null && ticket.AddOrUpdateServiceModels.Any())
                        {
                            foreach (var service in ticket.AddOrUpdateServiceModels)
                            {
                                totalPrice += service.Price * service.Quantity;
                            }
                        }
                    });
                    if (bookingModel.AddOrUpdateBookingModel.Quantity != totalQuantity ||
                      bookingModel.AddOrUpdateBookingModel.TotalBill != totalPrice)
                    {
                        throw new BadRequestException("Mismatch in quantity or total bill.");
                    }
                    if (bookingModel.AddOrUpdateBookingModel.IsBalance == true)
                    {

                        totalBalance = await _unitOfWork.UserRepository.FindByCondition(_ => _.UserID == bookingModel.AddOrUpdateBookingModel.UserID).Select(_ => _.Balance).FirstOrDefaultAsync();
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
                        else throw new BadRequestException("Blance does not enough to serve this service!");
                    }            
                    await _unitOfWork.BookingRepository.AddAsync(newBooking);
                    Parallel.ForEach(bookingModel.AddOrUpdateTicketModels, async (ticketDetailItem) =>
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
                            Status = SD.Booking_TicketStatus.NOTPAYING_TICKET
                        };
                        var ticketRs = await _unitOfWork.TicketDetailRepository.AddAsync(newTicketDetail);

                        if (ticketDetailItem.AddOrUpdateServiceModels != null && ticketDetailItem.AddOrUpdateServiceModels.Any())
                        {
                            Parallel.ForEach(ticketDetailItem.AddOrUpdateServiceModels, async (ticketService) =>
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
                                    Status = SD.Booking_ServiceStatus.NOTPAYING_TICKETSERVICE
                                };
                                await _unitOfWork.TicketDetail_ServiceRepository.AddAsync(newTicketService);
                            });
                        }
                        else
                        {
                            isValid = false;
                        }
                    });
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

                var findBooking = await _unitOfWork.BookingRepository.FindByCondition(_ => _.BookingID == bookingID).Include(_ => _.Trip.Route_Company.Route).Include(_ => _.User).FirstOrDefaultAsync();
                if (findBooking == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("Booking"));
                }
                findBooking.BookingTime = DateTime.Now;
                findBooking.PaymentStatus = SD.BookingStatus.PAYING_BOOKING;
                _unitOfWork.BookingRepository.Update(findBooking);
                var findTicket = await _unitOfWork.TicketDetailRepository.GetAll().Include(_ => _.Booking).Where(_ => _.BookingID == bookingID).ToListAsync();
                Parallel.ForEach(findTicket, async (ticket) =>
                {
                    string qr = GenerateCode();
                    ticket.Status = SD.Booking_TicketStatus.UNUSED_TICKET;
                    var checkQRCodeExisted = await _unitOfWork.TicketDetailRepository.GetAll().FirstOrDefaultAsync(_ => _.QRCode == qr);
                    while (checkQRCodeExisted != null)
                    {
                        qr = GenerateCode();
                        checkQRCodeExisted = await _unitOfWork.TicketDetailRepository.GetAll().FirstOrDefaultAsync(_ => _.QRCode == qr);
                    }

                    ticket.QRCode = qr;
                    var imagePathQr = FirebasePathName.TICKET_BOOKINGQR + $"{ticket.TicketDetailID}";
                    var imageUploadQrResult = await _firebaseService.UploadFileToFirebase(GenerateQRCode(qr), imagePathQr);
                    if (imageUploadQrResult.IsSuccess)
                    {
                        ticket.QRCodeImage = (string)imageUploadQrResult.Result;
                    }
                    _unitOfWork.TicketDetailRepository.Update(ticket);
                    var findService = await _unitOfWork.TicketDetail_ServiceRepository.GetAll().Where(_ => _.TicketDetailID == ticket.TicketDetailID).ToListAsync();
                    Parallel.ForEach(findService, async (service) =>
                    {
                        service.Status = SD.Booking_ServiceStatus.PAYING_TICKETSERVICE;
                        _unitOfWork.TicketDetail_ServiceRepository.Update(service);
                    });
                });
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
                Parallel.ForEach(findTicket, async (ticket) =>
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
                });
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
                var getEmail = await _unitOfWork.BookingRepository.FindByCondition(_ => _.BookingID == bookingID).FirstOrDefaultAsync();
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

        public async Task<ActionOutcome> GetEmailBooking(Guid bookingID)
        {
            try
            {
                var result = new ActionOutcome();
                var getEmail = await _unitOfWork.BookingRepository.FindByCondition(_ => _.BookingID == bookingID).Select(_ => _.Email).FirstOrDefaultAsync();
                if (getEmail == null)
                {
                    throw new NotFoundException("Not Found!");
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