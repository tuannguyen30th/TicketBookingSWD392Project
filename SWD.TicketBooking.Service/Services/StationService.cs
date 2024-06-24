﻿using AutoMapper;
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
        //private readonly IRepository<Station_Route, Guid> _unitOfWork.Station_RouteRepository;
        //private readonly IRepository<Station, Guid> _unitOfWork.StationRepository;
        //private readonly IRepository<Company, Guid> _unitOfWork.CompanyRepository;
        //private readonly IRepository<City, Guid> _unitOfWork.CityRepository;
        //private readonly IRepository<Trip, Guid> _unitOfWork.TripRepository;
        private readonly IMapper _mapper;
        public StationService(IUnitOfWork unitOfWork, IRepository<Station_Route, Guid> stationRouteRepository, IRepository<Station, Guid> stationRepository, IMapper mapper, IRepository<Company, Guid> companyRepository, IRepository<City, Guid> cityRepository, IRepository<Trip, Guid> tripRepository)
        {
            _unitOfWork = unitOfWork;
            //_unitOfWork.Station_RouteRepository = stationRouteRepository;
            //_unitOfWork.StationRepository = stationRepository;
            _mapper = mapper;
            //_unitOfWork.CompanyRepository = companyRepository;
            //_unitOfWork.CityRepository = cityRepository;
            //_unitOfWork.TripRepository = tripRepository;
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
                //var station = await _unitOfWork.StationRepository.GetAll().Where(s => s.Status.Trim().Equals(SD.GeneralStatus.ACTIVE)).ToListAsync();
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
                var check = await _unitOfWork.StationRepository.GetAll().Where(s =>s.Name.Equals(stationModel.StationName)).FirstOrDefaultAsync();
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
                        throw new BadRequestException("Cannot add new station");
                    }
                    //await _unitOfWork.StationRepository.Commit();
                    _unitOfWork.Complete();
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
                var check  = await _unitOfWork.StationRepository.GetAll().Where(s=> s.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && s.StationID == stationId).FirstOrDefaultAsync();
                if (check == null)
                {
                    throw new NotFoundException("Station not found!");
                }
                else
                {
                    var checkName = await _unitOfWork.StationRepository.GetAll().Where(s => s.Name.ToLower().Equals(stationModel.StationName)).FirstOrDefaultAsync();
                    if (checkName == null)
                    {
                        check.Name = stationModel.StationName;
                        _unitOfWork.StationRepository.Update(check);
                        //await _unitOfWork.StationRepository.Commit();
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
                var route = await _unitOfWork.TripRepository.FindByCondition(s=> s.TripID == id && s.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && s.Route_Company.Route.Status.Equals(SD.GeneralStatus.ACTIVE)).Include(_ => _.Route_Company).Select(s=>s.Route_Company.RouteID).FirstOrDefaultAsync();
                var stations = await _unitOfWork.Station_RouteRepository
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
