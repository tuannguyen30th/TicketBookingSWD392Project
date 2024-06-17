
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;

namespace SWD.TicketBooking.Service.Services
{
    public class FeedbackService : IFeedbackService
    {
        public readonly IRepository<Feedback, Guid> _feedbackRepository;
        public readonly IRepository<Feedback_Image, Guid> _feedbackImageRepository;
        public readonly IRepository<Trip, Guid> _tripRepository;
        public readonly IRepository<Booking, Guid> _bookingRepository;
        public readonly IRepository<Feedback_Image, Guid> _fbImageRepository; 
        public readonly IRepository<User, Guid> _userRepository;
        public readonly IFirebaseService _firebaseService;
        public readonly IMapper _mapper;
        public FeedbackService(IRepository<Feedback, Guid> feedbackRepository, IRepository<Feedback_Image, Guid> feedbackImageRepository, IRepository<Booking, Guid> bookingRepository, IFirebaseService firebaseService, IMapper mapper, IRepository<Trip, Guid> tripRepository, IRepository<Feedback_Image, Guid> fbImageRepository, IRepository<User, Guid> userRepository)
        {
            _feedbackRepository = feedbackRepository;
            _bookingRepository = bookingRepository;
            _feedbackImageRepository = feedbackImageRepository;
            _firebaseService = firebaseService;
            _mapper = mapper;
            _tripRepository = tripRepository;
            _fbImageRepository = fbImageRepository;
            _userRepository = userRepository;
        }
        public async Task<bool> CreateRating(FeedbackRequestModel ratingModel)
        {
            try
            {
                var checkHadBooked = _bookingRepository.FindByCondition(_ => _.UserID == ratingModel.UserID && _.TripID == ratingModel.TripID).FirstOrDefaultAsync();
                if (checkHadBooked == null)
                {
                    throw new BadRequestException("Not had booked this Trip yet!");
                }
                var checkHadRated = await _feedbackRepository.FindByCondition(_ => _.UserID == ratingModel.UserID && _.TripID == ratingModel.TripID).FirstOrDefaultAsync();
                if (checkHadRated != null)
                {
                    throw new BadRequestException("Had rated before!");
                }
                if (ratingModel.Rating > 5 || ratingModel.Rating <= 0)
                {
                    throw new BadRequestException("The point does not suitable!");
                }
                var getTemplateID = await _tripRepository.FindByCondition(_ => _.TripID == ratingModel.TripID).Select(_ => _.TemplateID).FirstOrDefaultAsync();

                var newRating = new Feedback
                {
                    FeedbackID = Guid.NewGuid(),
                    UserID = ratingModel.UserID,
                    TripID = ratingModel.TripID,
                    Rating = ratingModel.Rating,
                    TemplateID = getTemplateID,
                    Description = ratingModel.Description,
                    Status = SD.ACTIVE,
                };
                await _feedbackRepository.AddAsync(newRating);
                await _feedbackRepository.Commit();
               
                var imageUrls = ratingModel.Files;
                foreach (var imageUrl in imageUrls)
                {
                    var newFeedbackImage = new Feedback_Image
                    {
                        Feedback_Image_ID = Guid.NewGuid(),
                        FeedbackID = newRating.FeedbackID,
                    };
                    await _feedbackImageRepository.AddAsync(newFeedbackImage);
                    await _feedbackImageRepository.Commit();
                    var imagePath = FirebasePathName.RATING + $"{newFeedbackImage.Feedback_Image_ID}";
                    var imageUploadResult = await _firebaseService.UploadFileToFirebase(imageUrl, imagePath);
                    if (!imageUploadResult.IsSuccess)
                    {
                        throw new InternalServerErrorException("Error uploading files to Firebase.");
                    }

                    newFeedbackImage.FeedbackID = newRating.FeedbackID;
                    newFeedbackImage.ImageUrl = (string)imageUploadResult.Result;
                    _feedbackImageRepository.Update(newFeedbackImage);
                }
                var rs = await _feedbackImageRepository.Commit();
                if (rs > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<TripFeedbackModel> GetAllFeedbackInTrip(Guid templateID, int pageNumber, int pageSize, int filter)
        {
            try
            {
                var existedTrip = await _tripRepository.FindByCondition(x=>x.TemplateID == templateID && x.Status.Trim().Equals(SD.ACTIVE)).FirstOrDefaultAsync();
                if (existedTrip != null)
                {
                    var feedback = await _feedbackRepository.FindByCondition(x => x.TemplateID == templateID)
                                 .ToListAsync();
                    var feedbacks = new List<Feedback>();
                    if (filter == 0)
                    {
                         feedbacks = await _feedbackRepository.FindByCondition(x => x.TemplateID == templateID)
                                 .Skip((pageNumber - 1) * pageSize)                                
                                 .Take(pageSize)                                                   
                                 .ToListAsync();                                                   
                    }                                                                              
                    else                                                                           
                    {                                                                              
                         feedbacks = await _feedbackRepository.FindByCondition(x => x.TemplateID == templateID && (x.Rating == filter))
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
                    }

                    var rs = new List<FeedbackModel>();

                    var totalRating = feedback.Sum(fb => fb.Rating);
                    var averageRating = feedback.Count > 0 ? (double)totalRating / feedback.Count : 0; 

                    foreach ( var fb in feedbacks)
                    {
                        var user = await _userRepository.GetByIdAsync(fb.UserID);
                        
                            var listImage = await _fbImageRepository.GetAll().Where(x => x.FeedbackID == fb.FeedbackID).Select(x => x.ImageUrl).ToListAsync();
                            var fbModel = new FeedbackModel
                            {
                                Avt = user.Avatar,
                                Date = existedTrip.StartTime,
                                Desciption = fb.Description,
                                Rating = fb.Rating,
                                UserName = user.UserName,
                                ImageUrl = listImage
                            };
                            
                        rs.Add(fbModel);
                    }
                    return new TripFeedbackModel
                    {
                        Feedbacks = rs,
                        TotalRating = averageRating
                    };
                }
                else throw new NotFoundException("Trip not found");
            }catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}