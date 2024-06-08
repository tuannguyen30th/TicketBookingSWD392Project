using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Exceptions;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Services.FirebaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD.TicketBooking.Service.Dtos.CreateTripModel;

namespace SWD.TicketBooking.Service.Services
{
    public class TripService
    {
        private readonly IRepository<Trip, int> _tripRepo;
        private readonly IRepository<Booking, int> _bookingRepo;
        private readonly IRepository<TripPicture, int> _tripPictureRepo;
        private readonly IRepository<TicketType_Trip, int> _ticketTypeTripRepo;
        private readonly IRepository<Route, int> _routeRepo;
        private readonly IRepository<Feedback, int> _feedbackRepo;
        private readonly IRepository<Trip_Utility, int> _tripUtilityRepo;
        private readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;

        public TripService(IRepository<Trip, int> tripRepo, IRepository<Booking, int> bookingRepo, IRepository<TicketType_Trip, int> ticketTypeTripRepo, IRepository<Route, int> routeRepo, IRepository<Feedback, int> feedbackRepo, IMapper mapper, IRepository<TripPicture, int> tripPictureRepo, IFirebaseService firebaseService, IRepository<Trip_Utility, int> tripUtilityRepo)

        {
            _tripRepo = tripRepo;
            _bookingRepo = bookingRepo;
            _ticketTypeTripRepo = ticketTypeTripRepo;
            _tripPictureRepo = tripPictureRepo;
            _routeRepo = routeRepo;
            _feedbackRepo = feedbackRepo;
            _firebaseService = firebaseService;
            _tripUtilityRepo = tripUtilityRepo;
            _mapper = mapper;
        }

        public async Task<List<PictureModel>> GetPictureOfTrip(int id)
        {
            try
            {
                var trip = await _tripRepo.GetByIdAsync(id);
                if (trip == null)
                {
                    throw new BadRequestException("Trip not found!");
                }
                else
                {
                    var pics = await _tripPictureRepo.GetAll().Where(x => x.TripID == id).Select(p => p.TripPictureID).ToListAsync();

                    var rs = new List<PictureModel>();
                    foreach (var p in pics)
                    {
                        var tripPic = await _tripPictureRepo.GetByIdAsync(p);
                        var tripPicModel = new PictureModel
                        {
                            ImageUrl = tripPic.ImageUrl,
                            TripId = tripPic.TripID
                        };
                        rs.Add(tripPicModel);
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
                var topTrips = _bookingRepo.GetAll()
                                            .GroupBy(b => b.TripID)
                                            .Select(g => new
                                            {
                                                TripID = g.Key,
                                                TotalQuantity = g.Sum(b => b.Quantity),
                                            })
                                            .OrderByDescending(t => t.TotalQuantity)
                                            .Take(5)
                                            .ToList();

                var trips = await _tripRepo.GetAll()
                                                .Include(t => t.Route.FromCity)
                                                .Include(t => t.Route.ToCity)
                                                .Where(t => t.Status.ToLower().Trim() == "active" && topTrips.Select(_ => _.TripID).Contains(t.TripID))
                                                .ToListAsync();

                var rs = new List<PopularTripModel>();

                foreach (var t in trips)
                {
                    var minPriceByTrip = await _ticketTypeTripRepo.GetAll()
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
                        FromCity = t.Route.FromCity.Name,
                        ToCity = t.Route.ToCity.Name,
                        /*ImageUrl = t.ImageUrl,*/
                        PriceFrom = minPriceByTrip.GetValueOrDefault(t.TripID, 0),
                    };

                    rs.Add(popuTrip);
                }

                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<PagedResult<SearchTripModel>> SearchTrip(int fromCity, int toCity, DateTime startTime, int pageNumber, int pageSize)
        {
            try
            {
                var startDate = startTime.Date;

                var tripsQuery = _tripRepo.GetAll()
                    .Include(_ => _.Route)
                    .ThenInclude(_ => _.Company)
                    .Where(_ => _.Route.FromCityID == fromCity
                                && _.Route.ToCityID == toCity
                                && _.StartTime.Date == startDate);

                var totalTrips = await tripsQuery.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalTrips / pageSize);

                var trips = await tripsQuery.Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToListAsync();

                var searchTripModels = new List<SearchTripModel>();

                foreach (var trip in trips)
                {
                    var feedbacks = await _feedbackRepo.FindByCondition(_ => _.TripID == trip.TripID).ToListAsync();
                    var ratingAverage = feedbacks.Select(_ => _.Rating).DefaultIfEmpty(0).Average();
                    var ratingQuantity = feedbacks.Count;

                    var totalSeatsInTrip = await _ticketTypeTripRepo.FindByCondition(_ => _.TripID == trip.TripID).SumAsync(_ => (int?)_.Quantity) ?? 0;
                    var seatsBookedInTrip = await _bookingRepo.FindByCondition(_ => _.TripID == trip.TripID && _.BookingTime < startTime).CountAsync();
                    var remainingSeats = totalSeatsInTrip - seatsBookedInTrip;

                    var tripImage = await _tripPictureRepo.GetAll()
                        .Where(_ => _.TripID == trip.TripID)
                        .Select(_ => _.ImageUrl)
                        .FirstOrDefaultAsync();

                    var lowestPrice = await _ticketTypeTripRepo.FindByCondition(_ => _.TripID == trip.TripID)
                        .Select(_ => (double?)_.Price)
                        .MinAsync() ?? 0;

                    var searchTrip = new SearchTripModel
                    {
                        TripID = trip.TripID,
                        RouteID = trip.RouteID,
                        CompanyName = trip.Route.Company.Name,
                        ImageUrl = tripImage,
                        AverageRating = ratingAverage,
                        QuantityRating = ratingQuantity,
                        EmptySeat = remainingSeats,
                        Price = lowestPrice,
                        StartLocation = trip.Route?.StartLocation,
                        EndLocation = trip.Route?.EndLocation,
                        StartTime = trip.StartTime,
                        EndTime = trip.EndTime,
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
                // Log the exception (ex) here if needed
                throw new Exception("An error occurred while searching for trips.", ex);
            }
        }


        public async Task<bool> CreateTrip(CreateTripModel createTrip)
        {
            try
            {
                if (createTrip.StartTime == null || createTrip.EndTime == null || createTrip.ImageUrls == null)
                {
                    throw new Exception("Not empty in any fields");
                }
                var trip = new Trip
                {
                    RouteID = createTrip.RouteID,
                    IsTemplate = true,
                    StartTime = createTrip.StartTime,
                    EndTime = createTrip.EndTime,
                    Status = "Active"
                };
                await _tripRepo.AddAsync(trip);
                await _tripRepo.Commit();
                var rs = 0;
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
                        TripID = trip.TripID,
                        ImageUrl = (string)imageUploadResult.Result,
                        UrlGuidID = guidPath,
                        Status = "Active"
                    };

                    await _tripPictureRepo.AddAsync(newtripImage);
                }

                rs = await _tripPictureRepo.Commit();
                if (rs < 0)
                {
                    return false;
                }
                foreach (var ticketType in createTrip.TicketType_TripModels)
                {
                    if (ticketType.TicketTypeID <= 0 || ticketType.Price <= 0 || ticketType.Quantity <= 0)
                    {
                        return false;
                    }
                    var newTicketType_Trip = new TicketType_Trip
                    {
                        TicketTypeID = ticketType.TicketTypeID,
                        TripID = trip.TripID,
                        Price = ticketType.Price,
                        Quantity = ticketType.Quantity,
                        Status = "Active"
                    };
                    await _ticketTypeTripRepo.AddAsync(newTicketType_Trip);
                }
                rs = await _ticketTypeTripRepo.Commit();
                if (rs < 0)
                {
                    return false;
                }
                foreach (var tripUtility in createTrip.Trip_UtilityModels)
                {
                    if (tripUtility.UtilityID <= 0)
                    {
                        return false;
                    }
                    var newTrip_Utility = new Trip_Utility
                    {
                        TripID = trip.TripID,
                        UtilityID = tripUtility.UtilityID,
                        Status = "Active"
                    };
                    await _tripUtilityRepo.AddAsync(newTrip_Utility);
                }
                rs = await _tripUtilityRepo.Commit();
                if (rs < 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        /*    public async Task UpdateTrip(UpdateTripModel updateTripModel, int tripID)
            {
                try
                {
                    var trip = await _tripRepo.GetByIdAsync(tripID);
                    if (trip == null)
                    {
                        throw new Exception("Trip not found.");
                    }

                    // Update trip properties
                    trip.RouteID = updateTripModel.RouteID;
                    trip.StartTime = updateTripModel.StartTime;
                    trip.EndTime = updateTripModel.EndTime;

                    var tripPictures = await _tripPictureRepo.FindByCondition(_ => _.TripID == tripID).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error updating trip.", ex);
                }
            }*/

        public async Task<bool> ChangeStatusTrip(int tripId)
        {
            try
            {
                var trip = await _tripRepo.FindByCondition(_ => _.TripID == tripId).FirstOrDefaultAsync();
                if (trip == null)
                {
                    throw new Exception("No exist!");
                }
                trip.Status = "Inactive";
                _tripRepo.Update(trip);
                var rs = await _tripPictureRepo.Commit();
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