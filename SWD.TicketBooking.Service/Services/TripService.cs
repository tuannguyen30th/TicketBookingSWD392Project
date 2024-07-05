using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;
using System.Linq;

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
                    var companyName = tripID.Route_Company.Company.Name;

                    var searchTrip = new SearchTripModel
                    {
                        TripID = trip.TripID,
                        RouteID = (Guid)trip.Route_Company.RouteID,
                        TemplateID = (Guid)trip.TemplateID,
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

                return searchTripModels;
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

                    var getTripID = await _unitOfWork.TripRepository
                                                     .FindByCondition(_ => _.TemplateID == getTemplateID)
                                                     .ToListAsync();
                    var tripID = getTripID.FirstOrDefault(_ => _.IsTemplate == true)?.TripID ?? Guid.Empty;

                    var pics = await _unitOfWork.TripPictureRepository
                                                .GetAll()
                                                .Where(_ => _.TripID == tripID)
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
                var topTrips = await _unitOfWork.BookingRepository.GetAll()
                                            .GroupBy(b => b.TripID)
                                            .Select(g => new
                                            {
                                                TripID = g.Key,
                                                TotalQuantity = g.Sum(b => b.Quantity),
                                            })
                                            .OrderByDescending(t => t.TotalQuantity)
                                            .Take(5)
                                            .ToListAsync();

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
                    tripsQuery.ToList();

                }
                var seatFilteredTrips = new List<Trip>();
                if (seatAvailability != null && seatAvailability.Length > 0)
                {
                    foreach (var seatType in seatAvailability)
                    {
                        switch (seatType.Trim().ToUpper())
                        {
                            case SD.FilterOption.SEAT_HEAD:
                                var headTripIds = await GetFilteredTripIdsBySeatCode(tripsQuery, SD.FilterOption.SEAT_A);
                                seatFilteredTrips.AddRange(headTripIds);
                                break;

                            case SD.FilterOption.SEAT_MIDDLE:
                                var middleTripIds = await GetFilteredTripIdsBySeatCode(tripsQuery, SD.FilterOption.SEAT_B);
                                seatFilteredTrips.AddRange(middleTripIds);
                                break;

                            case SD.FilterOption.SEAT_BACK:
                                var backTripIds = await GetFilteredTripIdsBySeatCode(tripsQuery, SD.FilterOption.SEAT_C);
                                seatFilteredTrips.AddRange(backTripIds);
                                break;

                            default:
                                break;
                        }
                    }
                    if (seatFilteredTrips != null && seatFilteredTrips.Count > 0)
                    {
                        tripsQuery = seatFilteredTrips.ToList();
                    }
                    tripsQuery.ToList();

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
                var totalTrips = tripsQuery.Count();
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
                                                                    TotalRating = _.Sum(_ => _.Rating)
                                                                })
                                                                .ToListAsync();

                        var allTemplateRatingAsc = tripsQuery.Select(_ => new
                                                             {
                                                                 TemplateID = _.TemplateID,
                                                                 TotalRating = ascRatingTripIds
                                                                               .FirstOrDefault(r => r.TemplateID == _.TemplateID)?.TotalRating ?? 0
                                                             })
                                                             .OrderBy(_ => _.TotalRating)
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
                                                                    TotalRating = _.Sum(_ => _.Rating)
                                                                })
                                                                .ToListAsync();

                        var allTemplateRatingDesc = tripsQuery.Select(_ => new
                                                              {
                                                                  TemplateID = _.TemplateID,
                                                                  TotalRating = descRatingTripIds
                                                                                .FirstOrDefault(r => r.TemplateID == _.TemplateID)?.TotalRating ?? 0
                                                              })
                                                              .OrderByDescending(_ => _.TotalRating)
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

    

        public async Task<bool> CreateTrip(CreateTripModel createTrip)
        {
            try
            {
                if (createTrip.IsTemplate == true)
                {
                    if (createTrip.StartTime == null || createTrip.EndTime == null || createTrip.ImageUrls == null)
                    {
                        throw new BadRequestException("TẤT CẢ CÁC TRƯỜNG PHẢI CÓ DỮ LIỆU!");
                    }
                    if (createTrip.StartTime > createTrip.EndTime)
                    {
                        throw new BadRequestException("THỜI GIAN BẮT ĐẦU PHẢI TRƯỚC THỜI GIAN KẾT THÚC CHUYẾN XE!");
                    }
                    var tripID = Guid.NewGuid();
                    var trip = new Trip
                    {
                        TripID = tripID,
                        Route_CompanyID = createTrip.Route_CompanyID,
                        StaffID = createTrip.StaffID,
                        IsTemplate = true,
                        StartTime = createTrip.StartTime,
                        EndTime = createTrip.EndTime,
                        TemplateID = tripID,
                        Status = SD.GeneralStatus.ACTIVE
                    };
                    await _unitOfWork.TripRepository.AddAsync(trip);
                    var imageUrls = createTrip.ImageUrls;
                    foreach (var imageUrl in imageUrls)
                    {
                        var guidPath = Guid.NewGuid().ToString();
                        var imagePath = FirebasePathName.TRIP + $"{guidPath}";
                        var imageUploadResult = await _firebaseService.UploadFileToFirebase(imageUrl, imagePath);
                        if (!imageUploadResult.IsSuccess)
                        {
                            throw new InternalServerErrorException(SD.Notification.Internal("HÌNH ẢNH", "KHI TẢI LÊN"));
                        }

                        var newtripImage = new TripPicture
                        {
                            TripPictureID = Guid.NewGuid(),
                            TripID = trip.TripID,
                            ImageUrl = (string)imageUploadResult.Result,
                            Status = SD.GeneralStatus.ACTIVE
                        };

                        await _unitOfWork.TripPictureRepository.AddAsync(newtripImage);
                    };
                    if (createTrip.TicketType_TripModels.Count < 2)
                    {
                        throw new BadRequestException("PHẢI CÓ ÍT NHẤT 2 LOẠI GHẾ!");
                    }
                    int totalQuantity = (int)createTrip.TicketType_TripModels.Sum(_ => _.Quantity);
                    if (totalQuantity < 20)
                    {
                        throw new BadRequestException("TỔNG SỐ LƯỢNG GHẾ PHẢI ÍT NHẤT LÀ 20!");
                    }

                    foreach (var ticketType in createTrip.TicketType_TripModels)
                    {
                        if (ticketType.Price <= 0 || ticketType.Quantity <= 0)
                        {
                            throw new BadRequestException("GIÁ VÉ VÀ SỐ LƯỢNG PHẢI LỚN HƠN 0!");
                        }
                        if (ticketType.Quantity % 4 != 0)
                        {
                            throw new BadRequestException("SỐ LƯỢNG GHẾ KHÔNG HỢP LỆ!");
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
                    foreach (var tripUtility in createTrip.Trip_UtilityModels)
                    {
                        var newTrip_Utility = new Trip_Utility
                        {
                            TripID = trip.TripID,
                            UtilityID = tripUtility.UtilityID,
                            Status = SD.GeneralStatus.ACTIVE
                        };
                        await _unitOfWork.Trip_UtilityRepository.AddAsync(newTrip_Utility);
                    };
                    var rs = _unitOfWork.Complete();
                    if (rs < 0)
                    {
                        return false;
                    }
                    return true;
                }
                else
                {
                    var getInformationTrip = await _unitOfWork.TripRepository
                                                              .FindByCondition(_ => _.TemplateID == createTrip.TemplateID)
                                                              .FirstOrDefaultAsync();
                    if (getInformationTrip == null)
                    {
                        throw new NotFoundException("THÔNG TIN CHUYẾN XE KHÔNG TÌM THẤY!");
                    }
                    /*   var getInformationTicketTypes = await _unitOfWork.TicketType_TripRepository
                                                                       .GetAll()
                                                                       .Where(_ => _.TripID == getInformationTrip.TripID)
                                                                       .ToListAsync();
                       var getInformationUtilitys = await _unitOfWork.Trip_UtilityRepository
                                                                    .GetAll()
                                                                    .Where(_ => _.TripID == getInformationTrip.TripID)
                                                                    .ToListAsync();
                       var getInformationTripPictures = await _unitOfWork.TripPictureRepository
                                                                        .GetAll()
                                                                        .Where(_ => _.TripID == getInformationTrip.TripID)
                                                                        .Select(_ => _.ImageUrl)
                                                                        .ToListAsync();*/
                    if (createTrip.StartTime == null || createTrip.EndTime == null)
                    {
                        throw new BadRequestException("TẤT CẢ CÁC TRƯỜNG PHẢI CÓ DỮ LIỆU!");
                    }
                    if (createTrip.StartTime > createTrip.EndTime)
                    {
                        throw new BadRequestException("THỜI GIAN BẮT ĐẦU PHẢI TRƯỚC THỜI GIAN KẾT THÚC CHUYẾN XE!");
                    }
                    var trip = new Trip
                    {
                        TripID = Guid.NewGuid(),
                        Route_CompanyID = getInformationTrip.Route_CompanyID,
                        StaffID = createTrip.StaffID,
                        IsTemplate = false,
                        StartTime = createTrip.StartTime,
                        EndTime = createTrip.EndTime,
                        TemplateID = createTrip.TemplateID,
                        Status = SD.GeneralStatus.ACTIVE
                    };
                    await _unitOfWork.TripRepository.AddAsync(trip);
                    var rs = _unitOfWork.Complete();
                    if (rs < 0)
                    {
                        return false;
                    }
                    return true;
                }
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
                var trip = await _unitOfWork.TripRepository
                                            .FindByCondition(_ => _.TripID == tripId)
                                            .FirstOrDefaultAsync();
                if (trip == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("CHUYẾN XE"));
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

        public async Task<GetSeatBookedFromTripModel> GetSeatBookedFromTrip(Guid tripID)
        {
            try
            {
                var bookingDetails = await _unitOfWork.TicketDetailRepository
                                                      .FindByCondition(_ => _.Booking.TripID == tripID && _.Status.Trim().Equals(SD.Booking_TicketStatus.UNUSED_TICKET) && _.TicketType_Trip.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
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
                    .FindByCondition(_ => _.TripID == tripIDFromDb.TripID && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
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
            var tripID = await GetTripIDFromTemplate(id);
            var utilities = await _unitOfWork.Trip_UtilityRepository
                                             .FindByCondition(_ => _.TripID == tripID.TripID && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
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