using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.TicketBooking.API.Common.RequestModels;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("rating")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IMapper _mapper;
        public readonly FeedbackService _feedbackService;
        public FeedbackController(FeedbackService feedbackService, IMapper mapper)
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
    }
}
