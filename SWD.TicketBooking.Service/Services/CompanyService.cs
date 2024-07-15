using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWD.TicketBooking.Service.Dtos.GetTemplatesFromCompanyModel;

namespace SWD.TicketBooking.Service.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ActionOutcome> GetTemplatesFromCompany(Guid companyID)
        {
            try
            {
                var result = new ActionOutcome();
                var listTemplates = new List<GetTemplatesFromCompanyModel>();
                var routeCompanies = await _unitOfWork.Route_CompanyRepository.GetAll()
                                                      .Where(_ => _.CompanyID == companyID)
                                                      .Select(_ => _.Route_CompanyID)
                                                      .ToListAsync();

                var trips = await _unitOfWork.TripRepository
                                            .GetAll()
                                            .Include(_ => _.User)
                                            .Include(_ => _.Route_Company)
                                            .ThenInclude(_ => _.Route)
                                            .ThenInclude(_ => _.FromCity)
                                            .Include(_ => _.Route_Company)
                                            .ThenInclude(_ => _.Route)
                                            .ThenInclude(_ => _.ToCity)
                                            .Where(_ => routeCompanies.Contains((Guid)_.Route_CompanyID) && _.IsTemplate == true)
                                            .ToListAsync();
                foreach (var trip in trips)
                {
                    var tripPictures = await _unitOfWork.TripPictureRepository.GetAll()
                                                        .Where(_ => _.TripID == trip.TemplateID && _.Status == SD.GeneralStatus.ACTIVE)
                                                        .Select(_ => _.ImageUrl)
                                                        .ToListAsync();
                    var tripUtilities = await _unitOfWork.Trip_UtilityRepository
                                                         .GetAll()
                                                         .Include(_ => _.Utility)
                                                         .Where(_ => _.TripID == trip.TemplateID && _.Status == SD.GeneralStatus.ACTIVE)
                                                         .Select(_ => new TripUtilityModel
                                                         {
                                                             UtilityName = _.Utility.Name,
                                                             Description = _.Utility.Description
                                                         })
                                                         .ToListAsync();
                    var getSeats = await _unitOfWork.TicketType_TripRepository
                                                    .GetAll()
                                                    .Include(_ => _.TicketType)
                                                    .Where(_ => _.TripID == trip.TemplateID && _.Status == SD.GeneralStatus.ACTIVE)
                                                    .Select(_ => new TripPriceSeat
                                                    {
                                                        SeatName = _.TicketType.Name,
                                                        Price = _.Price,
                                                        Quantity = _.Quantity
                                                    })
                                                    .ToListAsync();
                    var stationsByRoute = await _unitOfWork.StationCompany_RouteRepository
                                                           .GetAll()
                                                           .Where(_ => _.RouteID == trip.Route_Company.RouteID
                                                                    && _.Station_Company.CompanyID == trip.Route_Company.CompanyID && _.Status == SD.GeneralStatus.ACTIVE)
                                                           .OrderBy(_ => _.OrderInRoute)
                                                           .Select(_ => _.Station_CompanyID)
                                                           .ToListAsync();
                    var stationsByCompany = await _unitOfWork.Station_CompanyRepository
                                                             .GetAll()
                                                             .Include(_ => _.Station.City)
                                                             .Where(_ => stationsByRoute.Contains(_.Station_CompanyID)
                                                                           && _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                                                             .Include(_ => _.Station)
                                                             .Select(_ => new TripStationModel
                                                             {
                                                                 StationID = _.StationID,
                                                                 StationName = _.Station.Name,
                                                                 AtCity = _.Station.City.Name,
                                                             })
                                                             .ToListAsync();
                    var template = new GetTemplatesFromCompanyModel
                    {
                        TemplateID = trip.TemplateID,
                        FromCity = trip.Route_Company.Route.FromCity.Name,
                        ToCity = trip.Route_Company.Route.ToCity.Name,
                        StartLocation = trip.Route_Company.Route.StartLocation,
                        EndLocation = trip.Route_Company.Route.EndLocation,
                        ImageUrls = tripPictures,
                        TripStationModels = stationsByCompany,
                        TripPriceSeats = getSeats,
                        TripUtilityModels = tripUtilities,
                        Status = trip.Status,
                    };
                    listTemplates.Add(template);
                }
                result.Result = listTemplates;
                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<GetCompanyModel>> GetAllActiveCompanies()
        {
            try
            {
                var companies = await _unitOfWork.CompanyRepository.GetAll().ToListAsync();
                var rs = _mapper.Map<List<GetCompanyModel>>(companies);
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<GetCompanyModel> GetCompanyById(Guid id)
        {
            try
            {
                var company = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
                var rs = _mapper.Map<GetCompanyModel>(company);
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> CreateCompany(CreateCompanyModel model)
        {
            try
            {
                var checkUser = await _unitOfWork.UserRepository
                                                 .GetAll()
                                                 .Where(_ => _.UserID.Equals(model.UserID))
                                                 .FirstOrDefaultAsync();

                if (checkUser == null)
                {
                    throw new BadRequestException(SD.Notification.NotFound("NGƯỜI DÙNG"));
                }

                var checkExisted = await _unitOfWork.CompanyRepository
                                                    .GetAll()
                                                    .Where(_ => _.Name ==  model.Name)
                                                    .FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException(SD.Notification.Existed("NHÀ XE", "TÊN"));
                }

                var company = await _unitOfWork.CompanyRepository.AddAsync(new Company
                {
                    CompanyID = Guid.NewGuid(),
                    Name = model.Name,
                    Status = SD.GeneralStatus.ACTIVE,
                    UserID = checkUser.UserID
                });
                if (company == null)
                {
                    throw new InternalServerErrorException(SD.Notification.Internal("NHÀ XE", "KHI TẠO MỚI NHÀ XE"));
                }
                var rs = await _unitOfWork.CompanyRepository.Commit();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateCompany(Guid companyId, CreateCompanyModel model)
        {
            try
            {
                var checkExisted = await _unitOfWork.CompanyRepository
                                                    .GetAll()
                                                    .Where(_ => _.Name == model.Name)
                                                    .FirstOrDefaultAsync();
                if (checkExisted != null)
                {
                    throw new BadRequestException(SD.Notification.Existed("NHÀ XE", "TÊN"));
                }

                var entity = await _unitOfWork.CompanyRepository
                                              .GetAll()
                                              .Where(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && _.CompanyID == companyId)
                                              .FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("NHÀ XE"));
                }

                entity.Name = model.Name;

                var companyUpdate = _unitOfWork.CompanyRepository.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException(SD.Notification.Internal("NHÀ XE", "KHI CẬP NHẬT NHÀ XE"));
                }
                var rs = _unitOfWork.Complete();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> ChangeStatus(Guid companyId, string status)
        {
            try
            {
                var entity = await _unitOfWork.CompanyRepository
                                              .GetAll()
                                              .Where(_ => _.Status.Trim().Equals(SD.GeneralStatus.ACTIVE) && _.CompanyID == companyId)
                                              .FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("NHÀ XE"));
                }

                entity.Status = status;

                var companyUpdate = _unitOfWork.CompanyRepository.Update(entity);

                if (companyUpdate == null)
                {
                    throw new InternalServerErrorException(SD.Notification.Internal("NHÀ XE", "KHI CẬP NHẬT NHÀ XE"));
                }
                var rs = _unitOfWork.Complete();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
