/*using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using TicketBooking.Common.Payloads.Requests;
using TicketBooking.Dtos;
using TicketBooking.Entities;
using TicketBooking.Exceptions;
using TicketBooking.Repositories;

namespace TicketBooking.Services
{
    public class Trip_LocationService
    {

        private readonly IRepository<Trip_Location, int> _tripLocationRepository;
        private readonly IRepository<Location, int> _locationRepository;

        private readonly IMapper _mapper;

        public Trip_LocationService(IRepository<Trip_Location, int> tripLocationRepository, IRepository<Location, int> locationRepository, IMapper mapper)
        {
            _tripLocationRepository = tripLocationRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<TimeLineDTO> returnModel, string message)> GetLocationsByTripID(int tripID)
        {
            try
            {
                var locationsAndTimes = await _tripLocationRepository
                    .GetAll()
                    .Where(tl => tl.TripID == tripID)
                    .Include(tl => tl.Location)
                    .Select(tl => new TimeLineDTO
                    {
                        Name = tl.Location.Name,
                        Time = tl.Time.ToString() 
                    })
                    .ToListAsync();

                if (!locationsAndTimes.Any())
                {
                    return (null, "Empty List");
                }

                return (locationsAndTimes, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public async Task<( IEnumerable<Trip_LocationDTO> returnModel, string message)> CreateTripLocation(IEnumerable<CreateTripLocationRequest> createTripLocationRequest)
        {
            try
            {
                var createTripLocations = new List<Trip_Location>();
                foreach (var activeLocation in createTripLocationRequest) {
                    var checkActiveLocation = await _locationRepository.FindByCondition(l => l.LocationID == activeLocation.LocationID).FirstOrDefaultAsync();
                    if (checkActiveLocation.Status.ToLower().Equals("inactive"))
                    {
                        return (null, "Location already does not exists on this trip");
                    }
                    var checkExistedTripLocation = await _tripLocationRepository.FindByCondition(x => x.TripID == activeLocation.TripID && x.LocationID == activeLocation.LocationID).FirstOrDefaultAsync();
                    if (checkExistedTripLocation != null)
                    {
                        return (null, "Location already exists on this trip");
                    }
                    var tripLocationEntity = new Trip_Location
                    {
                        TripID = activeLocation.TripID,
                        LocationID = activeLocation.LocationID,
                        Time = activeLocation.Time,
                    };

                    await _tripLocationRepository.AddAsync(tripLocationEntity);
                    createTripLocations.Add(tripLocationEntity);
                }

                int result = await _tripLocationRepository.Commit();
                if (result > 0)
                {
                    var tripLocationDTOs = _mapper.Map<IEnumerable<Trip_LocationDTO>>(createTripLocations);
                    return (tripLocationDTOs, "Ok");
                }
                return (null, "Fail");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public async Task<(Trip_LocationDTO returnModel, string message)> UpdateTripLocation(Trip_LocationDTO trip_LocationDTO, int trip_LocationID)
        {
            try
            {
                var findTripLocation = await _tripLocationRepository.GetByIdAsync(trip_LocationID);
                if (findTripLocation == null) {
                    return (null, "Not found!");
                }

                findTripLocation.LocationID = trip_LocationDTO.LocationID;
                findTripLocation.Time = trip_LocationDTO.Time;
                var checkExistedTripLocation = await _tripLocationRepository.FindByCondition(x => x.TripID == findTripLocation.TripID && x.LocationID == findTripLocation.LocationID).FirstOrDefaultAsync();
                if (checkExistedTripLocation != null)
                {
                    return (null, "Location already exists on this trip");
                }
                var tripLocationEntity = _mapper.Map<Trip_Location>(findTripLocation);
                _tripLocationRepository.Update(findTripLocation);
                await _tripLocationRepository.Commit();
                var mapTripLocation = _mapper.Map<Trip_LocationDTO>(findTripLocation);
                return (mapTripLocation, "Ok");
            }
            
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

    }
}
*/