using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SWD.TicketBooking.API.Installer;
using SWD.TicketBooking.API.RequestModels;
using SWD.TicketBooking.API.ResponseModels;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Services;

namespace SWD.TicketBooking.API.Controllers
{
    [Route("feedback-management")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IMapper _mapper;
        public readonly IFeedbackService _feedbackService; 
        private readonly IDistributedCache _cache;
        private readonly IResponseCacheService _responseCacheService;
        private readonly ILogger<TripController> _logger;
        public FeedbackController(IDistributedCache cache, IResponseCacheService responseCacheService, ILogger<TripController> logger,  IFeedbackService feedbackService, IMapper mapper)
        {
            _feedbackService = feedbackService;
            _mapper = mapper;
            _cache = cache;
            _responseCacheService = responseCacheService;
            _logger = logger;
        }
        [HttpPost("managed-feedbacks")]
        public async Task<IActionResult> CreateRating([FromForm] FeedbackRequest feedbackRequest)
        {
            var feedback = _mapper.Map<FeedbackRequestModel>(feedbackRequest);
            await _feedbackService.CreateRating(feedback);
            await _responseCacheService.RemoveCacheResponseAsync("/trip-management/managed-trips/from-city");
            await _responseCacheService.RemoveCacheResponseAsync("/feedback-management/managed-feedbacks/trips");
            return Ok();
        }

        [HttpGet("managed-feedbacks/trips/{tripID}/rate-scales/{filter}")]
        [Cache(1200)]
        public async Task<IActionResult> GetAllFeedbackInTrip(Guid tripID, int filter, int pageNumber =1, int pageSize =5)
        {
            var fb = _mapper.Map<FeedbackInTripResponse>(await _feedbackService.GetAllFeedbackInTrip(tripID, pageNumber,pageSize, filter));
            return Ok(fb);  
        }
    }
}
