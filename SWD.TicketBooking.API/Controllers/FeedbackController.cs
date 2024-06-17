using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.RequestModels;
using SWD.TicketBooking.API.Common.ResponseModels;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("rating")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IMapper _mapper;
        public readonly IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService, IMapper mapper)
        {
            _feedbackService = feedbackService;
            _mapper = mapper;
        }
        [HttpPost("feedback-trip")]
        public async Task<IActionResult> CreateRating([FromForm] FeedbackRequest feedbackRequest)
        {
            var feedback = _mapper.Map<FeedbackRequestModel>(feedbackRequest);
            await _feedbackService.CreateRating(feedback);
            return Ok();
        }

        [HttpGet("feedback-in-trip/{templateID}/{filter}")]
        public async Task<IActionResult> GetAllFeedbackInTrip(Guid templateID, int filter, int pageNumber =1, int pageSize =5)
        {
            var fb = _mapper.Map<FeedbackInTripResponse>(await _feedbackService.GetAllFeedbackInTrip(templateID, pageNumber,pageSize, filter));
            return Ok(fb);
        }
    }
}
