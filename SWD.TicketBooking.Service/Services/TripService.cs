using AutoMapper;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Sockets;
using static SWD.TicketBooking.Service.Dtos.GetListTripsAfterCreateModel;
using static SWD.TicketBooking.Service.Dtos.GetTripAfterCreateModel;
using static SWD.TicketBooking.Service.Dtos.GetTripDetailsModel;

namespace SWD.TicketBooking.Service.Services
{
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;

        public TripService(IUnitOfWork unitOfWork, IFirebaseService firebaseService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }

        public async Task<List<SearchTripModel>> GetAllTripsByStaffAndDate(Guid staffID, DateTime startTime)
        {
            try
            {
                var startDate = startTime.Date;
                var trips = await _unitOfWork.TripRepository.GetAll()
                                                      .Include(_ => _.Route_Company.Route)
                                                      .Where(_ => _.StaffID.Equals(staffID)
                                                               && _.StartTime.Value.Date == startDate && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                      .ToListAsync();

                var searchTripModels = new List<SearchTripModel>();

                foreach (var trip in trips)
                {
                    var feedbacks = await _unitOfWork.FeedbackRepository
                                                     .FindByCondition(_ => _.TemplateID == trip.TemplateID)
                                                     .ToListAsync();
                    var ratingAverage = feedbacks.Select(_ => _.Rating).DefaultIfEmpty(0).Average();
                    var roundedRatingAverage = Math.Round((decimal)ratingAverage, 1);
                    var ratingQuantity = feedbacks.Count();
                    var tripID = await GetTripIDFromTemplate(trip.TripID);
                    var totalSeatsInTrip = await _unitOfWork.TicketType_TripRepository
                                                            .FindByCondition(_ => _.TripID == tripID.TripID)
                                                            .SumAsync(_ => (int?)_.Quantity) ?? 0;
                    var bookings = await _unitOfWork.BookingRepository
                                                    .GetAll()
                                                    .Where(_ => _.TripID == trip.TripID)
                                                    .Select(_ => _.BookingID)
                                                    .ToListAsync();
                    var totalUnusedSeats = await _unitOfWork.TicketDetailRepository
                                                            .FindByCondition(_ => bookings.Contains((Guid)_.BookingID)
                                                                             && _.Status.Equals(SD.Booking_TicketStatus.UNUSED_TICKET))
                                                            .CountAsync();
                    var remainingSeats = totalSeatsInTrip - totalUnusedSeats;
                    var tripImage = await _unitOfWork.TripPictureRepository
                                                     .GetAll()
                                                     .Where(_ => _.TripID == tripID.TripID)
                                                     .Select(_ => _.ImageUrl)
                                                     .FirstOrDefaultAsync();

                    var lowestPrice = await _unitOfWork.TicketType_TripRepository
                                                       .FindByCondition(_ => _.TripID == tripID.TripID)
                                                       .Select(_ => (double?)_.Price)
                                                       .MinAsync() ?? 0;

                    var searchTrip = new SearchTripModel
                    {
                        TripID = trip.TripID,
                        RouteID = (Guid)trip.Route_Company.RouteID,
                        TemplateID = (Guid)trip.TemplateID,
                        CompanyName = tripID.Route_Company.Company.Name,
                        CompanyID = tripID.Route_Company.Company.CompanyID,
                        ImageUrl = tripImage,
                        AverageRating = (double)roundedRatingAverage,
                        QuantityRating = ratingQuantity,
                        EmptySeat = remainingSeats,
                        Price = lowestPrice,
                        StartLocation = trip.Route_Company.Route?.StartLocation,
                        EndLocation = trip.Route_Company.Route?.EndLocation,
                        StartDate = trip.StartTime?.ToString("yyyy-MM-dd"),
                        EndDate = trip.EndTime?.ToString("yyyy-MM-dd"),
                        StartTime = trip.StartTime?.ToString("HH:mm"),
                        EndTime = trip.EndTime?.ToString("HH:mm")
                    };
                    searchTripModels.Add(searchTrip);
                };

                return searchTripModels;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<GetTripDetailsModel> TripDetails(Guid tripID)
        {
            try
            {
                var trip = await _unitOfWork.TripRepository
                                            .GetAll()
                                            .Include(_ => _.User)
                                            .Include(_ => _.Route_Company)
                                            .ThenInclude(_ => _.Route)
                                            .ThenInclude(_ => _.FromCity)
                                            .Include(_ => _.Route_Company)
                                            .ThenInclude(_ => _.Route)
                                            .ThenInclude(_ => _.ToCity)
                                            .Where(_ => _.TripID == tripID)
                                            .FirstOrDefaultAsync();

                var tripPictures = await _unitOfWork.TripPictureRepository.GetAll()
                                                    .Where(_ => _.TripID == trip.TemplateID && _.Status == SD.GeneralStatus.ACTIVE)
                                                    .Select(_ => _.ImageUrl)
                                                    .ToListAsync();
                var tripUtilities = await _unitOfWork.Trip_UtilityRepository
                                                     .GetAll()
                                                     .Include(_ => _.Utility)
                                                     .Where(_ => _.TripID == trip.TemplateID && _.Status == SD.GeneralStatus.ACTIVE)
                                                     .Select(_ => new TripUtilityModel
                                                     {
                                                         UtilityName = _.Utility.Name,
                                                         Description = _.Utility.Description
                                                     })
                                                     .ToListAsync();
                var getSeats = await _unitOfWork.TicketType_TripRepository
                                                .GetAll()
                                                .Include(_ => _.TicketType)
                                                .Where(_ => _.TripID == trip.TemplateID && _.Status == SD.GeneralStatus.ACTIVE)
                                                .Select(_ => new TripPriceSeat
                                                {
                                                    SeatName = _.TicketType.Name,
                                                    Price = _.Price,
                                                    Quantity = _.Quantity
                                                })
                                                .ToListAsync();
                var stationsByRoute = await _unitOfWork.StationCompany_RouteRepository
                                                       .GetAll()
                                                       .Where(_ => _.RouteID == trip.Route_Company.RouteID
                                                                && _.Station_Company.CompanyID == trip.Route_Company.CompanyID && _.Status == SD.GeneralStatus.ACTIVE)
                                                       .OrderBy(_ => _.OrderInRoute)
                                                       .Select(_ => _.Station_CompanyID)
                                                       .ToListAsync();
                var stationsByCompany = await _unitOfWork.Station_CompanyRepository
                                                         .GetAll()
                                                         .Include(_ => _.Station.City)
                                                         .Where(_ => stationsByRoute.Contains(_.Station_CompanyID)
                                                                       && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                         .Include(_ => _.Station)
                                                         .Select(_ => new TripStationModel
                                                         {
                                                             StationID = _.StationID,
                                                             StationName = _.Station.Name,
                                                             AtCity = _.Station.City.Name,
                                                         })
                                                         .ToListAsync();
                var result = new GetTripDetailsModel
                {
                    TripID = trip.TripID,
                    FromCity = trip.Route_Company.Route.FromCity.Name,
                    ToCity = trip.Route_Company.Route.ToCity.Name,
                    StartLocation = trip.Route_Company.Route.StartLocation,
                    EndLocation = trip.Route_Company.Route.EndLocation,
                    StartDate = trip.StartTime?.ToString("yyyy-MM-dd"),
                    StartTime = trip.StartTime?.ToString("HH:mm"),
                    EndDate = trip.EndTime?.ToString("yyyy-MM-dd"),
                    EndTime = trip.EndTime?.ToString("HH:mm"),
                    StaffName = trip?.User?.FullName,
                    StaffID = trip?.User?.UserID,
                    StaffEmail = trip?.User?.Email,
                    ImageUrls = tripPictures,
                    TripStationModels = stationsByCompany,
                    TripPriceSeats = getSeats,
                    TripUtilityModels = tripUtilities,
                    Status = trip.Status,
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ActionOutcome> GetAllSeatsFromTrip(Guid tripID)
        {
            try
            {
                var rs = new ActionOutcome();
                var bookingFromTrip = await _unitOfWork.BookingRepository
                                                       .FindByCondition(_ => _.TripID == tripID)
                                                       .Select(_ => _.BookingID)
                                                       .ToListAsync();
                if (bookingFromTrip == null)
                {
                    throw new NotFoundException("KHÔNG CÓ HÓA ĐƠN NÀO CHO CHUYẾN ĐI NÀY!");
                }

                var ticketFromTrip = await _unitOfWork.TicketDetailRepository.GetAll()
                                                   .Where(_ => bookingFromTrip.Contains((Guid)_.BookingID)
                                                               && _.Status != SD.Booking_TicketStatus.CANCEL_TICKET
                                                               && _.Status != SD.Booking_TicketStatus.NOTPAYING_TICKET)
                                                   .ToListAsync();
                rs.Result = ticketFromTrip.Select(_ => new List<string>
                                          {
                                              _.TicketDetailID.ToString(),
                                              _.SeatCode,
                                              _.Status
                                          }).ToList();

                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<string>> GetPictureOfTrip(Guid id)
        {
            try
            {
                var trip = await _unitOfWork.TripRepository.GetByIdAsync(id);

                if (trip == null)
                {
                    throw new BadRequestException(SD.Notification.NotFound("CHUYẾN XE"));
                }
                else
                {
                    var getTemplateID = await _unitOfWork.TripRepository
                                                         .GetAll()
                                                         .Where(_ => _.TripID == id)
                                                         .Select(_ => _.TemplateID)
                                                         .FirstOrDefaultAsync();

                    var pics = await _unitOfWork.TripPictureRepository
                                                .GetAll()
                                                .Where(_ => _.TripID == getTemplateID)
                                                .Select(_ => _.TripPictureID)
                                                .ToListAsync();

                    var rs = new List<string>();
                    foreach (var p in pics)
                    {
                        var tripPic = await _unitOfWork.TripPictureRepository.GetByIdAsync(p);
                        rs.Add(tripPic.ImageUrl);
                    };
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
                var listTripFromNow = await _unitOfWork.TripRepository
                                                            .GetAll()
                                                            .Include(t => t.Route_Company.Route.FromCity)
                                                            .Include(t => t.Route_Company.Route.ToCity)
                                                            .Where(t => t.Status.Trim() == SD.GeneralStatus.ACTIVE && t.StartTime.Value.Date == DateTime.UtcNow.Date)
                                                            .ToListAsync();

                var topTrips = await _unitOfWork.BookingRepository
                                                            .GetAll()
                                                            .Where(b => b.PaymentStatus.Equals(SD.BookingStatus.PAYING_BOOKING)
                                                                        && listTripFromNow.Select(t => t.TripID).Contains((Guid)b.TripID))
                                                            .GroupBy(b => b.TripID)
                                                            .OrderByDescending(g => g.Sum(b => b.Quantity))
                                                            .Select(g => new { TripID = g.Key, TotalQuantity = g.Sum(b => b.Quantity) })
                                                            .Take(5)
                                                            .ToListAsync();

                var trips = listTripFromNow.Where(t => topTrips.Select(_ => _.TripID).Contains(t.TripID)).ToList();

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
                        FromCityID = t.Route_Company.Route.FromCity.CityID,
                        FromCity = t.Route_Company.Route.FromCity.Name,
                        ToCityID = t.Route_Company.Route.ToCity.CityID,
                        ToCity = t.Route_Company.Route.ToCity.Name,
                        ImageUrl = listImg.ToList(),
                        PriceFrom = (double)minPriceByTrip.GetValueOrDefault(t.TripID, 0),
                    };

                    rs.Add(popuTrip);
                };

                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<TicketType>> GetAllTicketType()
        {
            try
            {
                var ticketTypes = await _unitOfWork.TicketTypeRepository.GetAll().ToListAsync();
                return ticketTypes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<PagedResult<SearchTripModel>> SearchTrip(Guid fromCity, Guid toCity, DateTime startTime, int pageNumber, int pageSize, string[]? seatAvailability, string? sortOption, Guid[]? sortCompany)
        {
            try
            {
                var startDate = startTime.Date;
                var tripsQuery = await _unitOfWork.TripRepository.GetAll()
                                            .Include(_ => _.Route_Company.Route)
                                            .Where(_ => _.Route_Company.Route.FromCityID == fromCity
                                                     && _.Route_Company.Route.ToCityID == toCity
                                                     && _.StartTime.Value.Date == startDate && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE)).ToListAsync();
                var filteredTripIds = new List<Trip>();
                if (sortCompany != null && sortCompany.Length > 0)
                {
                    foreach (var company in sortCompany)
                    {
                        var filteredTripIdsForCompany = tripsQuery.Where(_ => _.Route_Company.CompanyID == company);
                        filteredTripIds.AddRange(filteredTripIdsForCompany);
                    }

                    if (filteredTripIds != null && filteredTripIds.Count > 0)
                    {
                        tripsQuery = filteredTripIds.ToList();
                    }
                    else
                    {
                        tripsQuery = null;
                    }
                }
                var seatFilteredTrips = new List<Trip>();
                if (seatAvailability != null && seatAvailability.Length > 0)
                {
                    foreach (var seatType in seatAvailability)
                    {
                        if (seatFilteredTrips.Count == 0 || seatFilteredTrips == null)
                            seatFilteredTrips = tripsQuery.ToList();

                        switch (seatType.Trim().ToUpper())
                        {
                            case SD.FilterOption.SEAT_HEAD:
                                var headTripIds = await GetFilteredTripIdsBySeatCode(seatFilteredTrips, SD.FilterOption.SEAT_A);
                                if (!headTripIds.Any())
                                {
                                    tripsQuery = null;
                                    break;
                                }
                                seatFilteredTrips = seatFilteredTrips.Where(_ => headTripIds.Contains(_)).ToList();
                                break;

                            case SD.FilterOption.SEAT_MIDDLE:
                                var middleTripIds = await GetFilteredTripIdsBySeatCode(seatFilteredTrips, SD.FilterOption.SEAT_B);
                                if (!middleTripIds.Any())
                                {
                                    tripsQuery = null;
                                    break;
                                }
                                seatFilteredTrips = seatFilteredTrips.Where(_ => middleTripIds.Contains(_)).ToList();
                                break;

                            case SD.FilterOption.SEAT_BACK:
                                var backTripIds = await GetFilteredTripIdsBySeatCode(seatFilteredTrips, SD.FilterOption.SEAT_C);
                                if (!backTripIds.Any())
                                {
                                    tripsQuery = null;
                                    break;
                                }
                                seatFilteredTrips = seatFilteredTrips.Where(_ => backTripIds.Contains(_)).ToList();
                                break;

                            default:
                                break;
                        }

                        if (tripsQuery == null)
                            break;
                    }

                    if (tripsQuery != null && seatFilteredTrips.Count > 0)
                    {
                        tripsQuery = seatFilteredTrips.Distinct().ToList();
                    }
                    else
                    {
                        tripsQuery = null;
                    }
                }

                if (sortOption != null && sortOption.Length > 0)
                {
                    switch (sortOption.Trim().ToUpper())
                    {
                        case SD.FilterOption.PRICE_ASC:
                            filteredTripIds = await GetFilteredTripIdsByOption(tripsQuery, SD.FilterOption.PRICE_ASC);
                            break;

                        case SD.FilterOption.PRICE_DESC:
                            filteredTripIds = await GetFilteredTripIdsByOption(tripsQuery, SD.FilterOption.PRICE_DESC);
                            break;

                        case SD.FilterOption.RATING_ASC:
                            filteredTripIds = await GetFilteredTripIdsByOption(tripsQuery, SD.FilterOption.RATING_ASC);
                            break;

                        case SD.FilterOption.RATING_DESC:
                            filteredTripIds = await GetFilteredTripIdsByOption(tripsQuery, SD.FilterOption.RATING_DESC);
                            break;

                        case SD.FilterOption.TIME_SOONER:
                            filteredTripIds = tripsQuery.OrderBy(_ => _.StartTime).ToList();
                            break;

                        case SD.FilterOption.TIME_LATER:
                            filteredTripIds = tripsQuery.OrderByDescending(_ => _.StartTime).ToList();
                            break;

                        default:
                            break;
                    }
                    if (filteredTripIds != null && filteredTripIds.Count > 0)
                    {
                        tripsQuery = filteredTripIds.ToList();
                    }
                    tripsQuery.ToList();
                }
                var totalTrips = tripsQuery?.Count() ?? 0;
                if (totalTrips == 0)
                {
                    throw new NotFoundException(SD.Notification.NotFound("CHUYẾN XE"));
                }
                var totalPages = (int)Math.Ceiling((double)totalTrips / pageSize);

                var trips = tripsQuery.Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize).ToList();

                var searchTripModels = new List<SearchTripModel>();

                foreach (var trip in trips)
                {
                    var feedbacks = await _unitOfWork.FeedbackRepository
                                                     .FindByCondition(_ => _.TemplateID == trip.TemplateID)
                                                     .ToListAsync();
                    var ratingAverage = feedbacks.Select(_ => _.Rating).DefaultIfEmpty(0).Average();
                    var roundedRatingAverage = Math.Round((decimal)ratingAverage, 1);
                    var ratingQuantity = feedbacks.Count();
                    var tripID = await GetTripIDFromTemplate(trip.TripID);
                    var totalSeatsInTrip = await _unitOfWork.TicketType_TripRepository
                                                            .FindByCondition(_ => _.TripID == tripID.TripID)
                                                            .SumAsync(_ => (int?)_.Quantity) ?? 0;
                    var bookings = await _unitOfWork.BookingRepository
                                                    .GetAll()
                                                    .Where(_ => _.TripID == trip.TripID)
                                                    .Select(_ => _.BookingID)
                                                    .ToListAsync();
                    var totalUnusedSeats = await _unitOfWork.TicketDetailRepository
                                                            .FindByCondition(_ => bookings.Contains((Guid)_.BookingID)
                                                                             && _.Status.Equals(SD.Booking_TicketStatus.UNUSED_TICKET))
                                                            .CountAsync();
                    var remainingSeats = totalSeatsInTrip - totalUnusedSeats;
                    var tripImage = await _unitOfWork.TripPictureRepository
                                                     .GetAll()
                                                     .Where(_ => _.TripID == tripID.TripID)
                                                     .Select(_ => _.ImageUrl)
                                                     .FirstOrDefaultAsync();

                    var lowestPrice = await _unitOfWork.TicketType_TripRepository
                                                       .FindByCondition(_ => _.TripID == tripID.TripID)
                                                       .Select(_ => (double?)_.Price)
                                                       .MinAsync() ?? 0;
                    var companyName = tripID.Route_Company.Company.Name;

                    var searchTrip = new SearchTripModel
                    {
                        TripID = trip.TripID,
                        RouteID = (Guid)trip.Route_Company.RouteID,
                        TemplateID = (Guid)trip.TemplateID,
                        CompanyID = (Guid)trip.Route_Company.CompanyID,
                        CompanyName = companyName,
                        ImageUrl = tripImage,
                        AverageRating = (double)roundedRatingAverage,
                        QuantityRating = ratingQuantity,
                        EmptySeat = remainingSeats,
                        Price = lowestPrice,
                        StartLocation = trip.Route_Company.Route?.StartLocation,
                        EndLocation = trip.Route_Company.Route?.EndLocation,
                        StartDate = trip.StartTime?.ToString("yyyy-MM-dd"),
                        EndDate = trip.EndTime?.ToString("yyyy-MM-dd"),
                        StartTime = trip.StartTime?.ToString("HH:mm"),
                        EndTime = trip.EndTime?.ToString("HH:mm")
                    };
                    searchTripModels.Add(searchTrip);
                };

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

        private async Task<List<Trip>> GetFilteredTripIdsBySeatCode(List<Trip> tripsQuery, string seatCode)
        {
            var filteredTripIds = new List<Trip>();

            foreach (var trip in tripsQuery)
            {
                var checkTrip = await _unitOfWork.BookingRepository
                                                 .FindByCondition(_ => _.TripID == trip.TripID)
                                                 .AsNoTracking()
                                                 .FirstOrDefaultAsync();

                int checkQuantitySeat;
                int checkQuantitySeatBooked = 0;
                int checkQuantitySeatEmpty;

                if (checkTrip != null)
                {
                    var seatBookings = await _unitOfWork.TicketDetailRepository.GetAll()
                                                        .Where(_ => _.BookingID == checkTrip.BookingID
                                                            && _.SeatCode.StartsWith(seatCode)
                                                            && _.Status.Equals(SD.Booking_TicketStatus.UNUSED_TICKET))
                                                        .AsNoTracking()
                                                        .ToListAsync();

                    checkQuantitySeatBooked = seatBookings.Count;
                }

                switch (seatCode)
                {
                    case "A":
                        checkQuantitySeat = await _unitOfWork.TicketType_TripRepository
                                                             .FindByCondition(_ => _.TripID == trip.TemplateID
                                                                 && _.TicketType.Name.Equals("Hàng đầu"))
                                                             .Select(_ => _.Quantity)
                                                             .FirstOrDefaultAsync() ?? 0;
                        break;

                    case "B":
                        checkQuantitySeat = await _unitOfWork.TicketType_TripRepository
                                                             .FindByCondition(_ => _.TripID == trip.TemplateID
                                                                 && _.TicketType.Name.Equals("Hàng giữa"))
                                                             .Select(_ => _.Quantity)
                                                             .FirstOrDefaultAsync() ?? 0;
                        break;

                    case "C":
                        checkQuantitySeat = await _unitOfWork.TicketType_TripRepository
                                                             .FindByCondition(_ => _.TripID == trip.TemplateID
                                                                 && _.TicketType.Name.Equals("Hàng sau"))
                                                             .Select(_ => _.Quantity)
                                                             .FirstOrDefaultAsync() ?? 0;
                        break;

                    default:
                        checkQuantitySeat = 0;
                        break;
                }

                checkQuantitySeatEmpty = checkQuantitySeat - checkQuantitySeatBooked;

                if (checkQuantitySeatEmpty > 0)
                {
                    filteredTripIds.Add(trip);
                }
            }

            return filteredTripIds;
        }

        private async Task<List<Trip>> GetFilteredTripIdsByOption(List<Trip> tripsQuery, string optionFilter)
        {
            try
            {
                var filteredTripIds = new List<Trip>();

                switch (optionFilter)
                {
                    case SD.FilterOption.PRICE_ASC:
                        var ticketTypesAsc = await _unitOfWork.TicketType_TripRepository
                                                              .GetAll()
                                                              .Include(_ => _.Trip)
                                                              .Include(_ => _.TicketType)
                                                              .ToListAsync();

                        var ascPriceTripIds = tripsQuery.Select(trip => new
                        {
                            Trip = trip,
                            MinPrice = ticketTypesAsc
                                                                       .Where(_ => _.TripID == trip.TemplateID)
                                                                       .Min(_ => (decimal?)_.Price) ?? 0
                        })
                                                        .OrderBy(_ => _.MinPrice)
                                                        .Select(_ => _.Trip)
                                                        .ToList();

                        filteredTripIds.AddRange(ascPriceTripIds);
                        break;

                    case SD.FilterOption.PRICE_DESC:

                        var ticketTypesDesc = await _unitOfWork.TicketType_TripRepository
                                                               .GetAll()
                                                               .Include(_ => _.Trip)
                                                               .Include(_ => _.TicketType)
                                                               .ToListAsync();

                        var descPriceTripIds = tripsQuery.Select(trip => new
                        {
                            Trip = trip,
                            MinPrice = ticketTypesDesc
                                                                        .Where(_ => _.TripID == trip.TemplateID)
                                                                        .Min(_ => (decimal?)_.Price) ?? 0
                        })
                                                         .OrderByDescending(_ => _.MinPrice)
                                                         .Select(_ => _.Trip)
                                                         .ToList();

                        filteredTripIds.AddRange(descPriceTripIds);
                        break;

                    case SD.FilterOption.RATING_ASC:
                        var ascRatingTripIds = await _unitOfWork.FeedbackRepository.GetAll()
                                                                .GroupBy(_ => _.TemplateID)
                                                                .Select(_ => new
                                                                {
                                                                    TemplateID = _.Key,
                                                                    AverageRating = _.Average(_ => _.Rating)
                                                                })
                                                                .ToListAsync();

                        var allTemplateRatingAsc = tripsQuery.Select(_ => new
                        {
                            TemplateID = _.TemplateID,
                            AverageRating = ascRatingTripIds
                                                                               .FirstOrDefault(r => r.TemplateID == _.TemplateID)?.AverageRating ?? 0
                        })
                                                             .OrderBy(_ => _.AverageRating)
                                                             .Select(_ => _.TemplateID)
                                                             .ToList();

                        filteredTripIds = tripsQuery
                                          .Where(_ => allTemplateRatingAsc.Contains(_.TemplateID))
                                          .OrderBy(_ => allTemplateRatingAsc.IndexOf(_.TemplateID))
                                          .Select(_ => _)
                                          .ToList();
                        break;

                    case SD.FilterOption.RATING_DESC:
                        var descRatingTripIds = await _unitOfWork.FeedbackRepository.GetAll()
                                                                .GroupBy(_ => _.TemplateID)
                                                                .Select(_ => new
                                                                {
                                                                    TemplateID = _.Key,
                                                                    AverageRating = _.Average(_ => _.Rating)
                                                                })
                                                                .ToListAsync();

                        var allTemplateRatingDesc = tripsQuery.Select(_ => new
                        {
                            TemplateID = _.TemplateID,
                            AverageRating = descRatingTripIds
                                                                                .FirstOrDefault(r => r.TemplateID == _.TemplateID)?.AverageRating ?? 0
                        })
                                                              .OrderByDescending(_ => _.AverageRating)
                                                              .Select(_ => _.TemplateID)
                                                              .ToList();

                        filteredTripIds = tripsQuery
                                          .Where(_ => allTemplateRatingDesc.Contains(_.TemplateID))
                                          .OrderBy(_ => allTemplateRatingDesc.IndexOf(_.TemplateID))
                                          .Select(_ => _)
                                          .ToList();
                        break;

                    default:
                        filteredTripIds = tripsQuery;
                        break;
                }
                return filteredTripIds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task<(bool isSuccess, string message, List<TripPicture> data)> CreateTripImages(CreateTripModel createTripModel, Guid createdTripId)
        {
    
            var successImages = new List<TripPicture>();
            foreach (var imageUrl in createTripModel.ImageUrls)
            {
                var imageUploadResult = await _firebaseService.UploadFileToFirebase(imageUrl, FirebasePathName.TRIP + $"{Guid.NewGuid().ToString()}");
                if (!imageUploadResult.IsSuccess)
                {
                    return (false, SD.Notification.Internal("HÌNH ẢNH", "KHI TẢI LÊN"), null);
                }

                successImages.Add(new TripPicture
                {
                    TripPictureID = Guid.NewGuid(),
                    TripID = createdTripId,
                    ImageUrl = (string)imageUploadResult.Result,
                    Status = SD.GeneralStatus.ACTIVE
                });
            }

            await _unitOfWork.TripPictureRepository.AddRangeAsync(successImages);
            return (true, string.Empty, successImages);
        }
        private async Task<(bool isSuccess, string message, List<TicketType_Trip> data)> CreateTicketTypes(CreateTripModel createTripModel, Guid createdTripId)
        {
            try
            {
                var prices = new List<double>();
                var successTicketTypes = new List<TicketType_Trip>();
                foreach (var ticketType in createTripModel.TicketType_TripModels)
                {
                    successTicketTypes.Add(new TicketType_Trip
                    {
                        TicketTypeID = ticketType.TicketTypeID,
                        TripID = createdTripId,
                        Price = ticketType.Price,
                        Quantity = ticketType.Quantity,
                        Status = SD.GeneralStatus.ACTIVE
                    });
                }
                await _unitOfWork.TicketType_TripRepository.AddRangeAsync(successTicketTypes);

                return (true, string.Empty, successTicketTypes);
            }
            catch (Exception ex)
            {
                return (false, SD.Notification.Internal("LOẠI VÉ", "KHI TẠO"), null);
            }
        }
        private async Task<(bool isSuccess, string message, List<Trip_Utility> data)> CreateUtility(CreateTripModel createTripModel, Guid createdTripId)
        {
            try
            {
                var successUtilities = new List<Trip_Utility>();
                foreach (var tripUtility in createTripModel.Trip_UtilityModels)
                {
                    successUtilities.Add(new Trip_Utility
                    {
                        TripID = createdTripId,
                        UtilityID = tripUtility.UtilityID,
                        Status = SD.GeneralStatus.ACTIVE
                    });
                }
                await _unitOfWork.Trip_UtilityRepository.AddRangeAsync(successUtilities);

                return (true, string.Empty, successUtilities);
            }
            catch (Exception ex)
            {
                return (true, SD.Notification.Internal("TIỆN ÍCH", "KHI TẠO"), null);
            }

        }
        public async Task<ActionOutcome> CreateTrip(CreateTripModel createTripModel)
        {
            try
            {
                if (createTripModel.IsTemplate == true)
                {
                    var validationResult = await createTripModel.ValidateForCreatingWithTemplate(_unitOfWork);
                    if (!validationResult.isSuccess)
                    {
                        throw new BadRequestException(validationResult.message);
                    }

                    var result = new ActionOutcome();

                    var createdTrip = createTripModel.MapToTrip();
                    var tripID = createdTrip.TripID;
                    await _unitOfWork.TripRepository.AddAsync(createdTrip);

                    var CreateTripImageResult = await CreateTripImages(createTripModel, createdTrip.TripID);
                    if (!CreateTripImageResult.isSuccess)
                    {
                        throw new BadRequestException(CreateTripImageResult.message);
                    }

                    var CreateTicketTypeResult = await CreateTicketTypes(createTripModel, createdTrip.TripID);
                    if (!CreateTicketTypeResult.isSuccess)
                    {
                        throw new BadRequestException(CreateTicketTypeResult.message);
                    }

                    var CreateUtilityResult = await CreateUtility(createTripModel, createdTrip.TripID);
                    if (!CreateUtilityResult.isSuccess)
                    {
                        throw new BadRequestException(CreateUtilityResult.message);
                    }
                    var rs = _unitOfWork.Complete();
                    if (rs < 0)
                    {
                        return new ActionOutcome
                        {
                            IsSuccess = false,
                            Message = "LỖI KHI TẠO CHUYẾN XE MẪU!"
                        };
                    }

                    var prices = CreateTicketTypeResult.data.Select(_ => _.Price.HasValue ? (double)_.Price : 0).ToList();
                    var city = await _unitOfWork.Route_CompanyRepository
                                            .GetAll()
                                            .Include(_ => _.Route)
                                            .ThenInclude(_ => _.FromCity)
                                            .Include(_ => _.Route)
                                            .ThenInclude(_ => _.ToCity)
                                            .Where(_ => _.Route_CompanyID == createTripModel.Route_CompanyID)
                                            .FirstOrDefaultAsync();
                    result.Result = new GetTripAfterCreateModel
                    {
                        TripID = tripID,
                        StaffName = null,
                        StartDate = null,
                        StartTime = null,
                        EndDate = null,
                        EndTime = null,
                        FromCity = city.Route.FromCity.Name,
                        ToCity = city.Route.ToCity.Name,
                        StartLocation = city.Route.StartLocation,
                        EndLocation = city.Route.EndLocation,
                        MinPrice = prices.Min(),
                        MaxPrice = prices.Max(),
                        Status = createdTrip.Status
                    };
                    result.Message = "TẠO CHUYẾN XE MẪU THÀNH CÔNG!";
                    result.IsSuccess = true;
                    return result;
                }

                else
                {
                    var result = new ActionOutcome();

                    var getInformationTrip = await _unitOfWork.TripRepository
                                                              .FindByCondition(_ => _.TemplateID == createTripModel.TemplateID)
                                                              .FirstOrDefaultAsync();

                    if (getInformationTrip == null)
                    {
                        throw new NotFoundException(SD.Notification.NotFound("CHUYẾN XE"));
                    }

                    foreach (var timeTrip in createTripModel.TimeTrips)
                    {
                        if (timeTrip.StartTime == null || timeTrip.EndTime == null)
                        {
                            throw new BadRequestException("SỐ LƯỢNG THỜI GIAN BẮT ĐẦU PHẢI TRÙNG VỚI SỐ LƯỢNG THỜI GIAN KẾT THÚC!");
                        }
                        if (timeTrip.StartTime >= timeTrip.EndTime)
                        {
                            throw new BadRequestException("THỜI GIAN BẮT ĐẦU PHẢI TRƯỚC THỜI GIAN KẾT THÚC CHUYẾN XE!");
                        }
                    }
                    /*   if (createTrip.StaffID.Count != createTrip.TimeTrips.Count)
                       {
                           throw new BadRequestException("MỖI CHUYẾN ĐI PHẢI CÓ NHÂN VIÊN GIÁM SÁT!");
                       }*/
                    bool hasOverlap = false;
                    for (int i = 0; i < createTripModel.TimeTrips.Count; i++)
                    {
                        for (int j = i + 1; j < createTripModel.TimeTrips.Count; j++)
                        {
                            if (createTripModel.TimeTrips[i].StartTime <= createTripModel.TimeTrips[j].EndTime && createTripModel.TimeTrips[j].StartTime <= createTripModel.TimeTrips[i].EndTime)
                            {
                                hasOverlap = true;
                            }
                        }
                    }
                    var resultGetTrips = new List<GetTripAfterCreateModel>();

                    if (hasOverlap)
                    {
                        throw new BadRequestException("KHUNG THỜI GIAN CHUYẾN XE NÀY BỊ TRÙNG LẶP VỚI MỘT CHUYẾN XE KHÁC TRONG CÙNG NGÀY!");
                    }
                    else
                    {
                        for (int i = 0; i < createTripModel.TimeTrips.Count; i++)
                        {
                            var createTime = createTripModel.TimeTrips[i];
                            var checkExistTemplateInTime = await _unitOfWork.TripRepository
                                                                            .FindByCondition(_ => _.TemplateID == createTripModel.TemplateID
                                                                                               && _.IsTemplate == false
                                                                                               && _.Status == SD.GeneralStatus.ACTIVE)
                                                                            .ToListAsync();

                            foreach (var existingTrip in checkExistTemplateInTime)
                            {
                                if (createTime.StartTime <= existingTrip.EndTime && createTime.EndTime >= existingTrip.StartTime)
                                {
                                    throw new BadRequestException("KHUNG THỜI GIAN CHUYẾN XE NÀY BỊ TRÙNG LẶP VỚI MỘT CHUYẾN XE KHÁC TRONG CÙNG NGÀY!");
                                }
                            }
                            //var staff = createTrip.StaffID[i];
                            var getStaffName = await _unitOfWork.UserRepository
                                                                .FindByCondition(_ => _.UserID == createTripModel.StaffID)
                                                                .Select(_ => _.FullName)
                                                                .FirstOrDefaultAsync() ?? "Unknown Staff";

                            var getPrices = await _unitOfWork.TicketType_TripRepository
                                                             .GetAll()
                                                             .Where(_ => _.TripID == createTripModel.TemplateID)
                                                             .Select(_ => _.Price)
                                                             .ToListAsync();
                            var route_CompanyID = await _unitOfWork.TripRepository
                                                        .GetAll()
                                                        .Where(_ => _.TemplateID == createTripModel.TemplateID && _.IsTemplate == true)
                                                        .Select(_ => _.Route_CompanyID)
                                                        .FirstOrDefaultAsync();
                            var city = await _unitOfWork.Route_CompanyRepository
                                                    .GetAll()
                                                    .Include(_ => _.Route)
                                                    .ThenInclude(_ => _.FromCity)
                                                    .Include(_ => _.Route)
                                                    .ThenInclude(_ => _.ToCity)
                                                    .Where(_ => _.Route_CompanyID == route_CompanyID)
                                                    .FirstOrDefaultAsync();
                            var trip = new Trip
                            {
                                TripID = Guid.NewGuid(),
                                Route_CompanyID = getInformationTrip.Route_CompanyID,
                                StaffID = createTripModel.StaffID,
                                IsTemplate = false,
                                StartTime = createTime.StartTime,
                                EndTime = createTime.EndTime,
                                TemplateID = createTripModel.TemplateID,
                                Status = SD.GeneralStatus.ACTIVE
                            };

                            await _unitOfWork.TripRepository.AddAsync(trip);
                            var resultGetTrip = new GetTripAfterCreateModel
                            {
                                TripID = trip.TripID,
                                StaffName = getStaffName,
                                StartDate = trip.StartTime?.ToString("dd-MM-yyyy"),
                                StartTime = trip.StartTime?.ToString("HH:mm"),
                                EndDate = trip.EndTime?.ToString("dd-MM-yyyy"),
                                EndTime = trip.EndTime?.ToString("HH:mm"),
                                FromCity = city.Route.FromCity.Name,
                                ToCity = city.Route.ToCity.Name,
                                StartLocation = city.Route.StartLocation,
                                EndLocation = city.Route.EndLocation,
                                MinPrice = getPrices.Min(),
                                MaxPrice = getPrices.Max(),
                                Status = trip.Status
                            };
                            resultGetTrips.Add(resultGetTrip);
                        }
                    }

                    var rs = _unitOfWork.Complete();

                    foreach (var trip in resultGetTrips)
                    {
                        var staffID = await _unitOfWork.TripRepository
                                                       .GetAll()
                                                       .Where(s => s.TripID.Equals(trip.TripID))
                                                       .Select(s => s.StaffID)
                                                       .FirstOrDefaultAsync();
                        var token = await _unitOfWork.UserRepository
                                                    .GetAll()
                                                    .Where(u => u.UserID.Equals(staffID))
                                                    .Select(u=>u.AccessToken)
                                                    .FirstOrDefaultAsync();
                        var route_companyID = await _unitOfWork.TripRepository
                                                     .GetAll()
                                                     .Where(r => r.TripID.Equals(trip.TripID))
                                                     .Select(r => r.Route_CompanyID)
                                                     .FirstOrDefaultAsync();
                        var routeCompany = await _unitOfWork.Route_CompanyRepository
                                                     .GetAll()
                                                     .Where(r => r.Route_CompanyID.Equals(route_companyID))
                                                     .FirstOrDefaultAsync();
                        var route = await _unitOfWork.RouteRepository
                                                      .GetAll()
                                                      .Where(r => r.RouteID.Equals(routeCompany.RouteID))
                                                      .FirstOrDefaultAsync();
                        var company = await _unitOfWork.CompanyRepository
                                                       .GetAll()
                                                       .Where(c => c.CompanyID.Equals(routeCompany.CompanyID))
                                                       .FirstOrDefaultAsync();
                        var fromCity = await _unitOfWork.CityRepository.FindByCondition(c => c.CityID.Equals(route.FromCityID)).FirstOrDefaultAsync();
                        var toCity = await _unitOfWork.CityRepository.FindByCondition(c => c.CityID.Equals(route.ToCityID)).FirstOrDefaultAsync();

                        var message = new Message()
                        {
                            Token = token,
                            Notification = new Notification()
                            {
                                Title = "The Bus Journey",
                                Body = $"Bạn vừa được {company.Name} phân công vào chuyến xe từ {fromCity.Name} ở {route.StartLocation} đến {toCity.Name} ở {route.EndLocation} vào lúc {trip.StartTime} ngày {trip.StartDate} đến {trip.EndTime} ngày {trip.EndDate}."
                            },
                            Data = new Dictionary<string, string>()
                            {
                                { "Key1","Value1"}
                            }
                        };
                        string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                    }

                    result.Result = resultGetTrips;
                    result.Message = "TẠO CHUYẾN XE THÀNH CÔNG!";
                    result.IsSuccess = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateTrip(UpdateTripModel updateTripModel, Guid tripID)
        {
            try
            {
                var trip = await _unitOfWork.TripRepository.GetAll()
                                                           .Where(_ => _.TripID.Equals(tripID) && _.Status.Equals(SD.GeneralStatus.ACTIVE))
                                                           .FirstOrDefaultAsync();
                if (trip == null)
                {
                    throw new NotFoundException(SD.Notification.NotFoundByField("CHUYẾN XE", "ID"));
                }

                var bookingOfTrip = await _unitOfWork.BookingRepository.GetAll()
                                                                       .Where(_ => _.TripID.Equals(trip.TripID))
                                                                       .ToListAsync();

                if (bookingOfTrip.Any())
                {
                    throw new BadRequestException("CHUYẾN XE ĐÃ TỒN TẠI BOOKING.");
                }

                var checkExistTemplateInTime = await _unitOfWork.TripRepository
                                                .FindByCondition(_ => _.TemplateID == trip.TemplateID
                                                                   && _.IsTemplate == false
                                                                   && _.Status == SD.GeneralStatus.ACTIVE)
                                                .ToListAsync();

                foreach (var existingTrip in checkExistTemplateInTime)
                {
                    if (updateTripModel.StartTime <= existingTrip.EndTime && updateTripModel.EndTime >= existingTrip.StartTime)
                    {
                        throw new BadRequestException("KHUNG THỜI GIAN CHUYẾN XE NÀY BỊ TRÙNG LẶP VỚI MỘT CHUYẾN XE KHÁC TRONG CÙNG NGÀY!");
                    }
                }

                trip.StaffID = updateTripModel.StaffID;
                trip.StartTime = updateTripModel.StartTime;
                trip.EndTime = updateTripModel.EndTime;

                _unitOfWork.TripRepository.Update(trip);

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

        public async Task<bool> ChangeStatusTrip(Guid tripId)
        {
            try
            {
                var trip = await _unitOfWork.TripRepository
                                            .FindByCondition(_ => _.TripID == tripId)
                                            .FirstOrDefaultAsync();
                if (trip == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("CHUYẾN XE"));
                }
                if (trip.StartTime < DateTime.Now)
                {
                    throw new BadRequestException("KHÔNG THỂ THAY ĐỔI TRẠNG THÁI CỦA CHUYẾN XE");
                }
                trip.Status = SD.GeneralStatus.INACTIVE;
                _unitOfWork.TripRepository.Update(trip);
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

        public async Task<List<GetTripFromCompanyModel>> GetTripsFromCompany(Guid companyID)
        {
            try
            {
                var list = new List<GetTripFromCompanyModel>();
                var getRouteCompanyIds = await _unitOfWork.Route_CompanyRepository
                                                          .GetAll()
                                                          .Where(_ => _.CompanyID == companyID)
                                                          .Select(_ => _.Route_CompanyID)
                                                          .ToListAsync();

                var getTrips = await _unitOfWork.TripRepository
                                                .GetAll()
                                                .Include(_ => _.User)
                                                .Include(_ => _.Route_Company.Route.FromCity)
                                                .Include(_ => _.Route_Company.Route.ToCity)
                                                .Where(_ => getRouteCompanyIds.Contains((Guid)_.Route_CompanyID))
                                                .ToListAsync();

                foreach (var trip in getTrips)
                {
                    var getPrice = await _unitOfWork.TicketType_TripRepository
                                                    .GetAll()
                                                    .Include(_ => _.TicketType)
                                                    .Where(_ => _.TripID == trip.TemplateID)
                                                    .Select(_ => _.Price)
                                                    .ToListAsync();
                    var data = new GetTripFromCompanyModel
                    {
                        TripID = trip.TripID,
                        StaffID = trip.StaffID ?? Guid.Empty,
                        FromCity = trip.Route_Company?.Route?.FromCity?.Name ?? null,
                        ToCity = trip.Route_Company?.Route?.ToCity?.Name ?? null,
                        StartLocation = trip.Route_Company?.Route?.StartLocation ?? null,
                        EndLocation = trip.Route_Company?.Route?.EndLocation ?? null,
                        StartDate = trip.StartTime?.ToString("yyyy-MM-dd"),
                        StartTime = trip.StartTime?.ToString("HH:mm"),
                        EndDate = trip.EndTime?.ToString("yyyy-MM-dd"),
                        EndTime = trip.EndTime?.ToString("HH:mm"),
                        StaffName = trip.User?.FullName ?? null,
                        MinPrice = getPrice.Any() ? getPrice.Min() : 0,
                        MaxPrice = getPrice.Any() ? getPrice.Max() : 0,
                        Status = trip.Status,
                    };
                    list.Add(data);
                }
                return list;
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
                                                      .FindByCondition(_ => _.Booking.TripID == tripID
                                                                    && _.Status.Trim().Equals(SD.Booking_TicketStatus.UNUSED_TICKET)
                                                                    && _.TicketType_Trip.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                      .Include(_ => _.Booking.Trip)
                                                      .Select(_ => new
                                                      {
                                                          _.SeatCode,
                                                          _.Booking.Trip.StartTime
                                                      })
                                                      .ToListAsync();

                var findTrip = await _unitOfWork.TripRepository.FindByCondition(_ => _.TripID == tripID).FirstOrDefaultAsync();

                var tripIDFromDb = await GetTripIDFromTemplate(tripID);

                var ticketTypeTrips = await _unitOfWork.TicketType_TripRepository
                                                       .FindByCondition(_ => _.TripID == tripIDFromDb.TripID
                                                                     && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                       .Select(_ => new GetSeatBookedFromTripModel.TicketType_TripModel
                                                       {
                                                           TicketType_TripID = _.TicketType_TripID,
                                                           TicketName = _.TicketType.Name,
                                                           Price = (double)_.Price,
                                                           Quantity = (int)_.Quantity,
                                                       })
                                                       .ToListAsync();
                var totalSeat = await _unitOfWork.TicketType_TripRepository
                                                 .FindByCondition(_ => _.TripID == tripIDFromDb.TripID && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                 .SumAsync(_ => _.Quantity);
                var seatBookeds = bookingDetails.Select(_ => _.SeatCode).ToList();
                var result = new GetSeatBookedFromTripModel
                {
                    TripID = tripID,
                    RouteID = (Guid)tripIDFromDb.Route_Company.RouteID,
                    CompanyName = tripIDFromDb.Route_Company.Company.Name,
                    SeatBooked = seatBookeds,
                    TotalSeats = (int)totalSeat,
                    StartLocation = tripIDFromDb.Route_Company.Route.StartLocation,
                    EndLocation = tripIDFromDb.Route_Company.Route.EndLocation,
                    StartDate = findTrip.StartTime?.ToString("yyyy-MM-dd"),
                    StartTime = findTrip.StartTime?.ToString("HH:mm"),
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
            var getTemplateID = await _unitOfWork.TripRepository
                                                        .GetAll()
                                                        .Where(_ => _.TripID == id)
                                                        .Select(_ => _.TemplateID)
                                                        .FirstOrDefaultAsync();
            var utilities = await _unitOfWork.Trip_UtilityRepository
                                             .FindByCondition(_ => _.TripID == getTemplateID && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                             .Select(_ => _.Utility)
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
            };
            return result;
        }

        public async Task<Trip> GetTripIDFromTemplate(Guid id)
        {
            var getTemplateID = await _unitOfWork.TripRepository
                                                 .GetAll()
                                                 .Where(_ => _.TripID == id)
                                                 .Select(_ => _.TemplateID)
                                                 .FirstOrDefaultAsync();

            var getTripID = await _unitOfWork.TripRepository
                                             .FindByCondition(_ => _.TemplateID == getTemplateID)
                                             .Include(_ => _.Route_Company)
                                             .Include(_ => _.Route_Company.Route)
                                             .Include(_ => _.Route_Company.Route.FromCity)
                                             .Include(_ => _.Route_Company.Route.ToCity)
                                             .Include(_ => _.Route_Company.Company)
                                             .ToListAsync();

            var tripID = getTripID.FirstOrDefault(_ => _.IsTemplate == true) ?? null;

            return tripID;
        }
    }
}