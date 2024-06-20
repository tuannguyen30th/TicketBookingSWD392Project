using Microsoft.AspNetCore.Http;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Dtos.BackendService;
using SWD.TicketBooking.Service.Dtos.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.IServices
{
    public interface IBookingService
    {
        Task<ActionOutcome> AddOrUpdateBooking(BookingModel bookingModel, HttpContext context);
        Task<List<SendMailBookingModel.MailBookingModel>> UpdateStatusBooking(Guid bookingID);
        Task<ActionOutcome> GetBooking(Guid bookingID);
        Task<ActionOutcome> GetEmailBooking(Guid bookingID);
    }
}
