using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Service.Dtos;
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
    }
}