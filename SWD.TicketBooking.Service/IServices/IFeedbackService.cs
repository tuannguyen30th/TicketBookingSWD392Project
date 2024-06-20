using SWD.TicketBooking.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.IServices
{
    public interface IFeedbackService
    {
        Task<bool> CreateRating(FeedbackRequestModel ratingModel);
        Task<TripFeedbackModel> GetAllFeedbackInTrip(Guid tripID, int pageNumber, int pageSize, int filter);
    }
}
