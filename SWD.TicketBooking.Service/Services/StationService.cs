using AutoMapper;
using DinkToPdf;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public class StationService
    {
        private readonly IRepository<Station_Route, Guid> _stationRouteRepository;
        private readonly IRepository<Station, Guid> _stationRepository;
        private readonly IRepository<Company, Guid> _companyRepository;
        private readonly IRepository<City, Guid> _cityRepository;
        private readonly IRepository<Trip, Guid> _tripRepository;
        private readonly IMapper _mapper;
        public StationService(IRepository<Station_Route, Guid> stationRouteRepository, IRepository<Station, Guid> stationRepository, IMapper mapper, IRepository<Company, Guid> companyRepository, IRepository<City, Guid> cityRepository, IRepository<Trip, Guid> tripRepository)
        {
            _stationRouteRepository = stationRouteRepository;
            _stationRepository = stationRepository;
            _mapper = mapper;
            _companyRepository = companyRepository;
            _cityRepository = cityRepository;
            _tripRepository = tripRepository;
        }
        public async Task<List<StationFromRouteModel>> GetStationsFromRoute(Guid routeID)
        {
            try
            {
                var stations = await _stationRouteRepository
                                    .FindByCondition(_ => _.RouteID == routeID && _.Status.Trim().Equals(SD.ACTIVE))
                                    .Include(_ => _.Station)
                                    .Select(_ => new StationFromRouteModel
                                     {
                                        StationID = _.StationID,
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
                var station = await _stationRepository.GetAll().Where(s => s.Status.Trim().Equals(SD.ACTIVE)).ToListAsync();
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
                var station = await _stationRepository.GetByIdAsync(id);
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
                var check = await _stationRepository.GetAll().Where(s =>s.Name.Equals(stationModel.StationName)).FirstOrDefaultAsync();
                if (check == null)
                {
                    var company = await _companyRepository.GetByIdAsync(stationModel.CompanyId);
                    var city = await _cityRepository.GetByIdAsync(stationModel.CityId);
                    var station = await _stationRepository.AddAsync(new Station 
                            {   
                                CityID = Guid.NewGuid(),
                                City = city,
                                Company = company,
                                Name = stationModel.StationName, 
                                Status = SD.ACTIVE,
                            });
                    if (station == null)
                    {
                        throw new BadRequestException("Cannot add new station");
                    }
                    await _stationRepository.Commit();
                    return "OK";
                }
                else throw new BadRequestException("Station exited!");

            } catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        } 

        public async Task<string> UpdateStation(Guid stationId, CreateStationModel stationModel)
        {
            try
            {
                var check  = await _stationRepository.GetAll().Where(s=> s.Status.Trim().Equals(SD.ACTIVE) && s.StationID == stationId).FirstOrDefaultAsync();
                if (check == null)
                {
                    throw new NotFoundException("Station not found!");
                }
                else
                {
                    var checkName = await _stationRepository.GetAll().Where(s => s.Name.ToLower().Equals(stationModel.StationName)).FirstOrDefaultAsync();
                    if (checkName == null)
                    {
                        check.Name = stationModel.StationName;
                        _stationRepository.Update(check);
                        await _stationRepository.Commit();
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
                var route = await _tripRepository.FindByCondition(s=> s.TripID == id && s.Status.Trim().Equals(SD.ACTIVE) && s.Route_Company.Route.Status.ToLower().Trim().Equals("active")).Include(_ => _.Route_Company).Select(s=>s.Route_Company.RouteID).FirstOrDefaultAsync();
                var stations = await _stationRouteRepository
                                    .FindByCondition(_ => _.RouteID == route)
                                    .Include(_ => _.Station)
                                    .OrderBy(_ => _.OrderInRoute)
                                    .Select(_ => new StationFromRouteModel
                                    {
                                        StationID = _.StationID,
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
