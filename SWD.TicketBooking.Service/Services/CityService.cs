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
     
        public CityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
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
                var checkExisted = await _unitOfWork.CityRepository
                                                    .GetAll()
                                                    .Where(_ => _.Name == model.CityName && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                    .FirstOrDefaultAsync();
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
                    throw new InternalServerErrorException(SD.Notification.Internal("Company", "Create"));
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
                var checkExisted = await _unitOfWork.CityRepository
                                                    .GetAll()
                                                    .Where(_ => _.Name == model.CityName && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                    .FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException(SD.Notification.Existed("City", "Name"));
                }

                var entity = await _unitOfWork.CityRepository
                                              .GetAll()
                                              .Where(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && _.CityID == cityId)
                                              .FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("City"));
                }

                entity.Name = model.CityName;

                var companyUpdate = _unitOfWork.CityRepository.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException(SD.Notification.Internal("City","Update"));
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
                var entity = await _unitOfWork.CityRepository
                                              .GetAll()
                                              .Where(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && _.CityID == cityId)
                                              .FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("City"));
                }

                entity.Status = status;

                var companyUpdate = _unitOfWork.CityRepository.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException(SD.Notification.Internal("City", "Update"));
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