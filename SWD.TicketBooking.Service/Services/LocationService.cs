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
    public class LocationService
    {
        private readonly IRepository<Location, int> _locationRepo;
        private readonly IMapper _mapper;
        public LocationService(IRepository<Location, int> locationRepo, IMapper mapper)
        {
            _locationRepo = locationRepo;
            _mapper = mapper;
        }
        public async Task<(IEnumerable<LocationDTO> returnModel, string message)> GetAllLocations()
        {
            try
            {
                var locations = await _locationRepo.GetAll().Where(l => l.Status.ToLower().Equals("active")).ToListAsync();
                if (locations == null)
                {
                    throw new NotFoundException("Empty List");
                }
                var locationModels = _mapper.Map<IEnumerable<LocationDTO>>(locations);
                return (locationModels, "OK");

            }
            catch (Exception ex)
            {
                return (null, ex.Message);

            }
        }
        public async Task<(IEnumerable<LocationDTO> returnModel, string message)> CreateLocations(IEnumerable<CreateLocationRequest> createLocationRequest)
        {
            try
            {
                var createdLocations = new List<Location>();

                foreach (var loca in createLocationRequest)
                {
                    var existedLocation = await _locationRepo.FindByCondition(s => s.Name.ToLower().Equals(loca.Name.ToLower())).FirstOrDefaultAsync();
                    if (existedLocation != null)
                    {
                        return (null, "This Location has been existed!");
                    }

                    var locationEntity = new Location
                    {
                        Name = loca.Name,
                        Status = "Active"
                    };

                    await _locationRepo.AddAsync(locationEntity);
                    createdLocations.Add(locationEntity);
                }

                int result = await _locationRepo.Commit();
                if (result > 0)
                {
                    var locationDTOs = _mapper.Map<IEnumerable<LocationDTO>>(createdLocations);
                    return (locationDTOs, "Ok");
                }
                return (null, "Fail");

            }
            catch (Exception ex)
            {
                return (null, ex.Message);

            }
        }
        public async Task<(LocationDTO returnModel, string message)> UpdateLocation(CreateLocationRequest updateLocationRequest, int locationID)
        {
            try
            {
                var checkExistedLocation = await _locationRepo.GetByIdAsync(locationID);
                if (checkExistedLocation == null)
                {
                    return (null, "Location not found");
                }

                var existedLocation = await _locationRepo
                    .FindByCondition(s => s.Name.ToLower().Equals(updateLocationRequest.Name.ToLower()) && s.LocationID != locationID)
                    .FirstOrDefaultAsync();
                if (existedLocation != null)
                {
                    return (null, "This location name already exists");
                }

                checkExistedLocation.Name = updateLocationRequest.Name;
                checkExistedLocation.Status = "Active";

                _locationRepo.Update(checkExistedLocation);
                int result = await _locationRepo.Commit();
                if (result > 0)
                {
                    var locationDTO = _mapper.Map<LocationDTO>(checkExistedLocation);
                    return (locationDTO, "Update successful");
                }

                return (null, "Update failed");

            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public async Task<(LocationDTO returnModel, string message)> ChangStatusLocations(ChangeStatusReq changeStatusReq, int locationID)
        {
            try
            {
                var checkExistedLocation = await _locationRepo.GetByIdAsync(locationID);
                if (checkExistedLocation == null)
                {
                    return (null, "Location not found");
                }

                checkExistedLocation.Status = changeStatusReq.Status;

                _locationRepo.Update(checkExistedLocation);
                int result = await _locationRepo.Commit();
                if (result > 0)
                {
                    var locationDTO = _mapper.Map<LocationDTO>(checkExistedLocation);
                    return (locationDTO, "Update status successful");
                }

                return (null, "Update status failed");
            }
            catch (Exception ex)
            {
              
                return (null, ex.Message);
            }
        }


    }
}
*/