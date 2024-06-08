using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD.TicketBooking.Service.Dtos.FromCityToCityModel;

namespace SWD.TicketBooking.Service.Services
{
    public class CityService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<City, int> _cityRepository;
        private readonly IRepository<Route, int> _routeRepository;

        public CityService(IRepository<City, int> cityRepository, IRepository<Route, int> routeRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _routeRepository = routeRepository;
            _mapper = mapper;
        }
        
        public async Task<CityModel> GetFromCityToCity()
        {
            try
            {
                var fromCities = await _routeRepository.GetAll().Select(r => r.FromCity)
                                                       .Distinct()
                                                       .Select(c => new CityInfo { CityID = c.CityID, CityName = c.Name })
                                                       .ToListAsync();

                var toCities = await _routeRepository.GetAll().Select(r => r.ToCity)
                                                       .Distinct()
                                                       .Select(c => new CityInfo { CityID = c.CityID, CityName = c.Name })
                                                       .ToListAsync();

                var rs =  new CityModel
                {
                    FromCities = fromCities,
                    ToCities = toCities
                };
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<int> CreateCity(CreateCityModel model)
        {
            try
            {
                var checkExisted = await _cityRepository.GetAll().Where(_ => _.Name == model.CityName).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("Company name existed");
                }
                var company = await _cityRepository.AddAsync(new City
                {
                    Name = model.CityName,
                    Status = "Active"
                });
                if (company == null)
                {
                    throw new InternalServerErrorException("Cannot create");
                }
                var rs = await _cityRepository.Commit();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateCity(int cityId, CreateCityModel model)
        {
            try
            {
                var checkExisted = await _cityRepository.GetAll().Where(_ => _.Name == model.CityName).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("City name already existed");
                }

                var entity = await _cityRepository.GetAll().Where(_ => _.Status.ToLower().Trim() == "active" && _.CityID == cityId).FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException("Cannot find city");
                }

                entity.Name = model.CityName;

                var companyUpdate = _cityRepository.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException("Cannot update");
                }
                var rs = await _cityRepository.Commit();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> ChangeStatus(int cityId, string status)
        {
            try
            {
                var entity = await _cityRepository.GetAll().Where(_ => _.Status.ToLower().Trim() == "active" && _.CityID == cityId).FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException("Cannot find company");
                }

                entity.Status = status;

                var companyUpdate = _cityRepository.Update(entity);

                if (companyUpdate == null)
                {
                    throw new Exception("Cannot update");
                }
                var rs = await _cityRepository.Commit();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}