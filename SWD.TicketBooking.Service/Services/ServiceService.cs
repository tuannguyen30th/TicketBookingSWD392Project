using SWD.TicketBooking.Repo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWD.TicketBooking.Repo.Entities;
using AutoMapper;
using SWD.TicketBooking.Service.Dtos;
using Microsoft.EntityFrameworkCore;


namespace SWD.TicketBooking.Service.Services
{
    public class ServiceService
    {
        private readonly IRepository<SWD.TicketBooking.Repo.Entities.Service, int> _serviceRepository;
        private readonly IRepository<Service_Trip, int> _serviceTripRepository;
        private readonly IRepository<Station_Service, int> _stationServiceRepository;
        private readonly IMapper _mapper;
        public ServiceService(IRepository<SWD.TicketBooking.Repo.Entities.Service, int> serviceRepository, IRepository<Service_Trip, int> serviceTripRepository, IRepository<Station_Service, int> stationServiceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _serviceTripRepository = serviceTripRepository;
            _stationServiceRepository = stationServiceRepository;
            _mapper = mapper;
        }
      /*  public async Task<GetStationServiceModel> GetServiceByStationID(int stationID)
        {
            try
            {
                var service = await _stationServiceRepository.FindByCondition(_ => _.StationID == stationID).ToListAsync();

                var response = new GetStationServiceModel
                {
                    
                };
            }
            catch (Exception ex)
            {

            }
        }*/
    }
}
