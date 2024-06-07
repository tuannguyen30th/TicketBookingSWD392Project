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
        private readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;

        public TripService(IRepository<Trip, int> tripRepo, IRepository<Booking, int> bookingRepo, IRepository<TicketType_Trip, int> ticketTypeTripRepo, IRepository<Route, int> routeRepo, IRepository<Feedback, int> feedbackRepo, IMapper mapper, IRepository<TripPicture, int> tripPictureRepo, IFirebaseService firebaseService)
        {
            _tripRepo = tripRepo;
            _bookingRepo = bookingRepo;
            _ticketTypeTripRepo = ticketTypeTripRepo;
            _tripPictureRepo = tripPictureRepo;
            _routeRepo = routeRepo;
            _feedbackRepo = feedbackRepo;
            _firebaseService = firebaseService;
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

        public async Task<List<SearchTripModel>> SearchTrip(int fromCity, int toCity, DateTime startTime)
        {
            try
            {
                var startDate = startTime.Date;
                var trips = await _tripRepo.GetAll().Include(_ => _.Route).ThenInclude(_ => _.Company).Where(_ => _.Route.FromCityID == fromCity
                                                              && _.Route.ToCityID == toCity
                                                              && _.StartTime.Date == startDate)
                                                                  .ToListAsync();

                var searchTripModels = new List<SearchTripModel>();

                foreach (var trip in trips)
                {
                    var feedbacks = await _feedbackRepo.FindByCondition(_ => _.TripID == trip.TripID).ToListAsync();
                    var ratingAverage = feedbacks.Select(_ => _.Rating).DefaultIfEmpty(0).Average();
                    var ratingQuantity = await _feedbackRepo.FindByCondition(fb => fb.TripID == trip.TripID).CountAsync();
                    ratingQuantity = ratingQuantity == 0 ? 0 : ratingQuantity;
                    var totalSeatsInTrip = await _ticketTypeTripRepo.FindByCondition(_ => _.TripID == trip.TripID).SumAsync(_ => _.Quantity);
                    var seatsBookedInTrip = await _bookingRepo.FindByCondition(_ => _.TripID == trip.TripID && _.BookingTime < startTime).CountAsync();
                    var remainingSeats = totalSeatsInTrip - seatsBookedInTrip;
                    var tripImage = await _tripPictureRepo.GetAll().Where(_ => _.TripID == trip.TripID).Select(_ => _.ImageUrl).FirstOrDefaultAsync();
                    var lowestPrice = await _ticketTypeTripRepo.FindByCondition(_ => _.TripID == trip.TripID).MinAsync(_ => _.Price);

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

                return searchTripModels;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<bool> CreateTrip(CreateTripModel createTrip)
        {
            try
            {
                if(createTrip.StartTime == null || createTrip.EndTime == null || createTrip.ImageUrls == null)
                {
                    throw new Exception("Not empty in any fields");
                }
                var trip = new Trip
                {
                    RouteID = createTrip.RouteID,
                    IsTemplate = true,
                    StartTime = createTrip.StartTime,
                    EndTime= createTrip.EndTime,
                    Status = "Active"
                };
                await _tripRepo.AddAsync(trip);
                await _tripRepo.Commit();
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
                var rs = await _tripPictureRepo.Commit();
                if(rs > 0)
                {
                    return true;
                }
                return false;
            }
            catch(Exception ex)
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
                if(trip == null)
                {
                    throw new Exception("No exist!");
                }
                trip.Status = "Inactive";
                 _tripRepo.Update(trip);
                var rs = await _tripPictureRepo.Commit();
                if(rs > 0)
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