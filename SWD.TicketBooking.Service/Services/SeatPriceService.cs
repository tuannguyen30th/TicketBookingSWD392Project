/*using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TicketBooking.Common.Payloads.Requests;
using TicketBooking.Dtos;
using TicketBooking.Entities;
using TicketBooking.Repositories;

namespace TicketBooking.Services
{
    public class SeatPriceService
    {
        private readonly IRepository<SeatPrice, int> _seatPriceRepo;
        private readonly IMapper _mapper;

        public SeatPriceService(IRepository<SeatPrice, int> seatPriceRepo, IMapper mapper)
        {
            _seatPriceRepo = seatPriceRepo;
            _mapper = mapper;
        }

        public async Task<(List<SeatPriceDTO> returnModel, string message)> GetSeatPriceByTripId(int tripId)
        {
            try
            {
                var seatPrices = await _seatPriceRepo.GetAll().Where(s => s.TripID == tripId).ToListAsync();
                if (seatPrices.IsNullOrEmpty())
                {
                    return (null, "Empty List");
                }
                var result = _mapper.Map<List<SeatPriceDTO>>(seatPrices);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(SeatPriceDTO returnModel, string message)> GetActiveSeatPriceByTripId(int tripId)
        {
            try
            {
                var seatPrice = await _seatPriceRepo.FindByCondition(v => v.SeatPriceID == tripId && v.Status.ToUpper() == "Active".ToUpper()).FirstOrDefaultAsync();
                if (seatPrice == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<SeatPriceDTO>(seatPrice);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(SeatPriceDTO returnModel, string message)> GetSeatPriceById(int id)
        {
            try
            {
                var seatPrice = await _seatPriceRepo.FindByCondition(v => v.SeatPriceID == id && v.Status.ToUpper() == "Active".ToUpper()).FirstOrDefaultAsync();
                if (seatPrice == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<SeatPriceDTO>(seatPrice);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(SeatPriceDTO returnModel, string message)> CreateSeatPrice(CreateSeatPriceReq req)
        {
            try
            {
                var checkExist = await _seatPriceRepo.FindByCondition(t => t.TripID == req.TripID && t.Status.ToLower() == "active").FirstOrDefaultAsync();
                if (checkExist != null)
                {
                    return (null, "This trip already have seat price");
                }
                var seatPrice = new SeatPrice();
                if(req.SingleCabinPrice == 0 && req.DoubleCabinPrice == 0 && req.ForeRowPrice != 0 && req.MiddleRowPrice != 0 && req.BackRowPrice != 0)
                {
                    seatPrice = await _seatPriceRepo.AddAsync(new SeatPrice
                    {
                        TripID = req.TripID,
                        ForeRowPrice = req.ForeRowPrice,
                        MiddleRowPrice = req.MiddleRowPrice,
                        BackRowPrice = req.BackRowPrice,
                        Status = "Active"
                    });
                }
                else if(req.SingleCabinPrice != 0 && req.DoubleCabinPrice != 0 &&  req.ForeRowPrice == 0 && req.MiddleRowPrice == 0 && req.BackRowPrice == 0)
                {
                    seatPrice = await _seatPriceRepo.AddAsync(new SeatPrice
                    {
                        TripID = req.TripID,
                        SingleCabinPrice = req.SingleCabinPrice,
                        DoubleCabinPrice = req.DoubleCabinPrice,
                        Status = "Active"
                    });
                }
                else if(req.SingleCabinPrice == 0 && req.DoubleCabinPrice == 0 && req.ForeRowPrice == 0 && req.MiddleRowPrice == 0 && req.BackRowPrice == 0)
                {
                    return (null, "Error: all field are 0 (Only cabin or row has price)");
                }
                else
                {
                    return (null, "Error: all field have price (Only cabin or row has price)");
                }

                await _seatPriceRepo.Commit();
                if (seatPrice == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<SeatPriceDTO>(seatPrice);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<(SeatPriceDTO returnModel, string message)> UpdateSeatPrice(int id, UpdateSeatPriceReq req)
        {
            try
            {
                var seatPrice = await _seatPriceRepo.FindByCondition(t => t.SeatPriceID == id && t.Status.ToLower() == "active")
                    .Include(t => t.Trip).ThenInclude(t => t.Transportation).ThenInclude(t => t.TransportationStructure)
                    .FirstOrDefaultAsync();
                if (seatPrice != null && seatPrice.Trip != null && seatPrice.Trip.Transportation != null && seatPrice.Trip.Transportation.TransportationStructure != null)
                {
                    var checkIsCabin = seatPrice.Trip.Transportation.TransportationStructure.IsCabin;

                    if (seatPrice == null)
                    {
                        return (null, "Cannot found this seat price");
                    }
                    else
                    {
                        if (!checkIsCabin && req.SingleCabinPrice == 0 && req.DoubleCabinPrice == 0 && req.ForeRowPrice != 0 && req.MiddleRowPrice != 0 && req.BackRowPrice != 0)
                        {
                            seatPrice.ForeRowPrice = req.ForeRowPrice;
                            seatPrice.MiddleRowPrice = req.MiddleRowPrice;
                            seatPrice.BackRowPrice = req.BackRowPrice;
                        }
                        else if (checkIsCabin && req.SingleCabinPrice != 0 && req.DoubleCabinPrice != 0 && req.ForeRowPrice == 0 && req.MiddleRowPrice == 0 && req.BackRowPrice == 0)
                        {
                            seatPrice.SingleCabinPrice = req.SingleCabinPrice;
                            seatPrice.DoubleCabinPrice = req.DoubleCabinPrice;
                        }
                        else if (req.SingleCabinPrice == 0 && req.DoubleCabinPrice == 0 && req.ForeRowPrice == 0 && req.MiddleRowPrice == 0 && req.BackRowPrice == 0)
                        {
                            return (null, "Error: all field are 0 (Only cabin or row has price)");
                        }
                        else
                        {
                            return (null, "Error: all field have price (Only cabin or row has price)");
                        }
                    }
                }
                else
                {
                    return (null, "Something null");
                }
                var updateResult = _seatPriceRepo.Update(seatPrice);
                await _seatPriceRepo.Commit();
                if (updateResult == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<SeatPriceDTO>(updateResult);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

*//*        public async Task<(SeatPriceDTO returnModel, string message)> ChangeSeatPriceStatus(int id, ChangeStatusReq req)
        {
            try
            {
                var seatPrice = await _seatPriceRepo.FindByCondition(v => v.SeatPriceID == id).FirstOrDefaultAsync();
                if (req.Status.ToLower() == "active")
                {

                }
                if (seatPrice == null)
                {
                    return (null, "Cannot found this seat price");
                }
                else
                {
                    seatPrice.Status = req.Status;
                }
                var updateResult = _seatPriceRepo.Update(seatPrice);
                await _seatPriceRepo.Commit();
                if (updateResult == null)
                {
                    return (null, "Null Object");
                }
                var result = _mapper.Map<SeatPriceDTO>(updateResult);
                return (result, "Ok");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }*//*
    }
}
*/