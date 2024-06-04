//using AutoMapper;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using TicketBooking.Common.Common.Payloads.Requests;
//using TicketBooking.Common.Dtos.Auth;
//using TicketBooking.Common.Dtos.InterprovincialTripDTO;
//using TicketBooking.Domain.Entities;
//using TicketBooking.Infrastructure.Repositories;

//namespace TicketBooking.Service.Services.InterprovincialTripService
//{
//    public class InterprovincialTripService
//    {
//        private readonly IRepository<Route, int> _interprovincialTripRepo;
//        private readonly IMapper _mapper;

//        public InterprovincialTripService() { }
//        public InterprovincialTripService(IRepository<Route, int> interprovincialTripRepo, IMapper mapper)
//        {
//            _interprovincialTripRepo = interprovincialTripRepo;
//            _mapper = mapper;
//        }

//        public async Task<(List<InterprovincialTripDTO> returnModel, string message)> GetAllInterprovincialTrips()
//        {
//            try
//            {
//                var interprovincialTrips = await _interprovincialTripRepo.GetAll().ToListAsync();
//                if (interprovincialTrips.IsNullOrEmpty())
//                {
//                    return (null, "Empty List");
//                }
//                var result = _mapper.Map<List<InterprovincialTripDTO>>(interprovincialTrips);
//                return (result, "Ok");
//            }
//            catch (Exception ex)
//            {
//                return (null, ex.Message);
//            }
//        }

//        public async Task<(InterprovincialTripDTO returnModel, string message)> GetInterprovincialTripById(int id)
//        {
//            try
//            {
//                var interprovincialTrip = await _interprovincialTripRepo.FindByCondition(it => it.RouteID == id && it.Status.ToUpper() == "Active".ToUpper()).FirstOrDefaultAsync();
//                if (interprovincialTrip == null)
//                {
//                    return (null, "Null Object");
//                }
//                var result = _mapper.Map<InterprovincialTripDTO>(interprovincialTrip);
//                return (result, "Ok");
//            }
//            catch (Exception ex)
//            {
//                return (null, ex.Message);
//            }
//        }

//        public async Task<(InterprovincialTripDTO returnModel, string message)> CreateInterprovincialTrip(CreateInterprovincialTripReq req)
//        {
//            try
//            {
//                var interprovincialTrip = await _interprovincialTripRepo.AddAsync(new Route
//                {
//                    StartLocation = req.StartLocation,
//                    EndLocation = req.EndLocation,
//                    Status = "Active"
//                });
//                await _interprovincialTripRepo.Commit();
//                if (interprovincialTrip == null)
//                {
//                    return (null, "Null Object");
//                }
//                var result = _mapper.Map<InterprovincialTripDTO>(interprovincialTrip);
//                return (result, "Ok");
//            }
//            catch (Exception ex)
//            {
//                return (null, ex.Message);
//            }
//        }

//        public async Task<(InterprovincialTripDTO returnModel, string message)> ChangeInterprovincialTripStatus(int id, ChangeStatusReq req)
//        {
//            try
//            {
//                var interprovincialTrip = await _interprovincialTripRepo.FindByCondition(it => it.RouteID == id).FirstOrDefaultAsync();
//                if (interprovincialTrip == null)
//                {
//                    return (null, "Cannot found this interprovincial trip");
//                }
//                else
//                {
//                    interprovincialTrip.Status = req.Status;
//                }
//                var updateResult = _interprovincialTripRepo.Update(interprovincialTrip);
//                await _interprovincialTripRepo.Commit();
//                if (updateResult == null)
//                {
//                    return (null, "Null Object");
//                }
//                var result = _mapper.Map<InterprovincialTripDTO>(updateResult);
//                return (result, "Ok");
//            }
//            catch (Exception ex)
//            {
//                return (null, ex.Message);
//            }
//        }
//    }
//}
