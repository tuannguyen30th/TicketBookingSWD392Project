
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;

namespace SWD.TicketBooking.Service.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        //public readonly IRepository<Feedback, Guid> _unitOfWork.FeedbackRepository;
        //public readonly IRepository<Feedback_Image, Guid> _unitOfWork.Feedback_ImageRepository;
        //public readonly IRepository<Trip, Guid> _unitOfWork.TripRepository;
        //public readonly IRepository<Booking, Guid> _unitOfWork.BookingRepository;
        //public readonly IRepository<Feedback_Image, Guid> _unitOfWork.Feedback_ImageRepository; 
        //public readonly IRepository<User, Guid> _unitOfWork.UserRepository;
        public readonly IFirebaseService _firebaseService;
        public readonly IMapper _mapper;
        public FeedbackService(IUnitOfWork unitOfWork, IRepository<Feedback, Guid> feedbackRepository, IRepository<Feedback_Image, Guid> feedbackImageRepository, IRepository<Booking, Guid> bookingRepository, IFirebaseService firebaseService, IMapper mapper, IRepository<Trip, Guid> tripRepository, IRepository<Feedback_Image, Guid> fbImageRepository, IRepository<User, Guid> userRepository)
        {
            _unitOfWork = unitOfWork;
            //_unitOfWork.FeedbackRepository = feedbackRepository;
            //_unitOfWork.BookingRepository = bookingRepository;
            //_unitOfWork.Feedback_ImageRepository = feedbackImageRepository;
            _firebaseService = firebaseService;
            _mapper = mapper;
            //_unitOfWork.TripRepository = tripRepository;
            //_unitOfWork.Feedback_ImageRepository = fbImageRepository;
            //_unitOfWork.UserRepository = userRepository;
        }
        public async Task<bool> CreateRating(FeedbackRequestModel ratingModel)
        {
            try
            {
                var checkHadBooked = _unitOfWork.BookingRepository.FindByCondition(_ => _.UserID == ratingModel.UserID && _.TripID == ratingModel.TripID).FirstOrDefaultAsync();
                if (checkHadBooked == null)
                {
                    throw new BadRequestException("Not had booked this Trip yet!");
                }
                var checkHadRated = await _unitOfWork.FeedbackRepository.FindByCondition(_ => _.UserID == ratingModel.UserID && _.TripID == ratingModel.TripID).FirstOrDefaultAsync();
                if (checkHadRated != null)
                {
                    throw new BadRequestException("Had rated before!");
                }
                if (ratingModel.Rating > 5 || ratingModel.Rating <= 0)
                {
                    throw new BadRequestException("The point does not suitable!");
                }
                var getTemplateID = await _unitOfWork.TripRepository.FindByCondition(_ => _.TripID == ratingModel.TripID).Select(_ => _.TemplateID).FirstOrDefaultAsync();

                var newRating = new Feedback
                {
                    FeedbackID = Guid.NewGuid(),
                    UserID = ratingModel.UserID,
                    TripID = ratingModel.TripID,
                    Rating = ratingModel.Rating,
                    TemplateID = getTemplateID,
                    Description = ratingModel.Description,
                    Status = SD.GeneralStatus.ACTIVE,
                };
                await _unitOfWork.FeedbackRepository.AddAsync(newRating);
                //await _unitOfWork.FeedbackRepository.Commit();
               
                var imageUrls = ratingModel.Files;
                foreach (var imageUrl in imageUrls)
                {
                    var newFeedbackImage = new Feedback_Image
                    {
                        Feedback_Image_ID = Guid.NewGuid(),
                        FeedbackID = newRating.FeedbackID,
                    };
                    await _unitOfWork.Feedback_ImageRepository.AddAsync(newFeedbackImage);
                    //await _unitOfWork.Feedback_ImageRepository.Commit();
                    var imagePath = FirebasePathName.RATING + $"{newFeedbackImage.Feedback_Image_ID}";
                    var imageUploadResult = await _firebaseService.UploadFileToFirebase(imageUrl, imagePath);
                    if (!imageUploadResult.IsSuccess)
                    {
                        throw new InternalServerErrorException("Error uploading files to Firebase.");
                    }

                    newFeedbackImage.FeedbackID = newRating.FeedbackID;
                    newFeedbackImage.ImageUrl = (string)imageUploadResult.Result;
                    _unitOfWork.Feedback_ImageRepository.Update(newFeedbackImage);
                }
                //var rs = await _unitOfWork.Feedback_ImageRepository.Commit();
                var rs = _unitOfWork.Complete();
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

        public async Task<TripFeedbackModel> GetAllFeedbackInTrip(Guid tripID, int pageNumber, int pageSize, int filter)
        {
            try
            {
                var existedTrip = await _unitOfWork.TripRepository.FindByCondition(x=>x.TemplateID == tripID && x.Status.Trim().Equals(SD.GeneralStatus.ACTIVE)).FirstOrDefaultAsync();
                if (existedTrip != null)
                {
                    var feedback = await _unitOfWork.FeedbackRepository.FindByCondition(x => x.TemplateID == tripID)
                                 .ToListAsync();
                    var feedbacks = new List<Feedback>();
                    if (filter == 0)
                    {
                         feedbacks = await _unitOfWork.FeedbackRepository.FindByCondition(x => x.TemplateID == tripID)
                                 .Skip((pageNumber - 1) * pageSize)                                
                                 .Take(pageSize)                                                   
                                 .ToListAsync();                                                   
                    }                                                                              
                    else                                                                           
                    {                                                                              
                         feedbacks = await _unitOfWork.FeedbackRepository.FindByCondition(x => x.TemplateID == tripID && (x.Rating == filter))
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
                    }

                    var rs = new List<FeedbackModel>();

                    var totalRating = feedback.Sum(fb => fb.Rating);
                    var averageRating = feedback.Count > 0 ? (double)totalRating / feedback.Count : 0; 

                    foreach ( var fb in feedbacks)
                    {
                        var user = await _unitOfWork.UserRepository.GetByIdAsync(fb.UserID);
                        
                            var listImage = await _unitOfWork.Feedback_ImageRepository.GetAll().Where(x => x.FeedbackID == fb.FeedbackID).Select(x => x.ImageUrl).ToListAsync();
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