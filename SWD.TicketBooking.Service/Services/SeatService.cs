/*using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Dtos;
using TicketBooking.Entities;
using TicketBooking.Exceptions;
using TicketBooking.Repositories;

namespace TicketBooking.Services
{
    public class SeatService
    {
        private readonly IRepository<Seat, int> _seatRepository;
        private readonly IMapper _mapper;

        public SeatService(IRepository<Seat, int> seatRepository, IMapper mapper)
        {
            _seatRepository = seatRepository;
            _mapper = mapper;
        }
        public async Task<(IEnumerable<SeatDTO> returnModel, string message)> GetAllSeatsHaveBookByTripID(int tripID)
        {
            try
            {
                var seats = await _seatRepository.GetAll().Where(x => x.TripID == tripID && x.IsBooked == true).ToListAsync();
                if (seats == null)
                {
                    return (null, "Empty List!");
                }
                var seatModels = _mapper.Map<IEnumerable<SeatDTO>>(seats);
                return (seatModels, "Ok");

            }
            catch (Exception ex)
            {
                return (null, ex.Message);

            }
        }
        public async Task<(IEnumerable<SeatDTO> returnModel, string message)> GetAllSeatsByTripID(int tripID)
        {
            try
            {
                var seats = await _seatRepository.GetAll().Where(x => x.TripID == tripID).ToListAsync();
                if (seats == null)
                {
                    return (null, "Empty List");
                }
                var seatModels = _mapper.Map<IEnumerable<SeatDTO>>(seats);
                return (seatModels, "Ok");

            }
            catch (Exception ex)
            {
                return (null, ex.Message);

            }
        }
        
    }
}
*/