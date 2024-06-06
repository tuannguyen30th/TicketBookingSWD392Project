
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Exceptions;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Services.FirebaseService;
using static SWD.TicketBooking.Service.Dtos.FeedbackRequestModel;

namespace SWD.TicketBooking.Service.Services
{
    public class FeedbackService
    {
        public readonly IRepository<Feedback, int> _feedbackRepository;
        public readonly IRepository<Feedback_Image, int> _feedbackImageRepository;
        public readonly IRepository<Trip, int> _tripRepository;
        public readonly IRepository<Booking, int> _bookingRepository;
        public readonly IRepository<Feedback_Image, int> _fbImageRepository; 
    public readonly IRepository<User, int> _userRepository;
        public readonly IFirebaseService _firebaseService;
        public readonly IMapper _mapper;
        public FeedbackService(IRepository<Feedback, int> feedbackRepository, IRepository<Feedback_Image, int> feedbackImageRepository, IRepository<Booking, int> bookingRepository, IFirebaseService firebaseService, IMapper mapper, IRepository<Trip, int> tripRepository, IRepository<Feedback_Image, int> fbImageRepository, IRepository<User, int> userRepository)
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
        public async Task CreateRating(FeedbackRequestModel ratingModel)
        {
            try
            {
                var checkHadBooked = await _bookingRepository.FindByCondition(_ => _.UserID == ratingModel.UserID && _.TripID == ratingModel.TripID).FirstOrDefaultAsync();
                if (checkHadBooked == null)
                {
                    throw new Exception("Not had booked this Trip yet!");
                }
                var checkHadRated = await _feedbackRepository.FindByCondition(_ => _.UserID == ratingModel.UserID && _.TripID == ratingModel.TripID).FirstOrDefaultAsync();
                if (checkHadRated != null)
                {
                    throw new Exception("Had rated before!");
                }
                if (ratingModel.Rating > 5 || ratingModel.Rating <= 0)
                {
                    throw new Exception("The point does not suitable!");
                }
                var newRating = new Feedback
                {
                    UserID = ratingModel.UserID,
                    TripID = ratingModel.TripID,
                    Rating = ratingModel.Rating,
                    Description = ratingModel.Description,
                    Status = "Active",
                };
                await _feedbackRepository.AddAsync(newRating);
                await _feedbackRepository.Commit();
                var imagePath = FirebasePathName.RATING + $"{newRating.FeedbackID}";
                var imageUploadResult = await _firebaseService.UploadFilesToFirebase(ratingModel.Files, imagePath);
                if (!imageUploadResult.IsSuccess)
                {
                    throw new Exception("Error uploading files to Firebase.");
                }

                foreach ( var imageUrl in (List<string>)imageUploadResult.Result)
                {
                    var newFeedbackImage = new Feedback_Image
                    {
                        FeedbackID = newRating.FeedbackID,
                        ImageUrl = imageUrl
                    };

                    await _feedbackImageRepository.AddAsync(newFeedbackImage);
                }
                await _feedbackImageRepository.Commit();


            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<TripFeedbackModel> GetAllFeedbackInTrip(int id, int pageNumber, int pageSize, int filter)
        {
            try
            {
                var existedTrip = await _tripRepository.FindByCondition(x=>x.TripID == id).FirstOrDefaultAsync();
                if (existedTrip != null)
                {
                    var feedback = await _feedbackRepository.FindByCondition(x => x.TripID == id)
                                 .ToListAsync();
                    var feedbacks = new List<Feedback>();
                    if (filter == 0)
                    {
                         feedbacks = await _feedbackRepository.FindByCondition(x => x.TripID == id)
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
                    }
                    else
                    {
                         feedbacks = await _feedbackRepository.FindByCondition(x => x.TripID == id && (x.Rating == filter))
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
                    }

                    var rs = new List<FeedbackModel>();

                    var totalRating = feedback.Sum(fb => fb.Rating);
                    var averageRating = feedback.Count > 0 ? (double)totalRating / feedback.Count : 0; // Calculate average rating

                    foreach ( var fb in feedbacks)
                    {
                        var user = await _userRepository.GetByIdAsync(fb.UserID);
                        
                            var listImage = await _fbImageRepository.GetAll().Where(x => x.FeedbackID == fb.FeedbackID).Select(x => x.ImageUrl).ToListAsync();
                            var fbModel = new FeedbackModel
                            {
                                avt = user.Avatar,
                                Date = existedTrip.StartTime,
                                Desciption = fb.Description,
                                rating = fb.Rating,
                                userName = user.UserName,
                                imageUrl = listImage
                            };
                            
                        rs.Add(fbModel);
                    }


                    return new TripFeedbackModel
                    {
                        Feedbacks = rs,
                        TotalRating = averageRating
                    };
                }
                else throw new Exception("Trip not found");
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}