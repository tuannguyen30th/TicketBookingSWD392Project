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
    public class TripService : ITripService
    {
        private IUnitOfWork _unitOfWork;
        //private readonly IRepository<Trip, Guid> _unitOfWork.TripRepository;
        //private readonly IRepository<Booking, Guid> _unitOfWork.BookingRepository;
        //private readonly IRepository<TicketDetail, Guid> _unitOfWork.TicketDetailRepository;
        //private readonly IRepository<TripPicture, Guid> _unitOfWork.TripPictureRepository;
        //private readonly IRepository<TicketType_Trip, Guid> _unitOfWork.TicketType_TripRepository;
        //private readonly IRepository<Route, Guid> _unitOfWork.RouteRepository;
        //private readonly IRepository<Route_Company, Guid> _unitOfWork.Route_CompanyRepository;
        //private readonly IRepository<Feedback, Guid> _unitOfWork.FeedbackRepository;
        //private readonly IRepository<Trip_Utility, Guid> _unitOfWork.Trip_UtilityRepository;
        //private readonly IRepository<Utility, Guid> _unitOfWork.UtilityRepository;
        private readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;

        public TripService(IUnitOfWork unitOfWork, IRepository<Utility, Guid> utilityRepository, IRepository<TicketDetail, Guid> ticketDetailRepo, IRepository<Route_Company, Guid> routeCompanyRepo, IRepository<Trip, Guid> tripRepo, IRepository<Booking, Guid> bookingRepo, IRepository<TicketType_Trip, Guid> ticketTypeTripRepo, IRepository<Route, Guid> routeRepo, IRepository<Feedback, Guid> feedbackRepo, IMapper mapper, IRepository<TripPicture, Guid> tripPictureRepo, IFirebaseService firebaseService, IRepository<Trip_Utility, Guid> tripUtilityRepo)
        {
            _unitOfWork = unitOfWork;
            //_unitOfWork.TripRepository = tripRepo;
            //_unitOfWork.BookingRepository = bookingRepo;
            //_unitOfWork.TicketDetailRepository = ticketDetailRepo;
            //_unitOfWork.TicketType_TripRepository = ticketTypeTripRepo;
            //_unitOfWork.TripPictureRepository = tripPictureRepo;
            //_unitOfWork.RouteRepository = routeRepo;
            //_unitOfWork.FeedbackRepository = feedbackRepo;
            _firebaseService = firebaseService;
            //_unitOfWork.Trip_UtilityRepository = tripUtilityRepo;
            //_unitOfWork.Route_CompanyRepository = routeCompanyRepo;
            //_unitOfWork.UtilityRepository = utilityRepository;
            _mapper = mapper;
        }

        public async Task<List<string>> GetPictureOfTrip(Guid id)
        {
            try
            {
                var trip = await _unitOfWork.TripRepository.GetByIdAsync(id);
                if (trip == null)
                {
                    throw new BadRequestException("Trip not found!");
                }
                else
                {
                    var pics = await _unitOfWork.TripPictureRepository.GetAll().Where(x => x.TripID == id).Select(p => p.TripPictureID).ToListAsync();

                    var rs = new List<string>();
                    foreach (var p in pics)
                    {
                        var tripPic = await _unitOfWork.TripPictureRepository.GetByIdAsync(p);
                        
                        rs.Add(tripPic.ImageUrl);
                    }
                    return rs;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<PopularTripModel>> GetPopularTrips()
        {
            try
            {
                var topTrips = _unitOfWork.BookingRepository.GetAll()
                                            .GroupBy(b => b.TripID)
                                            .Select(g => new
                                            {
                                                TripID = g.Key,
                                                TotalQuantity = g.Sum(b => b.Quantity),
                                            })
                                            .OrderByDescending(t => t.TotalQuantity)
                                            .Take(5)
                                            .ToList();

                var trips = await _unitOfWork.TripRepository.GetAll()
                                                .Include(t => t.Route_Company.Route.FromCity)
                                                .Include(t => t.Route_Company.Route.ToCity)
                                                .Where(t => t.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && topTrips.Select(_ => _.TripID).Contains(t.TripID))
                                                .ToListAsync();

                var rs = new List<PopularTripModel>();

                foreach (var t in trips)
                {
                     var listImg = await GetPictureOfTrip(t.TripID);

                     var minPriceByTrip = await _unitOfWork.TicketType_TripRepository.GetAll()
                     .Where(_ => _.TripID == t.TripID)
                     .GroupBy(_ => _.TripID)
                     .Select(g => new
                     {
                         TripId = g.Key,
                         MinPrice = g.Min(_ => _.Price)
                     })
                     .ToDictionaryAsync(x => x.TripId, x => x.MinPrice);

                    var popuTrip = new PopularTripModel
                    {
                        TripId = t.TripID,
                        FromCity = t.Route_Company.Route.FromCity.Name,
                        ToCity = t.Route_Company.Route.ToCity.Name,
                        ImageUrl = listImg.ToList(),
                        PriceFrom = minPriceByTrip.GetValueOrDefault(t.TripID, 0),
                    };

                    rs.Add(popuTrip);
                }

                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PagedResult<SearchTripModel>> SearchTrip(Guid fromCity, Guid toCity, DateTime startTime, int pageNumber, int pageSize)
        {
            try
            {
                var startDate = startTime.Date;

                var tripsQuery = _unitOfWork.TripRepository.GetAll()
                    .Include(_ => _.Route_Company.Route)
                    .Where(_ => _.Route_Company.Route.FromCityID == fromCity
                                && _.Route_Company.Route.ToCityID == toCity
                                && _.StartTime.Date == startDate && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE));

                var totalTrips = await tripsQuery.CountAsync();
                if (totalTrips == 0)
                {
                    throw new NotFoundException("No trips found!");
                }
                var totalPages = (int)Math.Ceiling((double)totalTrips / pageSize);

                var trips = await tripsQuery.Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToListAsync();

                var searchTripModels = new List<SearchTripModel>();

                foreach (var trip in trips)
                {
                    var feedbacks = await _unitOfWork.FeedbackRepository.FindByCondition(_ => _.TemplateID == trip.TemplateID).ToListAsync();
                    var ratingAverage = feedbacks.Select(_ => _.Rating).DefaultIfEmpty(0).Average();
                    var roundedRatingAverage = Math.Round(ratingAverage, 1);
                    var ratingQuantity = feedbacks.Count;
                    var totalSeatsInTrip = await _unitOfWork.TicketType_TripRepository.FindByCondition(_ => _.TripID == trip.TripID).SumAsync(_ => (int?)_.Quantity) ?? 0;
                    var bookings = await _unitOfWork.BookingRepository.GetAll().Where(_ => _.TripID == trip.TripID).Select(_ => _.BookingID).ToListAsync();
                    var totalUnusedSeats = await _unitOfWork.TicketDetailRepository.FindByCondition(_ => bookings.Contains(_.BookingID) && _.Status.ToUpper().Equals("UNUSED")).CountAsync();
                    var remainingSeats = totalSeatsInTrip - totalUnusedSeats;
                    var tripImage = await _unitOfWork.TripPictureRepository.GetAll()
                        .Where(_ => _.TripID == trip.TripID)
                        .Select(_ => _.ImageUrl)
                        .FirstOrDefaultAsync();

                    var lowestPrice = await _unitOfWork.TicketType_TripRepository.FindByCondition(_ => _.TripID == trip.TripID)
                        .Select(_ => (double?)_.Price)
                        .MinAsync() ?? 0;

                    var searchTrip = new SearchTripModel
                    {
                        TripID = trip.TripID,
                        RouteID = trip.Route_Company.RouteID,
                        TemplateID = trip.TemplateID,
                        CompanyName = await _unitOfWork.Route_CompanyRepository.GetAll().Where(_ => _.RouteID == trip.Route_Company.RouteID).Select(_ => _.Company.Name).FirstOrDefaultAsync(),
                        ImageUrl = tripImage,
                        AverageRating = roundedRatingAverage,
                        QuantityRating = ratingQuantity,
                        EmptySeat = remainingSeats,
                        Price = lowestPrice,
                        StartLocation = trip.Route_Company.Route?.StartLocation,
                        EndLocation = trip.Route_Company.Route?.EndLocation,
                        StartDate = trip.StartTime.ToString("yyyy-MM-dd"),
                        EndDate = trip.EndTime.ToString("yyyy-MM-dd"),
                        StartTime = trip.StartTime.ToString("HH:mm"),
                        EndTime = trip.EndTime.ToString("HH:mm")
                    };
                    searchTripModels.Add(searchTrip);
                }
         
                    return new PagedResult<SearchTripModel>
                    {
                        Items = searchTripModels,
                        TotalCount = totalPages
                    };
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<bool> CreateTrip(CreateTripModel createTrip)
        {
            try
            {
                if (createTrip.StartTime == null || createTrip.EndTime == null || createTrip.ImageUrls == null)
                {
                    throw new BadRequestException("Not empty in any fields");
                }
                if(createTrip.StartTime > createTrip.EndTime)
                {
                    throw new BadRequestException("StartTime must greater than EndTime!");
                }
                var trip = new Trip
                {
                    TripID = Guid.NewGuid(),    
                    Route_CompanyID = createTrip.Route_CompanyID,
                    IsTemplate = true,
                    StartTime = createTrip.StartTime,
                    EndTime = createTrip.EndTime,
                    Status = SD.GeneralStatus.ACTIVE
                };
                await _unitOfWork.TripRepository.AddAsync(trip);
                //await _unitOfWork.TripRepository.Commit();
                //var rs = 0;
                var imageUrls = createTrip.ImageUrls;
                foreach (var imageUrl in imageUrls)
                {
                    var guidPath = Guid.NewGuid().ToString();
                    var imagePath = FirebasePathName.TRIP + $"{guidPath}";
                    var imageUploadResult = await _firebaseService.UploadFileToFirebase(imageUrl, imagePath);
                    if (!imageUploadResult.IsSuccess)
                    {
                        throw new Exception("Error uploading files to Firebase.");
                    }

                    var newtripImage = new TripPicture
                    {
                        TripPictureID = Guid.NewGuid(),
                        TripID = trip.TripID,
                        ImageUrl = (string)imageUploadResult.Result,
                        Status = SD.GeneralStatus.ACTIVE
                    };

                    await _unitOfWork.TripPictureRepository.AddAsync(newtripImage);
                }

                //rs = await _unitOfWork.TripPictureRepository.Commit();
                //if (rs < 0)
                //{
                //    throw new BadRequestException("Fail!");
                //}
                foreach (var ticketType in createTrip.TicketType_TripModels)
                {
                    if (ticketType.Price <= 0 || ticketType.Quantity <= 0)
                    {
                        throw new BadRequestException("Error format in Price or Quantity of TicketType_Trip!");
                    }
                    var newTicketType_Trip = new TicketType_Trip
                    {
                        TicketTypeID = ticketType.TicketTypeID,
                        TripID = trip.TripID,
                        Price = ticketType.Price,
                        Quantity = ticketType.Quantity,
                        Status = SD.GeneralStatus.ACTIVE
                    };
                    await _unitOfWork.TicketType_TripRepository.AddAsync(newTicketType_Trip);
                }
                //rs = await _unitOfWork.TicketType_TripRepository.Commit();
                //if (rs < 0)
                //{
                //    return false;
                //}
                foreach (var tripUtility in createTrip.Trip_UtilityModels)
                {
                  
                    var newTrip_Utility = new Trip_Utility
                    {
                        TripID = trip.TripID,
                        UtilityID = tripUtility.UtilityID,
                        Status = SD.GeneralStatus.ACTIVE
                    };
                    await _unitOfWork.Trip_UtilityRepository.AddAsync(newTrip_Utility);
                }
                //var rs = await _unitOfWork.Trip_UtilityRepository.Commit();
                var rs = _unitOfWork.Complete();
                if (rs < 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /*    public async Task UpdateTrip(UpdateTripModel updateTripModel, int tripID)
            {
                try
                {
                    var trip = await _unitOfWork.TripRepository.GetByIdAsync(tripID);
                    if (trip == null)
                    {
                        throw new Exception("Trip not found.");
                    }

                    // Update trip properties
                    trip.RouteID = updateTripModel.RouteID;
                    trip.StartTime = updateTripModel.StartTime;
                    trip.EndTime = updateTripModel.EndTime;

                    var tripPictures = await _unitOfWork.TripPictureRepository.FindByCondition(_ => _.TripID == tripID).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error updating trip.", ex);
                }
            }*/

        public async Task<bool> ChangeStatusTrip(Guid tripId)
        {
            try
            {
                var trip = await _unitOfWork.TripRepository.FindByCondition(_ => _.TripID == tripId).FirstOrDefaultAsync();
                if (trip == null)
                {
                    throw new NotFoundException("No exist!");
                }
                trip.Status = SD.GeneralStatus.INACTIVE;
                _unitOfWork.TripRepository.Update(trip);
                //var rs = await _unitOfWork.TripPictureRepository.Commit();
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
        public async Task<GetSeatBookedFromTripModel> GetSeatBookedFromTrip(Guid tripID)
        {
            try
            {
                var bookingDetails = await _unitOfWork.TicketDetailRepository
                    .FindByCondition(_ => _.Booking.TripID == tripID && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && _.TicketType_Trip.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                    .Select(_ => new
                    {
                        _.Booking.Trip.Route_Company.RouteID,
                        _.SeatCode,
                        _.Booking.Trip.Route_Company.Route.StartLocation,
                        _.Booking.Trip.Route_Company.Route.EndLocation,
                        _.Booking.Trip.StartTime,
                    })
                    .ToListAsync();

                if (bookingDetails == null || !bookingDetails.Any())
                {
                    throw new BadRequestException("Not Found!");
                }

                var ticketTypeTrips = await _unitOfWork.TicketType_TripRepository
                    .FindByCondition(_ => _.TripID == tripID && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                    .Select(_ => new GetSeatBookedFromTripModel.TicketType_TripModel
                    {
                        TicketType_TripID = _.TicketType_TripID,
                        TicketName = _.TicketType.Name,
                        Price = _.Price,
                        Quantity = _.Quantity,
                    })
                    .ToListAsync();
                if (ticketTypeTrips == null || !ticketTypeTrips.Any())
                {
                    throw new BadRequestException("Not Found!");
                }
                var totalSeat = await _unitOfWork.TicketType_TripRepository.FindByCondition(_ => _.TripID == tripID && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE)).SumAsync(_ => _.Quantity);
                var firstBooking = bookingDetails.First();
                var seatBookeds = bookingDetails.Select(_ => _.SeatCode).ToList();
                var result = new GetSeatBookedFromTripModel
                {
                    TripID = tripID,
                    RouteID = firstBooking.RouteID,
                    SeatBooked = seatBookeds,
                    TotalSeats = totalSeat,
                    StartLocation = firstBooking.StartLocation,
                    EndLocation = firstBooking.EndLocation,
                    StartDate = firstBooking.StartTime.ToString("yyyy-MM-dd"),
                    StartTime = firstBooking.StartTime.ToString("HH:mm"),
                    TicketType_TripModels = ticketTypeTrips
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<UtilityModel>> GetAllUtilityByTripID(Guid id)
        {
            var utilities = await _unitOfWork.Trip_UtilityRepository
               .FindByCondition(tu => tu.TripID == id && tu.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
               .Select(tu => tu.Utility)
               .ToListAsync();
            var result = new List<UtilityModel>();
            foreach (var trip in utilities)
            {
                var newModel = new UtilityModel
                {
                    Name = trip.Name,
                    Description = trip.Description,
                    Status = trip.Status,
                };
                result.Add(newModel);
            }
            return result;
        }
    }
}