using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
using static SWD.TicketBooking.Service.Dtos.FromCityToCityModel;

namespace SWD.TicketBooking.Service.Services
{
    public class CityService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<City, Guid> _cityRepository;
        private readonly IRepository<Route, Guid> _routeRepository;

        public CityService(IRepository<City, Guid> cityRepository, IRepository<Route, Guid> routeRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _routeRepository = routeRepository;
            _mapper = mapper;
        }
        
        public async Task<CityModel> GetFromCityToCity()
        {
            try
            {
                var fromCities = await _routeRepository.GetAll().Where(_ => _.Status.Trim().Equals(SD.ACTIVE)).Select(_ => _.FromCity )
                                                       .Distinct()
                                                       .Select(_ => new CityInfo { CityID = _.CityID, CityName = _.Name })
                                                       .ToListAsync();

                var toCities = await _routeRepository.GetAll().Where(_ => _.Status.Trim().Equals(SD.ACTIVE)).Select(_ => _.ToCity)
                                                       .Distinct()
                                                       .Select(_ => new CityInfo { CityID = _.CityID, CityName = _.Name })
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
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> CreateCity(CreateCityModel model)
        {
            try
            {
                var checkExisted = await _cityRepository.GetAll().Where(_ => _.Name == model.CityName && _.Status.Trim().Equals(SD.ACTIVE)).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("Company name existed");
                }
                var company = await _cityRepository.AddAsync(new City
                {
                    CityID = Guid.NewGuid(),
                    Name = model.CityName,
                    Status = SD.ACTIVE
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

        public async Task<int> UpdateCity(Guid cityId, CreateCityModel model)
        {
            try
            {
                var checkExisted = await _cityRepository.GetAll().Where(_ => _.Name == model.CityName && _.Status.Trim().Equals(SD.ACTIVE)).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("City name already existed");
                }

                var entity = await _cityRepository.GetAll().Where(_ => _.Status.Trim().Equals(SD.ACTIVE) && _.CityID == cityId).FirstOrDefaultAsync();

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

        public async Task<int> ChangeStatus(Guid cityId, string status)
        {
            try
            {
                var entity = await _cityRepository.GetAll().Where(_ => _.Status.Trim().Equals(SD.ACTIVE) && _.CityID == cityId).FirstOrDefaultAsync();

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