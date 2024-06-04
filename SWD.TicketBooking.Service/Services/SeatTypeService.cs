/*using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using TicketBooking.Dtos;
using TicketBooking.Entities;
using TicketBooking.Exceptions;
using TicketBooking.Repositories;

namespace TicketBooking.Services
{
    public class SeatTypeService
    {
        private readonly IRepository<TicketType, int> _seatTypeRepository;
        private readonly IMapper _mapper;
        
        public SeatTypeService(IRepository<TicketType, int> seatTypeRepository, IMapper mapper)
        {
            _seatTypeRepository = seatTypeRepository;
            _mapper = mapper;
        }
        public async Task<(IEnumerable<SeatTypeDTO> returnModel, string message)> GetAllSeatTypes()
        {
            try
            {
                var seatTypes = _seatTypeRepository.GetAll();
                if(seatTypes == null)
                {
                    throw new NotFoundException("Empty List");
                }
                var seatTypeModels = _mapper.Map<IEnumerable<SeatTypeDTO>>(seatTypes);
                return (seatTypeModels, "OK");

            }
            catch (Exception ex)
            {
                return (null, ex.Message);

            }
        }
        public async Task<(SeatTypeDTO returnModel, string message)> GetSeatTypeByID(int id)
        {
            try
            {
                var seatType = await _seatTypeRepository.GetByIdAsync(id);
                if (seatType == null)
                {
                    return (null, "Not Found");
                }
                var seatTypeModel = _mapper.Map<SeatTypeDTO>(seatType);
                return (seatTypeModel, "Ok");

            }
            catch (Exception ex)
            {
                return (null, ex.Message);

            }
        }
        public async Task<(SeatTypeDTO returnModel, string message)> CreateSeatType(SeatTypeDTO seatTypeDTO)
        {
            try
            {
                var seatType = _mapper.Map<TicketType>(seatTypeDTO);
                var existedSeatType = await _seatTypeRepository.FindByCondition(s => s.Name.ToLower().Equals(seatTypeDTO.Name.ToLower())).FirstOrDefaultAsync(); 
                 if(existedSeatType != null) {
                    return (null, "This SeatType has been existed!");            
                   }
                await _seatTypeRepository.AddAsync(seatType);
                int result = await _seatTypeRepository.Commit();
                if (result > 0)
                {
                    return (_mapper.Map<SeatTypeDTO>(seatType), "Ok");
                }
                return (null, "Fail");

            }
            catch (Exception ex)
            {
                return (null, ex.Message);

            }
        }

    }
}
*/