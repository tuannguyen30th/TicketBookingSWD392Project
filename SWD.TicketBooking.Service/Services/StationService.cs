using AutoMapper;
using DinkToPdf;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public class StationService : IStationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StationService(IUnitOfWork unitOfWork,  IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<StationFromRouteModel>> GetStationsFromRoute(Guid routeID)
        {
            try
            {
                var stations = await _unitOfWork.Station_RouteRepository
                                                .FindByCondition(_ => _.RouteID == routeID && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                .Include(_ => _.Station)
                                                .Select(_ => new StationFromRouteModel
                                                 {
                                                    StationID = (Guid)_.StationID,
                                                    Name = _.Station.Name
                                                 })
                                                .ToListAsync();

                return stations;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<GetStationModel>> GetAllStationActive()
        {
            try
            {
                var station = await _unitOfWork.StationRepository.GetAll().ToListAsync();
                var rs = _mapper.Map<List<GetStationModel>>(station);
                return rs;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public async Task<GetStationModel> GetStationById(Guid id)
        {
            try
            {
                var station = await _unitOfWork.StationRepository.GetByIdAsync(id);
                var rs = _mapper.Map<GetStationModel>(station);
                return rs;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public async Task<string> CreateStation(CreateStationModel stationModel)
        {
            try
            {
                var check = await _unitOfWork.StationRepository
                                             .GetAll().Where(s =>s.Name.Equals(stationModel.StationName))
                                             .FirstOrDefaultAsync();
                if (check == null)
                {
                    var company = await _unitOfWork.CompanyRepository.GetByIdAsync(stationModel.CompanyId);
                    var city = await _unitOfWork.CityRepository.GetByIdAsync(stationModel.CityId);
                    var station = await _unitOfWork.StationRepository.AddAsync(new Station 
                            {   
                                CityID = Guid.NewGuid(),
                                City = city,
                                Company = company,
                                Name = stationModel.StationName, 
                                Status = SD.GeneralStatus.ACTIVE,
                            });
                    if (station == null)
                    {
                        throw new InternalServerErrorException(SD.Notification.Internal("TRẠM", "KHI KHÔNG THỂ TẠO MỚI TRẠM NÀY"));
                    }
                    _unitOfWork.Complete();
                    return "OK";
                }
                else throw new BadRequestException("TRẠM NÀY ĐÃ TỒN TẠI!");

            } catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        } 

        public async Task<string> UpdateStation(Guid stationId, CreateStationModel stationModel)
        {
            try
            {
                var check  = await _unitOfWork.StationRepository
                                              .GetAll()
                                              .Where(s=> s.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && s.StationID == stationId)
                                              .FirstOrDefaultAsync();
                if (check == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("TRẠM"));
                }
                else
                {
                    var checkName = await _unitOfWork.StationRepository
                                                     .GetAll()
                                                     .Where(s => s.Name.ToLower().Equals(stationModel.StationName))
                                                     .FirstOrDefaultAsync();
                    if (checkName == null)
                    {
                        check.Name = stationModel.StationName;
                        _unitOfWork.StationRepository.Update(check);
                        _unitOfWork.Complete();
                    }
                    return "OK";
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<List<StationFromRouteModel>> GetAllStationInRoute(Guid id)
        {
            try
            {
                var route = await _unitOfWork.TripRepository
                                             .FindByCondition(s=> s.TripID == id && s.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) 
                                                               && s.Route_Company.Route.Status.Equals(SD.GeneralStatus.ACTIVE))
                                             .Include(_ => _.Route_Company)
                                             .Select(s=>s.Route_Company.RouteID).FirstOrDefaultAsync();
                var stations = await _unitOfWork.Station_RouteRepository
                                                .FindByCondition(_ => _.RouteID == route)
                                                .Include(_ => _.Station)
                                                .OrderBy(_ => _.OrderInRoute)
                                                .Select(_ => new StationFromRouteModel
                                                {
                                                    StationID = (Guid)_.StationID,
                                                    Name = _.Station.Name
                                                })
                                                .ToListAsync();
                return stations;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
