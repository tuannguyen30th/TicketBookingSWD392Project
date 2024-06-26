﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.Repositories;
using SWD.TicketBooking.Repo.UnitOfWork;
using SWD.TicketBooking.Service.Dtos;
using SWD.TicketBooking.Service.Exceptions;
using SWD.TicketBooking.Service.IServices;
using SWD.TicketBooking.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility = SWD.TicketBooking.Repo.Entities.Utility;

namespace SWD.TicketBooking.Service.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UtilityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
       /* public async Task<List<UtilityModel>> GetAllUtilityByTripID(Guid id)
        {
            var getTemplateID = await _unitOfWork.TripRepository
                                          .GetAll()
                                          .Where(_ => _.TripID == id)
                                          .Select(_ => _.TemplateID)
                                          .FirstOrDefaultAsync();

            var getTripID = await _unitOfWork.TripRepository
                                             .FindByCondition(_ => _.TemplateID == getTemplateID)
                                             .FirstOrDefaultAsync();

            var tripID = getTripID?.IsTemplate == true ? getTripID.TripID : id;
            var utilities = await _unitOfWork.Trip_UtilityRepository
                .FindByCondition(tu => tu.TripID == tripID && tu.Status.Trim().Equals(SD.GeneralStatus.ACTIVE))
                .Select(tu => tu.Utility)
                .ToListAsync();
            var result = new List<UtilityModel>();
            foreach (var trip in utilities)
            {
                var newModel = new UtilityModel
                {
                    Name = trip.Name,
                    Description = trip.Description,
                    Status = trip.Status,
                };
                result.Add(newModel);
            }
            return result;
        }
*/
        public async Task<List<Utility>> GetAllUtility()
        {

            var result = await _unitOfWork.UtilityRepository.GetAll().ToListAsync();
            return result;
        }

        public async Task<int> CreateNewUtility(CreateUtilityModel utility)
        {
            try
            {
                var checkUtility = await _unitOfWork.UtilityRepository.GetAll().Where(u => u.Name.Equals(utility.Name)).FirstOrDefaultAsync();
                if (checkUtility != null)
                {
                    throw new BadRequestException(SD.Notification.Existed("TIỆN ÍCH", "TÊN"));
                }
                var newUtility = await _unitOfWork.UtilityRepository.AddAsync
                    (
                        new Utility
                        {
                            Name = utility.Name,
                            Description = utility.Description,
                            Status = SD.GeneralStatus.ACTIVE
                        }
                    );
                if (newUtility == null)
                {
                    throw new InternalServerErrorException(SD.Notification.Internal("TIỆN ÍCH", "KHI TẠO MỚI TIỆN ÍCH"));
                }
                var result = _unitOfWork.Complete();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            
        }
    }
}
