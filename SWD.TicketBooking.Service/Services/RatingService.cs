/*using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Common.Payloads.Requests;
using TicketBooking.Dtos;
using TicketBooking.Entities;
using TicketBooking.Exceptions;
using TicketBooking.Repositories;

namespace TicketBooking.Services
{
    public class RatingService
    {
        private readonly IRepository<Rating, int> _ratingRepo;
        private readonly IRepository<RatingPicture, int> _ratingPictureRepo;
        private readonly IRepository<Booking, int> _bookingRepo;
        private readonly IMapper _mapper;
        public RatingService(IRepository<Rating, int> ratingRepo, IRepository<RatingPicture, int> ratingPictureRepo, IRepository<Booking, int> bookingRepo, IMapper mapper)
        {
            _ratingRepo = ratingRepo;
            _ratingPictureRepo = ratingPictureRepo;
            _bookingRepo = bookingRepo;
            _mapper = mapper;
        }
        public async Task<(IEnumerable<RatingDTO> returnModel, string message)> GetAllRatings(int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    throw new ArgumentException("PageNumber and PageSize must be positive integers.");
                }

                var totalItems = await _ratingRepo.GetAll().CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                if (pageNumber > totalPages)
                {
                    throw new ArgumentException("PageNumber exceeds total number of pages.");
                }

                var ratings = await _ratingRepo.GetAll()
                    .OrderByDescending(x => x.RatingID) 
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (!ratings.Any())
                {
                    return (null, "No ratings found");
                }


                var ratingIds = ratings.Select(r => r.RatingID).ToList();
                var ratingPictures = await _ratingPictureRepo.GetAll()
                    .Where(rp => ratingIds.Contains(rp.RatingID))
                    .ToListAsync();

                var ratingDTOs = ratings.Select(rating => new RatingDTO
                {
                    RatingID = rating.RatingID,
                    UserID = rating.UserID,
                    TripID = rating.TripID,
                    Point = rating.Point,
                    Description = rating.Description,
                    RatingPictures = ratingPictures
                        .Where(rp => rp.RatingID == rating.RatingID)
                        .Select(rp => new RatingPictureDTO
                        {
                            RatingPictureID = rp.RatingPictureID,
                            ImageUrl = rp.ImageUrl
                        })
                        .ToList()
                }).ToList();

                return (ratingDTOs, "Ok");
            }
            catch (Exception ex)
            {
             
                return (null, ex.Message);
            }
        }

        public async Task<(IEnumerable<RatingDTO> returnModel, string message, int totalPages)> GetAllRatingsByTripID(int tripID, int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    throw new ArgumentException("PageNumber and PageSize must be positive integers.");
                }

                var totalItems = await _ratingRepo.GetAll().Where(x => x.TripID == tripID).CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                if (pageNumber > totalPages)
                {
                    throw new ArgumentException("PageNumber exceeds total number of pages.");
                }

     
                var ratingList = await _ratingRepo.GetAll()
                    .Where(x => x.TripID == tripID)
                    .OrderByDescending(x => x.RatingID)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (!ratingList.Any())
                {
                    return (null, "No ratings found", 0);
                }
                var ratingIds = ratingList.Select(r => r.RatingID).ToList();
                var ratingPictures = await _ratingPictureRepo.GetAll()
                    .Where(rp => ratingIds.Contains(rp.RatingID))
                    .ToListAsync();

                var ratingDTOs = ratingList.Select(rating => new RatingDTO
                {
                    RatingID = rating.RatingID,
                    UserID = rating.UserID,
                    TripID = rating.TripID,
                    Point = rating.Point,
                    Description = rating.Description,
                    RatingPictures = ratingPictures
                        .Where(rp => rp.RatingID == rating.RatingID)
                        .Select(rp => new RatingPictureDTO
                        {
                            RatingPictureID = rp.RatingPictureID,
                            ImageUrl = rp.ImageUrl
                        })
                        .ToList()
                }).ToList();

                return (ratingDTOs, "Ok", totalPages);
            }
            catch (Exception ex)
            {
                return (null, ex.Message, 0);
            }
        }
        public async Task<(RatingDTO returnModel, string message)> CreateRating(CreateRatingReq createRatingReq)
        {
            try
            {
                var checkHadBooked = await _bookingRepo
                    .FindByCondition(b => b.UserID == createRatingReq.UserID && b.TripID == createRatingReq.TripID)
                    .FirstOrDefaultAsync();
                if (checkHadBooked == null)
                {
                    return (null, "Not had booked this Trip yet!");
                }

                var checkHadRated = await _ratingRepo
                    .FindByCondition(r => r.UserID == createRatingReq.UserID && r.TripID == createRatingReq.TripID)
                    .FirstOrDefaultAsync();
                if (checkHadRated != null)
                {
                    return (null, "Had rated before!");
                }

                if (createRatingReq.Point > 5 || createRatingReq.Point <= 0)
                {
                    return (null, "The point does not suitable!");
                }

                var newRating = new Rating
                {
                    UserID = createRatingReq.UserID,
                    TripID = createRatingReq.TripID,
                    Point = createRatingReq.Point,
                    Description = createRatingReq.Description
                };
                await _ratingRepo.AddAsync(newRating);
                await _ratingRepo.Commit();

                var ratingPictures = new List<RatingPicture>();
                foreach (var ratingPictureDTO in createRatingReq.RatingPictures)
                {
                    var ratingPicture = new RatingPicture
                    {
                        RatingID = newRating.RatingID,
                        ImageUrl = ratingPictureDTO.ImageUrl
                    };

                    ratingPictures.Add(ratingPicture);
                    await _ratingPictureRepo.AddAsync(ratingPicture);
                }

                await _ratingPictureRepo.Commit();

                var ratingDTO = new RatingDTO
                {
                    RatingID = newRating.RatingID,
                    UserID = newRating.UserID,
                    TripID = newRating.TripID,
                    Point = newRating.Point,
                    Description = newRating.Description,
                    RatingPictures = ratingPictures.Select(rp => new RatingPictureDTO
                    {
                        RatingPictureID = rp.RatingPictureID,
                        ImageUrl = rp.ImageUrl
                    }).ToList()
                };

                return (ratingDTO, "Rating created successfully");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
    }
}
*/