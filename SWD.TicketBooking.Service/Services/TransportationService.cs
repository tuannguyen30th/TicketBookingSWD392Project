/*using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TicketBooking.Common.Payloads.Requests;
using TicketBooking.Dtos;
using TicketBooking.Entities;
using TicketBooking.Repositories;

namespace TicketBooking.Services
{
    public class TransportationService
    {
        private readonly IRepository<Transportation, int> _transportationRepo;
        private readonly IMapper _mapper;

        public TransportationService() { }
        public TransportationService(IRepository<Transportation, int> transportationRepo, IMapper mapper)
        {
            _transportationRepo = transportationRepo;
            _mapper = mapper;
        }

        public async Task<(List<TransportationDTO> returnModel, string message)> GetAllTransportations()
        {
            try
            {
                var transportations = await _transportationRepo.GetAll().ToListAsync();
                if (transportations.IsNullOrEmpty())
                {
                    return (null, "Empty List");
                }
                var result = _mapper.Map<List<TransportationDTO>>(transportations);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(TransportationDTO returnModel, string message)> GetTransportationById(int id)
        {
            try
            {
                var transportation = await _transportationRepo.FindByCondition(t => t.TransportationID == id && (t.Status == null || t.Status.ToUpper() == "ACTIVE")).FirstOrDefaultAsync();
                if (transportation == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<TransportationDTO>(transportation);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(TransportationDTO returnModel, string message)> CreateTransportation(CreateTransportationReq req)
        {
            try
            {
                var transportation = await _transportationRepo.AddAsync(new Transportation
                {
                    Name = req.Name,
                    FirstUseDate = req.FirstUseDate,
                    SeatTypeID = req.SeatTypeID,
                    StationID = req.StationID,
                    TransportationStructureID = req.TransportationStructureID,
                    VehicleTypeID = req.VehicleTypeID,
                    Status = "Active"
                });
                await _transportationRepo.Commit();
                if (transportation == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<TransportationDTO>(transportation);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(TransportationDTO returnModel, string message)> UpdateTransportation(int id, UpdateTransportationReq req)
        {
            try
            {
                var transportation = await _transportationRepo.FindByCondition(t => t.TransportationID == id).FirstOrDefaultAsync();
                if (transportation == null)
                {
                    return (null, "Cannot found this transportation");
                }
                else
                {
                    transportation.Name = req.Name;
                    transportation.VehicleTypeID = req.VehicleTypeID;
                    transportation.TransportationStructureID = req.TransportationStructureID;
                    transportation.FirstUseDate = req.FirstUseDate;
                    transportation.SeatTypeID = req.SeatTypeID;
                    transportation.StationID = req.StationID;
                }
                var updateResult = _transportationRepo.Update(transportation);
                await _transportationRepo.Commit();
                if (updateResult == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<TransportationDTO>(updateResult);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(TransportationDTO returnModel, string message)> ChangeTransportationStatus(int id, ChangeStatusReq req)
        {
            try
            {
                var transportation = await _transportationRepo.FindByCondition(t => t.TransportationID == id).FirstOrDefaultAsync();
                if (transportation == null)
                {
                    return (null, "Cannot found this transportation");
                }
                else
                {
                    transportation.Status = req.Status;
                }
                var updateResult = _transportationRepo.Update(transportation);
                await _transportationRepo.Commit();
                if (updateResult == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<TransportationDTO>(updateResult);
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