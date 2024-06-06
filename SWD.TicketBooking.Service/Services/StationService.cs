using AutoMapper;
using DinkToPdf;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Service.Services
{
    public class StationService
    {
        private readonly IRepository<Station_Route, int> _stationRouteRepository;
        private readonly IRepository<Station, int> _stationRepository;
        private readonly IRepository<Company, int> _companyRepository;
        private readonly IRepository<City, int> _cityRepository;
        private readonly IMapper _mapper;
        public StationService(IRepository<Station_Route, int> stationRouteRepository, IRepository<Station, int> stationRepository, IMapper mapper, IRepository<Company, int> companyRepository, IRepository<City, int> cityRepository)
        {
            _stationRouteRepository = stationRouteRepository;
            _stationRepository = stationRepository;
            _mapper = mapper;
            _companyRepository = companyRepository;
            _cityRepository = cityRepository;
        }
        public async Task<List<StationFromRouteModel>> GetStationsFromRoute(int routeID)
        {
            try
            {
                var stations = await _stationRouteRepository
                                    .FindByCondition(_ => _.RouteID == routeID)
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
                throw new Exception();
            }
        }

        public async Task<List<GetStationModel>> GetAllStationActive()
        {
            try
            {
                var station = await _stationRepository.GetAll().Where(s => s.Status.ToLower().Trim().Equals("active")).ToListAsync();
                var rs = _mapper.Map<List<GetStationModel>>(station);
                return rs;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }

        public async Task<GetStationModel> GetStationById(int id)
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
                var check = await _stationRepository.GetAll().Where(s =>s.Name.Equals(stationModel.stationName)).FirstOrDefaultAsync();
                if (check == null)
                {
                    var company = await _companyRepository.GetByIdAsync(stationModel.companyId);
                    var city = await _cityRepository.GetByIdAsync(stationModel.cityId);
                    var station = await _stationRepository.AddAsync(new Station 
                            {   
                                City = city,
                                Company = company,
                                Name = stationModel.stationName, 
                                Status = "Active" 
                            });
                    if (station == null)
                    {
                        throw new Exception("Cannot add new station");
                    }
                    await _stationRepository.Commit();
                    return "OK";
                }
                else throw new Exception("Station exited!");

            } catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        } 

        public async Task<string> UpdateStation(int stationId, CreateStationModel stationModel)
        {
            try
            {
                var check  = await _stationRepository.GetAll().Where(s=> s.Status.ToLower().Equals("active") && s.StationID == stationId).FirstOrDefaultAsync();
                if (check == null)
                {
                    throw new Exception("Station not found!");
                }
                else
                {
                    var checkName = await _stationRepository.GetAll().Where(s => s.Name.ToLower().Equals(stationModel.stationName)).FirstOrDefaultAsync();
                    if (checkName == null)
                    {
                        check.Name = stationModel.stationName;
                        _stationRepository.Update(check);
                        _stationRepository.Commit();
                    }
                    return "OK";
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        
    }
}
