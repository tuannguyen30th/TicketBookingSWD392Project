using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Exceptions;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private readonly IMapper _mapper;


        public TripService(IRepository<Trip, int> tripRepo, IRepository<Booking, int> bookingRepo, IRepository<TicketType_Trip, int> ticketTypeTripRepo, IRepository<Route, int> routeRepo, IRepository<Feedback, int> feedbackRepo, IMapper mapper, IRepository<TripPicture, int> tripPictureRepo)

        {
            _tripRepo = tripRepo;
            _bookingRepo = bookingRepo;
            _ticketTypeTripRepo = ticketTypeTripRepo;
            _tripPictureRepo = tripPictureRepo;
            _routeRepo = routeRepo;
            _feedbackRepo = feedbackRepo;
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
                    var tripImages = await _tripPictureRepo.FindByCondition(_ => _.TripID == trip.TripID).ToListAsync();
                    var tripImage = tripImages.Select(_ => _.ImageUrl).FirstOrDefault();
                    var lowestPrice = await _ticketTypeTripRepo.FindByCondition(_ => _.TripID == trip.TripID).MinAsync(_ => _.Price);

                    var searchTrip = new SearchTripModel
                    {
                        TripID = trip.TripID,
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
    }
}