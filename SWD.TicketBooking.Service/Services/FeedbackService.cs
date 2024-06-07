
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
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
        public readonly IRepository<Booking, int> _bookingRepository;
        public readonly IFirebaseService _firebaseService;
        public readonly IMapper _mapper;
        public FeedbackService(IRepository<Feedback, int> feedbackRepository, IRepository<Feedback_Image, int> feedbackImageRepository, IRepository<Booking, int> bookingRepository, IFirebaseService firebaseService, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _bookingRepository = bookingRepository;
            _feedbackImageRepository = feedbackImageRepository;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }
        public async Task<bool> CreateRating(FeedbackRequestModel ratingModel)
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
                /*  var guidPath = Guid.NewGuid().ToString();
                  var imagePath = FirebasePathName.RATING + $"{guidPath}";
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
                          ImageUrl = imageUrl,
                          UrlGuidID = guidPath,
                      };

                      await _feedbackImageRepository.AddAsync(newFeedbackImage);
                  }
                  await _feedbackImageRepository.Commit();*/
                var imageUrls = ratingModel.Files;
                foreach (var imageUrl in imageUrls)
                {
                    var guidPath = Guid.NewGuid().ToString();
                    var imagePath = FirebasePathName.RATING + $"{guidPath}";
                    var imageUploadResult = await _firebaseService.UploadFileToFirebase(imageUrl, imagePath);
                    if (!imageUploadResult.IsSuccess)
                    {
                        throw new Exception("Error uploading files to Firebase.");
                    }

                    var newFeedbackImage = new Feedback_Image
                    {
                        FeedbackID = newRating.FeedbackID,
                        ImageUrl = (string)imageUploadResult.Result,
                        UrlGuidID = guidPath
                    };

                    await _feedbackImageRepository.AddAsync(newFeedbackImage);
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
                throw new Exception();
            }
        }
       
    }
}