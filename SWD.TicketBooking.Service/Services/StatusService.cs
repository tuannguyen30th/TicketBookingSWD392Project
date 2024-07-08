using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.UnitOfWork;
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
    public class StatusService : IStatusService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StatusService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<int> ChangeStatus(string entity, Guid Id)
        {
            try
            {
                var rs = 0;
                switch (entity.Trim().ToUpper())
                {
                    case SD.Entity.ENTITY_CITY:
                        var city = await _unitOfWork.CityRepository
                                                    .GetByIdAsync(Id);
                        if (city.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            city.Status = SD.GeneralStatus.INACTIVE;
                        } else city.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_COMPANY:
                        var company = await _unitOfWork.CompanyRepository
                                                    .GetByIdAsync(Id);
                        if (company.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            company.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else company.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_FEEDBACK:
                        var feedback = await _unitOfWork.FeedbackRepository
                                                    .GetByIdAsync(Id);
                        if (feedback.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            feedback.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else feedback.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_FEEDBACK_IMAGE:
                        var feedback_Image = await _unitOfWork.Feedback_ImageRepository
                                                    .GetByIdAsync(Id);
                        if (feedback_Image.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            feedback_Image.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else feedback_Image.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_ROUTE:
                        var route = await _unitOfWork.RouteRepository
                                                    .GetByIdAsync(Id);
                        if (route.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            route.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else route.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_ROUTE_COMPANY:
                        var route_company = await _unitOfWork.Route_CompanyRepository
                                                    .GetByIdAsync(Id);
                        if (route_company.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            route_company.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else route_company.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_SERVICE:
                        var service = await _unitOfWork.ServiceRepository
                                                    .GetByIdAsync(Id);
                        if (service.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            service.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else service.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_SERVICE_TYPE:
                        var servicetype = await _unitOfWork.ServiceTypeRepository
                                                    .GetByIdAsync(Id);
                        if (servicetype.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            servicetype.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else servicetype.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_STATION:
                        var station = await _unitOfWork.StationRepository
                                                    .GetByIdAsync(Id);
                        if (station.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            station.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else station.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_STATION_COMPANY_ROUTE:
                        var station_route = await _unitOfWork.StationCompany_RouteRepository
                                                    .GetByIdAsync(Id);
                        if (station_route.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            station_route.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else station_route.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_STATION_SERVICE:
                        var station_service = await _unitOfWork.Station_ServiceRepository
                                                    .GetByIdAsync(Id);
                        if (station_service.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            station_service.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else station_service.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;

                    case SD.Entity.ENTITY_TRIP:
                        var trip = await _unitOfWork.TripRepository
                                                    .GetByIdAsync(Id);
                        if (trip.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            trip.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else trip.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_TRIP_UTILITY:
                        var trip_utility = await _unitOfWork.Trip_UtilityRepository
                                                    .GetByIdAsync(Id);
                        if (trip_utility.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            trip_utility.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else trip_utility.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_USER:
                        var user = await _unitOfWork.UserRepository
                                                    .GetByIdAsync(Id);
                        if (user.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            user.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else user.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_USER_ROLE:
                        var userRole = await _unitOfWork.UserRoleRepository
                                                    .GetByIdAsync(Id);
                        if (userRole.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            userRole.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else userRole.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_UTILITY:
                        var utility = await _unitOfWork.UtilityRepository
                                                    .GetByIdAsync(Id);
                        if (utility.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            utility.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else utility.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;
                    case SD.Entity.ENTITY_TRIP_PICTURE:
                        var trip_picture = await _unitOfWork.TripPictureRepository
                                                    .GetByIdAsync(Id);
                        if (trip_picture.Status.Equals(SD.GeneralStatus.ACTIVE))
                        {
                            trip_picture.Status = SD.GeneralStatus.INACTIVE;
                        }
                        else trip_picture.Status = SD.GeneralStatus.ACTIVE;
                        rs = _unitOfWork.Complete();
                        break;

                }
                if (rs == 0)
                {
                    throw new InternalServerErrorException(SD.Notification.Internal("CẬP NHẬT", "KHI CẬP NHẬT TRẠNG THÁI"));
                }
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }
}
