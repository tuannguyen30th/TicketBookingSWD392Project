/*using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TicketBooking.Common.Payloads.Requests;
using TicketBooking.Dtos;
using TicketBooking.Entities;
using TicketBooking.Repositories;

namespace TicketBooking.Services
{
    public class TransportationStructureService
    {
        private readonly IRepository<TransportationStructure, int> _transportationStructureRepo;
        private readonly IMapper _mapper;

        public TransportationStructureService(IRepository<TransportationStructure, int> transportationStructureRepo, IMapper mapper)
        {
            _transportationStructureRepo = transportationStructureRepo;
            _mapper = mapper;
        }

        public async Task<(List<TransportationStructureDTO> returnModel, string message)> GetAllTransportationStructure()
        {
            try
            {
                var transportationStructures = await _transportationStructureRepo.GetAll().ToListAsync();
                if (transportationStructures.IsNullOrEmpty())
                {
                    return (null, "Empty List");
                }
                var result = _mapper.Map<List<TransportationStructureDTO>>(transportationStructures);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(TransportationStructureDTO returnModel, string message)> GetTransportationStructureById(int id)
        {
            try
            {
                var transportationStructure = await _transportationStructureRepo.FindByCondition(v => v.TransportationStructureID == id && v.Status.ToUpper() == "Active".ToUpper()).FirstOrDefaultAsync();
                if (transportationStructure == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<TransportationStructureDTO>(transportationStructure);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(TransportationStructureDTO returnModel, string message)> CreateTransportationStructure(CreateTransportationStructureReq req)
        {
            try
            {
                var transportationStructure = await _transportationStructureRepo.AddAsync(new TransportationStructure
                {
                    Row = req.Row,
                    IsCabin = req.IsCabin,
                    SeatPerRow = req.SeatPerRow,
                    Status = "Active"
                });
                await _transportationStructureRepo.Commit();
                if (transportationStructure == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<TransportationStructureDTO>(transportationStructure);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(TransportationStructureDTO returnModel, string message)> ChangeTransportationStructureStatus(int id, ChangeStatusReq req)
        {
            try
            {
                var transportationStructure = await _transportationStructureRepo.FindByCondition(v => v.TransportationStructureID == id).FirstOrDefaultAsync();
                if (transportationStructure == null)
                {
                    return (null, "Cannot found this vehicle type");
                }
                else
                {
                    transportationStructure.Status = req.Status;
                }
                var updateResult = _transportationStructureRepo.Update(transportationStructure);
                await _transportationStructureRepo.Commit();
                if (updateResult == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<TransportationStructureDTO>(updateResult);
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