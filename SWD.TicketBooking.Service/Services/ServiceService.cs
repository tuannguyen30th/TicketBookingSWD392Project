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
using SWD.TicketBooking.Repo.Helpers;
using SWD.TicketBooking.Service.Services.FirebaseService;
using Microsoft.IdentityModel.Tokens;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.Utilities;


namespace SWD.TicketBooking.Service.Services
{
    public class ServiceService
    {
        /*private readonly IRepository<SWD.TicketBooking.Repo.Entities.Service, int> _serviceRepository;
        private readonly IRepository<Station_Service, int> _stationServiceRepository;
        public readonly IFirebaseService _firebaseService;
        private readonly IMapper _mapper;
        public ServiceService(IRepository<SWD.TicketBooking.Repo.Entities.Service, int> serviceRepository, IRepository<Station_Service, int> stationServiceRepository, IFirebaseService firebaseService, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _stationServiceRepository = stationServiceRepository;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }

        public async Task<int> CreateService(CreateServiceModel createServiceModel)
        {
            try
            {
                var checkExistedName = await _serviceRepository.GetAll().Where(_ => _.ServiceTypeID == createServiceModel.ServiceTypeID && _.Name.ToLower().Trim().Equals(createServiceModel.Name.ToLower())).FirstOrDefaultAsync();
                if (checkExistedName != null)
                {
                    throw new BadRequestException("Service name existed");
                }

                var service = new SWD.TicketBooking.Repo.Entities.Service
                {
                    Name = createServiceModel.Name,
                    Price = createServiceModel.Price,
                    ServiceTypeID = createServiceModel.ServiceTypeID,
                    UrlGuidID = Guid.NewGuid().ToString(),
                    ImageUrl = "",
                    Status = SD.ACTIVE
                };
                await _serviceRepository.AddAsync(service);
                await _serviceRepository.Commit();

                var imagePath = FirebasePathName.SERVICE + $"{service.UrlGuidID}";
                var imageUploadResult = await _firebaseService.UploadFileToFirebase(createServiceModel.ImageUrl, imagePath);
                if (imageUploadResult.IsSuccess)
                {
                    service.ImageUrl = (string)imageUploadResult.Result;
                }

                _serviceRepository.Update(service);
                var rs = await _serviceRepository.Commit();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> UpdateService(UpdateServiceModel updateServiceModel)
        {
            try
            {
                var service = await _serviceRepository.GetByIdAsync(updateServiceModel.ServiceID);
                if (service == null)
                {
                    throw new NotFoundException("Service not found.");
                }

                var checkExistedName = await _serviceRepository.GetAll()
                                      .Where(_ => _.ServiceTypeID == updateServiceModel.ServiceTypeID
                                       && _.Name.ToLower().Trim().Equals(updateServiceModel.Name.ToLower())
                                       && _.ServiceID != updateServiceModel.ServiceID)
                                      .FirstOrDefaultAsync();

                if (checkExistedName != null)
                {
                    throw new BadRequestException("Service name existed");
                }

                service.Name = updateServiceModel.Name;
                service.Price = updateServiceModel.Price;
                service.ServiceTypeID = updateServiceModel.ServiceTypeID;

                if (updateServiceModel.ImageUrl != null && updateServiceModel.ImageUrl.Length > 0)
                {
                    if (!string.IsNullOrEmpty(service.ImageUrl))
                    {
                        string url = $"{FirebasePathName.SERVICE}{service.UrlGuidID}";
                        var deleteResult = await _firebaseService.DeleteFileFromFirebase(url);
                        if (!deleteResult.IsSuccess)
                        {
                            throw new InternalServerErrorException($"Failed to delete old image");
                        }
                    }
                    service.UrlGuidID = Guid.NewGuid().ToString();
                    var imagePath = $"{FirebasePathName.SERVICE}{service.UrlGuidID}";
                    var imageUploadResult = await _firebaseService.UploadFileToFirebase(updateServiceModel.ImageUrl, imagePath);

                    if (imageUploadResult.IsSuccess)
                    {
                        service.ImageUrl = (string)imageUploadResult.Result;
                    }
                    else
                    {
                        throw new InternalServerErrorException($"Failed to upload new image:");
                    }
                }

                _serviceRepository.Update(service);
                var rs = await _serviceRepository.Commit();
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating service", ex);
            }
        }

        public async Task<bool> UpdateStatus(int serviceID)
        {
            try
            {
                var service = await _serviceRepository.GetByIdAsync(serviceID);
                if (service == null)
                {
                    throw new NotFoundException("Service not found.");
                }
                service.Status = "Inactive";
                _serviceRepository.Update(service);
                var rs = await _serviceRepository.Commit();
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
        }*/
    }
}
