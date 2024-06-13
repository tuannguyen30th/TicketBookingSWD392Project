using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos.Booking;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SWD.TicketBooking.Service.Services
{
    public class BookingService
    {
        private readonly IRepository<Booking, Guid> _bookingRepository;
        private readonly IRepository<TicketDetail, Guid> _ticketDetailRepository;
        private readonly IRepository<TicketDetail_Service, Guid> _ticketDetailServiceRepository;
        private readonly IMapper _mapper;
        public BookingService(IRepository<Booking, Guid> bookingRepository, IRepository<TicketDetail, Guid> ticketDetailRepository, IRepository<TicketDetail_Service, Guid> ticketDetailServiceRepository, IMapper mapper)
        {
            _ticketDetailRepository = ticketDetailRepository;
            _ticketDetailServiceRepository = ticketDetailServiceRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }
        public async Task<BookingModel> AddOrUpdateBooking(BookingModel bookingModel)
        {
            bool isValid = true;
            BookingModel result = null;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (bookingModel.AddOrUpdateBookingModel == null || bookingModel.AddOrUpdateTicketModels == null)
                    {
                        throw new BadRequestException("BookingModel or TicketModels cannot be null.");
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
                        Quantity = bookingModel.AddOrUpdateBookingModel.Quantity,
                        TotalBill = bookingModel.AddOrUpdateBookingModel.TotalBill,
                        Status = SD.BOOKING_NOTCOMPLETED,
                    };

                    await _bookingRepository.AddAsync(newBooking);
                    await _bookingRepository.Commit();

                    foreach (var ticketDetailItem in bookingModel.AddOrUpdateTicketModels)
                    {
                        var newTicketDetail = new TicketDetail{
                            TicketDetailID = Guid.NewGuid() ,
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
                                var newTicketService = new TicketDetail_Service
                                {
                                    TicketDetail_ServiceID = Guid.NewGuid(),
                                    TicketDetailID = ticketRs.TicketDetailID,
                                    StationID = ticketService.StationID,
                                    ServiceID = ticketService.ServiceID,
                                    Quantity = ticketService.Quantity,
                                    Price = ticketService.Price,
                                    Status= SD.NOTPAYING_TICKETSERVICE
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
                    return bookingModel;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occurred while adding or updating booking.", ex);
                }
            }
        }

    }
}
