/*using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TicketBooking.Common.Payloads.Requests;
using TicketBooking.Dtos;
using TicketBooking.Entities;
using TicketBooking.Repositories;

namespace TicketBooking.Services
{
    public class VehicleTypeService
    {
        private readonly IRepository<VehicleType, int> _vehicleTypeRepo;
        private readonly IMapper _mapper;

        public VehicleTypeService() { }
        public VehicleTypeService(IRepository<VehicleType, int> vehicleTypeRepo, IMapper mapper)
        {
            _vehicleTypeRepo = vehicleTypeRepo;
            _mapper = mapper;
        }

        public async Task<(List<VehicleTypeDTO> returnModel, string message)> GetAllVehicleType()
        {
            try
            {
                var vehicleTypes = await _vehicleTypeRepo.GetAll().ToListAsync();
                if(vehicleTypes.IsNullOrEmpty())
                {
                    return (null, "Empty List");
                }
                var result = _mapper.Map<List<VehicleTypeDTO>>(vehicleTypes);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(VehicleTypeDTO returnModel, string message)> GetVehicleTypeById(int id)
        {
            try
            {
                var vehicleType = await _vehicleTypeRepo.FindByCondition(v => v.VehicleTypeID == id && v.Status.ToUpper() == "Active".ToUpper()).FirstOrDefaultAsync();
                if(vehicleType == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<VehicleTypeDTO>(vehicleType);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(VehicleTypeDTO returnModel, string message)> CreateVehicleType(CreateVehicleTypeReq req)
        {
            try
            {
                var vehicleType = await _vehicleTypeRepo.AddAsync(new VehicleType
                {
                    Name = req.Name,
                    Status = "Active"
                });
                await _vehicleTypeRepo.Commit();
                if (vehicleType == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<VehicleTypeDTO>(vehicleType);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(VehicleTypeDTO returnModel, string message)> ChangeVehicleTypeStatus(int id, ChangeStatusReq req)
        {
            try
            {
                var vehicleType = await _vehicleTypeRepo.FindByCondition(v => v.VehicleTypeID == id).FirstOrDefaultAsync();
                if (vehicleType == null)
                {
                    return (null, "Cannot found this vehicle type");
                }
                else
                {
                    vehicleType.Status = req.Status;
                }
                var updateResult = _vehicleTypeRepo.Update(vehicleType);
                await _vehicleTypeRepo.Commit();
                if (updateResult == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<VehicleTypeDTO>(updateResult);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
    }
}
*/