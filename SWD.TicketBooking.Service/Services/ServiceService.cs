using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.Entities;
using AutoMapper;
using SWD.TicketBooking.Service.Dtos;
using Microsoft.EntityFrameworkCore;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Utilities;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Repo.UnitOfWork;
using NuGet.Protocol;
using static SWD.TicketBooking.Service.Dtos.ServiceFromStationModel;
using System.Collections.Frozen;


namespace SWD.TicketBooking.Service.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;
        public ServiceService(IUnitOfWork unitOfWork, IFirebaseService firebaseService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }

        public async Task<int> CreateService(CreateServiceModel createServiceModel)
        {
            try
            {
                var checkExistedName = await _unitOfWork.ServiceRepository
                                                        .GetAll()
                                                        .Where(_ => _.ServiceTypeID == createServiceModel.ServiceTypeID && _.Name.ToLower().Trim().Equals(createServiceModel.Name.ToLower()))
                                                        .FirstOrDefaultAsync();
                if (checkExistedName != null)
                {
                    throw new BadRequestException(SD.Notification.Existed("DỊCH VỤ", "TÊN"));
                }

                var service = new SWD.TicketBooking.Repo.Entities.Service
                {
                    ServiceID = Guid.NewGuid(),
                    Name = createServiceModel.Name,
                    ServiceTypeID = createServiceModel.ServiceTypeID,
                    Status = SD.GeneralStatus.ACTIVE
                };
                await _unitOfWork.ServiceRepository.AddAsync(service);
                var rs = _unitOfWork.Complete();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
      
        public async Task<int> UpdateService(UpdateServiceModel updateServiceModel, Guid serviceID)
        {
            try
            {
                var service = await _unitOfWork.ServiceRepository.GetByIdAsync(serviceID);
                if (service == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("DỊCH VỤ"));
                }

                var checkExistedName = await _unitOfWork.ServiceRepository
                                                        .GetAll()
                                                        .Where(_ => _.ServiceTypeID == updateServiceModel.ServiceTypeID
                                                                 && _.Name.ToLower().Trim().Equals(updateServiceModel.Name.ToLower())
                                                                 && _.ServiceID != serviceID)
                                                        .FirstOrDefaultAsync();

                if (checkExistedName != null)
                {
                    throw new BadRequestException(SD.Notification.Existed("DỊCH VỤ", "TÊN"));
                }

                service.Name = updateServiceModel.Name;
                service.ServiceTypeID = updateServiceModel.ServiceTypeID;            
                _unitOfWork.ServiceRepository.Update(service);
                var rs = _unitOfWork.Complete();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateStatus(Guid serviceID)
        {
            try
            {
                var service = await _unitOfWork.ServiceRepository.GetByIdAsync(serviceID);
                if (service == null)
                {
                    throw new NotFoundException(SD.Notification.NotFound("DỊCH VỤ"));
                }
                service.Status = SD.GeneralStatus.INACTIVE;
                _unitOfWork.ServiceRepository.Update(service);
                var rs = _unitOfWork.Complete();
                if (rs > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /*public async Task<List<ServicesInStationResponse>> ServicesFromStations(Guid stationID)
        {
            try
            {
                var checkStation = await _unitOfWork.StationRepository.GetByIdAsync(stationID);
                if (checkStation != null)
                {
                    var listServiceID = await _unitOfWork.Station_ServiceRepository
                                                    .GetAll()
                                                    .Where(s => s.StationID.Equals(stationID))
                                                    .Select(s => s.ServiceID)
                                                    .ToListAsync();
                    var listServices = await _unitOfWork.ServiceRepository
                                                        .GetAll()
                                                        .Where(s => listServiceID.Contains(s.ServiceID))
                                                        .ToListAsync();
                    var serviceType = await _unitOfWork.ServiceTypeRepository
                                                     .GetAll()
                                                     .Where(s => listServices.Select(s=> s.ServiceTypeID).Contains(s.ServiceTypeID))
                                                     .ToListAsync();
                    var result = new List<ServicesInStationResponse>();
                    foreach (var item in serviceType) 
                    {
                        var listServiceInServiceType = new List<ServiceInStationModel> ();
                        var 


                        var serviceResponse = new ServicesInStationResponse
                        {
                            ServiceTypeID = item.ServiceTypeID,
                            ServiceTypeName = item.Name,
                            ServiceInStation = listServiceInServiceType
                        };
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }

*/

    }
}
