using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;
using static SWD.TicketBooking.Service.Dtos.FromCityToCityModel;

namespace SWD.TicketBooking.Service.Services
{
    public class CityService : ICityService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IRepository<City, Guid> _unitOfWork.CityRepository;
        //private readonly IRepository<Route, Guid> _unitOfWork.RouteRepository;

        public CityService(IUnitOfWork unitOfWork, IRepository<City, Guid> cityRepository, IRepository<Route, Guid> routeRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_unitOfWork.CityRepository = cityRepository;
            //_unitOfWork.RouteRepository = routeRepository;
            _mapper = mapper;
        }
        public async Task<List<CitiesModel>> GetAllCities()
        {
            var cityEntities = await _unitOfWork.CityRepository.GetAll().ToListAsync();
            var cityModels = _mapper.Map<List<CitiesModel>>(cityEntities);
            return cityModels;
        }
        public async Task<CityModel> GetFromCityToCity()
        {
            try
            {
                var fromCities = await _unitOfWork.RouteRepository.GetAll().Where(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE)).Select(_ => _.FromCity )
                                                       .Distinct()
                                                       .Select(_ => new CityInfo { CityID = _.CityID, CityName = _.Name })   
                                                       .OrderBy(x => x.CityName)
                                                       .ToListAsync();

                var toCities = await _unitOfWork.RouteRepository.GetAll().Where(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE)).Select(_ => _.ToCity)
                                                       .Distinct()
                                                       .Select(_ => new CityInfo { CityID = _.CityID, CityName = _.Name })
                                                       .OrderBy(x => x.CityName)
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
                var checkExisted = await _unitOfWork.CityRepository.GetAll().Where(_ => _.Name == model.CityName && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE)).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("Company name existed");
                }
                var company = await _unitOfWork.CityRepository.AddAsync(new City
                {
                    CityID = Guid.NewGuid(),
                    Name = model.CityName,
                    Status = SD.GeneralStatus.ACTIVE
                });
                if (company == null)
                {
                    throw new InternalServerErrorException("Cannot create");
                }
                var rs = _unitOfWork.Complete();
                if(rs >  0)
                {
                   return rs;
                }
                else throw new BadRequestException("Fail!");
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
                var checkExisted = await _unitOfWork.CityRepository.GetAll().Where(_ => _.Name == model.CityName && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE)).FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException("City name already existed");
                }

                var entity = await _unitOfWork.CityRepository.GetAll().Where(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && _.CityID == cityId).FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException("Cannot find city");
                }

                entity.Name = model.CityName;

                var companyUpdate = _unitOfWork.CityRepository.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException("Cannot update");
                }
                var rs = _unitOfWork.Complete();
                if(rs > 0)
                {
                    return rs;
                }
                else throw new BadRequestException("Fail!");

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
                var entity = await _unitOfWork.CityRepository.GetAll().Where(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && _.CityID == cityId).FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException("Cannot find company");
                }

                entity.Status = status;

                var companyUpdate = _unitOfWork.CityRepository.Update(entity);

                if (companyUpdate == null)
                {
                    throw new Exception("Cannot update");
                }
                var rs = _unitOfWork.Complete();
                if (rs > 0)
                {
                    return rs;
                }
                else throw new BadRequestException("Fail!");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}